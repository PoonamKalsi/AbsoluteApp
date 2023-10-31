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
    public class UpdatePicksController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId, string SKU, string OrderNumber, string type)
        {
            try
            {
                UpdatePickssucessresponse sucessresponse = new UpdatePickssucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();

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
                        using (SqlCommand cmd = new SqlCommand("select top 1 * from PicklistOrdersForApp where BatchId='" + BatchId + "' and sku='" + SKU + "' and SKU not like '%fba%' and IsShippedOnCA='false' and [Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            //fill the dataset
                            adapter.Fill(dt);
                            con.Close();
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' where [Order Number]='" + dt.Rows[0]["Order Number"] + "' and SKU not like '%fba%' and IsShippedOnCA='false' and [Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False')", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            using (SqlCommand cmd = new SqlCommand("Insert into QtytoPickLogs values('" + BatchId + "', '" + SKU + "', 1)", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        else
                        {
                            UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
                            failedresponse.code = 0;
                            failedresponse.message = "No order to pick!";
                            var response2 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            response2.Headers.Add("Access-Control-Allow-Origin", "*");
                            response2.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                            return response2;
                        }
                        sucessresponse.message = "updated!";
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Headers.Add("Access-Control-Allow-Origin", "*");

                        response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        using (SqlCommand cmd = new SqlCommand("select top 1 * from PicklistOrdersForApp where BatchId='" + BatchId + "' and sku='" + SKU + "'and [Order Number]='" + OrderNumber + "' and SKU not like '%fba%' and IsShippedOnCA='false' and [Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            //fill the dataset
                            adapter.Fill(dt);
                            con.Close();
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' where BatchId='" + BatchId + "' and sku='" + SKU + "'and [Order Number]='" + OrderNumber + "' and SKU not like '%fba%' and IsShippedOnCA='false' and [Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False')", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            using (SqlCommand cmd = new SqlCommand("Insert into QtytoPickLogs values('" + BatchId + "', '" + SKU + "', 1)", con))
                            {
                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        else
                        {
                            UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
                            failedresponse.code = 0;
                            failedresponse.message = "No order to pick!";
                            var response2 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            response2.Headers.Add("Access-Control-Allow-Origin", "*");
                            response2.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                            return response2;
                        }
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
    #region RESPONSE CLASSES

    public class UpdatePickssucessresponse
    {
        public string message { get; set; }
    }


    public class UpdatePicksfailedresponse
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}