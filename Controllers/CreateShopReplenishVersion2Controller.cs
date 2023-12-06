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


namespace AbsoluteApp.Controllers
{
    public class CreateShopReplenishVersion2Controller : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string data, string UserId = "")
        {
            try
            {
                ListEanQuantity listEanQuantity = new ListEanQuantity();

                try
                {
                    var serializedobj = Newtonsoft.Json.JsonConvert.DeserializeObject<ListEanQuantity>(data);
                }
                catch (Exception ex)
                {
                    CreateShopReplenishfailedresponse failedresponse = new CreateShopReplenishfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = ex.Message;
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }

                var i = "Shop" + Guid.NewGuid().ToString();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();
                DataTable dt = new DataTable();

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
                   
                        using (SqlCommand cmd = new SqlCommand("CreateShopReplenishPicklist", con))
                        {
                            if (con.State == System.Data.ConnectionState.Closed)
                                con.Open();
                            cmd.Parameters.AddWithValue("@BatchId", i);
                            cmd.Parameters.AddWithValue("@RequestType", "SIW");
                            cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                            //cmd.Parameters.AddWithValue("@skip", skip);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 90;
                            cmd.ExecuteNonQuery();
                            //SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            //adp.Fill(dt);
                            con.Close();
                        }
                   
                    //using (SqlCommand cmd = new SqlCommand("Insert into JadlamPickList (BatchId,[Status],[Request Type],CreatedOn,CreatedByUser) values('" + i + "', 'Processed', 'SIW', GetDate(),'" + UserId + "')", con))
                    //{
                    //    if (con.State == ConnectionState.Closed)
                    //        con.Open();

                    //    cmd.ExecuteNonQuery();
                    //}

                    //Getting the SKUs
                    //DataTable SKUs = new DataTable();
                    //using (SqlCommand cmd = new SqlCommand("select * from Absolute.dbo.JADLAM_EAN_SKU_MAPPING where EAN is not null", con))
                    //{
                    //    if (con.State == ConnectionState.Closed)
                    //        con.Open();
                    //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //    //fill the dataset
                    //    adapter.Fill(SKUs);
                    //}

                    var serializedobj = Newtonsoft.Json.JsonConvert.DeserializeObject<ListEanQuantity>(data);

                    foreach(var ean in serializedobj.eanQuanities)
                    {
                        //var SkuMapping = (from mapds in SKUs.AsEnumerable()
                        //                  where mapds.Field<string>("EAN") == ean.ean
                        //                  select new
                        //                  {
                        //                      ProductId = mapds.Field<int>("ID"),
                        //                      type = mapds.Field<string>("ProductType"),
                        //                      ean = mapds.Field<string>("EAN"),
                        //                      SKU = mapds.Field<string>("Sku")
                        //                  }).ToList().FirstOrDefault();

                        using (SqlCommand cmd = new SqlCommand("Insert into PicklistOrdersForApp([Profile Id],[Order Number],[Quantity],BatchId,IsShippedOnCA,[Shipping Status],ShippingCarrier,ShippingClass,SKU,[Site Name],[Site Order Id],DistributionCenterCode) values ('73000354','1234'," + ean.quantity+",'"+ i + "','false','none','','','"+ean.ean+"','','','')", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    //Process p = new Process();
                    //p.StartInfo.FileName = ConfigurationManager.AppSettings["ExePath"].ToString();
                    //p.StartInfo.Arguments = i.ToString() + " " + "SIW" + " " + data;
                    //p.Start();
                    Process p1 = new Process();
                    p1.StartInfo.FileName = @"H:\Applications\Jadlam\Picklist Orders Status Update\JadlamJITStatusUpdate.exe";
                    p1.StartInfo.Arguments = i;
                    p1.Start();
                    CreateShopReplenishsucessresponse sucessresponse = new CreateShopReplenishsucessresponse();
                    sucessresponse.message = "Successfully created";
                    var response1 = Request.CreateResponse(HttpStatusCode.OK);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
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



            }
            catch (Exception ex)
            {
                CreateShopReplenishfailedresponse failedresponse = new CreateShopReplenishfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }

        #region HELPER CLASSES
        public class CreateShopReplenishfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        public class CreateShopReplenishsucessresponse
        {
            public string message { get; set; }
        }
        public class ListEanQuantity
        {
            public IList<EanQuanity> eanQuanities { get; set; }
        }
        public class EanQuanity
        {
            public string ean { get; set; }
            public string quantity { get; set; }
        }
        #endregion
    }
}