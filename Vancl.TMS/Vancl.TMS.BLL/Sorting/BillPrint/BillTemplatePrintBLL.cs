using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.BillPrint;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Caching;
using System.Data;
using System.Text.RegularExpressions;
using Vancl.TMS.Core.Logging;

namespace Vancl.TMS.BLL.Sorting.BillPrint
{
    public class BillTemplatePrintBLL : IBillTemplatePrintBLL
    {
        IBillPrintFieldDAL BillPrintFieldDAL = ServiceFactory.GetService<IBillPrintFieldDAL>();
        IBillPrintTemplateDAL BillPrintTemplateDAL = ServiceFactory.GetService<IBillPrintTemplateDAL>();

        public IList<Model.Sorting.BillPrint.BillPrintFieldModel> GetBillPrintFields()
        {
            return BillPrintFieldDAL.GetBillPrintField();
        }

        public Model.Common.ResultModel AddBillPrintField(Model.Sorting.BillPrint.BillPrintFieldModel model)
        {
            var count = BillPrintFieldDAL.Add(model);
            return ResultModel.Create(count > 0);
        }

        public IList<BillPrintTemplateModel> GetBillPrintTemplates(string distributionCode)
        {
            return BillPrintTemplateDAL.GetBillPrintTemplates(distributionCode);
        }

        public ResultModel<PrintTemplateArgModel> SaveBillPrintTemplate(PrintTemplateArgModel argModel)
        {
            if (argModel == null) throw new ArgumentNullException();
            try
            {
                var CurrentUser = UserContext.CurrentUser;
                bool IsAdd = argModel.Id <= 0;
                BillPrintTemplateModel model;

                int count = 0;
                if (IsAdd)
                {//新增 
                    model = new BillPrintTemplateModel();
                    model.Id = BillPrintTemplateDAL.GetNextSequence(model.SequenceName);
                    model.CreateBy = CurrentUser.ID;
                    model.CreateTime = DateTime.Now;
                }
                else
                {//修改
                    model = BillPrintTemplateDAL.Get(argModel.Id);
                    if (model == null) throw new Exception("未找到对应的打印模板");
                    model.UpdateBy = CurrentUser.ID;
                    model.UpdateTime = DateTime.Now;
                }
                model.DistributionCode = CurrentUser.DistributionCode;
                if (string.IsNullOrWhiteSpace(argModel.Background))
                {
                    argModel.Background = " ";
                }
                model.Background = argModel.Background;
                model.Storage = "PrintTemplate_" + model.Id + DateTime.Now.ToString("_yyyyMMddHHmmss");
                model.Height = argModel.Height;
                model.Width = argModel.Width;
                model.IsDeleted = false;
                model.Name = argModel.Name;
                model.Remark = "";
                model.IsDefault = argModel.IsDefault;
                //存储为文件
                CachingManager.Set(model.Storage, argModel.Content);

                if (model.IsDefault)
                {
                    BillPrintTemplateDAL.ClearDefault(model.DistributionCode);
                }
                if (IsAdd) count = BillPrintTemplateDAL.Add(model);
                else count = BillPrintTemplateDAL.Update(model);

                argModel.Id = model.Id;

                return ResultModel.Create(argModel, count > 0);
            }
            catch (Exception ex)
            {

                Vancl.TMS.Core.Logging.Log.loggeremail.Error(ex.Message, ex);
                return ResultModel.Create(argModel, false, "保存模板发生异常", ex);
            }
        }


        public BillPrintTemplateModel GetBillPrintTemplate(long id)
        {
            return BillPrintTemplateDAL.Get(id);
        }

        public ResultModel DeletePrintTemplate(long id)
        {
            var model = GetBillPrintTemplate(id);
            if (model == null)
                return ResultModel.Create(false, "未找到此模板数据，请确认是否已被删除！");
            try
            {
                model.IsDeleted = true;
                int count = BillPrintTemplateDAL.Update(model);
                return ResultModel.Create(true, "删除成功！");
            }
            catch (Exception ex)
            {
                return ResultModel.Create(false, "程序执行异常！", ex);
            }
        }

        public string GetBillPrintTemplateData(long id)
        {
            var model = this.GetBillPrintTemplate(id);
            if (model == null) return null;

            return CachingManager.Get<string>(model.Storage);
        }



