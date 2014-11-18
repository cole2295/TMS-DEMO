using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL;
using Vancl.TMS.Model.BaseInfo;
using System.Threading;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Pool;
using System.IO;
using System.Reflection;
using System.Web;
using System.Net.Sockets;
using Vancl.TMS.Core.FormulaManager;
using System.Data;
using System.Data.Common;
using Vancl.TMS.Util.DbHelper;
using System.Transactions;
using Oracle.DataAccess.Client;
using Vancl.TMS.BLL.Formula.Common;
using Vancl.TMS.Core;
using Vancl.TMS.Core.Base;
using Vancl.TMS.Core.ACIDManager;
using System.Data.SqlClient;
using Vancl.TMS.Model.Report.ComplexReport;
using System.Diagnostics;
using Vancl.TMS.Model.Common;
using Vancl.TMS.DAL.Sql2008.Synchronous;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.LMS;


namespace ConsoleAppTest
{
    class Program
    {
        static string Connstr = "User Id=ps_tms; Password=Vancl_123; Data Source=TMS_TEST;";
        static string Connstrlmstest = "User Id=ps_lms; Password=Vancl_123; Data Source=TMS_TEST;";
        static string ConnstrLmsSQLTest = "Database=LMS_RFD;Data Source=PEK7-DEV-02.vancloa.cn;Network=dbmssocn;Application Name=TR-FrameWork;Connection Timeout=30;Password=24E7E0C8-16D4-4C6C-8E7E-3741A97E4486;User ID=devuser;";
        static string Connstr2 = "Database=TR_TestDB;Data Source=PEK7-DEV-02.vancloa.cn;Network=dbmssocn;Application Name=TR-FrameWork;Connection Timeout=30;Password=24E7E0C8-16D4-4C6C-8E7E-3741A97E4486;User ID=devuser;min pool size = 2;max pool size = 5";
        //<add name="oracleODP" connectionString="User Id=ps_tms; Password=Vancl_123; Data Source=TMS_TEST;" providerName="Oracle.DataAccess.Client"/>  

        static void DisplayLayOut(string msg)
        {
            Console.WriteLine(msg);
        }

        static void SQLUserTransactionNoQuery()
        {
            string sql1 = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(SEQ_OR_TEST_ID.NEXTVAL,'AAA威武{0}',1,0,10.08)", Guid.NewGuid().ToString());
            string sql2 = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(SEQ_OR_TEST_ID.NEXTVAL,'BBB威武{0}',1,0,10.08)", Guid.NewGuid().ToString());
            string sql21 = string.Format(@"INSERT INTO ps_tms.OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(ps_tms.SEQ_OR_TEST_ID.NEXTVAL,'BBB威武{0}',1,0,1000.08)", Guid.NewGuid().ToString());
            //nll
            //lkk
            using (IACID acid = ACIDScopeFactory.GetTmsACID())
            {


                OracleHelper.ExecuteSqlNonQuery(VanclDbConnection.TMSWriteConnection.PoolObject, sql1);
                OracleHelper.ExecuteSqlNonQuery(VanclDbConnection.TMSWriteConnection.PoolObject, sql2);
                using (IACID inneracid = ACIDScopeFactory.GetTmsACID())
                {
                    //int i = 10;
                    //int j = 0;
                    //Console.WriteLine(i / j);
                    OracleHelper.ExecuteSqlNonQuery(VanclDbConnection.TMSWriteConnection.PoolObject, sql21);
                    inneracid.Complete();
                }
                acid.Complete();
            }

        }

