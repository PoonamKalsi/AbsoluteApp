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
    public class GetShopReplenishSKUController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string EAN = "")
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
            DataTable dt = new DataTable();
            IList<ShopReplenishSKU> Sku = new List<ShopReplenishSKU>();
            try
            {
                using (SqlConnection con = connection)
                {
                    using (SqlCommand cmd = new SqlCommand("GetShopReplenishEANs", con))
                    {
                        if (con.State == System.Data.ConnectionState.Closed)
                            con.Open();

                        cmd.Parameters.AddWithValue("@EAN", EAN);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        con.Close();
                    }
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    GetShopReplenishSKUfailedresponse failedresponse = new GetShopReplenishSKUfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "No EAN Found!";
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        ShopReplenishSKU sku = new ShopReplenishSKU();
                        sku.EAN = r["EAN"].ToString();
                        sku.PackageType = r["PackageType"].ToString();
                        sku.ParentSku = r["ParentSku"].ToString();
                        sku.ProductType = r["ProductType"].ToString();
                        sku.sku = r["Sku"].ToString();
                        sku.Title = r["Title"].ToString();
                        sku.WarehouseLocation = r["WarehouseLocation"].ToString();
                        sku.Url = r["Url"].ToString();
                        sku.Quantity = r["Quantity"].ToString();
                        Sku.Add(sku);
                    }

                    GetShopReplenishSKUsucessresponse sucessresponse = new GetShopReplenishSKUsucessresponse();
                    sucessresponse.Sku = Sku;
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
            }
            catch (Exception ex)
            {
                GetShopReplenishSKUfailedresponse failedresponse = new GetShopReplenishSKUfailedresponse();
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
#region RESPONSE CLASSES

public class GetShopReplenishSKUsucessresponse
{
    public IList<ShopReplenishSKU> Sku { get; set; }
}

public class ShopReplenishSKU
{
    public string EAN { get; set; }
    public string PackageType { get; set; }
    public string ParentSku { get; set; }
    public string ProductType { get; set; }
    public string sku { get; set; }
    public string Title { get; set; }
    public string WarehouseLocation { get; set; }
    public string Url { get; set; }
    public string Quantity { get; set; }

}
public class GetShopReplenishSKUfailedresponse
{
    public string message { get; set; }
    public int code { get; set; }
}
#endregion