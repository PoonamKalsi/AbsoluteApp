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
    public class ReverseValidateAllPicklistController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string OrderNumbers)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();

            try
            {
                using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'false' where [Order Number] in (" + OrderNumbers + ")", con))
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Process p1 = new Process();
                p1.StartInfo.FileName = @"H:\Applications\Jadlam\Picklist Orders Status Update\JadlamJITStatusUpdate.exe";
                //p1.StartInfo.Arguments = processID;
                p1.Start();

                SplitPickListsucessresponse sucessresponse1 = new SplitPickListsucessresponse();
                sucessresponse1.message = "Reverted!";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse1)).ToString(), Encoding.UTF8, "application/json");
                return response;
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