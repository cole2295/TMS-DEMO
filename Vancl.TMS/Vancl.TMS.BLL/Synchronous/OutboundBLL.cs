using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Util.IO;
using System.IO;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Except;
using System.Data.SqlClient;
using Vancl.TMS.Model.Common;
using System.Data.Common;
using Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;

namespace Vancl.TMS.BLL.Synchronous
{
    public class OutboundBLL : BaseBLL, IOutboundBLL
    {
        private IOutboundLMSDAL lmsDAL = ServiceFactory.GetService<IOutboundLMSDAL>("lmsOutboundDAL");
        /// <summary>
        /// TMS数据交互服务
        /// </summary>
        IDeliveryDataEntranceBLL DeliveryDataEntranceBLL = ServiceFactory.GetService<IDeliveryDataEntranceBLL>("DeliveryDataEntranceBLL");
        //private IOutboundTMSDAL tmsDAL = ServiceFactory.GetService<IOutboundTMSDAL>("tmsOutboundDAL");

        private class GoodsTypeFlag
        {
            public Enums.GoodsType Type
            {
                get;
                set;
            }

            public bool IsChecked
            {
                get;
                set;
            }
        }

        /// <summary>
        /// 当前正在处理的序列化完整文件名称
        /// </summary>
        public string CurFileFullName
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前正在处理的序列化短文件名称
        /// </summary>
        public string CurFileName
        {
            get;
            private set;
        }

        #region IOutboundBLL 成员

        /// <summary>
        /// 订单相关属性汇总
        /// </summary>
        /// <param name="boxModel">箱对象</param>
        private void OrderInfoSum(OutBoxModel boxModel)
        {
            if (null == boxModel) throw new ArgumentNullException("boxModel");
            if (null != boxModel.Order && boxModel.Order.Count > 0)
            {
                boxModel.TotalAmount = boxModel.Order.Sum(p => p.Price);
                //如果为批次，则需求Linq汇总，装箱自带重量和数量
                if (boxModel.NoType == Enums.SyncNoType.Batch)
                {
                    boxModel.TotalCount = boxModel.Order.Count;
                    boxModel.Weight = boxModel.Order.Sum(p => p.Weight);
                }
                boxModel.ContentType = GetBoxContentGoodsType(boxModel);
            }
        }

        public void LMSOutbondDataToFile(Model.Synchronous.OutboundReadParam argument)
        {
            if (null == argument) throw new ArgumentNullException("argument");
            OutBoxModel boxModel = lmsDAL.GetBoxModel(argument);
            if (null == boxModel || String.IsNullOrWhiteSpace(boxModel.BoxNo))
            {
                return;
            }
            boxModel.Order = lmsDAL.GetOrderList(boxModel);
            if (null == boxModel.Order
               || boxModel.Order.Count < 1)
            {
                boxModel.SC2TMSFlag = Model.Common.Enums.SC2TMSSyncFlag.Error;
                lmsDAL.UpdateBoxStatus(boxModel, Model.Common.Enums.SC2TMSSyncFlag.Notyet);
                return;             //箱子没有订单，需要在监控服务监控
            }
            OrderInfoSum(boxModel);
            IOHelper.EntityToFile<OutBoxModel>(boxModel, argument.FileWrittenName);
            try
            {
                boxModel.SC2TMSFlag = Model.Common.Enums.SC2TMSSyncFlag.Synchronizing;
                lmsDAL.UpdateBoxStatus(boxModel, Model.Common.Enums.SC2TMSSyncFlag.Notyet);
                //ReName
                File.Move(argument.FileWrittenName, argument.FileReadName);
            }
            catch (Exception)
            {
                File.Delete(argument.FileWrittenName);
            }
        }

        /// <summary>
        /// 取得箱子的ContentGoodsType
        /// </summary>
        /// <param name="syncModel"></param>
        /// <returns></returns>
        private Enums.GoodsType GetBoxContentGoodsType(OutBoxModel syncModel)
        {
            var NormalObj = syncModel.Order.Find(p => p.GoodsType == Enums.GoodsType.Normal);
            var FrangibleObj = syncModel.Order.Find(p => p.GoodsType == Enums.GoodsType.Frangible);
            var ContrabandObj = syncModel.Order.Find(p => p.GoodsType == Enums.GoodsType.Contraband);
            GoodsTypeFlag[] arrObjs = new GoodsTypeFlag[] 
            { 
                new GoodsTypeFlag() { IsChecked = NormalObj != null , Type = Enums.GoodsType.Normal},
                new GoodsTypeFlag() { IsChecked = FrangibleObj != null , Type = Enums.GoodsType.Frangible},
                new GoodsTypeFlag() { IsChecked = ContrabandObj != null , Type = Enums.GoodsType.Contraband},
            };
            Enums.GoodsType? tmpContentType = null;
            for (int i = 0; i < arrObjs.Length; i++)
            {
                if (arrObjs[i].IsChecked)
                {
                    if (!tmpContentType.HasValue)
                    {
                        tmpContentType = arrObjs[i].Type;
                        continue;
                    }
                    tmpContentType = tmpContentType.Value | arrObjs[i].Type;
                }
            }
            return tmpContentType.Value;
        }

