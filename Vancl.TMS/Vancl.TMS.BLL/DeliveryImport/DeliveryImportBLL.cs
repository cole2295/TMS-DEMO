using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.DeliveryImport;
using Vancl.TMS.IDAL.DeliveryImport;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Util.ModelUtil;
using Vancl.TMS.Util.Extensions;
using Vancl.TMS.BLL.Transport.Dispatch;
using Vancl.TMS.Core.Security;
using System.Web;
using System.IO;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.IDAL.BaseInfo;

namespace Vancl.TMS.BLL.DeliveryImport
{
    public class DeliveryImportBLL : IDeliveryImportBLL
    {
        private IDeliveryImportDAL dal = ServiceFactory.GetService<IDeliveryImportDAL>();
        private IPreDispatchDAL preDispatchDal = ServiceFactory.GetService<IPreDispatchDAL>();
        private DispatchBLL dispatchService = new DispatchBLL();
        IBoxDAL _boxDAL = ServiceFactory.GetService<IBoxDAL>("BoxDAL");
        private ICarrierWaybillDAL _carrierWaybillDAL = ServiceFactory.GetService<ICarrierWaybillDAL>("CarrierWaybillDAL");
        private static readonly string errorFilePath = "DeliveryImportErrorFile/{0}/{1}/{2}/";

        #region IDeliveryImportBLL 成员

        public int AddRecord(Model.ImportRecord.DeliveryInRecordModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model", "参数不能为空");
            return dal.AddRecord(model);
        }

        public List<Model.ImportRecord.DeliveryInRecordModel> GetRecordList(Model.ImportRecord.DeliveryInRecordSearchModel conditions)
        {
            return dal.GetRecordList(conditions);
        }

        public ResultModel AddDelivery(string[,] data, string templatePath)
        {
            ResultModel r = new ResultModel();
            int sucCount = 0;
            int totalCount = 0;
            //生成导入批次号
            string batchNo = DateTime.Now.ToString("yyyyMMddHHmmssms") + (new Random()).Next(1000).ToString();
            //回执文件相对路径
            string filePath = string.Empty;
            if (data != null && data.GetLength(0) > 0)
            {
                try
                {
                    //获取导入模版
                    string[] template = GetTemplate(templatePath);
                    //检查表头
                    ResultModel headerCheckResult = CheckHeader(GetHeader(data), template);

                    if (headerCheckResult.IsSuccess)
                    {
                        //获取导入内容
                        List<ImportTemplateModel> content = GetContent(data, batchNo);
                        if (content.Count > 0)
                        {
                            totalCount = content.Count;
                            //遍历所有需要导入的内容
                            for (int i = 0; i < content.Count; i++)
                            {
                                //检查内容
                                ResultModel contentCheckResult = CheckContent(content[i]);
                                if (contentCheckResult.IsSuccess)
                                {
                                    //写入提货单主表
                                    ResultModel result = dispatchService.Add(content[i]);
                                    if (result.IsSuccess)
                                    {
                                        //增加导入成功条数
                                        sucCount++;
                                    }
                                    else
                                    {
                                        //记录导入失败原因
                                        content[i].ErrorMessage = result.Message;
                                    }
                                }
                                else
                                {
                                    //检查内容未通过,写入出错信息
                                    content[i].ErrorMessage = contentCheckResult.Message;
                                }
                            }

                            r.IsSuccess = true;

                            //导入成功数量不等于总数量,生成回执文件
                            if (sucCount < totalCount)
                            {
                                DateTime now = DateTime.Now;
                                //回执文件保存文件夹
                                string dir = String.Format(errorFilePath, now.Year, now.Month, now.Day);
                                filePath = Path.Combine(dir, batchNo + ".xlsx");
                                //构造回执文件表头(原表头基础上增加错误原因列)
                                string[] errorHeader = template.Concat(new List<string>() { "错误原因" }).ToArray();
                                //构造回执文件内容
                                string[,] errorContent = content.To2Array(new string[] { "DepartrueID", "ArrivalID", "CarrierID", "BatchNo" });
                                //生成回执文件,并保存在FTP服务器
                                ResultModel result = WriteErrorFile(BuildErrorData(errorHeader, errorContent), filePath);
                                if (result.IsSuccess)
                                {
                                    r.Message = "未能全部导入成功(" + sucCount.ToString() + "/" + totalCount.ToString() + "),详细请查看回执文件";
                                }
                                else
                                {
                                    r.Message = "未能全部导入成功(" + sucCount.ToString() + "/" + totalCount.ToString() + "),但回执文件生成失败";
                                }
                            }
                            else
                            {
                                r.Message = "全部导入成功";
                            }

                        }
                        else
                        {
                            r.IsSuccess = false;
                            r.Message = "导入内容为空";
                        }
                    }
                    else
                    {
                        r.IsSuccess = false;
                        r.Message = "导入文件与模版文件不匹配.";
                    }
                }
                catch (Exception e)
                {
                    r.IsSuccess = false;
                    r.Message = e.Message;
                }
            }

            //插入导入日志
            WriteLog(r, sucCount, totalCount, batchNo, filePath, Enums.DeliverySource.Import);

            return r;
        }

