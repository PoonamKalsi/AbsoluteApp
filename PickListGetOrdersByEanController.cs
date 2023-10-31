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

namespace AbsoluteApp
{
    public class PickListGetOrdersByEanController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(string EAN, string BatchId)
        {
            PickListGetOrdersByEanControllersucessresponse sucessresponse = new PickListGetOrdersByEanControllersucessresponse();

            if (string.IsNullOrEmpty(EAN) || string.IsNullOrEmpty(BatchId))
            {
                PickListGetOrdersByEanControllerfailedresponse failedresponse = new PickListGetOrdersByEanControllerfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = "EAN or BatchId is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
            }
            
            #region GETTING DATA FROM PROCEDURE
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
            DataTable dt = new DataTable();
            using (SqlConnection con = connection)
            {
                using (SqlCommand cmd = new SqlCommand("GetOrdersByEANOfPicklist", con))
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();
                    cmd.Parameters.AddWithValue("@Batch", BatchId);
                    cmd.Parameters.AddWithValue("@EAN", EAN);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    con.Close();
                }
            }
            #endregion

            if (dt == null || dt.Rows.Count == 0)
            {
                PickListGetOrdersByEanControllerfailedresponse failedresponse = new PickListGetOrdersByEanControllerfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = "order not found!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response1;
            }
            IList<PickListGetOrdersByEanControllerResult> rs = new List<PickListGetOrdersByEanControllerResult>();
            foreach (DataRow r in dt.Rows)
            {
                PickListGetOrdersByEanControllerResult res = new PickListGetOrdersByEanControllerResult();
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
                res.WarehouseLocation = r["WarehouseLocation"].ToString();
                res.ShippingCarrier = r["ShippingCarrier"].ToString();
                res.ShippingClass = r["ShippingClass"].ToString();
                rs.Add(res);
            }
            sucessresponse.Result = rs;

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
            return response;
        }

        #region RESPONSE CLASSES
        public class PickListGetOrdersByEanControllersucessresponse
        {
            public IList<PickListGetOrdersByEanControllerResult> Result { get; set; }
        }

        public class PickListGetOrdersByEanControllerResult
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
            public string WarehouseLocation { get; set; }
            public string ShippingCarrier { get; set; }
            public string ShippingClass { get; set; }
        }

        public class PickListGetOrdersByEanControllerfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #endregion
    }
}