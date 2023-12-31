﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;

namespace AbsoluteApp.Controllers
{
    public class GetPicklistByBatchIdController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId, string ShowPickedOrders = "false")
        {
            try
            {
                if (string.IsNullOrEmpty(BatchId))
                {
                    GetPicklistByBatchIdfailedresponse failedresponse = new GetPicklistByBatchIdfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "Batch Id is empty!";
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                #region GETTING DATA FROM PROCEDURE

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection con = connection)
                {
                    using (SqlCommand cmd = new SqlCommand("GetPickListByBatchId", con))
                    {
                        if (con.State == System.Data.ConnectionState.Closed)
                            con.Open();
                        cmd.Parameters.AddWithValue("@Batch", BatchId);
                        cmd.Parameters.AddWithValue("@ShowPickedOrders", ShowPickedOrders);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        con.Close();
                    }
                }
                #endregion
                GetPicklistByBatchIdsucessresponse getPicklistByBatchIdsucessresponse = new GetPicklistByBatchIdsucessresponse();
                IList<PickList> pickList = new List<PickList>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool issingleSku = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Type"].ToString() == "MSMQW")
                        {
                            PickList pickList1 = new PickList();
                            pickList1.Id = Convert.ToInt32(row["Id"].ToString());
                            pickList1.sku = row["SKU"].ToString();
                            pickList1.title = row["Title"].ToString();
                            pickList1.WarehouseLocation = row["WarehouseLocation"].ToString();
                            pickList1.OrderNumber = row["OrderNumber"].ToString();
                            pickList1.SiteOrderId = row["SiteOrderId"].ToString();
                            pickList1.ean = row["EAN"].ToString();
                            pickList1.url = row["Url"].ToString();
                            pickList1.QtyToPick = row["Qty to Pick"].ToString();
                            pickList1.SiteName = row["Site Name"].ToString();
                            pickList1.ShippingCarrierForMSMQW = row["ShippingCarrier"].ToString();
                            pickList1.ShippingClassForMSMQW = row["ShippingClass"].ToString();
                            pickList1.AmazonLabelPrinted= string.IsNullOrEmpty(row["ShipmentID"].ToString())?false:true;
                            pickList1.EasyPostLabelPrinted = string.IsNullOrEmpty(row["labelUrl"].ToString()) ? false : true;
                            pickList1.DistributionCenterCode = row["DistributionCenterCode"].ToString();
                            pickList1.AttEAN = string.IsNullOrEmpty(row["AttEAN"].ToString()) ? "" : row["AttEAN"].ToString();
                            pickList1.IsHold = string.IsNullOrEmpty(row["IsHold"].ToString()) ? "" : row["IsHold"].ToString();
                            pickList1.IsHold = row["IsHold"].ToString()=="false" ? "" : pickList1.IsHold;

                            IList<OrderQuantity> orderQuantities = new List<OrderQuantity>();
                            pickList1.OrderQuantity = orderQuantities;
                            pickList.Add(pickList1);
                        }
                        else
                        {
                            issingleSku = true;
                            PickList pickList1 = new PickList();
                            pickList1.Id = Convert.ToInt32(row["Id"].ToString());
                            pickList1.sku = row["SKU"].ToString();
                            pickList1.title = row["Title"].ToString();
                            pickList1.WarehouseLocation = row["WarehouseLocation"].ToString();
                            pickList1.OrderNumber = row["Orders"].ToString();
                            pickList1.SiteOrderId = row["SiteOrderId"].ToString();
                            if (BatchId.StartsWith("Shop"))
                            {
                                pickList1.ean = row["SKU"].ToString();

                            }
                            else
                            {
                                pickList1.ean = row["EAN"].ToString();
                            }
                            pickList1.url = row["Url"].ToString();
                            pickList1.QtyToPick = row["Qty to Pick"].ToString();
                            pickList1.ShippingCarrierForMSMQW = row["ShippingCarrier"].ToString();
                            pickList1.ShippingClassForMSMQW = row["ShippingClass"].ToString();
                            IList<OrderQuantity> orderQuantities = new List<OrderQuantity>();
                            foreach (var order in row["OrdersQuantity"].ToString().Split(','))
                            {
                                OrderQuantity orderQuantity = new OrderQuantity();
                                orderQuantity.OrderNumber = Convert.ToInt32(order.Split('?')[0].Trim());
                                orderQuantity.Quanity = Convert.ToInt32(order.Split('?')[1].Trim());
                                orderQuantity.ShippingClass = order.Split('?')[2].Trim();
                                orderQuantity.ShippingCarrier = order.Split('?')[3].Trim();                             
                                orderQuantity.AmazonLabel = string.IsNullOrEmpty(order.Split('?')[4].Trim()) ? false : true;
                                orderQuantity.EasyPostLabel = string.IsNullOrEmpty(order.Split('?')[5].Trim()) ? false : true;
                                orderQuantity.SiteName = order.Split('?')[6].Trim();
                                orderQuantity.SiteOrderId = order.Split('?')[7].Trim();
                                orderQuantity.DistributionCenterCode = order.Split('?')[8].Trim();
                                orderQuantity.IsHold = order.Split('?')[9].Trim();
                                orderQuantities.Add(orderQuantity);
                                    //pickList1.OrderQuantity = row["OrdersQuantity"].ToString();
                            }
                            pickList1.AmazonLabelPrinted = string.IsNullOrEmpty(row["ShipmentID"].ToString()) ? false : true;
                            pickList1.EasyPostLabelPrinted = string.IsNullOrEmpty(row["labelUrl"].ToString()) ? false : true;
                            pickList1.SiteName = row["Site Name"].ToString();
                            pickList1.OrderQuantity = orderQuantities;
                            pickList1.AttEAN = string.IsNullOrEmpty(row["AttEAN"].ToString()) ? "" : row["AttEAN"].ToString();
                            pickList.Add(pickList1);
                        }
                    }

                    getPicklistByBatchIdsucessresponse.Sku = pickList;
                    if (issingleSku)
                    {
                        getPicklistByBatchIdsucessresponse.ShippingClassForSingleSku = "";
                        getPicklistByBatchIdsucessresponse.ShippingCarrierForSingleSku = "";
                        getPicklistByBatchIdsucessresponse.OrderForSingleSku = "";
                    }

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");

                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(getPicklistByBatchIdsucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;

                }
                else
                {
                    GetPicklistByBatchIdfailedresponse failedresponse = new GetPicklistByBatchIdfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "No Orders Found!";
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception ex)
            {
                GetPicklistByBatchIdfailedresponse failedresponse = new GetPicklistByBatchIdfailedresponse();
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

    public class GetPicklistByBatchIdsucessresponse
    {
        public IList<PickList> Sku { get; set; }
        public string ShippingCarrierForSingleSku { get; set; }
        public string ShippingClassForSingleSku { get; set; }
        public string OrderForSingleSku { get; set; }
    }
    public class PickList
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
        public string ShippingCarrierForMSMQW { get; set; }
        public string ShippingClassForMSMQW { get; set; }
        public IList<OrderQuantity> OrderQuantity { get; set; }
        public bool AmazonLabelPrinted { get; set; }
        public bool EasyPostLabelPrinted { get; set; }
        public string SiteName { get; set; }
        public string DistributionCenterCode { get; set; }
        public string AttEAN { get; set; }
        public string IsHold { get; set; }

    }

    public class OrderQuantity
    {
        public int OrderNumber { get; set; }
        public int Quanity { get; set; }
        public string ShippingClass { get; set; }
        public string ShippingCarrier { get; set; }
        public bool AmazonLabel { get; set; }
        public bool EasyPostLabel { get; set; }
        public string SiteName { get; set; }
        public string SiteOrderId { get; set; }
        public string DistributionCenterCode { get; set; }
        public string IsHold { get; set; }

    }
    public class GetPicklistByBatchIdfailedresponse
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}