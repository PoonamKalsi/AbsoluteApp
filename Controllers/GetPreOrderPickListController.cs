﻿using Newtonsoft.Json.Linq;
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
    public class GetPreOrderPickListController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                GetPickListsucessresponse sucessresponse = new GetPickListsucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='In Progress'where BatchId in ( select BatchId from JadlamPickList where [Request Type]<>'MSMQW' AND PickedSKUs<>0 and PickedSKUs< TotalSkus and PickListStatus is null )", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set  [Status To Display]='In Progress'where BatchId in (select BatchId from JadlamPickList where [Request Type] = 'MSMQW' AND PickedOrders <> 0  and PickedOrders < TotalOrders and PickListStatus is null)", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='In Progress'where BatchId in ( select BatchId from JadlamPickList where [Request Type] <> 'MSMQW' AND PickedSKUs <> 0 and PickedSKUs = TotalSkus and PickListStatus is null and PartialSKUs <> 0)", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='Complete' where BatchId in ( select BatchId from JadlamPickList where [Request Type]='MSMQW' AND TotalOrders=PickedOrders and PartialOrders=0 )", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='Complete' where BatchId in ( select BatchId from JadlamPickList where [Request Type]<>'MSMQW' AND TotalSKUs=PickedSKUs and PartialSKUs=0 )", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                DataTable dt = new DataTable();

                //using (SqlCommand cmd = new SqlCommand("select 'PickList-'+Cast((ROW_NUMBER() OVER(ORDER BY Id)) as nvarchar(100)) [Picklist], BatchId,CreatedOn,[Request Type],[Status] , (select count(*) from ( (SELECT t.sku,r.MaxTime FROM ( SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY SKU ) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku group by t.sku,r.MaxTime having count(*)>= r.MaxTime)) as [Picked SKU])[Picked SKU], (select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and BatchId=jd.BatchId GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] group by t.[Order Number],r.MaxTime having count(*)<= r.MaxTime)) as [Picked Orders])[Picked Orders], (select count(*) from ((select distinct pic.SKU from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url] )) as [Total SKUs]) [Total SKUs] , (select count(*) from ((select distinct pic.[Order Number] from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId=jd.BatchId and pic.SKU not like '%fba%' and [Shipping Status]<>'Shipped' and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],[Order Number] )) as [Total Orders]) [Total Orders] from JadlamPickList jd where  CreatedOn>=DATEADD(DAY,-5,GETDATE()) and   Status <> 'Failed' order by Id desc", con))
                using (SqlCommand cmd = new SqlCommand("select * from JadlamPickList where Status = 'Processed' and BatchId like 'Pre%' order by Id desc", con))
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
                    if (r["PickListStatus"].ToString() != "No EAN found!")
                    {
                        Batches batches = new Batches();
                        batches.Picklist = "Picklist-" + i.ToString();

                        var loc = r["BatchId"].ToString().Split('/');
                        if (loc.Length > 1)
                            batches.Picklist = "Picklist-" + i.ToString() + loc[1];

                        batches.BatchId = r["BatchId"].ToString();
                        batches.CreatedOn = r["CreatedOn"].ToString();
                        batches.Request_Type = r["Request Type"].ToString();
                        batches.status = r["Status"].ToString();
                        batches.pickedsku = r["PickedSKUs"].ToString();
                        batches.totalsku = r["TotalSKUs"].ToString();
                        batches.pickedorder = r["PickedOrders"].ToString();
                        batches.totalorder = r["TotalOrders"].ToString();
                        batches.partialOrders = r["PartialOrders"].ToString();
                        batches.partialSkus = r["PartialSKUs"].ToString();
                        batches.status = r["Status To Display"].ToString();
                        batches.IsAlreadyOpened = r["IsAlreadyOpened"].ToString();
                        //if (Convert.ToInt32(r["PickedSKUs"].ToString()) == Convert.ToInt32(r["TotalSKUs"].ToString()))
                        //{
                        //    batches.status = "Complete";
                        //}
                        //if (Convert.ToInt32(r["PickedSKUs"].ToString()) < Convert.ToInt32(r["TotalSKUs"].ToString()))
                        //{
                        //    batches.status = "In Progress";
                        //}
                        //if (Convert.ToInt32(r["PickedSKUs"].ToString()) == 0)
                        //{
                        //    batches.status = " Not Started";
                        //}
                        if (loc.Length == 1)
                        {
                            i = i + 1;
                        }
                        listBatch.Add(batches);
                    }
                    else
                    {
                        Batches batches = new Batches();
                        var ifany = from myRow in dt.AsEnumerable() where myRow.Field<string>("BatchId").StartsWith(r["BatchId"].ToString()) && myRow.Field<string>("status") != "No EAN found!" select myRow;
                        if (ifany.ToList().Count() > 1)
                        {
                            batches.Picklist = "Picklist-" + i.ToString();
                        }
                        batches.BatchId = r["BatchId"].ToString();
                        batches.CreatedOn = r["CreatedOn"].ToString();
                        batches.Request_Type = r["Request Type"].ToString();
                        batches.status = r["Status"].ToString();
                        batches.pickedsku = r["PickedSKUs"].ToString();
                        batches.totalsku = r["TotalSKUs"].ToString();
                        batches.pickedorder = r["PickedOrders"].ToString();
                        batches.totalorder = r["TotalOrders"].ToString();
                        batches.partialOrders = r["PartialOrders"].ToString();
                        batches.partialSkus = r["PartialSKUs"].ToString();
                        batches.status = r["PickListStatus"].ToString();
                        batches.IsAlreadyOpened = r["IsAlreadyOpened"].ToString();
                        if (ifany.ToList().Count() > 1)
                        {
                            i = i + 1;
                        }
                        listBatch.Add(batches);
                    }
                }
                i = i - 1;
                foreach (var r in listBatch)
                {
                    //var ifany = from myRow in dt.AsEnumerable() where myRow.Field<string>("BatchId") == r.BatchId.ToString() select myRow;
                    //var ifany = from myRow in dt.AsEnumerable() where myRow.Field<string>("BatchId") == r.BatchId.ToString() select myRow;

                    //if (ifany.ToList().Count() > 1)
                    //{
                    //    r.Picklist = "Picklist-" + i.ToString();
                    //    i = i - 1;
                    //}
                    if (r.status != "No EAN found!")
                    {
                        var loc = r.BatchId.ToString().Split('/');

                        if (loc.Length > 1)
                        {
                            var PicklistId = listBatch.Where(x => x.BatchId == loc[0]).Select(x => x.Picklist).FirstOrDefault().ToString();
                            r.Picklist = PicklistId + "-" + loc[1];
                        }
                        else
                        {
                            r.Picklist = "Picklist-" + i.ToString();
                            i = i - 1;
                        }

                        //if (r.BatchId.StartsWith("Pre"))
                        //{
                        //    r.Picklist = r.Picklist + "-" + "preorder";
                       // }
                    }
                    else
                    {
                        var ifany = from myRow in dt.AsEnumerable() where myRow.Field<string>("BatchId").StartsWith(r.BatchId.ToString()) && myRow.Field<string>("status") != "No EAN found!" select myRow;
                        if (ifany.ToList().Count() > 1)
                        {
                            r.Picklist = "Picklist-" + i.ToString();
                            i = i - 1;
                        }
                    }
                }

                foreach (var r in listBatch)
                {
                    if (r.status != "No EAN found!")
                    {
                        var loc = r.BatchId.ToString().Split('/');
                        if (loc.Length > 1)
                        {
                            var PicklistId = listBatch.Where(x => x.BatchId == loc[0]).Select(x => x.Picklist).FirstOrDefault().ToString();
                            r.Picklist = PicklistId + "-" + loc[1];
                        }


                    }
                    if (r.status == "No EAN found!")
                    {
                        r.Picklist = null;
                    }
                }
                sucessresponse.Batch = listBatch;
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