        //
        /// <summary>
        /// 汉字转换为char即使加size限制也容易出错
        /// </summary>
        static void CharLengthTest()
        {
            
            var model = new GPSOrderEntityModel();
            model.orderno = "9527000001";
            model.truckno = "天津邮局自提（广州） ";
            var strSql = @"
INSERT INTO GPSOrder (orderno ,city ,Warehouse ,stations ,cid ,address ,lng ,lat ,createtime ,gpsid ,time ,truckno ,isSyn)
VALUES (@orderno ,@city ,@warehouse ,@stations ,@cid ,@address ,@lng ,@lat ,@createtime ,@gpsid ,@time ,@truckno ,@IsSyn)
";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter(){ ParameterName = "@orderno", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.orderno },
                    new SqlParameter(){ ParameterName = "@city", SqlDbType = SqlDbType.NVarChar , Size = 50 ,Value = model.city },
                    new SqlParameter(){ ParameterName = "@warehouse", SqlDbType = SqlDbType.NVarChar , Size = 20,Value = model.warehouse },
                    new SqlParameter(){ ParameterName = "@stations", SqlDbType = SqlDbType.NVarChar , Size = 50 ,Value = model.stations },
                    new SqlParameter(){ ParameterName = "@cid", SqlDbType = SqlDbType.Int ,Value = model.cid },
                    new SqlParameter(){ ParameterName = "@address", SqlDbType = SqlDbType.NVarChar , Size = 120 ,Value = model.address },
                    new SqlParameter(){ ParameterName = "@lng", SqlDbType = SqlDbType.Float ,Value = model.lng },
                    new SqlParameter(){ ParameterName = "@lat", SqlDbType = SqlDbType.Float ,Value = model.lat },
                    new SqlParameter(){ ParameterName = "@createtime", SqlDbType = SqlDbType.DateTime ,Value = model.createtime },
                    new SqlParameter(){ ParameterName = "@gpsid", SqlDbType = SqlDbType.NVarChar , Size = 20, Value = model.gpsid },
                    new SqlParameter(){ ParameterName = "@time",  SqlDbType = SqlDbType.DateTime ,Value = model.time },
                    new SqlParameter(){ ParameterName = "@truckno",  SqlDbType = SqlDbType.Char , Size = 16, Value = model.truckno },
                    new SqlParameter(){ ParameterName = "@IsSyn", SqlDbType = SqlDbType.Bit ,Value = model.IsSyn }
             };
            SqlHelper.ExecuteNonQuery(ConnstrLmsSQLTest, CommandType.Text,  strSql, parameters);
        }

