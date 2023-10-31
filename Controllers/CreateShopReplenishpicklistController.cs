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
    public class CreateShopReplenishpicklistController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string SkuandQuantity)
        {
            try
            {
                CreatingPicklistOutOfPreOrderssucessresponse sucessresponse = new CreatingPicklistOutOfPreOrderssucessresponse();

                //SqlConnection con = new SqlConnection();
                //con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());

                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(SkuandQuantity))
                {
                    using (SqlConnection con = connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("CreatePicklistForShopReplenish", con))
                        {
                            if (con.State == System.Data.ConnectionState.Closed)
                                con.Open();

                            cmd.Parameters.AddWithValue("@OrderQuantity", SkuandQuantity);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    //Process p2 = new Process();
                    //p2.StartInfo.FileName = @"H:\Applications\Jadlam\JadlamDimesionUpdate\JadlamDimensionUpdate.exe";
                    ////p2.StartInfo.Arguments = processID;
                    //p2.Start();

                    Process p1 = new Process();
                    p1.StartInfo.FileName = @"H:\Applications\Jadlam\Picklist Orders Status Update\JadlamJITStatusUpdate.exe";
                    p1.StartInfo.Arguments = "";
                    p1.Start();

                    sucessresponse.message = "Successfully created";
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;

                }
                else
                {
                    CreatingPicklistOutOfPreOrdersfailedresponse failedresponse = new CreatingPicklistOutOfPreOrdersfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "SKU and Quantity not sent";
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }



            }
            catch (Exception ex)
            {
                CreatingPicklistOutOfPreOrdersfailedresponse failedresponse = new CreatingPicklistOutOfPreOrdersfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }
    }
}