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
    public class JustkeepValidatePicklistController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId, string SKU, string OrderNumber, string type, string UserId = null)
        {
            try
            {
                UpdatePickssucessresponse sucessresponse = new UpdatePickssucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnjustkeep"].ToString();

                if (string.IsNullOrEmpty(BatchId) || string.IsNullOrEmpty(SKU))
                {
                    UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "Batch Id or SKU is empty";
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                else
                {
                    if (type != "MSMQW")
                    {
                        DataTable dt = new DataTable();


                        using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true', ValidatedByUser='" + UserId + "', IsHold='false' where [Order Number] in (" + OrderNumber + ") and  BatchId='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        Process p = new Process();
                        p.StartInfo.FileName = @"H:\Applications\JustKeeper\JUSTKEEPER'S APP WORK\PICKLIST ORDER STATUS UPDATE\Justkeep_Order_Status_Update.exe";
                        //p.StartInfo.Arguments = "Order:" + (String.IsNullOrEmpty(OrderNumber) ? "" : OrderNumber.Trim()) + " " + "Batch:" + BatchId.Trim().Replace(" ", "**") + " " + "sku:" + (String.IsNullOrEmpty(SKU) ? "" : SKU);
                        p.Start();
                        sucessresponse.message = "updated!";
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Headers.Add("Access-Control-Allow-Origin", "*");

                        response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response;

                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true',  ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "' and sku='" + SKU + "' and [Order Number]='" + OrderNumber + "' ", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            cmd.CommandTimeout = 90;
                            con.Close();
                        }
                        Process p = new Process();
                        p.StartInfo.FileName = @"H:\Applications\JustKeeper\JUSTKEEPER'S APP WORK\PICKLIST ORDER STATUS UPDATE\Justkeep_Order_Status_Update.exe";
                        //p.StartInfo.Arguments = "Order:" + (String.IsNullOrEmpty(OrderNumber) ? "" : OrderNumber.Trim()) + " " + "Batch:" + BatchId.Trim().Replace(" ", "**") + " " + "sku:" + (String.IsNullOrEmpty(SKU) ? "" : SKU);
                        p.Start();
                        sucessresponse.message = "updated!";
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Headers.Add("Access-Control-Allow-Origin", "*");

                        response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
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