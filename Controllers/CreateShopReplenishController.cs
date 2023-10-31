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
    public class CreateShopReplenishController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string data)
        {
            try
            {
                ListEanQuantity listEanQuantity = new ListEanQuantity();

                try
                {
                    var serializedobj = Newtonsoft.Json.JsonConvert.DeserializeObject<ListEanQuantity>(data);
                }
                catch(Exception ex)
                {
                    CreateShopReplenishfailedresponse failedresponse = new CreateShopReplenishfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = ex.Message;
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }

                var i = "Shop" + Guid.NewGuid().ToString();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand("select * from JadlamPickList where Status = 'In Progress'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //fill the dataset
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("Insert into JadlamPickList (BatchId,[Status],[Request Type],CreatedOn) values('" + i + "', 'Pending', 'SIW', GetDate())", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        cmd.ExecuteNonQuery();
                    }

                    Process p = new Process();
                    p.StartInfo.FileName = ConfigurationManager.AppSettings["ExePath"].ToString();
                    p.StartInfo.Arguments = i.ToString() + " " + "SIW"+" "+ data;
                    p.Start();

                    CreateShopReplenishsucessresponse sucessresponse = new CreateShopReplenishsucessresponse();
                    sucessresponse.message = "Successfully created";
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                else
                {
                    createpicklistfailedresponset failedresponse = new createpicklistfailedresponset();
                    failedresponse.code = 0;
                    failedresponse.message = "Picklist already running. Please wait and try again later!";
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }



            }
            catch (Exception ex)
            {
                CreateShopReplenishfailedresponse failedresponse = new CreateShopReplenishfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }
        #region HELPER CLASSES
        public class CreateShopReplenishfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        public class CreateShopReplenishsucessresponse
        {
            public string message { get; set; }
        }
        public class ListEanQuantity
        {
            public IList<EanQuanity> eanQuanities { get; set; }
        }
        public class EanQuanity
        {
            public string ean { get; set; }
            public string quantity { get; set; }
        }
        #endregion
    }
}