using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace AmscoMncrData.Controllers
{


    [EnableCorsAttribute(origins: "http://192.168.1.6:5500,http://192.168.1.6:80", headers: "*", methods: "*")]

    public class AmscoMcnrController : ApiController
    {


        // GET call  for total IOC AMSCO

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //  public string GetData(DateTime startDate, DateTime endDate)
        public string GetData(string Month)
        {
           
            string jsonMessage = "";
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_IOCScanned", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth = new SqlParameter();
            pMonth.ParameterName = "@MONTH_IP";
            // pStartDate.DbType = DbType.DateTime;
            pMonth.Direction = ParameterDirection.Input;
            pMonth.Value = Month;

            //SqlParameter pEndDate = new SqlParameter();
            //pEndDate.ParameterName = "@END_DATE";
            //pEndDate.DbType = DbType.DateTime;
            //pEndDate.Direction = ParameterDirection.Input;
            //pEndDate.Value = endDate;

            cmd.Parameters.Add(pMonth);
            //cmd.Parameters.Add(pEndDate);

            //SqlParameter final_output = cmd.Parameters.Add("@FINAL_OUTPUT", SqlDbType.VarChar, 100000);
            //final_output.Direction = ParameterDirection.Output;

            try
            {
                conn.Open();
                jsonMessage = cmd.ExecuteScalar().ToString();
                jsonMessage = jsonMessage.Replace("\"", "");
                conn.Close();
            }
            catch (Exception ex)
            {

                conn.Close();
            }

            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return jsonMessage;

        }







        //Get Call For AMSCO IOC submitted to vendor

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, Dictionary<string, int>> GetIOCSubmitted(string Month2)
        {
            var MalwareList = new Dictionary<string, Dictionary<string, int>>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";
            
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_IOCSubmitted", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth2 = new SqlParameter();
            pMonth2.ParameterName = "@MONTH_IP1";
            // pStartDate.DbType = DbType.DateTime;
            pMonth2.Direction = ParameterDirection.Input;
            pMonth2.Value = Month2;
            cmd.Parameters.Add(pMonth2);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                //  List<Models.TEST> TestList = new List<Models.TEST>();

                //var MalwareList = new Dictionary<string, Dictionary<string, int>>();



                while (reader.Read())
                {

                    var malware = reader["MALWARE_NAME"].ToString();
                    var vendor = reader["VENDORE_NAME"].ToString();
                    var count = Int32.Parse(reader["CountOfIOC"].ToString());

                    if (MalwareList.ContainsKey(malware))
                    {
                        var value = MalwareList[malware];
                        value.Add(vendor, count);
                        MalwareList[malware] = value;
                    }
                    else
                    {
                        var value = new Dictionary<string, int>();
                        value.Add(vendor, count);
                        MalwareList.Add(malware, value);
                    }
                }

                connection.Close();

            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return MalwareList;
            


        }







        //Get Call For AMSCO Signature Released vendor

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, Dictionary<string, int>> GetSignatureReleased(string Month3)
        {
            var SigList = new Dictionary<string, Dictionary<string, int>>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_SignatureReleased", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth3 = new SqlParameter();
            pMonth3.ParameterName = "@MONTH_IP3";
            // pStartDate.DbType = DbType.DateTime;
            pMonth3.Direction = ParameterDirection.Input;
            pMonth3.Value = Month3;
            cmd.Parameters.Add(pMonth3);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                //  List<Models.TEST> TestList = new List<Models.TEST>();

                // var SigList = new Dictionary<string, Dictionary<string, int>>();



                while (reader.Read())
                {

                    var malwareName = reader["SIG_MALWARE_NAME"].ToString();
                    var vendorName = reader["SIG_VENDORE_NAME"].ToString();
                    var SigStatus = Int32.Parse(reader["SIG_SIGNATURE_RELEASED_BY_VENDOR"].ToString());

                    if (SigList.ContainsKey(malwareName))
                    {
                        var value = SigList[malwareName];
                        value.Add(vendorName, SigStatus);
                        SigList[malwareName] = value;
                    }
                    else
                    {
                        var value = new Dictionary<string, int>();
                        value.Add(vendorName, SigStatus);
                        SigList.Add(malwareName, value);
                    }
                }

            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return SigList;



        }









        // Get call for MCNR TOTAL IOCs 

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        //  public string GetData(DateTime startDate, DateTime endDate)
        public string GetDataMCNR(string Month4)
        {
   
            string jsonMessage4 = "";
            string connString4 =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection conn4 = new SqlConnection(connString4);
            SqlCommand cmd4 = new SqlCommand("dbo.sp_McnrIOCScanned", conn4);
            cmd4.CommandType = CommandType.StoredProcedure;
            cmd4.Parameters.Clear();

            SqlParameter pMonth4 = new SqlParameter();
            pMonth4.ParameterName = "@MONTH_IP_MCNR";
            // pStartDate.DbType = DbType.DateTime;
            pMonth4.Direction = ParameterDirection.Input;
            pMonth4.Value = Month4;

            //SqlParameter pEndDate = new SqlParameter();
            //pEndDate.ParameterName = "@END_DATE";
            //pEndDate.DbType = DbType.DateTime;
            //pEndDate.Direction = ParameterDirection.Input;
            //pEndDate.Value = endDate;

            cmd4.Parameters.Add(pMonth4);
            //cmd.Parameters.Add(pEndDate);

            //SqlParameter final_output = cmd.Parameters.Add("@FINAL_OUTPUT", SqlDbType.VarChar, 100000);
            //final_output.Direction = ParameterDirection.Output;

            try
            {
                conn4.Open();
                jsonMessage4 = cmd4.ExecuteScalar().ToString();
                jsonMessage4 = jsonMessage4.Replace("\"", "");
                conn4.Close();
            }
            catch (Exception ex4)
            {

                conn4.Close();
            }

            finally
            {
                conn4.Close();
                conn4.Dispose();
            }

            return jsonMessage4;

        }








        //GET Call for MCNR IOC submitted to vendor


        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, Dictionary<string, int>> GetMcnrIOCSubmitted(string Month5)
        {
            var McnrMalwareList = new Dictionary<string, Dictionary<string, int>>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_MCNRIOCSubmitted", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth5 = new SqlParameter();
            pMonth5.ParameterName = "@MONTH_IP2";
            // pStartDate.DbType = DbType.DateTime;
            pMonth5.Direction = ParameterDirection.Input;
            pMonth5.Value = Month5;
            cmd.Parameters.Add(pMonth5);


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                //  List<Models.TEST> TestList = new List<Models.TEST>();

                //var McnrMalwareList = new Dictionary<string, Dictionary<string, int>>();



                while (reader.Read())
                {

                    var Mcnrmalware = reader["MCNR_MALWARE_NAME"].ToString();
                    var Mcnrvendor = reader["MCNR_VENDORE_NAME"].ToString();
                    var Mcnrcount = Int32.Parse(reader["McnrCountOfIOC"].ToString());

                    if (McnrMalwareList.ContainsKey(Mcnrmalware))
                    {
                        var value = McnrMalwareList[Mcnrmalware];
                        value.Add(Mcnrvendor, Mcnrcount);
                        McnrMalwareList[Mcnrmalware] = value;
                    }
                    else
                    {
                        var value = new Dictionary<string, int>();
                        value.Add(Mcnrvendor, Mcnrcount);
                        McnrMalwareList.Add(Mcnrmalware, value);
                    }
                }
            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }


            return McnrMalwareList;



        }









        //Get Call For MCNR Signature Released vendor

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, Dictionary<string, int>> GetMcnrSignatureReleased(string Month6)
        {
            
            var McnrSigList = new Dictionary<string, Dictionary<string, int>>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_McnrSigantureReleased", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth6 = new SqlParameter();
            pMonth6.ParameterName = "@MONTH_IP4";
            // pStartDate.DbType = DbType.DateTime;
            pMonth6.Direction = ParameterDirection.Input;
            pMonth6.Value = Month6;
            cmd.Parameters.Add(pMonth6);

            try
            {

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                //  List<Models.TEST> TestList = new List<Models.TEST>();

               // var McnrSigList = new Dictionary<string, Dictionary<string, int>>();



                while (reader.Read())
                {
                    var McnrmalwareName = reader["MCNR_SIG_MALWARE_NAME"].ToString();
                    var McnrvendorName = reader["MCNR_SIG_VENDORE_NAME"].ToString();
                    var McnrSigStatus = Int32.Parse(reader["MCNR_SIG_SIGNATURE_RELEASED_BY_VENDOR"].ToString());

                    if ((McnrSigList.ContainsKey(McnrmalwareName)))
                    {
                        var value = McnrSigList[McnrmalwareName];
                        value.Add(McnrvendorName, McnrSigStatus);
                        McnrSigList[McnrmalwareName] = value;
                    }
                    else
                    {
                        var value = new Dictionary<string, int>();
                        value.Add(McnrvendorName, McnrSigStatus);
                        McnrSigList.Add(McnrmalwareName, value);
                    }
                }
            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }



            return McnrSigList;



        }












        //Get Call For ASR

        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, Dictionary<string, int>> GetAsrData(string Month7)
        {

            var ManagerList = new Dictionary<string, Dictionary<string, int>>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_AsrData", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth7 = new SqlParameter();
            pMonth7.ParameterName = "@MONTH_IP5";
            // pStartDate.DbType = DbType.DateTime;
            pMonth7.Direction = ParameterDirection.Input;
            pMonth7.Value = Month7;
            cmd.Parameters.Add(pMonth7);

            try
            {

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                //  List<Models.TEST> TestList = new List<Models.TEST>();

                //var ManagerList = new Dictionary<string, Dictionary<string, int>>();



                while (reader.Read())
                {

                    var ManagerName = reader["MANAGER_NAME"].ToString();
                    var AsrCopiesAndFeedback = reader["ASR_COPIES_AND_FEEDBACK"].ToString();
                    var Copies = Int32.Parse(reader["CountOfCopies"].ToString());




                    if (ManagerList.ContainsKey(ManagerName))
                    {
                        var value = ManagerList[ManagerName];
                        value.Add(AsrCopiesAndFeedback, Copies);
                        ManagerList[ManagerName] = value;
                    }
                    else
                    {
                        var value = new Dictionary<string, int>();
                        value.Add(AsrCopiesAndFeedback, Copies);
                        ManagerList.Add(ManagerName, value);
                    }
                }
            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }


            return ManagerList;



        }










        // Get call for Achievement



        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        public Dictionary<string, string> GetAchievement(string Month8)
        {

          //  var AchievementList = new Dictionary<string, string>();
            Dictionary<string, string> Achievelist = new Dictionary<string, string>();
            string connString =

               "Server = XMD-LAB\\XMD; Database = TEST2; " +
               "User ID = sa; Password=P@ssw0rd";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("dbo.sp_Achievement", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter pMonth8 = new SqlParameter();
            pMonth8.ParameterName = "@MONTH_IP8";
            // pStartDate.DbType = DbType.DateTime;
            pMonth8.Direction = ParameterDirection.Input;
            pMonth8.Value = Month8;
            cmd.Parameters.Add(pMonth8);

            try
            {

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();



                while (reader.Read())
                {

                    var Achievement = reader["ACHIEVEMENT"].ToString();
                    var Achievement1 = reader["ACHIEVEMENT1"].ToString();


                    Achievelist.Add(Achievement, Achievement1);
                 
                }
                reader.Close();
            }

            catch (Exception ex)
            {

                connection.Close();
            }

            finally
            {
                connection.Close();
                connection.Dispose();
            }


            return Achievelist;



        }

















        //Post call for AMSCO data


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        // [System.Web.Http.Route("[action]")]
        [System.Web.Http.Route("api/AmscoMcnr/PostAMSCO")]

        public HttpResponseMessage PostAMSCO([FromBody] object inboundData)
        {
            File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside Post Call \r\n");
            File.AppendAllText(@"C:\AMSCOLogs.txt", inboundData.ToString() + "\r\n");
            string malwareName = "";
            string operationName = "";
            string dateReceived;
            List<string> IOCS_SCANNED;
            List<string> IOCS_SUB_TO_SYMANTEC;
            List<string> IOCS_SUB_TO_MCAFEE;
            List<string> IOCS_SUB_TO_TRENDMICRO;
            List<string> IOCS_SUB_TO_MICROSOFT;

            string signatureBySymantec = "";
            string signatureByMcafee = "";
            string signatureByTrendmicro = "";
            string signatureByMicrosoft = "";

            int requestId = 0;


            string connString =

                "Server = XMD-LAB\\XMD; Database = TEST2; " +
                "User ID = sa; Password=P@ssw0rd";
            SqlTransaction tran = null;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = null;


            try
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside Try block \r\n");
                // TODO - check if input data is in valid json format
                JObject jInboundData = JObject.Parse(inboundData.ToString());

                malwareName = jInboundData["Malware_Name"].ToString();
                operationName = jInboundData["Operation_Name"].ToString();
                dateReceived = jInboundData["Date"].ToString();
                string[] formats = new string[] { "MM/dd/yyyy" };
                DateTime dateFormatted = DateTime.ParseExact(dateReceived, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
                string dateToDatabase = dateFormatted.ToString("yyyy-MM-dd");
                IOCS_SCANNED = jInboundData["IOC_Scanned"].ToString().Split(',').ToList();
                IOCS_SUB_TO_SYMANTEC = jInboundData["IOCs_Submitted_to_Symantec"].ToString().Split(',').ToList();
                IOCS_SUB_TO_MCAFEE = jInboundData["IOCs_Submitted_to_McAfee"].ToString().Split(',').ToList();
                IOCS_SUB_TO_TRENDMICRO = jInboundData["IOCs_Submitted_to_TrendMicro"].ToString().Split(',').ToList();
                IOCS_SUB_TO_MICROSOFT = jInboundData["IOCs_Submitted_to_Microsoft"].ToString().Split(',').ToList();

                signatureBySymantec = jInboundData["Signature_Released_By_Symantec"].ToString();
                signatureByMcafee = jInboundData["Signature_Released_By_McAfee"].ToString();
                signatureByTrendmicro = jInboundData["Signature_Released_By_TrendMicro"].ToString();
                signatureByMicrosoft = jInboundData["Signature_Released_By_Microsoft"].ToString();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "data recived in jInbounData \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", jInboundData.ToString() + "\r\n");


                conn.Open();
                tran = conn.BeginTransaction("OnlyTransaction");

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Transaction Begin, Connection opened..\r\n");

                tran.Save("OnlySave");


                string selectRequestId =
                    "SELECT " +
                    "   CASE WHEN ISNULL(MAX(Request_ID),'') = ''  THEN 0 " +
                    "   ELSE MAX(Request_ID)" +
                    "   END " +
                    "FROM AMSCO_MCNR_DATA";

                cmd = new SqlCommand(selectRequestId, conn, tran);

                requestId = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Request id fetched in Post call \r\n");

                string insertMcnrData =
                    "INSERT INTO dbo.AMSCO_MCNR_DATA" +
                    "(" +
                        "REQUEST_ID," +
                        "MALWARE_NAME," +
                        "OPERATION_NAME," +
                        "DATE_RECEIVED," +
                        "SIGNATURE_RELEASED_BY_SYMANTEC," +
                        "SIGNATURE_RELEASED_BY_MCAFEE," +
                        "SIGNATURE_RELEASED_BY_TRENDMICRO," +
                        "SIGNATURE_RELEASED_BY_MICROSOFT" +
                    ")" +
                    "VALUES" +
                    "(" +
                        requestId + ",'" +
                        malwareName + "','" +
                        operationName + "','" +
                        dateToDatabase + "','" +
                        signatureBySymantec + "','" +
                        signatureByMcafee + "','" +
                        signatureByTrendmicro + "','" +
                        signatureByMicrosoft +
                    "')";
                cmd = new SqlCommand(insertMcnrData, conn, tran);
                cmd.ExecuteNonQuery();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into AMSCO_MCNR_DATA table \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertMcnrData.ToString() + "\r\n");

                string insertIOCdata = "";

                foreach (string IOC_SCANNED in IOCS_SCANNED)
                {
                    insertIOCdata =
                        "INSERT INTO dbo.IOC_SCANNED" +
                        "(" +
                            "REQUEST_ID," +
                            "IOC_SCANNED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId + ", '" +
                            IOC_SCANNED +
                        "')";

                    cmd = new SqlCommand(insertIOCdata, conn, tran);
                    cmd.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into IOC_SCANNED table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata.ToString() + "\r\n");

                foreach (string IOC_SUB_TO_SYMANTEC in IOCS_SUB_TO_SYMANTEC)
                {
                    insertIOCdata =
                        "INSERT INTO dbo.IOC_SUB_TO_SYMANTEC" +
                        "(" +
                            "REQUEST_ID," +
                            "IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId + ", '" +
                            IOC_SUB_TO_SYMANTEC +
                        "')";

                    cmd = new SqlCommand(insertIOCdata, conn, tran);
                    cmd.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into IOC_SUB_TO_SYMANTEC table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata.ToString() + "\r\n");

                foreach (string IOC_SUB_TO_MCAFEE in IOCS_SUB_TO_MCAFEE)
                {
                    insertIOCdata =
                        "INSERT INTO dbo.IOC_SUB_TO_MCAFEE" +
                        "(" +
                            "REQUEST_ID," +
                            "IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId + ", '" +
                            IOC_SUB_TO_MCAFEE +
                        "')";

                    cmd = new SqlCommand(insertIOCdata, conn, tran);
                    cmd.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into IOC_SUBMITTED_TO_MCAFEE table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata.ToString() + "\r\n");

                foreach (string IOC_SUB_TO_TRENDMICRO in IOCS_SUB_TO_TRENDMICRO)
                {
                    insertIOCdata =
                        "INSERT INTO dbo.IOC_SUB_TO_TRENDMICRO" +
                        "(" +
                            "REQUEST_ID," +
                            "IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId + ", '" +
                            IOC_SUB_TO_TRENDMICRO +
                        "')";

                    cmd = new SqlCommand(insertIOCdata, conn, tran);
                    cmd.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into IOC_SUB_TO_TRENDMICRO table table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata.ToString() + "\r\n");

                foreach (string IOC_SUB_TO_MICROSOFT in IOCS_SUB_TO_MICROSOFT)
                {
                    insertIOCdata =
                        "INSERT INTO dbo.IOC_SUB_TO_MICROSOFT" +
                        "(" +
                            "REQUEST_ID," +
                            "IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId + ", '" +
                            IOC_SUB_TO_MICROSOFT +
                        "')";

                    cmd = new SqlCommand(insertIOCdata, conn, tran);
                    cmd.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into IOC_SUB_TO_MICROSOFT\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata.ToString() + "\r\n");

                tran.Commit();

                File.AppendAllText(@"C:\AMSCOLogs.txt", " Transaction commited \r\n");

                var message = Request.CreateResponse(HttpStatusCode.Created, "DATA INSERTED INTO DATABASE SUCCESSFULY");
                message.Headers.Location = new Uri(Request.RequestUri + "");
                return message;
            }

            catch (Exception ex)
            {
                File.WriteAllText(@"C:\AMSCOLogs.txt", "Inside catch" + "\r\n");
                if (tran != null)
                {
                    tran.Rollback("OnlyTransaction");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
            finally
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside Finally\r\n");
                conn.Close();
                conn.Dispose();
            }
        }









        // Post call for MCNR

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
       // [System.Web.Http.Route("[action]")]
        [System.Web.Http.Route("api/AmscoMcnr/PostMCNRData")]

        public HttpResponseMessage PostMCNRData([FromBody] object inboundData1)
        {
            File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside MCNR Post Call \r\n");
            File.AppendAllText(@"C:\AMSCOLogs.txt", inboundData1.ToString() + "\r\n");
            string malwareName1 = "";
            string operationName1 = "";
            string dateReceived1;
            List<string> MCNR_IOCS_SCANNED;
            List<string> MCNR_IOCS_SUB_TO_SYMANTEC;
            List<string> MCNR_IOCS_SUB_TO_MCAFEE;
            List<string> MCNR_IOCS_SUB_TO_TRENDMICRO;
            List<string> MCNR_IOCS_SUB_TO_MICROSOFT;

            string signatureBySymantec1 = "";
            string signatureByMcafee1 = "";
            string signatureByTrendmicro1 = "";
            string signatureByMicrosoft1 = "";
            string mcnrDescription = "";

            int requestId1 = 0;


            string connStringMcnr =

                "Server = XMD-LAB\\XMD; Database = TEST2; " +
                "User ID = sa; Password=P@ssw0rd";
            SqlTransaction tranMcnr = null;
            SqlConnection connMcnr = new SqlConnection(connStringMcnr);
            SqlCommand cmdMcnr = null;


            try
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside MCNR POST call's Try block \r\n");
                // TODO - check if input data is in valid json format
                JObject jInboundData1 = JObject.Parse(inboundData1.ToString());

                malwareName1 = jInboundData1["Malware_Name"].ToString();
                operationName1 = jInboundData1["Operation_Name"].ToString();
                dateReceived1 = jInboundData1["Date"].ToString();
                string[] formatsMcnr = new string[] { "MM/dd/yyyy" };
                DateTime dateFormatted1 = DateTime.ParseExact(dateReceived1, formatsMcnr, CultureInfo.InvariantCulture, DateTimeStyles.None);
                string dateToDatabase1 = dateFormatted1.ToString("yyyy-MM-dd");
                MCNR_IOCS_SCANNED = jInboundData1["IOC_Scanned"].ToString().Split(',').ToList();
                MCNR_IOCS_SUB_TO_SYMANTEC = jInboundData1["IOCs_Submitted_to_Symantec"].ToString().Split(',').ToList();
                MCNR_IOCS_SUB_TO_MCAFEE = jInboundData1["IOCs_Submitted_to_McAfee"].ToString().Split(',').ToList();
                MCNR_IOCS_SUB_TO_TRENDMICRO = jInboundData1["IOCs_Submitted_to_TrendMicro"].ToString().Split(',').ToList();
                MCNR_IOCS_SUB_TO_MICROSOFT = jInboundData1["IOCs_Submitted_to_Microsoft"].ToString().Split(',').ToList();

                signatureBySymantec1 = jInboundData1["Signature_Released_By_Symantec"].ToString();
                signatureByMcafee1 = jInboundData1["Signature_Released_By_McAfee"].ToString();
                signatureByTrendmicro1 = jInboundData1["Signature_Released_By_TrendMicro"].ToString();
                signatureByMicrosoft1 = jInboundData1["Signature_Released_By_Microsoft"].ToString();
                mcnrDescription = jInboundData1["Description"].ToString();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "data recived in jInbounData \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", jInboundData1.ToString() + "\r\n");


                connMcnr.Open();
                tranMcnr = connMcnr.BeginTransaction("OnlyTransaction");

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Transaction Begin, Connection opened..\r\n");

                tranMcnr.Save("OnlySave");


                string selectRequestId1 =
                    "SELECT " +
                    "   CASE WHEN ISNULL(MAX(Request_ID),'') = ''  THEN 0 " +
                    "   ELSE MAX(Request_ID)" +
                    "   END " +
                    "FROM MCNR_DATA";

                cmdMcnr = new SqlCommand(selectRequestId1, connMcnr, tranMcnr);

                requestId1 = Convert.ToInt32(cmdMcnr.ExecuteScalar()) + 1;

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Request id fetched in MCNR Post call \r\n");

                string insertMcnrData1 =
                    "INSERT INTO dbo.MCNR_DATA" +
                    "(" +
                        "REQUEST_ID," +
                        "MCNR_MALWARE_NAME," +
                        "MCNR_OPERATION_NAME," +
                        "MCNR_DATE_RECEIVED," +
                        "MCNR_SIGNATURE_RELEASED_BY_SYMANTEC," +
                        "MCNR_SIGNATURE_RELEASED_BY_MCAFEE," +
                        "MCNR_SIGNATURE_RELEASED_BY_TRENDMICRO," +
                        "MCNR_SIGNATURE_RELEASED_BY_MICROSOFT," +
                        "MCNR_DESCRIPTION" +
                    ")" +
                    "VALUES" +
                    "(" +
                        requestId1 + ",'" +
                        malwareName1 + "','" +
                        operationName1 + "','" +
                        dateToDatabase1 + "','" +
                        signatureBySymantec1 + "','" +
                        signatureByMcafee1 + "','" +
                        signatureByTrendmicro1 + "','" +
                        signatureByMicrosoft1 + "','" +
                        mcnrDescription +
                    "')";
                cmdMcnr = new SqlCommand(insertMcnrData1, connMcnr, tranMcnr);
                cmdMcnr.ExecuteNonQuery();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_DATA table \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertMcnrData1.ToString() + "\r\n");

                string insertIOCdata1 = "";

                foreach (string MCNR_IOC_SCANNED in MCNR_IOCS_SCANNED)
                {
                    insertIOCdata1 =
                        "INSERT INTO dbo.MCNR_IOC_SCANNED" +
                        "(" +
                            "REQUEST_ID," +
                            "MCNR_IOC_SCANNED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId1 + ", '" +
                            MCNR_IOC_SCANNED +
                        "')";

                    cmdMcnr = new SqlCommand(insertIOCdata1, connMcnr, tranMcnr);
                    cmdMcnr.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_IOC_SCANNED table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata1.ToString() + "\r\n");

                foreach (string MCNR_IOC_SUB_TO_SYMANTEC in MCNR_IOCS_SUB_TO_SYMANTEC)
                {
                    insertIOCdata1 =
                        "INSERT INTO dbo.MCNR_IOC_SUB_TO_SYMANTEC" +
                        "(" +
                            "REQUEST_ID," +
                            "MCNR_IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId1 + ", '" +
                            MCNR_IOC_SUB_TO_SYMANTEC +
                        "')";

                    cmdMcnr = new SqlCommand(insertIOCdata1, connMcnr, tranMcnr);
                    cmdMcnr.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_IOC_SUB_TO_SYMANTEC table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata1.ToString() + "\r\n");

                foreach (string MCNR_IOC_SUB_TO_MCAFEE in MCNR_IOCS_SUB_TO_MCAFEE)
                {
                    insertIOCdata1 =
                        "INSERT INTO dbo.MCNR_IOC_SUB_TO_MCAFEE" +
                        "(" +
                            "REQUEST_ID," +
                            "MCNR_IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId1 + ", '" +
                            MCNR_IOC_SUB_TO_MCAFEE +
                        "')";

                    cmdMcnr = new SqlCommand(insertIOCdata1, connMcnr, tranMcnr);
                    cmdMcnr.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_IOC_SUBMITTED_TO_MCAFEE table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata1.ToString() + "\r\n");

                foreach (string MCNR_IOC_SUB_TO_TRENDMICRO in MCNR_IOCS_SUB_TO_TRENDMICRO)
                {
                    insertIOCdata1 =
                        "INSERT INTO dbo.MCNR_IOC_SUB_TO_TRENDMICRO" +
                        "(" +
                            "REQUEST_ID," +
                            "MCNR_IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId1 + ", '" +
                            MCNR_IOC_SUB_TO_TRENDMICRO +
                        "')";

                    cmdMcnr = new SqlCommand(insertIOCdata1, connMcnr, tranMcnr);
                    cmdMcnr.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_IOC_SUB_TO_TRENDMICRO table table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata1.ToString() + "\r\n");

                foreach (string MCNR_IOC_SUB_TO_MICROSOFT in MCNR_IOCS_SUB_TO_MICROSOFT)
                {
                    insertIOCdata1 =
                        "INSERT INTO dbo.MCNR_IOC_SUB_TO_MICROSOFT" +
                        "(" +
                            "REQUEST_ID," +
                            "MCNR_IOC_SUBMITTED" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId1 + ", '" +
                            MCNR_IOC_SUB_TO_MICROSOFT +
                        "')";

                    cmdMcnr = new SqlCommand(insertIOCdata1, connMcnr, tranMcnr);
                    cmdMcnr.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into MCNR_IOC_SUB_TO_MICROSOFT\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertIOCdata1.ToString() + "\r\n");

                tranMcnr.Commit();

                File.AppendAllText(@"C:\AMSCOLogs.txt", " Transaction commited \r\n");

                var message1 = Request.CreateResponse(HttpStatusCode.Created, "DATA INSERTED INTO MCNR TABLE SUCCESSFULY");
                message1.Headers.Location = new Uri(Request.RequestUri + "");
                return message1;
            }

            catch (Exception exM)
            {
                File.WriteAllText(@"C:\AMSCOLogs.txt", "Inside catch" + "\r\n");
                if (tranMcnr != null)
                {
                    tranMcnr.Rollback("OnlyTransaction");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exM);

            }
            finally
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside MCNR POST calls Finally\r\n");
                connMcnr.Close();
                connMcnr.Dispose();
            }
        }










        
        
        //Post Call for ASR


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AmscoMcnr/PostASRData")]

        public HttpResponseMessage PostASRData([FromBody] object inboundData2)
        {
            File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside ASR Post Call \r\n");
            File.AppendAllText(@"C:\AMSCOLogs.txt", inboundData2.ToString() + "\r\n");
            string managerName = "";
            string dateReceived2;
            string FeedbackFromClient;
            List<string> ASR_NO_OF_ASR_COPIES_SENT;
            List<string> ASR_NO_OF_FEEDBACK_RECEIVED_FOR_ASR;



            int requestId2 = 0;


            string connStringASR =

                "Server = XMD-LAB\\XMD; Database = TEST2; " +
                "User ID = sa; Password=P@ssw0rd";
            SqlTransaction tranASR = null;
            SqlConnection connASR = new SqlConnection(connStringASR);
            SqlCommand cmdASR = null;


            try
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside ASR POST call's Try block \r\n");
                // TODO - check if input data is in valid json format
                JObject jInboundData2 = JObject.Parse(inboundData2.ToString());

                managerName = jInboundData2["Manager_Name"].ToString();
                dateReceived2 = jInboundData2["Date"].ToString();
                string[] formatsMcnr = new string[] { "MM/dd/yyyy" };
                DateTime dateFormatted2 = DateTime.ParseExact(dateReceived2, formatsMcnr, CultureInfo.InvariantCulture, DateTimeStyles.None);
                string dateToDatabase2 = dateFormatted2.ToString("yyyy-MM-dd");
                FeedbackFromClient = jInboundData2["Feedback_From_Client"].ToString();
                ASR_NO_OF_ASR_COPIES_SENT = jInboundData2["No_of_Asr_Copies_Sent"].ToString().Split(',').ToList();
                ASR_NO_OF_FEEDBACK_RECEIVED_FOR_ASR = jInboundData2["No_Of_Feedback_Received_For_ASR"].ToString().Split(',').ToList();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "data recived in jInbounData \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", jInboundData2.ToString() + "\r\n");


                connASR.Open();
                tranASR = connASR.BeginTransaction("OnlyTransaction");

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Transaction Begin, Connection opened..\r\n");

                tranASR.Save("OnlySave");


                string selectRequestId2 =
                    "SELECT " +
                    "   CASE WHEN ISNULL(MAX(Request_ID),'') = ''  THEN 0 " +
                    "   ELSE MAX(Request_ID)" +
                    "   END " +
                    "FROM ASR_DATA";

                cmdASR = new SqlCommand(selectRequestId2, connASR, tranASR);

                requestId2 = Convert.ToInt32(cmdASR.ExecuteScalar()) + 1;

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Request id fetched in ASR Post call \r\n");

                string insertASRData =
                    "INSERT INTO dbo.ASR_DATA" +
                    "(" +
                        "REQUEST_ID," +
                        "MANAGER_NAME," +       
                        "DATE_RECEIVED," +
                        "FEEDBACK_FROM_THE_CLIENT" +
                    ")" +
                    "VALUES" +
                    "(" +
                        requestId2 + ",'" +
                        managerName + "','" +
                        dateToDatabase2 + "','" +
                        FeedbackFromClient +
                    "')";

                cmdASR = new SqlCommand(insertASRData, connASR, tranASR);
                cmdASR.ExecuteNonQuery();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into ASR_DATA table \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertASRData.ToString() + "\r\n");

                string insertAsr = "";

                foreach (string ASR_NO_OF_ASRS_COPIES_SENT in ASR_NO_OF_ASR_COPIES_SENT)
                {
                    insertAsr =
                        "INSERT INTO dbo.ASR_NO_OF_ASR_COPIES_SENT" +
                        "(" +
                            "REQUEST_ID," +
                            "ASR_COPIES_SENT" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId2 + ", '" +
                            ASR_NO_OF_ASRS_COPIES_SENT +
                        "')";

                    cmdASR = new SqlCommand(insertAsr, connASR, tranASR);
                    cmdASR.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into ASR_NO_OF_ASR_COPIES_SENT table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertAsr.ToString() + "\r\n");


                foreach (string ASR_NO_OF_FEEDBACKS_RECEIVED_FOR_ASR in ASR_NO_OF_FEEDBACK_RECEIVED_FOR_ASR)
                {
                    insertAsr =
                        "INSERT INTO dbo.ASR_NO_OF_FEEDBACK_RECEIVED_FROM_CLIENT" +
                        "(" +
                            "REQUEST_ID," +
                            "NO_OF_FEEDBACK_RECEIVED_FOR_ASR" +
                        ")" +
                        "VALUES" +
                        "(" +
                            requestId2 + ", '" +
                            ASR_NO_OF_FEEDBACKS_RECEIVED_FOR_ASR +
                        "')";

                    cmdASR = new SqlCommand(insertAsr, connASR, tranASR);
                    cmdASR.ExecuteNonQuery();

                }

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into ASR_NO_OF_FEEDBACK_RECEIVED_FOR_ASR table\r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertAsr.ToString() + "\r\n");

                
                tranASR.Commit();

                File.AppendAllText(@"C:\AMSCOLogs.txt", " Transaction commited \r\n");

                var message2 = Request.CreateResponse(HttpStatusCode.Created, "DATA INSERTED INTO ASR TABLE SUCCESSFULY");
                message2.Headers.Location = new Uri(Request.RequestUri + "");
                return message2;
            }

            catch (Exception exASR)
            {
                File.WriteAllText(@"C:\AMSCOLogs.txt", "Inside catch" + "\r\n");
                if (tranASR != null)
                {
                    tranASR.Rollback("OnlyTransaction");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exASR);

            }
            finally
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside ASR POST calls Finally\r\n");
                connASR.Close();
                connASR.Dispose();
            }
        }









        // Post call for achievements



        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/AmscoMcnr/PostAchievements")]

        public HttpResponseMessage PostAchievements([FromBody] object inboundDt)
        {
            File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside Achievements Post Call \r\n");
            File.AppendAllText(@"C:\AMSCOLogs.txt", inboundDt.ToString() + "\r\n");
            string Achievement1 = "";
            string Achievement2 = "";
            string Achievement3 = "";
            string Achievement4 = "";
            string Achievement5 = "";
            string Achievement6 = "";
            string dateSubmitted;


            string connStringACH =

                "Server = XMD-LAB\\XMD; Database = TEST2; " +
                "User ID = sa; Password=P@ssw0rd";
            SqlTransaction tranACH = null;
            SqlConnection connACH = new SqlConnection(connStringACH);
            SqlCommand cmdACH = null;


            try
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside Achievements POST call's Try block \r\n");
                // TODO - check if input data is in valid json format
                JObject jInboundDt = JObject.Parse(inboundDt.ToString());

                Achievement1 = jInboundDt["Achievement1"].ToString();
                Achievement2 = jInboundDt["Achievement2"].ToString();
                Achievement3 = jInboundDt["Achievement3"].ToString();
                Achievement4 = jInboundDt["Achievement4"].ToString();
                Achievement5 = jInboundDt["Achievement5"].ToString();
                Achievement6 = jInboundDt["Achievement6"].ToString();
                dateSubmitted = jInboundDt["Date"].ToString();
                string[] formatsACH = new string[] { "MM/dd/yyyy" };
                DateTime dateFormattedACH = DateTime.ParseExact(dateSubmitted, formatsACH, CultureInfo.InvariantCulture, DateTimeStyles.None);
                string dateToDatabase4 = dateFormattedACH.ToString("yyyy-MM-dd");

                //dateReceived2 = jInboundData2["Date"].ToString();
                //string[] formatsMcnr = new string[] { "MM/dd/yyyy" };
                //DateTime dateFormatted2 = DateTime.ParseExact(dateReceived2, formatsMcnr, CultureInfo.InvariantCulture, DateTimeStyles.None);
                //string dateToDatabase2 = dateFormatted2.ToString("yyyy-MM-dd");

                File.AppendAllText(@"C:\AMSCOLogs.txt", "data recived in jInbounData \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", jInboundDt.ToString() + "\r\n");


                connACH.Open();
                tranACH = connACH.BeginTransaction("OnlyTransaction");

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Transaction Begin, Connection opened..\r\n");

                tranACH.Save("OnlySave");


                string insertACHData =
                    "INSERT INTO dbo.ACHIEVEMENTS" +
                    "(" +
                        "Achievement1," +
                        "Achievement2," +
                        "Achievement3," +
                        "Achievement4," +
                        "Achievement5," +
                        "Achievement6," +
                        "Date_submitted" +
                    ")" +
                    "VALUES" +
                    "('" +
                        Achievement1 + "','" +
                        Achievement2 + "','" +
                        Achievement3 + "','" +
                        Achievement4 + "','" +
                        Achievement5 + "','" +
                        Achievement6 + "','" +
                        dateToDatabase4 +
                    "')";

                cmdACH = new SqlCommand(insertACHData, connACH, tranACH);
                cmdACH.ExecuteNonQuery();

                File.AppendAllText(@"C:\AMSCOLogs.txt", "Data inserted into ACHIEVEMENTS table \r\n");
                File.AppendAllText(@"C:\AMSCOLogs.txt", insertACHData.ToString() + "\r\n");

                tranACH.Commit();

                File.AppendAllText(@"C:\AMSCOLogs.txt", " Transaction commited \r\n");

                var message4 = Request.CreateResponse(HttpStatusCode.Created, "DATA INSERTED INTO ACHIEVEMENTS TABLE SUCCESSFULY");
                message4.Headers.Location = new Uri(Request.RequestUri + "");
                return message4;
            }

            catch (Exception exACH)
            {
                File.WriteAllText(@"C:\AMSCOLogs.txt", "Inside catch" + "\r\n");
                if (tranACH != null)
                {
                    tranACH.Rollback("OnlyTransaction");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exACH);

            }
            finally
            {
                File.AppendAllText(@"C:\AMSCOLogs.txt", "Inside ASR POST calls Finally\r\n");
                connACH.Close();
                connACH.Dispose();
            }
        }
















    }


}