        static void SQLArrayQuery()
        {
            string sql = @"
select * from ExpressCompany 
where EXPRESSCOMPANYID = :EXPRESSCOMPANYID
";
            //            string sql = @"
            //UPDATE ExpressCompany 
            //SET ISDELETED = 0
            //where EXPRESSCOMPANYID = :EXPRESSCOMPANYID
            //";

            int[] ArrID = new int[] { 73, 74, 75, 76, 77 };

            OracleConnection conn = new OracleConnection(Connstr);
            conn.Open();
            OracleCommand cmd = new OracleCommand(sql, conn);
            cmd.ArrayBindCount = ArrID.Length;
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="EXPRESSCOMPANYID",DbType = DbType.Int32 , Value= ArrID}
            };
            cmd.Parameters.AddRange(arguments);
            DbDataReader reader = cmd.ExecuteReader();
            conn.Close();
            Console.WriteLine(reader.HasRows);


            //OracleHelper.ExecuteSqlArrayNonQuery(conn, sql, ArrID.Length, arguments);


        }

        static void SQLDataReaderTest()
        {
            String sql = @"
SELECT * FROM sc_bill
WHERE formcode='1321111200'
";
            OracleConnection conn = new OracleConnection(Connstr);
            conn.Open();
            for (int i = 0; i < 3000; i++)
            {
                OracleCommand cmd1 = new OracleCommand(sql, conn);
                var reader = cmd1.ExecuteReader();
            }
            conn.Close();
            conn.Open();
            for (int i = 0; i < 3000; i++)
            {
                OracleCommand cmd1 = new OracleCommand(sql, conn);
                var reader = cmd1.ExecuteReader();

            }
            conn.Close();
        


        }


        static void SQLTransactionNoQuery()
        {
            string sql1 = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(SEQ_OR_TEST_ID.NEXTVAL,'TR-AAA威武{0}',19,0,10.08)", Guid.NewGuid().ToString());
            string sql2 = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(SEQ_OR_TEST_ID.NEXTVAL,'TR-BBB威武{0}',19,022,10.08)", Guid.NewGuid().ToString());
            string sql21 = string.Format(@"INSERT INTO ps_tms.OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(ps_tms.SEQ_OR_TEST_ID.NEXTVAL,'BBB威武{0}',19,0,10.08)", Guid.NewGuid().ToString());
            OracleConnection conn = new OracleConnection(Connstr);
            conn.Open();
            OracleTransaction trans = conn.BeginTransaction();

            OracleCommand cmd1 = new OracleCommand(sql1, conn);
            //cmd1.Transaction = trans;
            OracleCommand cmd2 = new OracleCommand(sql2, conn);
            // cmd2.Transaction = trans;

            Console.WriteLine(cmd1.Transaction == null ? "NULL" : "NOT NULL");
            try
            {

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                //OracleConnection conn222 = new OracleConnection(Connstr2);
                //conn222.Open();
                //OracleTransaction trans222 = conn222.BeginTransaction();
                //try
                //{
                //    OracleCommand cmd221 = new OracleCommand(sql21, conn222);
                //    cmd221.ExecuteNonQuery();
                //    trans222.Commit();
                //}
                //catch (Exception)
                //{
                //    trans222.Rollback();
                //    conn222.Close();
                //    throw;
                //}
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.StackTrace); ;
            }

            //            string sql3 = @"
            //UPDATE OR_TEST
            //SET AGE = :AGE
            //WHERE ID=:ID
            //";
            string sql3 = @"
