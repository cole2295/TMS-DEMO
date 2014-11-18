using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Vancl.TMS.Model.Common;
using System.IO;
using System.Net;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Model.LadingBill.OracleDAL;
using Vancl.TMS.Util.Net;

namespace TMSUnitTestPrj
{
    /// <summary>
    /// SendMailTest 的摘要说明
    /// </summary>
    [TestClass]
    public class SendMailTest
    {
        public SendMailTest()
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
        public void SendMail()
        {

            MailMessage mail = new MailMessage(Consts.SMTP_FROM, "zhangbendong@vancl.cn", "my test Mail", "emailBody");
            mail.To.Add("weichuanhai@vancl.cn");
            mail.CC.Add("zhangkea@vancl.cn");
            byte[] bytes = new byte[] { 1, 2, 3 };
            mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "出库单明细.xlsx"));

            MailHelper.Send(Consts.SMTP_HOST, new NetworkCredential(Consts.SMTP_ACCOUNT, Consts.SMTP_PASSWORD), mail);
        }

        [TestMethod]
        public void SendMailbytask()
        {
            string MailBody = "<p style=\"font-size: 10pt\">你好！ 【如风达快递有限公司】已下达如下提货任务，请安排提货。</p>  <table width=\"700\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">";
            MailBody += "<div align=\"center\">";
            MailBody += "<tr> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">任务编号</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">商家</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">库房地址</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">联系人</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">联系电话</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">预计提货单量</td> <td style=\"border-top:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">预计提货重量</td> <td style=\"border-top:1px solid #CCCCCC;border-right:1px solid #CCCCCC;border-left:1px solid #CCCCCC;\">提货时间</td> </tr>";

            for (int row = 0; row < 1; row++)
            {
                MailBody += "<tr>";
                for (int col = 0; col < 8; col++)
                {
                    if (col == 7)
                    {
                        MailBody += "<td style=\"border-left:1px solid #ccc;border-top:1px solid #ccc;border-bottom:1px solid #ccc;border-right:1px solid #ccc;\">";
                    }
                    else
                    {
                        MailBody += "<td   style=\"border-left:1px solid #ccc;border-top:1px solid #ccc;border-bottom:1px solid #ccc;\">";
                    }

                    MailBody += "好好好" + col + row;
                    MailBody += "</td>";
                }
                MailBody += "</tr>";
            }
            MailBody += "</table>";
            MailBody += "</div>";

            MailMessage mail = new MailMessage(Consts.SMTP_FROM, "", "my test Mail", MailBody);
            mail.IsBodyHtml = true;
            mail.To.Add("lining@vancl.cn");
            mail.To.Add("weidonga@vancl.cn");
            //byte[] bytes = new byte[] { 1, 2, 3 };
            //mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "出库单明细.xlsx"));

            MailHelper.Send(Consts.SMTP_HOST, new NetworkCredential(Consts.SMTP_ACCOUNT, Consts.SMTP_PASSWORD), mail);
        }

        [TestMethod]
        public void testTASK()
        {
            LB_TASKDAL dTaskdal = new LB_TASKDAL();
            IList<TaskViewModel> list = dTaskdal.GetTaskPage("10037,10036");
        }
    }
}
