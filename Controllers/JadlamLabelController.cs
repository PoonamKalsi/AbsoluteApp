using FikaAmazonAPI;
using FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment;
using FikaAmazonAPI.AmazonSpApiSDK.Models.Orders;
using Newtonsoft.Json.Linq;
using PickPackQuick;
using RestSharp;
//using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WeblegsClasses.api.channeladvisor.OrderService;
using WeblegsClasses.ChannelAdvisor;
using System.Threading.Tasks;
using EasyPost;
using EasyPost.Models.API;
using Newtonsoft.Json;

namespace AbsoluteApp.Controllers
{
    public class JadlamLabelController : ApiController
    {
        #region GLOBAL VARAIABLES
        static string token = string.Empty;
        static string shipmentLabelPath = "";
        static ShipmentRequestDetails shipmentRequestDetails;
        static string LogPath = "";
        static StreamWriter txtwriter = null;
        static string UserId = string.Empty;
        static bool ErrorAmazonShipmentRequest = false;
        #endregion
        // GET: JadlamLabel
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string OrderNumber)
        {

            // If order number is sent empty in the API
            if (string.IsNullOrEmpty(OrderNumber))
            {
                failedresponsejadlam failedresponse = new failedresponsejadlam();
                failedresponse.code = 0;
                failedresponse.message = "Order Number not sent.";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            //Check if order exists on channel and check the site name
            OrdersManager OM_jadlam;
            string[] aa = { OrderNumber };

            OM_jadlam = new OrdersManager("136fce41-90f7-4400-9eb2-5654bc7a1f99");
            OrderCriteria criteria = new OrderCriteria();
            criteria.ClientOrderIdentifierList = aa;
            criteria.PageNumberFilter = 1;
            criteria.OrderCreationFilterBeginTimeGMT = DateTime.Now.Date.AddDays(-1);  //uncomment
            criteria.PageSize = 100;
            criteria.PaymentStatusFilter = PaymentStatusCode.Cleared;
            criteria.DetailLevel = DetailLevelType.High;
            criteria.ShippingStatusFilter = ShippingStatusCode.Unshipped;

            OrderResponseItem[] orderResponses = OM_jadlam.GetOrdersList(criteria);

            if (orderResponses == null && orderResponses.Count() == 0)
            {
                failedresponsejadlam failedresponse = new failedresponsejadlam();
                failedresponse.code = 0;
                failedresponse.message = "Order doesn't exists on channel";
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            foreach(OrderResponseDetailHigh ord in orderResponses)
            {
                if (ord.PaymentInfo.PaymentType.ToLower().Contains("amazon"))
                {
                    DataSet ds = new DataSet();
                    Response responses = new Response();

                    try
                    {
                        using (SqlConnection con = GetConnection())
                        {
                            using (SqlCommand cmd = new SqlCommand("SP_BARCODESCAN", con))
                            {
                                if (con.State == System.Data.ConnectionState.Closed)
                                    con.Open();
                                cmd.Parameters.AddWithValue("@BARCODE", OrderNumber.Trim());
                                cmd.CommandType = CommandType.StoredProcedure;
                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                adp.Fill(ds);
                                con.Close();
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        failedresponsejadlam failedresponse = new failedresponsejadlam();
                        failedresponse.code = 0;
                        failedresponse.message = exp.Message;
                        //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                        var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShipmentID"].ToString()))
                        {
                            shipmentRequestDetails = AmazonShipmentRequest(ds);

                            if (!ErrorAmazonShipmentRequest)
                            {
                                responses = AmazonProcessShipment(shipmentRequestDetails, ds);
                            }
                            else
                            {
                                failedresponsejadlam failedresponse = new failedresponsejadlam();
                                failedresponse.code = 0;
                                failedresponse.message = "Error in Shipment Request Details.";
                                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                return response;
                            }
                            if (responses != null)
                            {
                                if (!String.IsNullOrEmpty(responses.OrderNumber))
                                {
                                    UpdateonCA(responses);
                                    sucessresponsejadlam sucessresponse = new sucessresponsejadlam();
                                    sucessresponse.OrderId = responses.OrderNumber;
                                    sucessresponse.TrackingId = responses.TrackingNumber;
                                    sucessresponse.ShipmentId = responses.ShipmentID;
                                    sucessresponse.LabelUrl = "https://pickpackquick.azurewebsites.net/JadlamShipmentLabel/" + responses.OrderNumber + ".png";
                                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                                    return response;
                                }
                            }
                            else
                            {
                                failedresponsejadlam failedresponse = new failedresponsejadlam();
                                failedresponse.code = 0;
                                failedresponse.message = "Error in creating shipment.";
                                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                return response;
                            }

                        }
                        else
                        {
                            if (File.Exists("H:/Websites/Absolute App Apis/jadlamlabels" + "/" + OrderNumber + ".png"))
                            {
                                sucessresponsejadlam sucessresponse = new sucessresponsejadlam();
                                sucessresponse.OrderId = OrderNumber;
                                sucessresponse.TrackingId = ds.Tables[0].Rows[0]["TrackingID"].ToString();
                                sucessresponse.ShipmentId = ds.Tables[0].Rows[0]["ShipmentID"].ToString();
                                sucessresponse.LabelUrl = "https://weblegs.info/AbsoluteApp/jadlamlabels/" + OrderNumber + ".png";
                                var response = Request.CreateResponse(HttpStatusCode.OK);
                                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                                return response;
                            }
                            else
                            {
                                failedresponsejadlam failedresponse = new failedresponsejadlam();
                                failedresponse.code = 0;
                                failedresponse.message = "Order is already shipped. Label not present in system.";
                                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                return response;
                            }

                        }
                    }
                    else
                    {
                        failedresponsejadlam failedresponse = new failedresponsejadlam();
                        failedresponse.code = 0;
                        failedresponse.message = "Order doesn't exists in our system.";
                        //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                        var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }

                    failedresponsejadlam failedresponsees = new failedresponsejadlam();
                    failedresponsees.code = 0;
                    failedresponsees.message = "Nothing happened";
                    //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponsees)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }

                else
                {
                    #region Easy Post
                    //create shipment request
                     CreateShipmentRequest(ord);
                    
                    #endregion
                }
            }

            failedresponsejadlam failedresponsen = new failedresponsejadlam();
            failedresponsen.code = 0;
            failedresponsen.message = "Order doesn't exists on channel";
            //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            var responsen = Request.CreateResponse(HttpStatusCode.InternalServerError);
            responsen.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponsen)).ToString(), Encoding.UTF8, "application/json");
            return responsen;


        }
        #region AMAZON METHODS

        public static FikaAmazonAPI.AmazonSpApiSDK.Models.Orders.Order GetOrderDetails(string amazonorderid)
        {
            List<string> amazonorder = new List<string>();
            amazonorder.Add(amazonorderid);
            AmzCreds amzCreds = new AmzCreds();
            DataSet creds = amzCreds.getkeys();
            AmazonConnection amazonConnection = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = creds.Tables[0].Rows[0]["Value"].ToString(),
                SecretKey = creds.Tables[0].Rows[1]["Value"].ToString(),
                RoleArn = creds.Tables[0].Rows[2]["Value"].ToString(),
                ClientId = creds.Tables[0].Rows[3]["Value"].ToString(),
                ClientSecret = creds.Tables[0].Rows[4]["Value"].ToString(),
                RefreshToken = creds.Tables[0].Rows[8]["Value"].ToString(),
                MarketPlace = FikaAmazonAPI.Utils.MarketPlace.UnitedKingdom
            });
            var data = amazonConnection.Orders.GetOrder(new FikaAmazonAPI.Parameter.Order.ParameterGetOrder()
            {
                OrderId = amazonorderid

            });

            return data;
        }
        public static ShipmentRequestDetails AmazonShipmentRequest(DataSet ds)
        {
            try
            {
                ShipmentRequestDetails requestDetails = new ShipmentRequestDetails();
                var now = DateTime.UtcNow;
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                var britishLocalTime = TimeZoneInfo.ConvertTime(now, timeZoneInfo);
                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ItemList items = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ItemList();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Item item1 = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Item();
                    item1.OrderItemId = ds.Tables[0].Rows[i]["OrderItemID"].ToString();
                    item1.Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                    items.Add(item1);
                }
                shipmentRequestDetails = new ShipmentRequestDetails();
                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Address address = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Address();
                address.Name = "Jadlam Racing Ltd ";
                address.AddressLine1 = "Units 3-5 Adlam Central Park";
                address.Email = "help@jadlamracing.com";
                address.City = "Glastonbury";
                address.PostalCode = "BA6 9XE";
                address.CountryCode = "GB";
                address.Phone = "01749 671809";
                address.StateOrProvinceCode = "NJ";

                PackageDimensions dimensions = new PackageDimensions();
                dimensions.Height = 46;
                dimensions.Width = 46;
                dimensions.Length = 61;
                dimensions.Unit = UnitOfLength.Centimeters;

                Weight weight = new Weight();
                weight.Unit = UnitOfWeight.G;
                weight.Value = Convert.ToDouble(500);

                ShippingServiceOptions serviceOptions = new ShippingServiceOptions();
                serviceOptions.CarrierWillPickUp = true;
                serviceOptions.DeliveryExperience = DeliveryExperienceType.DeliveryConfirmationWithoutSignature;
                CurrencyAmount currency = new CurrencyAmount();
                currency.Amount = 0.30;
                currency.CurrencyCode = "GBP";
                serviceOptions.DeclaredValue = currency;


                shipmentRequestDetails.AmazonOrderId = ds.Tables[0].Rows[0]["ClientOrderIdentifier"].ToString();
                shipmentRequestDetails.ShipDate = britishLocalTime;//latestShipDate;
                shipmentRequestDetails.ItemList = items;
                shipmentRequestDetails.ShipFromAddress = address;
                shipmentRequestDetails.PackageDimensions = dimensions;
                shipmentRequestDetails.Weight = weight;
                shipmentRequestDetails.ShippingServiceOptions = serviceOptions;
                //return shipmentRequestDetails;
            }
            catch (Exception ex)
            {
                txtwriter.WriteLine("Error found in method ShipmentRequestDetails: " + ex.Message);
                ErrorAmazonShipmentRequest = true;
            }
            return shipmentRequestDetails;
        }

