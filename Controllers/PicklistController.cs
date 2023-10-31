using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace AbsoluteApp.Controllers
{
    public class PicklistController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string type, string UserId= null)
        {
            try
            {
                createpicklistsucessresponse sucessresponse = new createpicklistsucessresponse();

                //Guid gd = new Guid();
                var i = Guid.NewGuid().ToString();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                DataTable dt = new DataTable();

                //using (SqlCommand cmd = new SqlCommand("select distinct  pic.[Order Number] OrderNumber,pic.[Site Order Id] SiteOrderId,pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],count(*) [Qty to Pick],IsShippedOnCa,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id from PicklistOrdersForApp pic left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING ean on ean.SKU=pic.SKU left join [Absolute].dbo.JADLAM_IMAGE_SKU_MAPPING img on img.ProductId=ean.Id where pic.BatchId='" + BatchId + "' and  pic.SKU not like '%fba%'  and pic.[Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False') group by pic.SKU,ean.Title,ean.WarehouseLocation,ean.EAN,img.[Url],IsShippedOnCa,pic.[Order Number],pic.[Site Order Id] order by pic.[Order Number],pic.sku ", con))
                using (SqlCommand cmd = new SqlCommand("select * from JadlamPickList where Status = 'In Progress'", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //fill the dataset
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("Insert into JadlamPickList (BatchId,[Status],[Request Type],CreatedOn,CreatedByUser) values('" + i + "', 'Pending', '" + type + "', GetDate(),'"+UserId+"')", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    createpicklistfailedresponset failedresponse = new createpicklistfailedresponset();
                    failedresponse.code = 0;
                    failedresponse.message = "Picklist already running. Please wait and try again later!";
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                Process p = new Process();
                p.StartInfo.FileName = ConfigurationManager.AppSettings["ExePath"].ToString();
                p.StartInfo.Arguments= i.ToString()+" " +type+ " "+ " ";
                p.Start();

                sucessresponse.data = "Successfully created";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                createpicklistfailedresponset failedresponse = new createpicklistfailedresponset();
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

    public class createpicklistsucessresponse
    {
        public string data { get; set; }
    }

    public class createpicklistfailedresponset
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}