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
    public class GetPreOrdersfirstController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string SKU = null)
        {
            try
            {
                GetPreOrdersfirstsucessresponse sucessresponse = new GetPreOrdersfirstsucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();

                DataTable dt = new DataTable();
                if (string.IsNullOrEmpty(SKU))
                {

                    //using (SqlCommand cmd = new SqlCommand("select pic.[Order Number],pic.[Site Order Id],pic.SKU,pic.Title,ean.WarehouseLocation,img.Url,pic.Quantity,ean.EAN,pic.[Order Date],pic.BatchId from NoPickListScanOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING ean on pic.sku = ean.Sku inner join Absolute.dbo.JADLAM_IMAGE_SKU_MAPPING img on ean.ID= img.ProductID where WarehouseLocation like 'PRE%'  and [Order Number] not in (select [Order Number] from NoPickListScanOrdersForApp group by [Order number] having count(*) > 1)order by pic.[Order Date]", con))
                    using (SqlCommand cmd = new SqlCommand("select pic.[Order Number],pic.[Site Order Id],pic.SKU,pic.Title,ean.WarehouseLocation,img.Url,pic.Quantity,ean.EAN, pic.[Order Date],pic.BatchId, case when (exists(select [Order Number] from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] having count(*) = 1) and pic.Quantity=1) then 'SIW' when (exists(select [Order Number] from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] having count(*) = 1) and pic.Quantity<>1) then 'SSMQW' else 'MSMQW' end [Order type],(select count(*) from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] ) TotalCount from NoPickListScanOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING ean on pic.sku = ean.Sku inner join Absolute.dbo.JADLAM_IMAGE_SKU_MAPPING img on ean.ID= img.ProductID where WarehouseLocation like 'PRE%' and BatchId='' and [Shipping Status]<>'shipped' order by pic.[Order Date]", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                else
                {
                    //using (SqlCommand cmd = new SqlCommand("select pic.[Order Number],pic.[Site Order Id],pic.SKU,pic.Title,ean.WarehouseLocation,img.Url,pic.Quantity,ean.EAN,pic.[Order Date],pic.BatchId from NoPickListScanOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING ean on pic.sku = ean.Sku inner join Absolute.dbo.JADLAM_IMAGE_SKU_MAPPING img on ean.ID= img.ProductID where WarehouseLocation like 'PRE%'  and pic.Sku='" + SKU + "' and [Order Number] not in (select [Order Number] from NoPickListScanOrdersForApp group by [Order number] having count(*) > 1) order by pic.[Order Date]", con))
                    using (SqlCommand cmd = new SqlCommand("select pic.[Order Number],pic.[Site Order Id],pic.SKU,pic.Title,ean.WarehouseLocation,img.Url,pic.Quantity,ean.EAN, pic.[Order Date],pic.BatchId, case when (exists(select [Order Number] from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] having count(*) = 1) and pic.Quantity=1) then 'SIW' when (exists(select [Order Number] from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] having count(*) = 1) and pic.Quantity<>1) then 'SSMQW' else 'MSMQW' end [Order type],(select count(*) from NoPickListScanOrdersForApp where [Order Number]=pic.[Order number] group by [Order number] ) TotalCount from NoPickListScanOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING ean on pic.sku = ean.Sku inner join Absolute.dbo.JADLAM_IMAGE_SKU_MAPPING img on ean.ID= img.ProductID where WarehouseLocation like 'PRE%'  and pic.Sku='" + SKU + "' and BatchId='' and [Shipping Status]<>'shipped' order by pic.[Order Date]", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }

                IList<OrderData> listskus = new List<OrderData>();
                foreach (DataRow r in dt.Rows)
                {
                    if (string.IsNullOrEmpty(r["BatchId"].ToString()))
                    {
                        OrderData sku = new OrderData();
                        sku.sku = r["SKU"].ToString();
                        sku.title = r["Title"].ToString();
                        sku.WarehouseLocation = r["WarehouseLocation"].ToString();
                        sku.OrderNumber = r["Order Number"].ToString();
                        sku.SiteOrderId = r["Site Order Id"].ToString();
                        sku.ean = r["EAN"].ToString();
                        sku.Quantity = r["Quantity"].ToString();
                        sku.OrderDate = r["Order Date"].ToString();
                        sku.ImageUrl = r["Url"].ToString();
                        sku.OrderType = r["Order type"].ToString();
                        sku.TotalCount = r["TotalCount"].ToString();
                        listskus.Add(sku);
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
                GetPreOrdersfirstfailedresponse failedresponse = new GetPreOrdersfirstfailedresponse();
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

public class GetPreOrdersfirstsucessresponse
{
    public IList<OrderData> Sku { get; set; }
}

public class OrderData
{
    public string OrderNumber { get; set; }
    public string SiteOrderId { get; set; }
    public string sku { get; set; }
    public string title { get; set; }
    public string WarehouseLocation { get; set; }
    public string ean { get; set; }
    public string Quantity { get; set; }
    public string OrderDate { get; set; }
    public string ImageUrl { get; set; }
    public string OrderType { get; set; }
    public string TotalCount { get; set; }

}
public class GetPreOrdersfirstfailedresponse
{
    public string message { get; set; }
    public int code { get; set; }
}
#endregion