INSERT INTO OR_TEST(ID,NAME)
VALUES(SEQ_OR_TEST_ID.NEXTVAL,:NAME)
";

            ///事务
            OracleConnection CONB = new OracleConnection(Connstr);
            CONB.Open();
            int[] arrAGE = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 21 };
            int[] arrID = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12 };

            string[] arrName = new string[] { "a", "s", "d", "f", "g", "h", "j", "k" };

            //OracleParameter age = new OracleParameter() { ParameterName = "AGE", DbType= System.Data.DbType.Int16, Value=arrAGE };
            OracleParameter id = new OracleParameter() { ParameterName = "NAME", DbType = System.Data.DbType.String, Value = arrName };
            OracleCommand cmd3 = new OracleCommand(sql3, CONB);
            cmd3.ArrayBindCount = 11;
            //cmd3.Parameters.Add(age);
            cmd3.Parameters.Add(id);
            cmd3.ExecuteNonQuery();
            CONB.Close();



        }



        static void SQLNoQuery()
        {
            //            string sql = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
            //VALUES(SEQ_OR_TEST_ID.NEXTVAL,'威武{0}',19,0,10.08)", Guid.NewGuid().ToString());

            string sql = string.Format(@"INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) 
VALUES(SEQ_OR_TEST_ID.NEXTVAL,'威武{0}',1900,0,10.08)", "1");
            try
            {
                DisplayLayOut(OracleHelper.ExecuteSqlNonQuery(Connstr, sql).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void SQLDataReader()
        {
            string sql = "SELECT * FROM OR_TEST";
            DbDataReader reader = OracleHelper.ExecuteSqlReader(Connstr, sql, null);
            using (reader)
            {
                while (reader.Read())
                {
                    DisplayLayOut(reader[0].ToString());
                }
            }
        }

        static void SQLDataSet()
        {
            string sql = "SELECT * FROM TMS_PREDISPATCH";
            //DataSet ds = OracleHelper.ExecuteSqlDataset(Connstr, sql, null);
            OracleConnection conn = new OracleConnection(Connstr);
            conn.Open();
            DataSet ds = OracleHelper.ExecuteSqlDataset(conn, sql, null);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DisplayLayOut(item[1].ToString());
            }
            Console.WriteLine("conn is open ? {0}", conn.State == ConnectionState.Open ? "true" : "false");
            if (conn.State == ConnectionState.Open)
            {
                Console.WriteLine("close it.");
                conn.Close();
            }
        }

        static void SQLScalar()
        {
            string sql = "SELECT COUNT(*) AS CT FROM OR_TEST";
            DisplayLayOut(OracleHelper.ExecuteSqlScalar(Connstr, sql, null).ToString());
        }

        static void MulSQL()
        {
            string sql = @"
            BEGIN 
                INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) VALUES(SEQ_OR_TEST_ID.NEXTVAL,'AB威武1234565354353535437',19,0,10.08);
                INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) VALUES(SEQ_OR_TEST_ID.NEXTVAL,'AB威武53535355335353535',19,0,10.08); 
            END;";

            OracleHelper.ExecuteSqlNonQuery(Connstr, sql);
        }

        static void TMSWCFTest()
        {
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                TMSEntranceModel entranceModel = new TMSEntranceModel()
                {
                    Arrival = "837",//"145",
                    BatchNo = DateTime.Now.ToString("yyyyMMddHHmmssms"),
                    ContentType = Enums.GoodsType.Normal,
                    Departure = "2", //"0101",
                    Source = Enums.TMSEntranceSource.VANCL,
                    Detail = new System.Collections.Generic.List<BillDetailModel>(),
                    Weight = 2347.4M,
                    TotalCount = 50,
                    TotalAmount = 0
                }; // TODO: 初始化为适当的值

                Random rm = new Random(40550000);
                for (int i = 0; i < 50; i++)
                {
                    entranceModel.Detail.Add(new BillDetailModel()
                    {
                        BillType = Enums.BillType.Exchange,
                        FormCode = rm.Next(100000000).ToString(),
                        Price = i * 2,
                        GoodsType = Enums.GoodsType.Contraband,
                        CustomerOrder = rm.Next(100000000).ToString()
                    });
                }

                ResultModel actual;
                actual = proxy.DataEntrance(entranceModel);
                Console.WriteLine(actual.Message);

                //var argument = new InboundSimpleArgModel()
                // {
                //     FormCode = "11208230000046",
                //     FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Waybill,
                //     OpUser = proxy.GetUserModel(1),
                //     PreCondition = proxy.GetPreCondition("rfd"),
                //     ToStation = proxy.GetToStationModel(9)
                // };
                ////11208230000104
                //var result = proxy.SimpleInbound(argument);
                //Console.WriteLine(result.IsSuccess ? String.Format(@"入库成功,当前入库单数:{0}", result.InboundCount) : result.Message);
                //Console.WriteLine(proxy.GetInboundCount(argument).ToString());
            }
        }


        static void SQLTest()
        {
            //SQLDataSet();

            //TMSWCFTest();

            //SQLNoQuery();
            //MulSQL();

            //SQLArrayQuery();
            //try
            //{
            //    int i = 10;
            //    int j = 0;

            //    Console.WriteLine(i / j);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //    throw;
            //}
            //finally
            //{
            //    Console.WriteLine("finally");
            //}

            //SQLUserTransactionNoQuery();


            //SQLUserTransactionNoQuery();
            // SQLTransactionNoQuery();
            //SQLNoQuery();
            //SQLDataReader();
            //SQLDataSet();
            //SQLScalar();

            //            //批量操作方案1：
            //            string sql = @"
            //BEGIN 
            //    INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) VALUES(SEQ_OR_TEST_ID.NEXTVAL,'B威武1234567',19,0,10.08);
            //    INSERT INTO OR_TEST(ID,NAME,AGE,ISDELETED,PRICE) VALUES(SEQ_OR_TEST_ID.NEXTVAL,'B威武1234567890',19,0,10.08); 
            //END;";
            //            //后续添加批量其他方案
        }

        private static void StringTest()
        {
            string strOrg = "ABC";
            string STRNew = strOrg;
            Console.WriteLine(STRNew);
            strOrg = "CDE";
            Console.WriteLine(STRNew);
            Console.WriteLine(strOrg);
        }

        private static void ObjectTest()
        {
            object Org = new object();
            object New = Org;
            Console.WriteLine(Org.ToString());
            Org = null;
            Console.WriteLine(New == null ? "true" : "false");

        }
        private static void DecimalTest()
        {
            decimal Value = 18.125M;
            Console.WriteLine(Value.ToString("N4"));
            Console.WriteLine(Value.ToString("N3"));
            Console.WriteLine(Value.ToString("N2"));
            Console.WriteLine(Value.ToString("N1"));
            decimal NewValue = decimal.Parse(Value.ToString("N1"));
            Console.WriteLine(NewValue);
            decimal PValue = NewValue - decimal.Truncate(NewValue);
            Console.WriteLine(PValue);

            Console.WriteLine(PValue < 0.3M ? "0" : "0.5");

        }

        static void ConnectionPool()
        {
            try
            {
                Thread.Sleep(1000 * 60);
                SqlConnection conn1 = new SqlConnection(Connstr2);
                conn1.Open();
                SqlConnection conn2 = new SqlConnection(Connstr2);
                conn2.Open();
                SqlConnection conn3 = new SqlConnection(Connstr2);
                conn3.Open();
                SqlConnection conn4 = new SqlConnection(Connstr2);
                conn4.Open();
                SqlConnection conn5 = new SqlConnection(Connstr2);
                conn5.Open();
                Console.WriteLine("OPEN ALL....");
                for (int i = 1; i < 6; i++)
                {
                    switch (i)
                    {
                        case 1:
                            conn1.Close();
                            Console.WriteLine("close 1");
                            break;
                        case 2:
                            conn2.Close();
                            Console.WriteLine("close 2");
                            break;
                        case 3:
                            conn3.Close();
                            Console.WriteLine("close 3");
                            break;
                        case 4:
                            conn4.Close();
                            Console.WriteLine("close 4");
                            break;
                        case 5:
                            conn5.Close();
                            Console.WriteLine("close 5");
                            break;
                        default:
                            break;
                    }
                    Thread.Sleep(1000 * 60);
                    Console.WriteLine("sleep 1 minute");
                }


                Thread.Sleep(1000 * 60 * 3);
                Console.WriteLine("sleep 3 minute");
            }
            catch (Exception)
            {

                throw;
            }
        }

        static void RefTest()
        {
            Stopwatch sp1 = new Stopwatch();
            sp1.Start();
            int count = 0;
            List<ViewComplexReport> listReport = new List<ViewComplexReport>(10000);
            while (count < 10000)
            {
                listReport.Add(new ViewComplexReport()
                {
                    ArrivalCity = "北京",
                    ArrivalName = "四川新城",
                    ArrivalTiming = 10,
                    BoxCount = 1000,
                    CarrierName = "fdfdfdfdfd比防打滑i好的",
                    ConfirmExpArrivalDate = DateTime.Now,
                    CustWaybillNo = "dsfsefsdfnifdhsihi",
                    DelayReason = "fdfdfdfd",
                    Delaytimespan = 10,
                    DelayType = Enums.DelayType.ExpressUnusual,
                    DeliveryNo = "fdfdfdfds09876543",
                    DeliveryStatus = Enums.ComplexReportDeliveryStatus.CannotRecognize,
                    DepartureName = "fdsfjisdjfisduhfisdhfisdhfisdh",
                    DepartureTime = DateTime.Now,
                    DesReceiveDate = DateTime.Now,
                    Goodstype = Enums.GoodsType.Contraband | Enums.GoodsType.Frangible | Enums.GoodsType.Normal,
                    IsDelay = true,
                    LostType = Enums.LostType.DamagedRejection,
                    OrderCount = 98765,
                    SignedUser = "0987654jhgf",
                    SortRowID = 1000,
                    TotalAmount = 87654,
                    TotalWeight = 876543,
                    TransportType = Enums.TransportType.Aviation
                });
                count++;
            }
            sp1.Stop();
            Console.WriteLine("ADD List 时间: {0} s", sp1.ElapsedMilliseconds / 1000);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            var test = listReport.To2Array();
            sp.Stop();
            Console.WriteLine("反射二维数组时间:{0} s", sp.ElapsedMilliseconds / 1000);
        }

        static void entranceModel()
        {
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {

                TMSEntranceModel entranceModel = new TMSEntranceModel()
                {
                    Arrival = "432",
                    BatchNo = "201212076000001",
                    ContentType = Enums.GoodsType.Contraband,
                    Departure = "0101",
                    Source = Enums.TMSEntranceSource.VANCL,
                    Detail = new System.Collections.Generic.List<BillDetailModel>(),
                    Weight = 99.7M,
                    TotalCount = 777,
                    TotalAmount = 89888.0M
                }; // TODO: 初始化为适当的值

                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "1234556789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345256789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345356789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345456789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345556789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345656789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345576789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345567889", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345576789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345567889", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345576789", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });
                entranceModel.Detail.Add(new BillDetailModel() { BillType = Enums.BillType.Exchange, FormCode = "12345567889", Price = 99, GoodsType = Enums.GoodsType.Frangible, CustomerOrder = "FSDFDSFDS" });

                var result = proxy.DataEntrance(entranceModel);
                Console.WriteLine(result.Message);
            }

        }

        static void StringComp()
        {
            List<String> listSource = new List<string>();
            listSource.Add("ST10001");
            listSource.Add("ST00001");
            listSource.Add("ST00012");
            listSource.Add("ST00100");
            listSource.Add("ST03001");
            listSource.Add("ST111111");
            var ORDERS = listSource.OrderByDescending(p => p);
            foreach (var item in ORDERS)
            {
                Console.WriteLine(item);
            }

        }

        static void Main(string[] args)
        {
            //StringComp();
            //TMSWCFTest();

            SQLDataReaderTest();
            //CharLengthTest();


            //SqlTransactionTest();
            //            IFormula<String, KeyCodeContextModel> tt = FormulasFactory.GetFormula<IFormula<String, KeyCodeContextModel>>("keycodeBLLFormula");
            //            Console.WriteLine(tt.Execute(new Vancl.TMS.Model.Common.KeyCodeContextModel()
            //            {
            //                TableCode="001",
            //                SequenceName = "seq_sc_inbound_ibid"
            //            })
            //);
            //            Console.WriteLine(tt.Execute(new Vancl.TMS.Model.Common.KeyCodeContextModel()
            //            {
            //                TableCode = "001",
            //                SequenceName = "seq_sc_inbound_ibid"
            //            })
            //); Console.WriteLine(tt.Execute(new Vancl.TMS.Model.Common.KeyCodeContextModel()
            //{
            //    TableCode = "001",
            //    SequenceName = "seq_sc_inbound_ibid"
            //})
            //);

            //RefTest();

            //ConnectionPool();


            //DecimalTest();

            //StringTest();
            //ObjectTest();
            //SQLTest();
            //int count = 0;
            //while (count < 10000)
            //{
            //    Vancl.TMS.BLL.Transport.Plan.TransportPlanBLL bll = new Vancl.TMS.BLL.Transport.Plan.TransportPlanBLL();
            //    using (IACID scope = ACIDScopeFactory.GetACID())
            //    {
            //        var model = bll.Get(1);
            //        ///
            //        scope.Complete();
            //    }
            //    count++;
            //    Thread.Sleep(6);
            //    if (count % 100 == 0)
            //    {
            //        Console.WriteLine("第{0}个100",count / 100);
            //    }
            //    //bll.UpdateNeedEffectived();
            //}

            //IFormula<decimal, AddContext> formula = FormulasFactory.GetFormula<IFormula<decimal,AddContext>>("AddFormula");
            //ITest<decimal, AddContext> service = ServiceFactory.GetService<ITest<decimal, AddContext>>();

            //service.OutputString(new TestModel() { Name = "123" });

            //Dictionary<int, AddContext> d = new Dictionary<int, AddContext>();

            //Console.WriteLine(d.GetType().FullName);
            //for (int i = 0; i < 20; i++)
            //{
            //    ThreadStart ts = new ThreadStart(DoWork);
            //    Thread th = new Thread(ts);
            //    th.Start();
            //    Thread.Sleep(40);
            //}
            //Console.Read();

            //Console.WriteLine("输入第一个参数:");
            //AddContext context = new AddContext();
            //context.First = decimal.Parse(Console.ReadLine());
            //Console.WriteLine("输入第一个参数:");
            //context.Secend = decimal.Parse(Console.ReadLine());
            //Console.WriteLine("结果:");
            //Console.WriteLine(formula.Execute(context));

            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();
            //Console.WriteLine(SerialNumFactory.GenerateSerialNum(Vancl.TMS.Model.Common.Enums.SerialNumberType.LineCode));
            //Console.ReadKey();

            //SQLTest();



            //Console.ReadKey();


            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            //Console.ReadKey();
            //object o = Activator.CreateInstance(typeof(Vancl.TMS.Core.FormulaManager.AddFormula), true);
            //Console.WriteLine(((Vancl.TMS.Core.FormulaManager.IFormula<decimal, AddContext>)o).Execute(null));
            Console.ReadKey();
        }

        static void DoWork()
        {
            using (VanclObjectInPool<Socket> vs = SocketPool.Get(Thread.CurrentThread.ManagedThreadId))
            {
                //if (vs != null)
                //{
                //    vs.Open();
                //}
                try
                {
                    Thread.Sleep(3000);
                }
                catch (ThreadInterruptedException ex)
                {
                }
            }
        }

        public interface IA
        {
            void MA();
        }

        public class A : IA
        {

            #region IA 成员

            public void MA()
            {
                Console.WriteLine("123");
            }

            #endregion
        }

        static void SqlTransactionTest()
        {
            try
            {
                Lms2TmsSyncLMSDAL dal = new Lms2TmsSyncLMSDAL();

                for (int i = 0; i < 10000; i++)
                {
                    using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                    {
                        //   dal.test1();
                        //    dal.test2();
                        scope.Complete();
                    }
                    if (i % 100 == 0)
                    {
                        Console.WriteLine("now count:" + i);
                    }
                    Thread.Sleep(10);
                }


                //SqlConnection con = new SqlConnection();
                //con.ConnectionString = ConfigurationHelper.GetConnectionString(Consts.LMS_RFD_WRITE_CONNECTION_KEY);
                //SqlCommand com1 = new SqlCommand();
                //com1.Connection = con;
                //com1.CommandType = CommandType.Text;
                //com1.CommandText = @"insert into test1(id,name) values (1,'a')";
                //con.Open();
                //SqlTransaction trans = con.BeginTransaction();
                //com1.Transaction = trans;
                //com1.ExecuteNonQuery();
                //SqlCommand com2 = new SqlCommand();
                //com2.Connection = con;
                //com2.CommandType = CommandType.Text;
                //com2.CommandText = @"insert into test2(name) values ('b')";
                //com2.Transaction = trans;
                //com2.ExecuteNonQuery();
                //trans.Commit();
                //con.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
