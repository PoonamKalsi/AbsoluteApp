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
    public class Search2Controller : ApiController
    {

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Search(HttpRequestMessage request, string EAN,string Location, string AccountType)
        {
            sucessresponse1 sucessresponse = new sucessresponse1();

            if (string.IsNullOrEmpty(EAN)&& string.IsNullOrEmpty(Location))
            {
                failedresponse1 failedresponse = new failedresponse1();
                failedresponse.code = 0;
                failedresponse.message = "EAN and Location is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            if (string.IsNullOrEmpty(AccountType))
            {
                failedresponse1 failedresponse = new failedresponse1();
                failedresponse.code = 0;
                failedresponse.message = "Account Type is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["DBConn"].ToString();
            try
            {
                DataTable dt = new DataTable();
                if (AccountType == "Absolute")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from EAN_SKU_MAPPING1 sk left join  IMAGE_SKU_MAPPING im on sk.ID = im.ProductID  where EAN='" + EAN + "' or WarehouseLocation='" + Location + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                if (AccountType == "Jadlam")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JADLAM_EAN_SKU_MAPPING sk left join  JADLAM_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where EAN='" + EAN + "' or WarehouseLocation='" + Location + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                //added for justkeeper
                if (AccountType == "Justkeeper")
                {
                    con.ConnectionString = ConfigurationManager.AppSettings["DBConnjustkeep"].ToString();

                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JUSTKEEPERS_EAN_SKU_MAPPING sk left join  JUSTKEEPERS_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where EAN='" + EAN + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                //added for justkeeper

                if (dt == null || dt.Rows.Count == 0)
                {
                    failedresponse1 failedresponse = new failedresponse1();
                    failedresponse.code = 0;
                    failedresponse.message = "EAN not found!";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                //sucessresponse.success = "true";
                IList<Result1> rs = new List<Result1>();
                foreach (DataRow r in dt.Rows)
                {
                    Result1 res = new Result1();
                    res.Id = r["Id"].ToString();
                    res.EAN = r["EAN"].ToString();
                    res.SKU = r["SKU"].ToString();
                    res.Title = r["Title"].ToString();
                    res.WarehouseLocation = r["WarehouseLocation"].ToString();
                    res.Url = r["Url"].ToString(); 
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


        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, int Id, string Location, string AccountType)
        {
            sucessresponse1 sucessresponse = new sucessresponse1();

            if (Id == 0)
            {
                failedresponse1 failedresponse = new failedresponse1();
                failedresponse.code = 0;
                failedresponse.message = "Id is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            if (string.IsNullOrEmpty(AccountType))
            {
                failedresponse1 failedresponse = new failedresponse1();
                failedresponse.code = 0;
                failedresponse.message = "Account Type is empty!";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            //if (string.IsNullOrEmpty(Location))
            //{
            //    failedresponse1 failedresponse = new failedresponse1();
            //    failedresponse.code = 0;
            //    failedresponse.message = "Location is empty!";
            //    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            //    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            //    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
            //    return response;
            //}

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.AppSettings["DBConn"].ToString();
            try
            {
                DataTable dt = new DataTable();
                if (AccountType == "Absolute")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from EAN_SKU_MAPPING1 sk left join  IMAGE_SKU_MAPPING im on sk.ID = im.ProductID  where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                if (AccountType == "Jadlam")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JADLAM_EAN_SKU_MAPPING sk left join  JADLAM_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                //added for justkeeper
                if (AccountType == "Justkeeper")
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();

                    con.ConnectionString = ConfigurationManager.AppSettings["DBConnjustkeep"].ToString();

                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JUSTKEEPERS_EAN_SKU_MAPPING sk left join  JUSTKEEPERS_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                //added for justkeeper
                if (dt == null || dt.Rows.Count == 0)
                {
                    failedresponse1 failedresponse = new failedresponse1();
                    failedresponse.code = 0;
                    failedresponse.message = "Product not found!";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                else
                {
                    if (AccountType == "Absolute")
                    {
                        using (SqlCommand cmd = new SqlCommand("Update EAN_SKU_MAPPING1 set WarehouseLocation='" + Location + "' where Id='" + Id + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (AccountType == "Jadlam")
                    {
                        using (SqlCommand cmd = new SqlCommand("Update JADLAM_EAN_SKU_MAPPING set WarehouseLocation='" + Location + "' where Id='" + Id + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    //added for justkeeper
                    if (AccountType == "Justkeeper")
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();

                        con.ConnectionString = ConfigurationManager.AppSettings["DBConnjustkeep"].ToString();

                        using (SqlCommand cmd = new SqlCommand("Update JUSTKEEPERS_EAN_SKU_MAPPING set WarehouseLocation='" + Location + "' where Id='" + Id + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    //added for justkeeper
                }

                DataTable dt1 = new DataTable();
                if (AccountType == "Absolute")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from EAN_SKU_MAPPING1 sk left join  IMAGE_SKU_MAPPING im on sk.ID = im.ProductID  where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt1);
                    }
                }
                if (AccountType == "Jadlam")
                {
                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JADLAM_EAN_SKU_MAPPING sk left join  JADLAM_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt1);
                    }
                }
                //added for justkeeper
                if (AccountType == "Justkeeper")
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();

                    con.ConnectionString = ConfigurationManager.AppSettings["DBConnjustkeep"].ToString();

                    using (SqlCommand cmd = new SqlCommand("select ID,EAN,Sku,Title,WarehouseLocation,Url from JUSTKEEPERS_EAN_SKU_MAPPING sk left join  JUSTKEEPERS_IMAGE_SKU_MAPPING im on sk.ID = im.ProductID where Id='" + Id + "'", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt1);
                    }
                }
                //added for justkeeper
                //sucessresponse.success = "true";
                IList<Result1> rs = new List<Result1>();
                foreach (DataRow r in dt1.Rows)
                {
                    Result1 res = new Result1();
                    res.Id = r["Id"].ToString();
                    res.EAN = r["EAN"].ToString();
                    res.SKU = r["SKU"].ToString();
                    res.Title = r["Title"].ToString();
                    res.WarehouseLocation = r["WarehouseLocation"].ToString();
                    res.Url = r["Url"].ToString();
                    rs.Add(res);
                }
                sucessresponse.Result = rs;

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                failedresponse1 failedresponse = new failedresponse1();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            }


        }
    }

    #region RESPONSE CLASSES
    public class sucessresponse1
    {
        public IList<Result1> Result { get; set; }
    }

    public class Result1
    {
        public string Id { get; set; }
        public string EAN { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string WarehouseLocation { get; set; }
        public string Url { get; set; }
    }

    public class failedresponse1
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}