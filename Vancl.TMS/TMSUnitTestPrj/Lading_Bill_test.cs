using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vancl.TMS.BLL.LadingBill;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;

namespace TMSUnitTestPrj
{
    /// <summary>
    /// Lading_Bill_test 的摘要说明
    /// </summary>
    [TestClass]
    public class Lading_Bill_test
    {
        public Lading_Bill_test()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void TestMethod1()
        {
            RunCreateTask();
        }

        public static void RunCreateTask()
        {
            var _lbTaskbll = new LB_TASKBLL();
            var _planService = new LB_PLANBLL();

            StreamWriter sw = new StreamWriter("D:\\1.txt");
            string w = DateTime.Now.ToString();
            sw.Write(w);
            sw.Close();

            IList<LB_PLAN> lbPlans = _planService.GetList();

            if (lbPlans == null)
            {
                //Cloud.log
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
                    lbTask.TASKCODE = _lbTaskbll.CreateTaskCode(lbPlan.TODISTRIBUTIONCODE);
                    lbTask.FROMDISTRIBUTIONCODE = lbPlan.FROMDISTRIBUTIONCODE;
                    lbTask.TASKSTATUS = 1;
                    lbTask.CREATETIME = time;
                    var week = (int)time.DayOfWeek; //星期几 

                    #endregion

                    //今天属于计划提货任务
                    if (lbPlan.WEEK.IndexOf(week.ToString(), System.StringComparison.Ordinal) > -1)
                    {
                        lbTask.PREDICTTIME =
                            Convert.ToDateTime(time.ToShortDateString() + " " + lbPlan.SPECIFICTIME);
                        try
                        {
                            if (_lbTaskbll.Add(lbTask))
                            {
                                _lbTaskbll.senMailByTask(lbTask);
                                _planService.UpCreate(lbPlan.ID, 1);
                            }
                        }
                        catch (Exception ex)
                        {
                            //写入日志
                            return;
                        }
                    }
                }
                scope.Complete();
            }

            #endregion
        }


        /// <summary>
        /// 生成任务编号
        /// </summary>
        /// <returns></returns>
        public string CreateTaskCode(UserModel curUser)
        {
            LB_TASKBLL _lbTaskbll = new LB_TASKBLL();
            LB_PLANBLL _planService = new LB_PLANBLL();

            string taskcode = string.Empty;

            DateTime dbtime = _lbTaskbll.GetDBTime();
            //需改
            taskcode = curUser.DistributionCode.ToUpper()
                             + dbtime.Year.ToString().Substring(2, 2)
                             + dbtime.Month.ToString().PadLeft(2, '0')
                             + dbtime.Day.ToString().PadLeft(2, '0');

            string todayCount = _lbTaskbll.ToDayTaskCount().ToString();

            if (Convert.ToInt32(todayCount) < 10)
            {
                todayCount = "00" + todayCount;
            }
            else if (Convert.ToInt32(todayCount) > 9 && Convert.ToInt32(todayCount) < 99)
            {
                todayCount = "0" + todayCount;
            }

            return taskcode + "-" + todayCount;
        }
    }
}