        public static Response AmazonProcessShipment(ShipmentRequestDetails srd, DataSet ds)
        {
            //return null;
            Response responses = new Response();
            string ShipmentID = "";
            string TrackingID = "";
            string shipmentResponse = "";

            AmzCreds amzCreds = new AmzCreds();
            DataSet creds = amzCreds.getkeys();
            AmazonConnection amazonConnection = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = creds.Tables[0].Rows[0]["Value"].ToString(),
                SecretKey = creds.Tables[0].Rows[1]["Value"].ToString(),
                RoleArn = creds.Tables[0].Rows[2]["Value"].ToString(),
                ClientId = creds.Tables[0].Rows[3]["Value"].ToString(),
                ClientSecret = creds.Tables[0].Rows[4]["Value"].ToString(),
                RefreshToken = creds.Tables[0].Rows[8]["Value"].ToString(),
                MarketPlace = FikaAmazonAPI.Utils.MarketPlace.UnitedKingdom
            });
            FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Shipment createShipmentResponse = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Shipment();
            try
            {
                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesRequest shippingServicesRequest = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesRequest();
                shippingServicesRequest.ShipmentRequestDetails = srd;

                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesResult shippingServicesResponse = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesResult();
                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ShippingServiceList shipservicelist = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ShippingServiceList();
                shippingServicesResponse = amazonConnection.MerchantFulfillment.GetEligibleShipmentServices(shippingServicesRequest);

                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesResponse eligibleShippingServicesResult = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.GetEligibleShipmentServicesResponse();
                shipservicelist = shippingServicesResponse.ShippingServiceList;
                bool shipmentExist = false;

                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ShippingService shippingservices = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ShippingService();

                foreach (FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.ShippingService shippingService in shipservicelist)
                {

                    if (shippingService.ShippingServiceId == "ROYAL_MFN_TRACKED_24" && (ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "NextDay" || ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "SecondDay" || ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "NextDay"))
                    {
                        shippingservices = (from shp in shipservicelist.Where(n => n.ShippingServiceId == "ROYAL_MFN_TRACKED_24") select shp).FirstOrDefault();
                        shipmentExist = true;
                        CreateShipmentRequest createshipmentrequest = new CreateShipmentRequest();
                        createshipmentrequest.ShippingServiceId = shippingservices.ShippingServiceId;
                        createshipmentrequest.ShipmentRequestDetails = srd;
                        createshipmentrequest.ShippingServiceId = "ROYAL_MFN_TRACKED_24";
                        createShipmentResponse = amazonConnection.MerchantFulfillment.CreateShipment(createshipmentrequest);

                    }
                    else if (shippingService.ShippingServiceId == "ROYAL_MFN_TRACKED_24" && (ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "NextDay" || ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "SecondDay" || ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "NextDay"))
                    {
                        shippingservices = (from shp in shipservicelist.Where(n => n.ShippingServiceId == "ROYAL_MFN_TRACKED_24") select shp).FirstOrDefault();
                        shipmentExist = true;
                        CreateShipmentRequest createshipmentrequest = new CreateShipmentRequest();
                        createshipmentrequest.ShippingServiceId = shippingservices.ShippingServiceId;
                        createshipmentrequest.ShipmentRequestDetails = srd;
                        createshipmentrequest.ShippingServiceId = "ROYAL_MFN_TRACKED_24";
                        createShipmentResponse = amazonConnection.MerchantFulfillment.CreateShipment(createshipmentrequest);

                    }
                    else if (shippingService.ShippingServiceId == "ROYAL_MFN_TRACKED_24" && ds.Tables[0].Rows[0]["ShipmentServiceLevelCategory"].ToString() == "Standard")
                    {
                        shippingservices = (from shp in shipservicelist.Where(n => n.ShippingServiceId == "ROYAL_MFN_TRACKED_24") select shp).FirstOrDefault();
                        shipmentExist = true;
                        CreateShipmentRequest createshipmentrequest = new CreateShipmentRequest();
                        createshipmentrequest.ShippingServiceId = shippingservices.ShippingServiceId;
                        createshipmentrequest.ShipmentRequestDetails = srd;
                        createshipmentrequest.ShippingServiceId = "ROYAL_MFN_TRACKED_24";
                        createShipmentResponse = amazonConnection.MerchantFulfillment.CreateShipment(createshipmentrequest);

                    }

                    if (createShipmentResponse != null)
                    {
                        responses.OrderNumber = ds.Tables[0].Rows[0]["ClientOrderIdentifier"].ToString();
                        responses.ShipmentID = createShipmentResponse.ShipmentId;
                        responses.TrackingNumber = createShipmentResponse.TrackingId;
                        ShipmentID = createShipmentResponse.ShipmentId;
                        TrackingID = createShipmentResponse.TrackingId;
                        //shipmentResponse = createShipmentResponse.ResponseHeaderMetadata.ResponseContext;
                    }

                    if (shipmentExist)
                    {
                        foreach (FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.LabelFormat lblformat in shippingservices.AvailableLabelFormats)
                        {
                            if (lblformat.ToString().ToLower() == "png")
                            {
                                FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Label formatlbl = new FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment.Label();
                                formatlbl = createShipmentResponse.Label;
                                string label = formatlbl.FileContents.Contents;
                                string path = shipmentLabelPath + "/";
                                FileStream destFileStream = File.Create(path + ds.Tables[0].Rows[0]["ClientOrderIdentifier"].ToString() + ".png");
                                byte[] inputBytes = Convert.FromBase64String(label);
                                MemoryStream inputStream = new MemoryStream(inputBytes);
                                GZipStream decompressingStream = new GZipStream(inputStream, CompressionMode.Decompress);
                                int byteRead;
                                while ((byteRead = decompressingStream.ReadByte()) != -1)
                                {
                                    destFileStream.WriteByte((byte)byteRead);
                                }
                                decompressingStream.Close();
                                inputStream.Close();
                                destFileStream.Close();

                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                txtwriter.WriteLine("Error in method AmazonProcessShipment:  " + exp.Message);
            }
            return responses;
        }
        #endregion

        #region CHANNEL METHODS
        public static void UpdateonCA(Response my)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "update shipment set TrackingID=@trackid,ShipmentID=@shipmentid,RequestDate=@Requestdate where ClientOrderIdentifier=@ClientOrderIdentifier";
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.Parameters.AddWithValue("@ClientOrderIdentifier", my.OrderNumber);
                    cmd.Parameters.AddWithValue("@shipmentid", my.ShipmentID);
                    cmd.Parameters.AddWithValue("@trackid", my.TrackingNumber);
                    cmd.Parameters.AddWithValue("@Requestdate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region HELPER CLASSES
        public class Response
        {
            public string Error { get; set; }
            public string TrackingNumber { get; set; }
            public string Title { get; set; }
            public string SKU { get; set; }
            public int QTY { get; set; }
            public string ShipmentID { get; set; }
            public string OrderNumber { get; set; }
            public string LabelUrl { get; set; }
            public string FolderPath { get; set; }
        }

        public class Token
        {
            public string token { get; set; }

        }
        #endregion

        #region HELPER METHODS
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString());
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }
        #endregion

        #region RESPONSE CLASSES
        public class sucessresponsejadlam
        {
            public string OrderId { get; set; }
            public string ShipmentId { get; set; }
            public string TrackingId { get; set; }
            public string LabelUrl { get; set; }
        }


        public class failedresponsejadlam
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #endregion

        #region Easy Post API

        public async void CreateShipmentRequest(OrderResponseDetailHigh ord)
        {
            //EasyPost.Shipment shipment = new EasyPost.Shipment();

            //EasyPost.Address toAddress = new EasyPost.Address();
            //toAddress.Name = ord.ShippingInfo.FirstName + ord.ShippingInfo.LastName;
            //toAddress.Street1 = ord.ShippingInfo.AddressLine1;
            //toAddress.Street2= ord.ShippingInfo.AddressLine2;
            //toAddress.City = ord.ShippingInfo.City;
            //toAddress.State = ord.ShippingInfo.Region;
            //toAddress.Zip = ord.ShippingInfo.PostalCode;
            //toAddress.Country = ord.ShippingInfo.CountryCode;
            //toAddress.Phone = ord.ShippingInfo.PhoneNumberDay;
            //toAddress.Email = "";
            //shipment.BuyerAddress = toAddress;

            //EasyPost.Address fromAddress = new EasyPost.Address();
            //fromAddress.Name = "Jadlam Racing Ltd";
            //fromAddress.Street1 = "Units 3-5 Adlam Central Park";
            //fromAddress.Street2 = "";
            //fromAddress.City = "Glastonbury";
            //fromAddress.State = "NJ";
            //fromAddress.Zip = "BA6 9XE";
            //fromAddress.Country = "GB";
            //fromAddress.Phone = "01749 671809";
            //fromAddress.Email = "help@jadlamracing.com";
            //shipment.FromAddress = fromAddress;

            //EasyPost.Parcel parcel = new EasyPost.Parcel();
            //parcel.Length = 61;
            //parcel.Width = 46;
            //parcel.Height = 46;
            //parcel.Weight = 500;
            //shipment.Parcel = parcel;


            var client = new EasyPost.Client("EZTK68e7381dd6624905a310c833fb90502cjgu7MbZ8azuWCPTFo2pjXQ");
            EasyPost.Models.API.Shipment shipment = await client.Shipment.Create(new Dictionary<string, object>()
            {
                {
                    "to_address", new Dictionary<string, object>()
                    {
                        { "name",ord.ShippingInfo.FirstName + ord.ShippingInfo.LastName },
                        { "street1", ord.ShippingInfo.AddressLine1},
                        { "street2", ord.ShippingInfo.AddressLine2},
                        { "city", ord.ShippingInfo.City },
                        { "state", ord.ShippingInfo.Region },
                        { "zip", ord.ShippingInfo.PostalCode },
                        { "country", ord.ShippingInfo.CountryCode },
                        { "phone", ord.ShippingInfo.PhoneNumberDay },
                        { "email", "" }
                    }
                },
                {
                    "from_address", new Dictionary<string, object>()
                    {
                        { "name", "Jadlam Racing Ltd" },
                        { "street1", "Units 3-5 Adlam Central Park" },
                        { "street2", "" },
                        { "city", "Glastonbury" },
                        { "state", "NJ" },
                        { "zip", "BA6 9XE" },
                        { "country", "GB" },
                        { "phone", "01749 671809" },
                        { "email","help@jadlamracing.com" }
                    }
                },
                {
                    "parcel", new Dictionary<string, object>()
                    {
                        { "length", 61 },
                        { "width", 46 },
                        { "height", 46 },
                        { "weight", 500 }
                    }
                }
            });
            //EasyPost.Models.API.Shipment result = await client.Shipment.Create(shipment);
            var i = JsonConvert.SerializeObject(shipment, Formatting.Indented);

        }


        static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        #endregion
    }
}