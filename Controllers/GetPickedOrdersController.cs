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
    public class GetPickedOrdersController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string type,string entity)
        {
            try
            {
                GetPickedOrderssucessresponse sucessresponse = new GetPickedOrderssucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();


                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(entity))
                {

                    //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number] OrderNumber,pic.[Site Order Id] SiteOrderId,pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],count(*) [Qty to Pick],IsShippedOnCa,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId='" + BatchId + "' and  pic.SKU not like '%fba%'  and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa,pic.[Order Number],pic.[Site Order Id] order by pic.[Order Number],pic.sku ", con))
                    //using (SqlCommand cmd = new SqlCommand("select distinct pic.[Order Number]  OrderNumber ,  pic.[Site Order Id]  SiteOrderId , pic.[Site Name], pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Sum(Cast (Quantity as int )) [Qty to Pick], IsShippedOnCa,pic.ShippingCarrier,pic.ShippingClass,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' ) and jd.[Request Type]='" + type + "' and IsShippedOnCA='true' and pic.[Shipping Status]<>'Shipped' group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type],  pic.[Order Number] ,  pic.[Site Order Id] , pic.[Site Name],pic.ShippingCarrier,pic.ShippingClass order by pic.sku", con))
                    using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number]  OrderNumber ,  pic.[Site Name], pic.[Site Order Id]  SiteOrderId , pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Cast (Quantity as int )as  [Qty to Pick], IsShippedOnCa,pic.ShippingCarrier,pic.ShippingClass,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' )  and jd.[Request Type]='" + type + "'   and (ean.EAN='" + entity + "' or pic.[Order Number]='" + entity + "') and IsShippedOnCA='true' and pic.[Shipping Status]<>'Shipped' and pic.MSMQWScanned is null group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type],  pic.[Order Number] ,  pic.[Site Order Id] , pic.[Site Name],pic.ShippingCarrier,pic.ShippingClass,pic.Quantity order by pic.sku", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }

                if (string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(entity))
                {

                    //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number] OrderNumber,pic.[Site Order Id] SiteOrderId,pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],count(*) [Qty to Pick],IsShippedOnCa,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId='" + BatchId + "' and  pic.SKU not like '%fba%'  and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa,pic.[Order Number],pic.[Site Order Id] order by pic.[Order Number],pic.sku ", con))
                    //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number]  OrderNumber ,  pic.[Site Name], pic.[Site Order Id]  SiteOrderId , pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Sum(Cast (Quantity as int )) [Qty to Pick], IsShippedOnCa,pic.ShippingCarrier,pic.ShippingClass,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' )  and jd.[Request Type]='" + type + "'   and (ean.EAN='"+entity+ "' or pic.[Order Number]='" + entity + "') and IsShippedOnCA='true' and pic.[Shipping Status]<>'Shipped' group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type],  pic.[Order Number] ,  pic.[Site Order Id] , pic.[Site Name],pic.ShippingCarrier,pic.ShippingClass order by pic.sku", con))
                    //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number]  OrderNumber ,  pic.[Site Name], pic.[Site Order Id]  SiteOrderId , pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Cast (Quantity as int )as  [Qty to Pick], IsShippedOnCa,pic.ShippingCarrier,pic.ShippingClass,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' )  and jd.[Request Type]='" + type + "'   and (ean.EAN='"+entity+ "' or pic.[Order Number]='" + entity + "') and IsShippedOnCA='true' and pic.[Shipping Status]<>'Shipped' group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type],  pic.[Order Number] ,  pic.[Site Order Id] , pic.[Site Name],pic.ShippingCarrier,pic.ShippingClass,pic.Quantity order by pic.sku", con))
                    using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number]  OrderNumber ,  pic.[Site Name], pic.[Site Order Id]  SiteOrderId , pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Cast (Quantity as int )as  [Qty to Pick], IsShippedOnCa,pic.ShippingCarrier,pic.ShippingClass,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' )   and (ean.EAN='"+entity+ "' or pic.[Order Number]='" + entity + "') and IsShippedOnCA='true' and pic.[Shipping Status]<>'Shipped' and pic.MSMQWScanned is null group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type],  pic.[Order Number] ,  pic.[Site Order Id] , pic.[Site Name],pic.ShippingCarrier,pic.ShippingClass,pic.Quantity order by pic.sku", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }

                IList<Skus1> listskus = new List<Skus1>();
                foreach (DataRow r in dt.Rows)
                {
                    if (true/*r["IsShippedOnCa"].ToString() == "true"*/)
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
                GetPickListfailedresponse failedresponse = new GetPickListfailedresponse();
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

public class GetPickedOrderssucessresponse
{
    public IList<Skus1> Sku { get; set; }
}

public class Skus1
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public string SiteOrderId { get; set; }
    public string sku { get; set; }
    public string title { get; set; }
    public string WarehouseLocation { get; set; }
    public string ean { get; set; }
    public string url { get; set; }
    public string SiteName { get; set; }
    public string QtyToPick { get; set; }
    public string ShippingCarrier { get; set; }
    public string ShippingClass { get; set; }
    public string TotalSKUs { get; set; }
    public string PickedSKUs { get; set; }
    public string IsPicked { get; set; }
    public string PackageType { get; set; }

}
public class GetPickedOrdersfailedresponse
{
    public string message { get; set; }
    public int code { get; set; }
}
    #endregion