        private void WriteLog(ResultModel r, int sucCount, int totalCount, string batchNo, string filePath, Enums.DeliverySource source)
        {
            DeliveryInRecordModel record = new DeliveryInRecordModel();
            record.CreateBy = UserContext.CurrentUser.ID;
            record.CreateByName = UserContext.CurrentUser.UserName;
            record.DeliverySource = source;
            record.FaultCount = totalCount - sucCount;
            record.Note = r.Message;
            record.RecordCount = totalCount;
            record.BatchNo = batchNo;
            record.FilePath = filePath;
            AddRecord(record);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 生成回执文件
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="errorFilePath">WEB服务器本地路径</param>
        /// <returns></returns>
        private ResultModel WriteErrorFile(string[,] data, string errorFilePath)
        {
            ResultModel r = new ResultModel();
            if (data != null && data.GetLength(0) > 0)
            {
                MemoryStream stream = new MemoryStream();
                using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
                {
                    helper.CreateNewWorksheet("提货单导入回执文件");
                    r.IsSuccess = helper.WriteData(data);
                }
                try
                {
                    stream.Seek(0, 0);
                    IFileTransfer tool = FileIOToolFactory.GetFileIOTool(FtpAction.UpLoad);
                    tool.LocalContext = new FtpTransferLocalContext()
                    {
                        DataStream = stream
                    };
                    tool.ServerContext = new FtpTransferServerContext();
                    tool.ServerContext.FileName = errorFilePath;
                    tool.ServerContext.ServerPath = Path.GetDirectoryName(errorFilePath);

                    tool.DoAction();
                    tool.LocalContext.DataStream.Close();
                }
                catch (Exception e)
                {
                    r.IsSuccess = false;
                }
            }

            return r;
        }

        /// <summary>
        /// 构造回执文件数据源(新增错误原因列)
        /// </summary>
        /// <param name="errorHeader">回执文件表头</param>
        /// <param name="content">回执文件内容</param>
        /// <returns></returns>
        private string[,] BuildErrorData(string[] errorHeader, string[,] content)
        {
            int rowCount = content.GetLength(0);
            int colCount = errorHeader.Length;
            string[,] data = new string[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (i == 0)
                    {
                        data[i, j] = errorHeader[j].Trim();
                    }
                    else
                    {
                        data[i, j] = content[i, j];
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 验证表头
        /// </summary>
        /// <param name="header"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private ResultModel CheckHeader(string[] header, string[] template)
        {
            ResultModel r = new ResultModel();
            r.IsSuccess = true;
            if (header == null)
            {
                r.IsSuccess = false;
                r.Message = "表头为空";
                return r;
            }
            if (template == null)
            {
                r.IsSuccess = false;
                r.Message = "模版为空";
                return r;
            }
            if (header.Length != template.Length)
            {
                r.IsSuccess = false;
                r.Message = "表头与模版不一致";
                return r;
            }
            List<string> templateList = template.ToList<string>();
            for (int i = 0; i < header.Length; i++)
            {
                if (!templateList.Contains(header[i]))
                {
                    r.IsSuccess = false;
                    r.Message = "模版中不存在列:" + header[i];
                    break;
                }
            }
            return r;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private ResultModel CheckContent(ImportTemplateModel content)
        {
            //ResultModel r = new ResultModel();
            //r.IsSuccess = true;
            //r.Message = string.Empty;
            //if (content != null)
            //{

            //}
            //else
            //{
            //    r.IsSuccess = false;
            //    r.Message = "内容不能为空";
            //}
            //return r;
            return new ResultModel() { IsSuccess = true, Message = "" };
        }

        /// <summary>
        /// 获取表头
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        private string[] GetHeader(string[,] data)
        {
            if (data != null)
            {
                string[] header = new string[data.GetLength(1)];
                for (int i = 0; i < header.Length; i++)
                {
                    header[i] = data[0, i];
                }
                return header;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        private List<ImportTemplateModel> GetContent(string[,] data, string batchNo)
        {
            int rowCount = data.GetLength(0);
            if (rowCount > 1)
            {
                return BuildContentList(data, batchNo);
            }
            return null;
        }

        /// <summary>
        /// 创建内容List
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        private List<ImportTemplateModel> BuildContentList(string[,] data, string batchNo)
        {
            List<ImportTemplateModel> list = new List<ImportTemplateModel>();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            int rowCount = data.GetLength(0);
            int colunmCount = data.GetLength(1);
            for (int i = 0; i < colunmCount; i++)
            {
                dic.Add(i, ModelDescriptionUtil.GetFieldNameByDescription<ImportTemplateModel>(data[0, i]));
            }

            string[] row = new string[colunmCount];
            for (int i = 1; i < rowCount; i++)
            {
                for (int j = 0; j < colunmCount; j++)
                {
                    row[j] = data[i, j] == null ? " " : data[i, j].Trim();
                }

                ImportTemplateModel model = new ImportTemplateModel();
                ImportTemplateModel m = model.SetValue<ImportTemplateModel>(dic, row);
                m.BatchNo = batchNo;
                list.Add(m);
            }

            return list;
        }

        /// <summary>
        /// 获取模版信息
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <returns></returns>
        private string[] GetTemplate(string templatePath)
        {
            using (OpenXMLHelper helper = new OpenXMLHelper(templatePath, OpenExcelMode.OpenForRead))
            {
                string[,] temp = helper.ReadUsedRangeToEndWithoutBlank();
                if (temp != null && temp.GetLength(0) > 0)
                {
                    string[] template = new string[temp.GetLength(1)];
                    for (int i = 0; i < temp.GetLength(1); i++)
                    {
                        template[i] = temp[0, i];
                    }

                    return template;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region IDeliveryImportBLL 成员


        public List<ViewPreDispatchModel> GetBatchPreDispatchInfo(PreDispatchSearchModel searchmodel)
        {
            return preDispatchDal.GetPreDispatchInfo(searchmodel);
        }
        public List<ViewPreDispatchModel> GetBatchPreDispatchInfoV1(PreDispatchSearchModel searchmodel)
        {
            return preDispatchDal.GetPreDispatchInfoV1(searchmodel);
        }

        public List<ViewPreDispatchModel> SearchPreDispatchInfo(PreDispatchSearchModel searchmodel)
        {
            return preDispatchDal.SearchPreDispatchInfo(searchmodel);

        }

        public List<ViewPreDispatchModel> SearchPreDispatchInfoV1(PreDispatchSearchModel searchmodel)
        {
            return preDispatchDal.SearchPreDispatchInfoV1(searchmodel);

        }

        public int UpdateToDisabledDispatchV1(List<long> listPDID, Enums.DispatchStatus UpdateStatus)
        {
            return preDispatchDal.UpdateToDisabledDispatchV1(listPDID, UpdateStatus);

        }

        #endregion

        #region IDeliveryImportBLL 成员


        public ResultModel AddToDispatch(ViewDispatchWithDetailsModel model, Enums.DeliverySource source)
        {
            if (model == null) throw new ArgumentNullException("model", new Exception("新增提货单参数不能为空."));
            ResultModel r = new ResultModel();
            string batchNo = DateTime.Now.ToString("yyyyMMddHHmmssms") + (new Random()).Next(1000).ToString();
            List<string> batchnos = new List<string>();
            foreach (var item in model.Details)
            {
                batchnos.Add(item.BatchNo);
            }
            if (!_boxDAL.IsExistsBatchDockingFailed(batchnos))
            {
                r = dispatchService.Add(model, batchNo, source);
            }
            else
            {
                r.Failed("存在同步信息不完整的批次号/箱号.请联系管理员或稍后重试.");
            }
            if (r.IsSuccess)
                WriteLog(r, 1, 1, batchNo, string.Empty, source);
            else
                WriteLog(r, 0, 1, batchNo, string.Empty, source);

            return r;
        }

        #endregion
    }
}