        public IList<string> RenderPrintData(long tpl, string data, List<int> row)
        {
            var template = GetBillPrintTemplateData(tpl);
            if (template == null) return null;

            var printData = new List<string>();
            var dt = CachingManager.Get<DataTable>(data);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (row.Any(x => x == i))
                {
                    //   dt.Rows.RemoveAt(i);
                    //   i--;
                    var d = this.ParsePrintData(template, dt.Rows[i]);
                    printData.Add(d);
                }
            }
            return printData;
        }

        private string ParsePrintData(string tpl, DataRow dr)
        {
            //处理特殊显示
            //日期 ${d:yyyy年MM月dd日}
            var p = @"\$\{d:(.*?)\}";
            var matchs = Regex.Matches(tpl, p, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                string old = m.Value;
                string fmt = m.Groups[1].Value;
                tpl = tpl.Replace(old, DateTime.Now.ToString(fmt));
            }

            foreach (DataColumn c in dr.Table.Columns)
            {
                string columnName = c.ColumnName
                    .Replace(@"\", @"\\")
                    .Replace(@"{", @"\{")
                    .Replace(@"}", @"\}")
                    .Replace(@"(", @"\(")
                    .Replace(@")", @"\)")
                    .Replace(@"[", @"\[")
                    .Replace(@"]", @"\]")
                    .Replace(@".", @"\.")
                    .Replace(@"^", @"\^")
                    .Replace(@"$", @"\$")
                    .Replace(@"*", @"\*")
                    .Replace(@"+", @"\+")
                    .Replace(@"?", @"\?");

                //处理特殊显示
                //总数 ${c:列名}
                var patternCount = @"\$\{c:(.*?)\}";
                matchs = Regex.Matches(tpl, patternCount, RegexOptions.IgnoreCase);
                foreach (Match m in matchs)
                {
                    string old = m.Value;
                    string clumnName = m.Groups[1].Value;
                    int count = 0;
                    if (dr[clumnName] != null)
                    {
                        var val = dr[clumnName].ToString();
                        if (string.IsNullOrWhiteSpace(val)) count = 0;
                        else
                        {
                            count = val.Split('\n').Count(x => !string.IsNullOrWhiteSpace(x));
                        }
                    }
                    tpl = tpl.Replace(old, count.ToString());
                }

                string value = dr[c].ToString().Replace("\n", "<br>");
                string pattern = @"\$\{" + columnName + @"\}";  // string.Format(@"\{{0}\}", columnName);
                tpl = Regex.Replace(tpl, pattern, value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            //替换其它未匹配的
            tpl = Regex.Replace(tpl, @"\$\{.*?\}", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return tpl;
        }




        public string FillTemplateData(string tpl, PrintBillNewModel model)
        {
            //处理特殊显示
            //日期 ${d:yyyy年MM月dd日}
            var p = @"\$\{d:(.*?)\}";
            var matchs = Regex.Matches(tpl, p, RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                string old = m.Value;
                string fmt = m.Groups[1].Value;
                tpl = tpl.Replace(old, DateTime.Now.ToString(fmt));
            }

            foreach (var c in model.List)
            {
                string columnName = c.Column
                    .Replace(@"\", @"\\")
                    .Replace(@"{", @"\{")
                    .Replace(@"}", @"\}")
                    .Replace(@"(", @"\(")
                    .Replace(@")", @"\)")
                    .Replace(@"[", @"\[")
                    .Replace(@"]", @"\]")
                    .Replace(@".", @"\.")
                    .Replace(@"^", @"\^")
                    .Replace(@"$", @"\$")
                    .Replace(@"*", @"\*")
                    .Replace(@"+", @"\+")
                    .Replace(@"?", @"\?");

                //处理特殊显示
                //总数 ${c:列名}
                var patternCount = @"\$\{c:(.*?)\}";
                matchs = Regex.Matches(tpl, patternCount, RegexOptions.IgnoreCase);
                foreach (Match m in matchs)
                {
                    string old = m.Value;
                    string clumnName = m.Groups[1].Value;
                    int count = 0;
                    
                    var val = c.Data;
                    if (string.IsNullOrWhiteSpace(val)) count = 0;
                    else
                    {
                        count = val.Split('\n').Count(x => !string.IsNullOrWhiteSpace(x));
                    }
                    
                    tpl = tpl.Replace(old, count.ToString());
                }

                string value = c.Data.Replace("\n", "<br>");
                string pattern = @"\$\{" + columnName + @"\}";  
                tpl = Regex.Replace(tpl, pattern, value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            //替换其它未匹配的
            tpl = Regex.Replace(tpl, @"\$\{.*?\}", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return tpl;
        }
    }
}
