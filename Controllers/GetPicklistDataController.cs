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
    public class GetPicklistDataController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId)
        {
            if (string.IsNullOrEmpty(BatchId))
            {
                GetPicklistDataControllerfailedresponse failedresponse = new GetPicklistDataControllerfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = "Batch Id is null";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            else
            {
                try
                {
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());

                    DataSet ds = new DataSet();
                    using (SqlConnection con = connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("GetPicklistData", con))
                        {
                            if (con.State == System.Data.ConnectionState.Closed)
                                con.Open();
                            cmd.Parameters.AddWithValue("@Batch", BatchId);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 90;
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(ds);
                            con.Close();
                        }
                    }
                    IList<data> DCList = new List<data>();
                    IList<data> WLList = new List<data>();
                    IList<data> SCList = new List<data>();

                    int i = 1;
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable tbl in ds.Tables)
                        {
                            if (i == 1)
                            {
                                foreach (DataRow row in tbl.Rows)
                                {
                                    data d1 = new data();
                                    d1.name = row["DistributionCenterCode"].ToString();
                                    d1.count = row["count"].ToString();
                                    DCList.Add(d1);
                                }
                            }

                            if (i == 2)
                            {
                                foreach (DataRow row in tbl.Rows)
                                {
                                    data d1 = new data();
                                    d1.name = row["Warehouse Location"].ToString();
                                    d1.count = row["count"].ToString();
                                    WLList.Add(d1);
                                }
                            }

                            if (i == 3)
                            {
                                foreach (DataRow row in tbl.Rows)
                                {
                                    data d1 = new data();
                                    d1.name = row["ShippingClass"].ToString();
                                    d1.count = row["count"].ToString();
                                    SCList.Add(d1);
                                }
                            }
                            i = i + 1;
                        }
                    }

                    GetPicklistDataControllersucessresponse getPicklistDataControllersucessresponse = new GetPicklistDataControllersucessresponse();
                    getPicklistDataControllersucessresponse.DistributionCenterList = DCList;
                    getPicklistDataControllersucessresponse.WarehouseLocationList = WLList;
                    getPicklistDataControllersucessresponse.ShippingClassList = SCList;

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");

                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(getPicklistDataControllersucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                catch(Exception ex)
                {
                    GetPicklistDataControllerfailedresponse failedresponse = new GetPicklistDataControllerfailedresponse();
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
        public class GetPicklistDataControllersucessresponse
        {
            public IList<data> DistributionCenterList { get; set; }
            public IList<data> WarehouseLocationList { get; set; }
            public IList<data> ShippingClassList { get; set; }
        }

        public class data
        {
            public string name { get; set; }
            public string count { get; set; }
        }
        public class GetPicklistDataControllerfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #endregion
    }
}