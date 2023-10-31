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
    public class DeleteLockController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                //using (SqlCommand cmd = new SqlCommand("select 'PickList-'+Cast((ROW_NUMBER() OVER(ORDER BY Id)) as nvarchar(100)) [Picklist], BatchId,CreatedOn,[Request Type],[Status] , (select count(*) from ( (SELECT t.sku,r.MaxTime FROM ( SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY SKU ) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku group by t.sku,r.MaxTime having count(*)>= r.MaxTime)) as [Picked SKU])[Picked SKU], (select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] group by t.[Order Number],r.MaxTime having count(*)<= r.MaxTime)) as [Picked Orders])[Picked Orders], (select count(*) from ((select distinct pic.SKU from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url] )) as [Total SKUs]) [Total SKUs] , (select count(*) from ((select distinct pic.[Order Number] from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and [Shipping Status]<>'Shipped' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],[Order Number] )) as [Total Orders]) [Total Orders] from JadlamPickList jd where  CreatedOn>=DATEADD(DAY,-5,GETDATE()) and   Status <> 'Failed' order by Id desc", con))
                using (SqlCommand cmd = new SqlCommand("Delete from LockPicklist where Id ="+id, con))
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                SplitPickListsucessresponse sucessresponse = new SplitPickListsucessresponse();
                sucessresponse.message = "Successfully deleted";
                var response1 = Request.CreateResponse(HttpStatusCode.OK);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
            }
            catch (Exception ex)
            {
                SplitPicklistfailedresponse failedresponse = new SplitPicklistfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }
        public class SplitPicklistfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        public class SplitPickListsucessresponse
        {
            public string message { get; set; }
        }
    }
}