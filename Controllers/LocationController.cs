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
    public class LocationController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get  (HttpRequestMessage request, string AccountType)
        {
            sucessresponse2 sucessresponse = new sucessresponse2();


            if (string.IsNullOrEmpty(AccountType))
            {
                failedresponse2 failedresponse = new failedresponse2();
                failedresponse.code = 0;
                failedresponse.message = "Account Type is empty!";
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
                if (AccountType == "Absolute")
                {
                    using (SqlCommand cmd = new SqlCommand("select Distinct WarehouseLocation from EAN_SKU_MAPPING1 where WarehouseLocation is not null", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                if (AccountType == "Jadlam")
                {
                    using (SqlCommand cmd = new SqlCommand("select Distinct WarehouseLocation from JADLAM_EAN_SKU_MAPPING where WarehouseLocation is not null", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    failedresponse2 failedresponse = new failedresponse2();
                    failedresponse.code = 0;
                    failedresponse.message = "No Location Found!";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                //sucessresponse.success = "true";
                IList<Result2> rs = new List<Result2>();
                foreach (DataRow r in dt.Rows)
                {
                    Result2 res = new Result2();
                    res.Location = r["WarehouseLocation"].ToString();
                    rs.Add(res);
                }
                sucessresponse.Result = rs;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                failedresponse2 failedresponse = new failedresponse2();
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
    public class sucessresponse2
    {
        public IList<Result2> Result { get; set; }
    }

    public class Result2
    {
        public string Location { get; set; }

    }

    public class failedresponse2
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}