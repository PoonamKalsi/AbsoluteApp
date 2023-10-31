using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
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
    public class SearchController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Search(HttpRequestMessage request, string EAN)
        {
            sucessresponse sucessresponse = new sucessresponse();

            if (string.IsNullOrEmpty(EAN))
            {
                failedresponse failedresponse = new failedresponse();
                failedresponse.code = 0;
                failedresponse.message = "EAN is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["DBConn"].ToString();
            try
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("Select * from EAN_SKU_MAPPING where EAN='" + EAN + "'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //fill the dataset
                    adapter.Fill(dt);
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    failedresponse failedresponse = new failedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "EAN not found!";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                //sucessresponse.success = "true";
                IList<Result> rs = new List<Result>();
                foreach (DataRow r in dt.Rows)
                {
                    Result res = new Result();
                    res.Id = r["Id"].ToString();
                    res.EAN = r["EAN"].ToString();
                    res.SKU = r["SKU"].ToString();
                    rs.Add(res);
                }
                sucessresponse.Result = rs;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                failedresponse failedresponse = new failedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            }


        }

    }
    #region RESPONSE CLASSES
    public class sucessresponse
    {
        public IList<Result> Result { get; set; }
    }

    public class Result
    {
        public string Id { get; set; }
        public string EAN { get; set; }
        public string SKU { get; set; }
    }

    public class failedresponse
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}

