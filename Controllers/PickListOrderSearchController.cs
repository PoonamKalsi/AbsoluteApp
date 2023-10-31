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
    public class PickListOrderSearchController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(string EAN, string BatchId)
        {
            PickListOrderSearchsucessresponse sucessresponse = new PickListOrderSearchsucessresponse();

            if (string.IsNullOrEmpty(EAN) || string.IsNullOrEmpty(BatchId))
            {
                PickListOrderSearchfailedresponse failedresponse = new PickListOrderSearchfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = "EAN or BatchId is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
            try
            {
                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand("select jdo.* from PicklistOrdersForApp jdo inner join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU = map.Sku where map.EAN = '" + EAN + "' and  BatchId='"+BatchId+"' and [Profile Id] = '73000354' and IsShippedOnCA='false'and [Order Number] not in (select Distinct [Order Number]from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and IsShippedOnCA='False')", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //fill the dataset
                    adapter.Fill(dt);
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    PickListOrderSearchfailedresponse failedresponse = new PickListOrderSearchfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "order not found!";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                IList<PickListOrderSearchResult> rs = new List<PickListOrderSearchResult>();
                foreach (DataRow r in dt.Rows)
                {
                    PickListOrderSearchResult res = new PickListOrderSearchResult();
                    res.ProfileId = r["Profile Id"].ToString();
                    res.Account = r["Account"].ToString();
                    res.OrderNumber = r["Order Number"].ToString();
                    res.SiteOrderId = r["Site Order Id"].ToString();
                    res.OrderDate = r["Order Date"].ToString();
                    res.PaymentStatus = r["Payment Status"].ToString();
                    res.PaymentDate = r["Payment Date"].ToString();
                    res.PaymentType = r["Payment Type"].ToString();
                    res.ShippingStatus = r["Shipping Status"].ToString();
                    res.ShippingDate = r["Shipping Date"].ToString();
                    res.RefundStatus = r["Refund Status"].ToString();
                    res.CheckoutStatus = r["Checkout Status"].ToString();
                    res.CheckoutDate = r["Checkout Date"].ToString();
                    res.SiteName = r["Site Name"].ToString();
                    rs.Add(res);
                }
                sucessresponse.Result = rs;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                PickListOrderSearchfailedresponse failedresponse = new PickListOrderSearchfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            }

        }


        #region RESPONSE CLASSES
        public class PickListOrderSearchsucessresponse
        {
            public IList<PickListOrderSearchResult> Result { get; set; }
        }

        public class PickListOrderSearchResult
        {
            public string ProfileId { get; set; }
            public string Account { get; set; }
            public string OrderNumber { get; set; }
            public string SiteOrderId { get; set; }
            public string OrderDate { get; set; }
            public string PaymentStatus { get; set; }
            public string PaymentDate { get; set; }
            public string PaymentType { get; set; }
            public string ShippingStatus { get; set; }
            public string ShippingDate { get; set; }
            public string RefundStatus { get; set; }
            public string CheckoutStatus { get; set; }
            public string CheckoutDate { get; set; }
            public string SiteName { get; set; }
        }

        public class PickListOrderSearchfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #endregion
    }
}