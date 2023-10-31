using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class CreatePickListController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get(HttpRequestMessage request, string type)
        {
            try
            {
                createpicklistsucessresponse sucessresponse = new createpicklistsucessresponse();

                Guid gd = new Guid();
                var i = gd.ToString();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.AppSettings["DBConn"].ToString();
                using (SqlCommand cmd = new SqlCommand("Insert into JadlamPickList (BatchId,[Status],[Request Type],CreatedOn) values("+gd+", 'Pending', "+type+", GetDate())", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                Process p = new Process();
                p.StartInfo.FileName = ConfigurationManager.AppSettings["ExePath"].ToString();
                p.StartInfo.Arguments = Identity.ToString();

                sucessresponse.data = "Successfully created";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                createpicklistfailedresponset failedresponse = new createpicklistfailedresponset();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
             
            }
        }

       
    }
    #region RESPONSE CLASSES

    public class createpicklistsucessresponse
    {
        public string data { get; set; }
    }

    public class createpicklistfailedresponset
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}