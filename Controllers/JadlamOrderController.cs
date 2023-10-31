using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class JadlamOrderController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string EAN, bool IsFiltered)
        {
            sucessresponse1 sucessresponse = new sucessresponse1();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
            try
            {
                if (string.IsNullOrEmpty(EAN))
                {
                    failedresponse1 failedresponse = new failedresponse1();
                    failedresponse.code = 0;
                    failedresponse.message = "EAN is empty";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                DataTable dt = new DataTable();
                if (IsFiltered == true)
                {

                    using (SqlCommand cmd = new SqlCommand("select Distinct[Profile Id],[Account] ,[Order Number],[Site Order Id] ,[Order Date] ,[Payment Status] ,[Payment Date] ,[Payment Type] ,[Shipping Status] ,[Shipping Date] ,[Refund Status] ,[Checkout Status] ,[Checkout Date] ,[Site Name]  from JadlamOrdersForApp jdo left join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU=map.Sku where ([Warehouse Location] like '%hc%' or  [Warehouse Location] like '%gm%') and [Payment Status]='Cleared' and [Refund Status]='NoRefunds' and [Shipping Status]='PendingShipment' and jdo.SKU not like '%fba%' and [Site Name]!='Shopify POS' and map.EAN='" + EAN + "' and IsShippedOnCA='false'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        SendErrorMail(EAN+ "- Order not found! Filtered = true");
                        failedresponse1 failedresponse = new failedresponse1();
                        failedresponse.code = 0;
                        failedresponse.message = "Order not found!";
                        //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                        var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response1.Headers.Add("Access-Control-Allow-Origin", "*");

                        response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response1;
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("select jdo.* from JadlamOrdersForApp jdo inner join [Absolute].dbo.JADLAM_EAN_SKU_MAPPING map on jdo.SKU = map.Sku where map.EAN = '"+EAN+ "' and [Profile Id] = '73000354' and IsShippedOnCA='false'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        SendErrorMail(EAN + "- Order not found! Filtered = false");
                        failedresponse1 failedresponse = new failedresponse1();
                        failedresponse.code = 0;
                        failedresponse.message = "Order not found!";
                        //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                        var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response1.Headers.Add("Access-Control-Allow-Origin", "*");

                        response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response1;
                    }
                }
                //else if(!string.IsNullOrEmpty(filter))
                //{
                //    if(filter== "06 - Hobby Co. Order")
                //    {
                //        using (SqlCommand cmd = new SqlCommand("select Distinct [Profile Id],[Account] ,[Order Number],[Site Order Id] ,[Order Date] ,[Payment Status] ,[Payment Date] ,[Payment Type] ,[Shipping Status] ,[Shipping Date] ,[Refund Status] ,[Checkout Status] ,[Checkout Date] ,[Site Name]  from JadlamOrdersForApp jdo where (cast([Shipping Date] as date)>=cast(GETDATE() as date) and cast([Shipping Date] as date)<=cast(DateAdd(DAY,1,GETDATE()) as date)) and[Warehouse Location] like '%hc%' and[Payment Status] = 'Cleared' and[Refund Status] = 'NoRefunds' and[Shipping Status] = 'PendingShipment' and jdo.SKU not like '%fba%' and[Site Name] != 'Shopify POS'", con))
                //        {
                //            if (con.State == ConnectionState.Closed)
                //                con.Open();
                //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //            //fill the dataset
                //            adapter.Fill(dt);
                //        }
                //        if (dt == null || dt.Rows.Count == 0)
                //        {
                //            failedresponse1 failedresponse = new failedresponse1();
                //            failedresponse.code = 0;
                //            failedresponse.message = "Order not found!";
                //            //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                //            var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //            response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                //            return response1;
                //        }
                //    }
                //    if (filter == "07 - Gaugemaster Order")
                //    {
                //        using (SqlCommand cmd = new SqlCommand("select Distinct [Profile Id],[Account] ,[Order Number],[Site Order Id] ,[Order Date] ,[Payment Status] ,[Payment Date] ,[Payment Type] ,[Shipping Status] ,[Shipping Date] ,[Refund Status] ,[Checkout Status] ,[Checkout Date] ,[Site Name]  from JadlamOrdersForApp jdo where (cast([Shipping Date] as date)>=cast(GETDATE() as date) and cast([Shipping Date] as date)<=cast(DateAdd(DAY,1,GETDATE()) as date)) and[Warehouse Location] like '%gm%' and[Payment Status] = 'Cleared' and[Refund Status] = 'NoRefunds' and[Shipping Status] = 'PendingShipment' and jdo.SKU not like '%fba%'", con))
                //        {
                //            if (con.State == ConnectionState.Closed)
                //                con.Open();
                //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //            //fill the dataset
                //            adapter.Fill(dt);
                //        }
                //        if (dt == null || dt.Rows.Count == 0)
                //        {
                //            failedresponse1 failedresponse = new failedresponse1();
                //            failedresponse.code = 0;
                //            failedresponse.message = "Order not found!";
                //            //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                //            var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //            response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                //            return response1;
                //        }
                //    }
                //}
                //sucessresponse.success = "true";
                IList<Result1> rs = new List<Result1>();
                foreach (DataRow r in dt.Rows)
                {
                    Result1 res = new Result1();
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
                failedresponse1 failedresponse = new failedresponse1();
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
        public class sucessresponse1
        {
            public IList<Result1> Result { get; set; }
        }

        public class Result1
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

        public class failedresponse1
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #region SEND MAIL
        public bool SendErrorMail(string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtpout.secureserver.net");

                mail.From = new MailAddress("hello@matridtech.net", "EAN scan Jadlam");
                mail.To.Add(new MailAddress("poonam.matrid341@gmail.com"));
                //mail.To.Add("jaspreet.matrid8899@gmail.com");
                mail.Subject = "Message from EAN SCAN JADLAM - " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                mail.Body = body;
                mail.IsBodyHtml = true;


                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("hello@matridtech.net", "Matrid888###");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}