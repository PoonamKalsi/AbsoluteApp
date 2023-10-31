using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class SplitPicklistController : ApiController
    {
        
        public HttpResponseMessage  Get(HttpRequestMessage request, string BatchId,string locations, string UserId = "")
        {
            try
            {
                #region GETTING DATA FROM PROCEDURE

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection con1 = connection)
                {
                    using (SqlCommand cmd = new SqlCommand("SplitPicklist", con1))
                    {
                        if (con1.State == System.Data.ConnectionState.Closed)
                            con1.Open();
                        cmd.Parameters.AddWithValue("@BatchId", BatchId);
                        cmd.Parameters.AddWithValue("@locations", locations);
                        cmd.Parameters.AddWithValue("@UserId", string.IsNullOrEmpty(UserId) ? "" : UserId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        con1.Close();
                    }
                }

                Process p1 = new Process();
                p1.StartInfo.FileName = @"H:\Applications\Jadlam\Picklist Orders Status Update\JadlamJITStatusUpdate.exe";
                //p1.StartInfo.Arguments = processID;
                p1.Start();

                SplitPickListsucessresponse sucessresponse = new SplitPickListsucessresponse();
                sucessresponse.message = "Successfully splited!";
                var response1 = Request.CreateResponse(HttpStatusCode.OK);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
                #endregion
            }
            catch (Exception ex)
            {
                SplitPicklistfailedresponse failedresponse = new SplitPicklistfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }
        public class SplitPicklistfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        public class SplitPickListsucessresponse
        {
            public string message { get; set; }
        }
    }
}