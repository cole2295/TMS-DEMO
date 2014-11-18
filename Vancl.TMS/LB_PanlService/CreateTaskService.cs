using System;
using System.Collections.Generic;
using Quartz;
using Vancl.TMS.BLL.LadingBill;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util;

namespace LB_PanlService
{
    public class CreateTaskService : IStatefulJob
    {

        public void Execute(JobExecutionContext context)
        {
            RunCreateTask();
        }

        public static void RunCreateTask()
        {
            int runCount = 0;

            try
            {
                MessageCollector.Instance.Collect("default", "提货计划正在执行", true);

                ILB_PLANBLL _planService = ServiceFactory.GetService<ILB_PLANBLL>("LB_PLANBLL");
                ILB_TASKBLL _lbTaskbll = ServiceFactory.GetService<ILB_TASKBLL>();


                IList<LB_PLAN> lbPlans = _planService.GetList();

                if (lbPlans == null)
                {
                    //Cloud.log
                    return;
                }
                if (lbPlans.Count == 0)
                {
                    return;
                }

                DateTime time = _lbTaskbll.GetDBTime();

                #region 生成任务，更新计划生成状态

                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    foreach (var lbPlan in lbPlans)
                    {
                        #region 赋值

                        LB_TASK lbTask = new LB_TASK();
                        lbTask.MERCHANTID = lbPlan.MERCHANTID;
                        lbTask.TODISTRIBUTIONCODE = lbPlan.TODISTRIBUTIONCODE;
                        lbTask.RECEIVEEMAIL = lbPlan.RECEIVEMAIL;
                        lbTask.PREDICTWEIGHT = lbPlan.PREDICTWEIGHT;
                        lbTask.MILEAGE = lbPlan.MILEAGE;
                        lbTask.WAREHOUSEID = lbPlan.WAREHOUSEID;
                        lbTask.DEPARTMENT = lbPlan.DEPARTMENT;
                        lbTask.PREDICTORDERQUANTITY = lbPlan.ORDERQUANTITY;
                        lbTask.PICKPRICETYPE = lbPlan.PRICETYPE;
                        lbTask.ONCEAMOUNT = Convert.ToDecimal(lbPlan.AMOUNT);
                        lbTask.ORDERAMOUNT = Convert.ToDouble(lbPlan.AMOUNT);
                        lbTask.TASKCODE = _lbTaskbll.CreateTaskCode(lbPlan.FROMDISTRIBUTIONCODE);
                        lbTask.FROMDISTRIBUTIONCODE = lbPlan.FROMDISTRIBUTIONCODE;
                        lbTask.TASKSTATUS = 1;
                        lbTask.CREATETIME = time;
                        var week = (int)time.DayOfWeek; //星期几 

                        #endregion

                        //今天属于计划提货任务
                        if (lbPlan.WEEK.IndexOf(week.ToString(), StringComparison.Ordinal) > -1)
                        {
                            lbTask.PREDICTTIME =
                                Convert.ToDateTime(time.ToShortDateString() + " " + lbPlan.SPECIFICTIME);
                            try
                            {
                                if (_lbTaskbll.Add(lbTask))
                                {
                                    runCount++;
                                    _lbTaskbll.senMailByTask(lbTask);
                                    _planService.UpCreate(lbPlan.ID, 1);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageCollector.Instance.Collect("default:", ex.StackTrace);
                                //写入日志
                                return;
                            }
                        }
                    }
                    scope.Complete();
                }

                #endregion

                MessageCollector.Instance.Collect("default", "计划条数:" + runCount, true);
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("default:", ex.StackTrace);
            }
        }



    }
}
