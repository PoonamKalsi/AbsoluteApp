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
    public class GetPickedOrdersVersion2Controller : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string type, string ean, string order)
        {
            try
            {
                GetPickedOrderssucessresponse sucessresponse = new GetPickedOrderssucessresponse();

                //SqlConnection con = new SqlConnection();
                //con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();

                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(type) && string.IsNullOrEmpty(ean) && !string.IsNullOrEmpty(order))
                {
                    //Nothing should display if customer had selected the ean if type is empty
                }
                else
                {
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
                    using (SqlConnection con = connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("GetPickedOrderNumbers", con))
                        {
                            if (con.State == System.Data.ConnectionState.Closed)
                                con.Open();
                            if (string.IsNullOrEmpty(type))
                            {
                                cmd.Parameters.AddWithValue("@type", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@type", type);
                            }
                            if (string.IsNullOrEmpty(order))
                            {
                                cmd.Parameters.AddWithValue("@OrderNumber", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@OrderNumber", order);
                            }
                            if (string.IsNullOrEmpty(ean))
                            {
                                cmd.Parameters.AddWithValue("@ean", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ean", ean);
                            }
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(dt);
                            con.Close();
                        }
                    }
                }
                IList<Skus1> listskus = new List<Skus1>();
                foreach (DataRow r in dt.Rows)
                {
                    if (true/*r["IsShippedOnCa"].ToString() == "true"*/)
                    {
                        if (type == "MSMQW")
                        {
                            Skus1 sku = new Skus1();
                            sku.Id = Convert.ToInt32(r["Id"].ToString());
                            sku.sku = r["SKU"].ToString();
                            sku.title = r["Title"].ToString();
                            sku.WarehouseLocation = r["WarehouseLocation"].ToString();
                            sku.OrderNumber = r["OrderNumber"].ToString();
                            sku.SiteOrderId = r["SiteOrderId"].ToString();
                            sku.ean = r["EAN"].ToString();
                            sku.url = r["Url"].ToString();
                            sku.QtyToPick = r["Qty to Pick"].ToString();
                            sku.SiteName = r["Site Name"].ToString();
                            sku.ShippingCarrier = r["ShippingCarrier"].ToString();
                            sku.ShippingClass = r["ShippingClass"].ToString();
                            sku.PickedSKUs = r["PickedCount"].ToString();
                            sku.TotalSKUs = r["TotalCount"].ToString();
                            sku.IsPicked = r["IsShippedOnCA"].ToString();
                            sku.PackageType = r["PackageType"].ToString();
                            listskus.Add(sku);
                        }
                        else
                        {
                            Skus1 sku = new Skus1();
                            sku.Id = Convert.ToInt32(r["Id"].ToString());
                            sku.sku = r["SKU"].ToString();
                            sku.title = r["Title"].ToString();
                            sku.WarehouseLocation = r["WarehouseLocation"].ToString();
                            sku.OrderNumber = r["OrderNumber"].ToString();
                            sku.SiteOrderId = r["SiteOrderId"].ToString();
                            sku.ean = r["EAN"].ToString();
                            sku.url = r["Url"].ToString();
                            sku.QtyToPick = r["Qty to Pick"].ToString();
                            sku.SiteName = r["Site Name"].ToString();
                            sku.ShippingCarrier = r["ShippingCarrier"].ToString();
                            sku.ShippingClass = r["ShippingClass"].ToString();
                            sku.PackageType = r["PackageType"].ToString();
                            listskus.Add(sku);
                        }
                    }
                }
                sucessresponse.Sku = listskus;
                var response1 = Request.CreateResponse(HttpStatusCode.OK);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
            }
            catch (Exception ex)
            {
                GetPickListfailedresponse failedresponse = new GetPickListfailedresponse();
                failedresponse.code = 0;
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