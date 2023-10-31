using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class GetManifestController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
            DataTable dt = new DataTable();

            List<Manifest> manifest = new List<Manifest>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select Distinct ManifestId,Cast(ManifestGeneratedOn as date) as ManifestGeneratedOn from EasypostShipments where Cast(ManifestGeneratedOn as date) >= Cast(DateAdd(DAY,-14,GETDATE()) as date)", con))
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();

                    if (con.State == ConnectionState.Closed)
                        con.Open();


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    con.Close();
                }


                if (dt == null || dt.Rows.Count == 0)
                {
                    SplitPickListsucessresponse sucessresponse1 = new SplitPickListsucessresponse();
                    sucessresponse1.message = "No manifest to display";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse1)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        Manifest man = new Manifest();
                        man.Id = row["ManifestId"].ToString();
                        man.ManifestDate = row["ManifestGeneratedOn"].ToString();
                        man.URL = "https://weblegs.info/EasyPost/Manifest Files/"+ row["ManifestId"].ToString()+".pdf";
                        manifest.Add(man);
                    }

                    SplitPickListsucessresponse sucessresponse1 = new SplitPickListsucessresponse();
                    sucessresponse1.data = manifest;
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse1)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
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
        public class Manifest
        {
            public string Id { get; set; }
            public string ManifestDate { get; set; }
            public string URL { get; set; }

        }
        public class SplitPickListsucessresponse
        {
            public string message { get; set; }
            public List<Manifest> data { get; set; }
        }
    }
}