        public void FileToTMSOrder(Model.Synchronous.OutboundWriteParam argument)
        {
            if (null == argument) throw new ArgumentNullException("argument");
            CurFileFullName = IOHelper.SearchFile(argument.FileDir, argument.FileSearchPattern);
            if (String.IsNullOrWhiteSpace(CurFileFullName))
            {
                return;                                 //No File To Sync
            }
            try
            {
                CurFileName = Path.GetFileName(CurFileFullName);
                OutBoxModel syncModel = IOHelper.FileToEntity<OutBoxModel>(CurFileFullName);
                if (null == syncModel
                    || String.IsNullOrWhiteSpace(syncModel.BoxNo)
                    || null == syncModel.Order
                    || syncModel.Order.Count < 1)
                {
                    RemoveAbnormalFile(argument.AbnormalFileDir, "无数据或者反序列化异常", null);
                }
                else
                {
                    TMSEntranceModel entranceModel = new TMSEntranceModel() 
                    {
                        Arrival = syncModel.ArrivalID.ToString() ,
                        BatchNo = syncModel.BoxNo ,
                        ContentType = syncModel.ContentType,
                        Departure = syncModel.DepartureID.ToString() , 
                        Source =  syncModel.NoType == Enums.SyncNoType.Batch? Enums.TMSEntranceSource.SortingOutbound: Enums.TMSEntranceSource.SortingPacking,
                        Detail = new System.Collections.Generic.List<BillDetailModel>(),
                        Weight = syncModel.Weight,
                        TotalCount = syncModel.TotalCount,
                        TotalAmount = syncModel.TotalAmount
                    };
                    syncModel.Order.ForEach(p => 
                    {
                        entranceModel.Detail.Add(new BillDetailModel() 
                        { 
                            BillType = (Enums.BillType)p.LMSwaybillType.Value, 
                            CustomerOrder = p.CustomerOrder, 
                            FormCode = p.FormCode ,
                            GoodsType = p.GoodsType,
                            Price = p.Price
                        });
                    });
                    var result = DeliveryDataEntranceBLL.DataEntrance(entranceModel);
                    if (result.IsSuccess)
                    {
                        syncModel.SC2TMSFlag = Model.Common.Enums.SC2TMSSyncFlag.Already;
                        lmsDAL.UpdateBoxStatus(syncModel, Model.Common.Enums.SC2TMSSyncFlag.Synchronizing);
                        BackUpCurFile(argument.BackUpFileDir);
                    }
                    else
                    {
                        syncModel.SC2TMSFlag = Model.Common.Enums.SC2TMSSyncFlag.Error;
                        lmsDAL.UpdateBoxStatus(syncModel, Model.Common.Enums.SC2TMSSyncFlag.Synchronizing);
                        RemoveAbnormalFile(argument.AbnormalFileDir, result.Message, null);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                RemoveAbnormalFile(argument.AbnormalFileDir, "反序列化文件异常", ex);
            }
            catch (DbException ex)
            {
                RemoveAbnormalFile(argument.AbnormalFileDir, "跨库异常", ex);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除异常文件
        /// </summary>
        /// <param name="strAbnormalMsg"></param>
        /// <param name="ex"></param>
        private void RemoveAbnormalFile(string abnormalFileDir, string strAbnormalMsg, Exception ex)
        {
            string abnormalFileName = abnormalFileDir + CurFileName;
            IOHelper.Remove(CurFileFullName, abnormalFileName);
            throw new SyncFileException(abnormalFileName
                , String.Format("文件{0}出现{1}问题", CurFileFullName, strAbnormalMsg)
                , ex);
        }

        /// <summary>
        /// 备份当前文件
        /// </summary>
        private void BackUpCurFile(string strbackupDir)
        {
            IOHelper.Remove(CurFileFullName, GetBackUpFileName(strbackupDir));
        }

        /// <summary>
        /// 备份文件夹目录名
        /// </summary>
        /// <returns></returns>
        private string GetBackUpFileName(string strbackupDir)
        {
            return string.Format(@"{0}{1}\{2}-{3}\{4}"
                , strbackupDir
                , DateTime.Now.ToString("yyyy-MM-dd")
                , DateTime.Now.ToString("HH")
                , (DateTime.Now.Minute % 3).ToString()
                , CurFileName
                );
        }


        #endregion
    }
}
