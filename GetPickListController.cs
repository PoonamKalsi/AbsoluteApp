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

namespace AbsoluteApp
{
    public class GetPickListController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId)
        {

            try
            {
                GetPickListsucessresponse sucessresponse = new GetPickListsucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();


                if (string.IsNullOrEmpty(BatchId))
                {
                    DataTable dt = new DataTable();

                    //using (SqlCommand cmd = new SqlCommand("select 'PickList-'+Cast((ROW_NUMBER() OVER(ORDER BY Id)) as nvarchar(100)) [Picklist], BatchId,CreatedOn,[Request Type],[Status] , (select count(*) from ( (SELECT t.sku,r.MaxTime FROM ( SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY SKU ) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku group by t.sku,r.MaxTime having count(*)>= r.MaxTime)) as [Picked SKU])[Picked SKU], (select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] group by t.[Order Number],r.MaxTime having count(*)<= r.MaxTime)) as [Picked Orders])[Picked Orders], (select count(*) from ((select distinct pic.SKU from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url] )) as [Total SKUs]) [Total SKUs] , (select count(*) from ((select distinct pic.[Order Number] from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and [Shipping Status]<>'Shipped' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],[Order Number] )) as [Total Orders]) [Total Orders] from JadlamPickList jd where  CreatedOn>=DATEADD(DAY,-5,GETDATE()) and   Status <> 'Failed' order by Id desc", con))
                    using (SqlCommand cmd = new SqlCommand("select 'PickList-'+Cast((ROW_NUMBER() OVER(ORDER BY Id)) as nvarchar(100)) [Picklist], BatchId,CreatedOn,[Request Type],[Status] , (select count(*) from ( (SELECT t.sku,r.MaxTime FROM ( SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and [Shipping Status]<>'Shipped' and BatchId=jd.BatchId GROUP BY SKU ) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku group by t.sku,r.MaxTime having count(*)>= r.MaxTime)) as [Picked SKU])[Picked SKU], (select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and [Shipping Status]<>'Shipped' and BatchId=jd.BatchId GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] group by t.[Order Number],r.MaxTime having count(*)<= r.MaxTime)) as [Picked Orders])[Picked Orders], (select count(*) from ((select distinct pic.SKU from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%'  and [Shipping Status]<>'Shipped'and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url] )) as [Total SKUs]) [Total SKUs] , (select count(*) from ((select distinct pic.[Order Number] from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and [Shipping Status]<>'Shipped' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],[Order Number] )) as [Total Orders]) [Total Orders] from JadlamPickList jd where CreatedOn>=DATEADD(DAY,-5,GETDATE()) and Status <> 'Failed' order by Id desc", con))
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                        con.Close();

                    }

                    IList<Batches> listBatch = new List<Batches>();
                    int i = 1;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["Total SKUs"].ToString() != "0")
                        {
                            Batches batches = new Batches();
                            batches.Picklist = "Picklist-" + i.ToString();
                            batches.BatchId = r["BatchId"].ToString();
                            batches.CreatedOn = r["CreatedOn"].ToString();
                            batches.Request_Type = r["Request Type"].ToString();
                            batches.status = r["Status"].ToString();
                            batches.pickedsku = r["Picked SKU"].ToString();
                            batches.totalsku = r["Total SKUs"].ToString();
                            batches.pickedorder = r["Picked Orders"].ToString();
                            batches.totalorder = r["Total Orders"].ToString();
                            if (Convert.ToInt32(r["Picked SKU"].ToString()) == Convert.ToInt32(r["Total SKUs"].ToString()))
                            {
                                batches.status = "Complete";
                            }
                            if (Convert.ToInt32(r["Picked SKU"].ToString()) < Convert.ToInt32(r["Total SKUs"].ToString()))
                            {
                                batches.status = "In Progress";
                            }
                            if (Convert.ToInt32(r["Picked SKU"].ToString()) == 0)
                            {
                                batches.status = " Not Started";
                            }
                            i = i + 1;
                            listBatch.Add(batches);
                        }
                        if (r["Total SKUs"].ToString() == "0" && r["Status"].ToString() == "Processed")
                        {
                            Batches batches = new Batches();
                            //batches.Picklist = "Picklist-" + i.ToString();
                            batches.BatchId = r["BatchId"].ToString();
                            batches.CreatedOn = r["CreatedOn"].ToString();
                            batches.Request_Type = r["Request Type"].ToString();
                            batches.status = r["Status"].ToString();
                            batches.pickedsku = r["Picked SKU"].ToString();
                            batches.totalsku = r["Total SKUs"].ToString();
                            batches.pickedorder = r["Picked Orders"].ToString();
                            batches.totalorder = r["Total Orders"].ToString();
                            batches.status = "No EAN found!";
                            //i = i + 1;
                            listBatch.Add(batches);
                        }
                    }
                    i = i - 1;
                    foreach (var r in listBatch)
                    {
                        if (r.totalsku != "0" && r.status != "No EAN found!")
                        {

                            r.Picklist = "Picklist-" + i.ToString();
                            i = i - 1;
                        }
                    }
                    sucessresponse.Batch = listBatch;
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                if (!string.IsNullOrEmpty(BatchId))
                {
                    DataTable dt = new DataTable();

                    //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number] OrderNumber,pic.[Site Order Id] SiteOrderId,pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],count(*) [Qty to Pick],IsShippedOnCa,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId='" + BatchId + "' and  pic.SKU not like '%fba%'  and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa,pic.[Order Number],pic.[Site Order Id] order by pic.[Order Number],pic.sku ", con))
                    using (SqlCommand cmd = new SqlCommand("select distinct case when jd.[Request Type] <>'MSMQW' then '0' else pic.[Order Number] end OrderNumber , case when jd.[Request Type] <>'MSMQW' then '0' else pic.[Site Order Id] end SiteOrderId , pic.SKU,ean.Title, ean.WarehouseLocation,ean.EAN,img.[Url], Sum(Cast (Quantity as int ))  [Qty to Pick],IsShippedOnCa,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic inner join JadlamPickList jd on jd.BatchId=pic.BatchId left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId='" + BatchId + "' and pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa, jd.[Request Type], case when jd.[Request Type] <>'MSMQW' then '0' else pic.[Order Number] end, case when jd.[Request Type] <>'MSMQW' then '0' else pic.[Site Order Id] end order by pic.sku", con))
                    {
                        cmd.CommandTimeout = 0;

                        if (con.State == ConnectionState.Open)
                            con.Close();

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                        con.Close();
                    }

                    IList<Skus> listskus = new List<Skus>();
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["IsShippedOnCa"].ToString() == "false")
                        {
                            Skus sku = new Skus();
                            sku.Id = Convert.ToInt32(r["Id"].ToString());
                            sku.sku = r["SKU"].ToString();
                            sku.title = r["Title"].ToString();
                            sku.WarehouseLocation = r["WarehouseLocation"].ToString();
                            sku.OrderNumber = r["OrderNumber"].ToString();
                            sku.SiteOrderId = r["SiteOrderId"].ToString();
                            sku.ean = r["EAN"].ToString();
                            sku.url = r["Url"].ToString();
                            sku.QtyToPick = r["Qty to Pick"].ToString();
                            listskus.Add(sku);
                        }
                    }
                    sucessresponse.Sku = listskus;
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
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
    #region RESPONSE CLASSES

    public class GetPickListsucessresponse
    {
        public IList<Batches> Batch { get; set; }

        public IList<Skus> Sku { get; set; }
    }

    public class Batches
    {
        public string Picklist { get; set; }
        public string BatchId { get; set; }
        public string CreatedOn { get; set; }
        public string Request_Type { get; set; }
        public string status { get; set; }
        public string pickedsku { get; set; }
        public string totalsku { get; set; }
        public string pickedorder { get; set; }
        public string totalorder { get; set; }
        public string partialSkus { get; set; }
        public string partialOrders { get; set; }
        public string IsAlreadyOpened { get; set; }
        public string TotalWarehouseLocation { get; set; }
        public string TotalDCs { get; set; }
        public string TotalShippingClasses { get; set; }
        public string HoldCount { get; set; }
        public string NewTotalOrders { get; set; }
        public string NewUnShippedOrders { get; set; }
    }
    public class Skus
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string SiteOrderId { get; set; }
        public string sku { get; set; }
        public string title { get; set; }
        public string WarehouseLocation { get; set; }
        public string ean { get; set; }
        public string url { get; set; }
        public string QtyToPick { get; set; }
    }
    public class GetPickListfailedresponse
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}