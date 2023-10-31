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
    public class ValidateAllPickListsController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                IList<string> orderNumbers = new List<string>();
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection con1 = connection)
                {
                    using (SqlCommand cmd = new SqlCommand("ValidateAllPicklists", con1))
                    {
                        if (con1.State == System.Data.ConnectionState.Closed)
                            con1.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.ExecuteNonQuery();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                        con1.Close();
                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    SplitPickListsucessresponse sucessresponse1 = new SplitPickListsucessresponse();
                    sucessresponse1.message = "No EAN to validate!";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse1)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }

                foreach (DataRow r in dt.Rows)
                {
                    orderNumbers.Add(r["OrderNum"].ToString());
                }

                Process p1 = new Process();
                p1.StartInfo.FileName = @"H:\Applications\Jadlam\Picklist Orders Status Update\JadlamJITStatusUpdate.exe";
                //p1.StartInfo.Arguments = processID;
                p1.Start();

                SplitPickListsucessresponse sucessresponse = new SplitPickListsucessresponse();
                sucessresponse.message = "Successfully validated!";
                sucessresponse.OrderNumbers = orderNumbers;
                var response1 = Request.CreateResponse(HttpStatusCode.OK);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
            }
            catch(Exception ex)
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
            public IList<string> OrderNumbers { get; set; }
        }
    }
}