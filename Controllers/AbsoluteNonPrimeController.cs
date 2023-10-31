using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class AbsoluteNonPrimeController : ApiController
    {
        static string shipmentLabelPath = "";
        static string isprime = "";
        static string token = string.Empty;
        static string[] EUCountries = { "AT", "BE", "BG", "HR", "CY", "CZ", "DK", "EE", "FI", "FR", "DE", "GR", "HU", "IE", "IT", "LV", "LT", "LU", "MT", "NL", "PL", "PT", "RO", "SI", "SK", "ES", "SE", "GB", "AW", "CN", "US", "IL", "CL", "AU", "JP", "HK", "TH", "SG", "RU", "CA", "MY", "MX", "TW", "NZ", "ZA", "CH", "PE", "NO", "AF", "AL", "DZ", "AZ", "GP", "AD", "AE", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "AS", "AU", "BA", "BB", "BD", "BF", "BH", "BI", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CI", "CK", "CM", "CO", "CR", "CU", "CV", "CX", "DE", "DJ", "DM", "DO", "EC", "EG", "ER", "ET", "FJ","RE" };
        static string Ordernum = string.Empty;
        public HttpResponseMessage Get(HttpRequestMessage request, string OrderNumber)
        {
            AbsoluteNonPrimesucessresponse absoluteNonPrimesucessresponse = new AbsoluteNonPrimesucessresponse();
            shipmentLabelPath = @"H:\Applications\Absolute\Barcode Project\Project\Shipment Label";

            System.Web.HttpCookie RoyalInfo = new System.Web.HttpCookie("RoyalInfo");
            try
            {
                token = GetToken();
            }
            catch (Exception ex)
            {
                AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return failresponse;
            }

            Response response = new Response();
            response.Error = "";
            DataSet ds = new DataSet();
            Ordernum = OrderNumber;

            try
            {
                using (SqlConnection con = new myConnection().GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SP_BarcodeScanNewWithoutLine", con))
                    {
                        if (con.State == System.Data.ConnectionState.Closed)
                            con.Open();
                        cmd.Parameters.AddWithValue("@Barcode", OrderNumber.Trim());
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(ds);
                        con.Close();
                    }
                }
            }
            catch (Exception exp)
            {
                AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                failedresponse.code = 0;
                failedresponse.message = exp.Message;
                var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return failresponse;
            }
            finally
            {
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool c = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPrime"]);

                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPrime"]))
                    {
                        AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                        failedresponse.code = 0;
                        failedresponse.message = "The selected order is prime.";
                        var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                        failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                        return failresponse;
                    }
                    else
                    {
                        string distributionCenter = ds.Tables[0].Rows[0]["dc"].ToString().ToLower();

                        if (distributionCenter == "greenstock" || distributionCenter == "greentransfer" || distributionCenter == "greendropship")
                        {
                            UpdateTrackingInfo("", Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]), Convert.ToInt32(ds.Tables[0].Rows[0]["position"]), "", "");
                            response.Error = "";
                            response.OrderNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]);
                            response.QTY = Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]);
                            response.SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                            response.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                            response.TrackingNumber = "";
                            response.LabelUrl = "";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingInstructions"].ToString()) && ds.Tables[0].Rows[0]["ShippingInstructions"].ToString() == "ASDACollect")
                            {
                                AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                                failedresponse.code = 0;
                                failedresponse.message = "The order is ASDA collect.";
                                var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                                failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                                failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                return failresponse;
                            }
                            else if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LabelUrl"].ToString()))
                            {
                                string ShippingInfo = ds.Tables[0].Rows[0]["ShippingInstructions"].ToString();

                                if (!string.IsNullOrEmpty(ShippingInfo) && ShippingInfo == "ASDADelivery")
                                {
                                    AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                                    failedresponse.code = 0;
                                    failedresponse.message = "The order is ASDA delivery.";
                                    var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                                    failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                                    failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                    return failresponse;
                                }
                                else
                                {
                                    SubmitRequest(ds, ref response);
                                }

                                if (!string.IsNullOrEmpty(response.LabelUrlDownload))
                                {
                                    response.Error = "";
                                    response.OrderNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]);
                                    response.QTY = Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]);
                                    response.SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                                    response.Title = ds.Tables[0].Rows[0]["Title"].ToString();

                                    absoluteNonPrimesucessresponse.response1 = response;
                                    var response1 = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(absoluteNonPrimesucessresponse)).ToString(), Encoding.UTF8, "application/json");
                                    return response1;

                                }
                                else
                                {
                                    AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                                    failedresponse.code = 0;
                                    failedresponse.message = "Please check with developer.";
                                    var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                                    failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                                    failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                                    return failresponse;
                                }
                            }
                            else
                            {
                                response.OrderNumber = Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]);
                                response.QTY = Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]);
                                response.SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                                response.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                                response.LabelUrl = ds.Tables[0].Rows[0]["LabelUrl"].ToString();
                                response.TrackingNumber = ds.Tables[0].Rows[0]["consignment number"].ToString();

                                if (ds.Tables[0].Rows[0]["ShippingInstructions"].ToString() == "ASDADelivery")
                                {
                                    response.LabelUrlDownload = ds.Tables[0].Rows[0]["Order Number"].ToString();
                                }
                                else
                                {
                                    response.LabelUrlDownload = ds.Tables[0].Rows[0]["Order Number"].ToString() + ds.Tables[0].Rows[0]["position"].ToString();
                                }
                                absoluteNonPrimesucessresponse.response1 = response;
                                var response1 = Request.CreateResponse(System.Net.HttpStatusCode.OK);
                                response1.Headers.Add("Access-Control-Allow-Origin", "*");
                                response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(absoluteNonPrimesucessresponse)).ToString(), Encoding.UTF8, "application/json");
                                return response1;
                            }
                        }
                    }

                }
                else
                {
                    AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "OOPS! there is no any Shipment Exist Corresponding to Barcode scanned...";
                    var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                    failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return failresponse;
                }
            }
            else
            {
                AbsoluteNonPrimefailedresponse failedresponse = new AbsoluteNonPrimefailedresponse();
                failedresponse.code = 0;
                failedresponse.message = "OOPS! there is no any Shipment Exist Corresponding to Barcode scanned...";
                var failresponse = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                failresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                failresponse.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return failresponse;
            }

            AbsoluteNonPrimefailedresponse failedresponselast = new AbsoluteNonPrimefailedresponse();
            failedresponselast.code = 0;
            failedresponselast.message = "Please check with developer.";
            var failresponsea = Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            failresponsea.Headers.Add("Access-Control-Allow-Origin", "*");
            failresponsea.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponselast)).ToString(), Encoding.UTF8, "application/json");
            return failresponsea;
        }

        #region Helper Method
        private string GetToken()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            var client = new RestClient("https://api.royalmail.net/shipping/v3/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            //request.AddHeader("x-rmg-security-password", "Tortoise1!");
            request.AddHeader("x-rmg-security-password", "DisneyAC123!");
            request.AddHeader("x-rmg-security-username", "0469122000API");
            //request.AddHeader("x-ibm-client-secret", "A6eV6bT6hA6rV4bA4rU7pA1jE0tK6hS4hT1wJ4rY1nH7bK4gP0");
            request.AddHeader("x-ibm-client-secret", "yC0jM0sR5wK2hU7kD5vJ5vG6iU2kN5nY4eW7dO7mG4oT2nG0dT");
            request.AddHeader("x-ibm-client-id", "7c16aebf-f723-44ae-8de4-bb2508640d09");
            IRestResponse response = client.Execute(request);
            Token Token = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(response.Content);
            return Token.token;
        }
        public static void UpdateTrackingInfo(string consigmentNumber, int Orderid, int Position, string response, string labelUrl)
        {
            #region sync TrackingInfoToDatabase
            using (SqlConnection con = new myConnection().GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SP_SyncTrackingInfo", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (string.IsNullOrEmpty(consigmentNumber))
                        cmd.Parameters.AddWithValue("@consignmentnumber", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@consignmentnumber", consigmentNumber);
                    cmd.Parameters.AddWithValue("@OrderNumber", Orderid);
                    cmd.Parameters.AddWithValue("@Position", Position);
                    cmd.Parameters.AddWithValue("@response", response);
                    cmd.Parameters.AddWithValue("@LabelUrl", labelUrl);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            #endregion sync TrackingInfoToDatabase
        }
        public static void SubmitRequest(DataSet ds, ref Response response)
        {
            RoyalCreateShipment shipmentRequest;
            try
            {
                string countrycode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim());
                string ShippingInfo = ds.Tables[0].Rows[0]["ShippingInstructions"].ToString();

                if (countrycode == "GB")
                    shipmentRequest = RoyalMailDomesticShipmentRequest(ds);
                else
                    shipmentRequest = RoyalMailInternationalShipmentRequest(ds);

                if (shipmentRequest == null)
                    response.Error = "OOPS! some issue occurred. Please contact with developer...";
                else
                {
                    Dictionary<string, string> dicTrackingInfo = RoyalMailProcessShipment(shipmentRequest, Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]), Convert.ToInt32(ds.Tables[0].Rows[0]["position"]), ds.Tables[0].Rows[0]["ShippingInstructions"].ToString());
                    if (dicTrackingInfo.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(ShippingInfo) && ShippingInfo == "ASDADelivery")
                            response.LabelUrlDownload = "https://weblegs.info//dispatchmodule/PrintLabel.aspx?path=" + ds.Tables[0].Rows[0]["Order Number"].ToString();
                        else
                            response.LabelUrlDownload = "https://weblegs.info//dispatchmodule/PrintLabel.aspx?path=" + ds.Tables[0].Rows[0]["Order Number"].ToString() + ds.Tables[0].Rows[0]["position"].ToString();
                    }
                    else
                        response.Error = "OOPS! some issue occurred. Please contact with developer...";
                }

            }
            catch (Exception exp)
            {
                //Operations.SendMail("Princy.matridtech@gmail.com", "Absolute Barcode Project", "In SubmitRequest Function-" + Convert.ToInt32(ds.Tables[0].Rows[0]["Order Number"]) + DateTime.Now + exp.Message);
            }
        }
        public static RoyalCreateShipment RoyalMailDomesticShipmentRequest(DataSet ds)
        {
            double weight = 0;
            weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["weight"]) * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
            try
            {
                double shipping = Convert.ToDouble(ds.Tables[0].Rows[0]["shipping"]);
                #region Shipper
                Shipper shipper = new Shipper();

                shipper.AddressLine1 = "2 Sabre Way";
                shipper.AddressLine2 = "";
                shipper.CompanyName = "Absolute Cult c/o Fresh Air";
                shipper.ContactName = "Absolute Cult";
                shipper.County = "United Kingdom";
                shipper.CountryCode = "GB";
                shipper.EmailAddress = "email@absolutecult.com";
                shipper.PhoneNumber = "0333 305 8466";
                shipper.Postcode = "PE1 5EJ";
                shipper.Town = "Peterborough";
                shipper.ShipperReference = ds.Tables[0].Rows[0]["Order Number"].ToString() + ds.Tables[0].Rows[0]["Position"].ToString();
                //shipper.ShipperReference = "Test";

                #endregion Shipper            

                #region Destination
                //string name = "";
                string companyName = "";

                string firstName = "";
                string lastName = "";


                firstName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["FirstName"].ToString().Trim());
                lastName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString().Trim());

                if (!string.IsNullOrEmpty(AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CompanyName"].ToString().Trim())))
                    companyName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CompanyName"].ToString().Trim());

                Destination destination = new Destination();

                string adressline1 = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["AddressLine1"].ToString().Trim());

                if (!string.IsNullOrEmpty(adressline1) && adressline1.Length > 35)
                    destination.AddressLine1 = adressline1.Substring(0, 35);
                else
                    destination.AddressLine1 = adressline1;

                string adressline2 = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["AddressLine2"].ToString().Trim());


                if (!string.IsNullOrEmpty(adressline2) && adressline2.Length > 35)
                    destination.AddressLine2 = adressline2.Substring(0, 35);
                else if (!string.IsNullOrEmpty(adressline2) && adressline2.Length < 35)
                    destination.AddressLine2 = adressline2;


                destination.AddressLine3 = "";
                destination.CompanyName = companyName;
                destination.ContactName = firstName + " " + lastName;
                destination.County = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["RegionDescription"].ToString().Trim());
                destination.CountryCode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim());
                destination.EmailAddress = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["email"].ToString().Trim());


                string phonenumberdaywithSpecialcharacter = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PhoneNumberDay"].ToString().Trim());
                string phonenumbereveningwithSpecialcharacter = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PhoneNumberEvening"].ToString().Trim());

                if (phonenumberdaywithSpecialcharacter.Contains("ext."))
                    phonenumberdaywithSpecialcharacter = phonenumberdaywithSpecialcharacter.Replace("ext.", "");

                if (phonenumbereveningwithSpecialcharacter.Contains("ext."))
                    phonenumbereveningwithSpecialcharacter = phonenumbereveningwithSpecialcharacter.Replace("ext.", "");

                string phonenumberday = RemoveSpecialCharacters(phonenumberdaywithSpecialcharacter);
                string phonenumberevening = RemoveSpecialCharacters(phonenumbereveningwithSpecialcharacter);

                if (!string.IsNullOrEmpty(phonenumberday) && phonenumberday.Length > 15)
                    destination.PhoneNumber = phonenumberday.Substring(0, 15);
                else if (!string.IsNullOrEmpty(phonenumberevening) && phonenumberevening.Length > 15)
                    destination.PhoneNumber = phonenumberevening.Substring(0, 15);
                else
                    destination.PhoneNumber = phonenumberday;



                destination.Postcode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PostalCode"].ToString().Trim());
                string town = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["City"].ToString().Trim());

                if (!string.IsNullOrEmpty(town) && town.Length >= 35)
                    destination.Town = town.Substring(0, 30);
                else
                    destination.Town = town;


                #endregion Destination

                #region Shipment Information

                ShipmentInformation shipmentInformation = new ShipmentInformation();
                shipmentInformation.Currency = ds.Tables[0].Rows[0]["currency"].ToString();
                shipmentInformation.DescriptionOfGoods = "Clothing";
                double? itemHeight = 0.0;
                double? itemWidth = 0.0;
                double? itemLength = 0.0;
                double? itemweight = 0.0;

                double? itemvalue = 0.0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    itemHeight = itemHeight + Convert.ToDouble(row["height"]);

                    if (itemWidth < Convert.ToDouble(row["width"]))
                        itemWidth = Convert.ToDouble(row["width"]);

                    if (itemLength < Convert.ToDouble(row["length"]))
                        itemLength = Convert.ToDouble(row["length"]);

                    itemweight = itemweight + (row["weight"] == DBNull.Value ? 0 : Convert.ToDouble(row["weight"]) * Convert.ToDouble(row["Qty"]));
                }
                bool IsHoodie = false;

                List<Item> items = new List<Item>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Item item = new Item();
                    item.CountryOfOrigin = "GB";// "CN";
                    item.Description = "T Shirts";// "White Tee-shirt";
                                                  //item.HsCode = "652534";
                    item.ItemId = "d32210f0-1a0f-44bd-9728-03bdbf6e95be";// "UNIQUEID1223";
                    item.PackageOccurrence = 1;
                    item.Quantity = 1;//Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]);
                    item.SkuCode = "WLSKU";// "SKU3455692";
                    item.Value = 20.00;// 19.99;
                    itemvalue = itemvalue + item.Value;
                    //item.Weight = weight;// 0.432;
                    item.Weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(row["weight"]) * Convert.ToDouble(row["Qty"]);
                    item.Weight = item.Weight <= 135 ? 200 : item.Weight;
                    items.Add(item);
                    if (row["Classification"].ToString() == "Hoodie")
                        IsHoodie = true;

                    if (row["SIZE"].ToString() == "XXX-Large" || row["SIZE"].ToString() == "XXXX-Large" || row["SIZE"].ToString() == "XXXXX-Large")
                        IsHoodie = true;
                }
                shipmentInformation.Items = items;
                List<Package> packages = new List<Package>();

                Package package = new Package();
                package.PackageOccurrence = 1;
                package.Height = itemHeight.Value;
                package.Length = itemLength.Value;
                package.Weight = itemweight.Value;// 0.500;// (0.500 * Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]));
                package.Weight = itemweight.Value <= 135 ? 200 : itemweight.Value;// 0.500;// (0.500 * Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]));
                package.Width = itemWidth.Value;
                packages.Add(package);
                weight = itemweight.Value;
                shipmentInformation.Packages = packages;
                shipmentInformation.Product = "NDX";
                shipmentInformation.ReasonForExport = "Sale of goods";


                if (weight > 0)
                    weight = Math.Round(weight / 1000, 2);

                if (ds.Tables[0].Rows[0]["ShippingInstructions"].ToString() == "ASDADelivery" && weight <= 0.75 && Convert.ToDouble(ds.Tables[0].Rows[0]["HEIGHT"]) <= 2.5 && Convert.ToDouble(ds.Tables[0].Rows[0]["length"]) <= 35.3 && Convert.ToDouble(ds.Tables[0].Rows[0]["WIDTH"]) <= 25)
                    shipmentInformation.ServiceCode = "TPSN";
                else if (ds.Tables[0].Rows[0]["ShippingInstructions"].ToString() == "ASDADelivery")
                    shipmentInformation.ServiceCode = "TRNN";
                else if (weight < .2 && shipping < 0.1)
                    shipmentInformation.ServiceCode = "PK9";
                else if (weight >= .2 && shipping < 0.1)
                    shipmentInformation.ServiceCode = "PPF1";
                else if (weight < .2 && shipping > 0.1)
                    shipmentInformation.ServiceCode = "PPF1";
                else if (weight >= .2 && shipping > 0.1)
                    shipmentInformation.ServiceCode = "PPF1";

                if (IsHoodie)
                    shipmentInformation.ServiceCode = "PPF1";

                if (IsHoodie && ds.Tables[0].Rows[0]["Service"].ToString().ToLower().Contains("nextday"))
                {
                    shipmentInformation.ServiceCode = "TPNN";
                }
                if (!IsHoodie && ds.Tables[0].Rows[0]["Service"].ToString().ToLower().Contains("nextday"))
                {
                    shipmentInformation.ServiceCode = "TRNN";
                }


                if (IsHoodie && ds.Tables[0].Rows[0]["Sales Channel"].ToString() == "AMZUK")
                {
                    shipmentInformation.ServiceCode = "TPNN";
                }
                if (!IsHoodie && ds.Tables[0].Rows[0]["Sales Channel"].ToString() == "AMZUK")
                {
                    shipmentInformation.ServiceCode = "TRNN";
                }

                if (destination.AddressLine1.Contains("BFPO"))
                {
                    shipmentInformation.ServiceCode = "BF1";
                }

                ServiceOptions serviceOptions = new ServiceOptions();
                serviceOptions.PostingLocation = "9000513133";
                serviceOptions.LocalCollect = false;
                serviceOptions.RecordedSignedFor = false;
                serviceOptions.SaturdayGuaranteed = false;
                if (shipmentInformation.ServiceCode == "PK9")
                    serviceOptions.ServiceFormat = "F";
                else
                    serviceOptions.ServiceFormat = "P";
                serviceOptions.ServiceLevel = "01";

                shipmentInformation.ServiceOptions = serviceOptions;
                weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["weight"]) * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
                shipmentInformation.ShipmentAction = "Process";
                shipmentInformation.ShipmentDate = DateTime.Now.ToString("yyyy-MM-dd");
                shipmentInformation.TotalPackages = 1;//
                shipmentInformation.TotalWeight = itemweight.Value <= 135 ? 200 : itemweight.Value;// 0.500;// (0.500 * Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]));
                shipmentInformation.Value = itemvalue.Value;//20.00;// 19.99;// (19.99 * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]));//
                shipmentInformation.WeightUnitOfMeasure = "Grams";

                #endregion ShipmentInformation

                RoyalCreateShipment createShipment = new RoyalCreateShipment();
                createShipment.Destination = destination;
                createShipment.Shipper = shipper;
                createShipment.ShipmentInformation = shipmentInformation;

                return createShipment;

            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(@"H:\Applications\Absolute\Barcode Project\Project\log.txt", true);
                writer.WriteLine(ex.Message + DateTime.Now);
                writer.Close();
                //Operations.SendMail("Vikash.matrid289@gmail.com", "Absolute Barcode Project", "In RoyalMailDomesticShipmentRequest Function  " + DateTime.Now + ex.Message);
                return null;
            }
        }

        public static RoyalCreateShipment RoyalMailInternationalShipmentRequest(DataSet ds)
        {


            double weight = 0;
            try
            {
                double shipping = Convert.ToDouble(ds.Tables[0].Rows[0]["shipping"]);
                #region Shipper
                Shipper shipper = new Shipper();

                shipper.AddressLine1 = "2 Sabre Way";
                shipper.AddressLine2 = "";
                shipper.CompanyName = "Absolute Cult c/o Fresh Air";
                shipper.ContactName = "Absolute Cult";
                shipper.County = "United Kingdom";
                shipper.CountryCode = "GB";
                shipper.EmailAddress = "email@absolutecult.com";
                shipper.PhoneNumber = "0333 305 8466";
                shipper.Postcode = "PE1 5EJ";
                shipper.Town = "Peterborough";
                //shipper.ShipperReference = ds.Tables[0].Rows[0]["Order Number"].ToString() + ds.Tables[0].Rows[0]["Position"].ToString();
                shipper.ShipperReference = ds.Tables[0].Rows[0]["ClientOrderID"].ToString() + ds.Tables[0].Rows[0]["Position"].ToString();
                //shipper.ShipperReference = "Test";

                #endregion Shipper

                #region Destination
                //string name = "";
                string companyName = "";
                string firstName = "";
                string lastName = "";
                firstName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["FirstName"].ToString().Trim());
                lastName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString().Trim());



                // name = companyName = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FirstName"].ToString()) ? ds.Tables[0].Rows[0]["LastName"].ToString().Trim() : ds.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                // name = companyName = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FirstName"].ToString()) ? (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LastName"].ToString().Trim()) ? "" : ds.Tables[0].Rows[0]["LastName"].ToString().Trim()) : ds.Tables[0].Rows[0]["FirstName"].ToString().Trim() + (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LastName"].ToString().Trim()) ? "" : " " + ds.Tables[0].Rows[0]["LastName"].ToString().Trim());
                // name = companyName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), name.Trim());

                if (!string.IsNullOrEmpty(AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CompanyName"].ToString().Trim())))
                    companyName = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CompanyName"].ToString().Trim());

                Destination destination = new Destination();

                string adressline1 = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["AddressLine1"].ToString().Trim());

                if (!string.IsNullOrEmpty(adressline1) && adressline1.Length > 35)
                    destination.AddressLine1 = adressline1.Substring(0, 35);
                else
                    destination.AddressLine1 = adressline1;

                string adressline2 = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["AddressLine2"].ToString().Trim());


                if (!string.IsNullOrEmpty(adressline2) && adressline2.Length > 35)
                    destination.AddressLine2 = adressline2.Substring(0, 35);
                else if (!string.IsNullOrEmpty(adressline2) && adressline2.Length < 35)
                    destination.AddressLine2 = adressline2;


                destination.AddressLine3 = "";
                destination.CompanyName = companyName;
                destination.ContactName = firstName + " " + lastName;
                destination.County = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["RegionDescription"].ToString().Trim());
                destination.CountryCode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim());
                string userEmail = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["email"].ToString().Trim());
                destination.EmailAddress = userEmail;



                string phonenumberdaywithSpecialcharacter = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PhoneNumberDay"].ToString().Trim());
                string phonenumbereveningwithSpecialcharacter = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PhoneNumberEvening"].ToString().Trim());

                if (phonenumberdaywithSpecialcharacter.Contains("ext."))
                    phonenumberdaywithSpecialcharacter = phonenumberdaywithSpecialcharacter.Replace("ext.", "");

                if (phonenumbereveningwithSpecialcharacter.Contains("ext."))
                    phonenumbereveningwithSpecialcharacter = phonenumbereveningwithSpecialcharacter.Replace("ext.", "");

                string phonenumberday = RemoveSpecialCharacters(phonenumberdaywithSpecialcharacter);
                string phonenumberevening = RemoveSpecialCharacters(phonenumbereveningwithSpecialcharacter);

                if (!string.IsNullOrEmpty(phonenumberday) && phonenumberday.Length > 15)
                    destination.PhoneNumber = phonenumberday.Substring(0, 15);
                else if (!string.IsNullOrEmpty(phonenumberevening) && phonenumberevening.Length > 15)
                    destination.PhoneNumber = phonenumberevening.Substring(0, 15);
                else
                    destination.PhoneNumber = phonenumberday;



                destination.Postcode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["PostalCode"].ToString().Trim());
                string town = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["City"].ToString().Trim());

                if (!string.IsNullOrEmpty(town) && town.Length >= 35)
                    destination.Town = town.Substring(0, 30);
                else
                    destination.Town = town;

                if (destination.Town == "APO")
                {
                    destination.County = "";
                    destination.AddressLine2 = destination.AddressLine2 + AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["RegionDescription"].ToString().Trim());
                    if (destination.AddressLine2.Length > 35)
                    {
                        var addressarr = destination.AddressLine2.Split(' ').Reverse().ToArray();
                        destination.AddressLine3 = addressarr[0];
                        destination.AddressLine2 = destination.AddressLine2.Replace(destination.AddressLine3, "");
                    }

                }
                if (destination.Town == "FPO")
                {
                    destination.County = "";
                    destination.AddressLine2 = destination.AddressLine2 + AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["RegionDescription"].ToString().Trim());
                    if (destination.AddressLine2.Length > 35)
                    {
                        var addressarr = destination.AddressLine2.Split(' ').Reverse().ToArray();
                        destination.AddressLine3 = addressarr[0];
                        destination.AddressLine2 = destination.AddressLine2.Replace(destination.AddressLine3, "");
                    }

                }

                #endregion Destination

                #region Shipment Information

                ShipmentInformation shipmentInformation = new ShipmentInformation();
                shipmentInformation.Currency = ds.Tables[0].Rows[0]["currency"].ToString();
                string title = ds.Tables[0].Rows[0]["Title"].ToString();

                if (title.Length <= 70)
                    title = ds.Tables[0].Rows[0]["Title"].ToString();
                else
                    title = title.Substring(0, Math.Min(title.Length, 68));


                shipmentInformation.DescriptionOfGoods = title;

                double? itemHeight = 0.0;
                double? itemWidth = 0.0;
                double? itemLength = 0.0;
                double? itemweight = 0.0;
                double? itemvalue = 0.0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    itemHeight = itemHeight + Convert.ToDouble(row["height"]);

                    if (itemWidth < Convert.ToDouble(row["width"]))
                        itemWidth = Convert.ToDouble(row["width"]);

                    if (itemLength < Convert.ToDouble(row["length"]))
                        itemLength = Convert.ToDouble(row["length"]);

                    itemweight = itemweight + (row["weight"] == DBNull.Value ? 0 : Convert.ToDouble(row["weight"]) * Convert.ToDouble(row["Qty"]));
                }
                List<Item> items = new List<Item>();
                bool IsHoodie = false;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Item item = new Item();
                    item.CountryOfOrigin = "GB";
                    item.Description = "T Shirts";
                    //item.HsCode = "652534";
                    item.HsCode = "6109100010";//"652534";
                    item.ItemId = "d32210f0-1a0f-44bd-9728-03bdbf6e95be";
                    item.PackageOccurrence = 1;
                    item.Quantity = 1;//Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]);
                    item.SkuCode = "WLSKU";

                    item.Value = 20.00;
                    itemvalue = itemvalue + item.Value;
                    item.Weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(row["weight"]) * Convert.ToDouble(row["Qty"]); //0.432;
                    item.Weight = item.Weight <= 135 ? 200 : item.Weight;
                    items.Add(item);
                    if (row["Classification"].ToString() == "Hoodie")
                        IsHoodie = true;

                    if (row["SIZE"].ToString() == "XXX-Large" || row["SIZE"].ToString() == "XXXX-Large" || row["SIZE"].ToString() == "XXXXX-Large")
                        IsHoodie = true;
                }

                shipmentInformation.Items = items;
                weight = itemweight.Value; // added on 21 june 2023
                weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["weight"]) * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
                List<Package> packages = new List<Package>();
                //Point to Discuss
                Package package = new Package();
                package.PackageOccurrence = 1;
                package.Height = itemHeight.Value;
                package.Length = itemLength.Value;


                package.Weight = itemweight.Value;
                package.Weight = package.Weight <= 135 ? 200 : package.Weight; //Resolving weight =135 issue for ASDA orders
                package.Width = itemWidth.Value;

                packages.Add(package);

                shipmentInformation.Packages = packages;
                shipmentInformation.Product = "NDX";
                shipmentInformation.ReasonForExport = "Sale of goods";
                shipmentInformation.ServiceCode = "";
                string countryCode = AesOperation.DecryptString(ConfigurationManager.AppSettings["key"].ToString(), ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim());

                weight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["weight"]) * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
                weight = itemweight.Value;
                if (weight > 0)
                    weight = Math.Round(weight / 1000, 2);

                if (countryCode.ToUpper() == "US" || countryCode.ToUpper() == "CA" ||
                   countryCode.ToUpper() == "AU" || countryCode.ToUpper() == "NZ")
                {
                    shipmentInformation.ServiceCode = "DW1";
                }
                else if (countryCode.ToUpper() == "DE" || countryCode.ToUpper() == "FR" || countryCode.ToUpper() == "IT" || countryCode.ToUpper() == "ES" || countryCode.ToUpper() == "AT" || countryCode.ToUpper() == "BG" || countryCode.ToUpper() == "HR"
                 || countryCode.ToUpper() == "CY"
                 || countryCode.ToUpper() == "CZ"
                 || countryCode.ToUpper() == "DK"
                 || countryCode.ToUpper() == "EE"
                 || countryCode.ToUpper() == "FI"
                 || countryCode.ToUpper() == "FR"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "IT"
                 || countryCode.ToUpper() == "BE"
                 || countryCode.ToUpper() == "LV"
                 || countryCode.ToUpper() == "LT"
                 || countryCode.ToUpper() == "LU"
                  || countryCode.ToUpper() == "MT"
                 || countryCode.ToUpper() == "NL"
                 || countryCode.ToUpper() == "PL"
                 || countryCode.ToUpper() == "PT"
                 || countryCode.ToUpper() == "RO"
                 || countryCode.ToUpper() == "SK"
                 || countryCode.ToUpper() == "SI"
                 || countryCode.ToUpper() == "ES"
                 || countryCode.ToUpper() == "SE")
                {
                    if (weight < .2)
                        shipmentInformation.ServiceCode = "DG4";
                    else if (weight >= .2)
                        shipmentInformation.ServiceCode = "DW1";

                    if (IsHoodie)
                        shipmentInformation.ServiceCode = "DW1";
                }
                else if (countryCode.ToUpper() == "HK" || countryCode.ToUpper() == "IL" || countryCode.ToUpper() == "CL" || countryCode.ToUpper() == "JP"
                   || countryCode.ToUpper() == "CN"
                   || countryCode.ToUpper() == "SG"
                   || countryCode.ToUpper() == "TR"
                   || countryCode.ToUpper() == "RU"
                   || countryCode.ToUpper() == "TH"
                   || countryCode.ToUpper() == "MY"
                   || countryCode.ToUpper() == "KR"
                   || countryCode.ToUpper() == "MX"
                   || countryCode.ToUpper() == "TW"

                   )
                {
                    shipmentInformation.ServiceCode = "IE1";
                }
                else
                {
                    if (weight < .2)
                        shipmentInformation.ServiceCode = "IG1";
                    else if (weight >= .2)
                        shipmentInformation.ServiceCode = "IE1";
                }

                ServiceOptions serviceOptions = new ServiceOptions();
                //serviceOptions.PostingLocation = "9000474016";
                serviceOptions.PostingLocation = "9000513133";
                serviceOptions.LocalCollect = false;
                serviceOptions.RecordedSignedFor = false;
                serviceOptions.SaturdayGuaranteed = false;

                if (shipmentInformation.ServiceCode == "DW1" || shipmentInformation.ServiceCode == "IE1")
                    serviceOptions.ServiceFormat = "P";
                else
                    serviceOptions.ServiceFormat = "F";

                serviceOptions.ServiceLevel = "01";

                shipmentInformation.ServiceOptions = serviceOptions;

                shipmentInformation.ShipmentAction = "Process";

                DateTime dt = DateTime.Now;
                string s = dt.ToString("yyyy-MM-dd");
                shipmentInformation.ShipmentDate = DateTime.Now.ToString("yyyy-MM-dd");
                shipmentInformation.TotalPackages = 1;//

                //shipmentInformation.TotalWeight = ds.Tables[0].Rows[0]["weight"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables[0].Rows[0]["weight"]) * Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]);
                shipmentInformation.TotalWeight = itemweight.Value;
                shipmentInformation.TotalWeight = shipmentInformation.TotalWeight <= 135 ? 200 : shipmentInformation.TotalWeight;
                shipmentInformation.Value = itemvalue.Value;//20.00;// (19.99 * Convert.ToInt32(ds.Tables[0].Rows[0]["QTY"]));
                shipmentInformation.WeightUnitOfMeasure = "Grams";



                #endregion ShipmentInformation


                #region  custominfo
                CustomsInformation customsInformation = new CustomsInformation();

                string MarketPlace = ds.Tables[0].Rows[0]["Sales Channel"].ToString();
                //Add logis for PreRegistrationNumber && PreRegistrationType




                if ((MarketPlace == "AMZFR" || MarketPlace == "AMZIT" || MarketPlace == "AMZES" || userEmail.Contains("marketplace.amazon.es") || userEmail.Contains("marketplace.amazon.fr") || userEmail.Contains("marketplace.amazon.it")) && (countryCode.ToUpper() == "AT" || countryCode.ToUpper() == "BE" || countryCode.ToUpper() == "BG" || countryCode.ToUpper() == "HR"
                 || countryCode.ToUpper() == "CY"
                 || countryCode.ToUpper() == "CZ"
                 || countryCode.ToUpper() == "DK"
                 || countryCode.ToUpper() == "EE"
                 || countryCode.ToUpper() == "FI"
                 || countryCode.ToUpper() == "FR"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "BE"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "IT"
                 || countryCode.ToUpper() == "LV"
                 || countryCode.ToUpper() == "LT"
                 || countryCode.ToUpper() == "LU"
                 || countryCode.ToUpper() == "MT"
                 || countryCode.ToUpper() == "NL"
                 || countryCode.ToUpper() == "PL"
                 || countryCode.ToUpper() == "PT"
                 || countryCode.ToUpper() == "RO"
                 || countryCode.ToUpper() == "SK"
                 || countryCode.ToUpper() == "SI"
                 || countryCode.ToUpper() == "ES"
                 || countryCode.ToUpper() == "SE"))
                {
                    shipmentInformation.Incoterms = "DDU";
                    customsInformation.PreRegistrationNumber = "IM4420001201";
                    customsInformation.PreRegistrationType = "PRS";
                }
                else if ((MarketPlace == "EBAYFR" || MarketPlace == "EBAYIT" || MarketPlace == "EBAYES") && (countryCode.ToUpper() == "AT" || countryCode.ToUpper() == "BE" || countryCode.ToUpper() == "BG" || countryCode.ToUpper() == "HR"
                 || countryCode.ToUpper() == "CY"
                 || countryCode.ToUpper() == "CZ"
                 || countryCode.ToUpper() == "DK"
                 || countryCode.ToUpper() == "EE"
                 || countryCode.ToUpper() == "FI"
                 || countryCode.ToUpper() == "FR"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "GR"
                 || countryCode.ToUpper() == "HU"
                 || countryCode.ToUpper() == "IE"
                 || countryCode.ToUpper() == "BE"
                 || countryCode.ToUpper() == "IT"
                 || countryCode.ToUpper() == "LV"
                 || countryCode.ToUpper() == "LT"
                 || countryCode.ToUpper() == "LU"
                 || countryCode.ToUpper() == "MT"
                 || countryCode.ToUpper() == "NL"
                 || countryCode.ToUpper() == "PL"
                 || countryCode.ToUpper() == "PT"
                 || countryCode.ToUpper() == "RO"
                 || countryCode.ToUpper() == "SK"
                 || countryCode.ToUpper() == "SI"
                 || countryCode.ToUpper() == "ES"
                 || countryCode.ToUpper() == "SE"))
                {
                    shipmentInformation.Incoterms = "DDU";
                    customsInformation.PreRegistrationNumber = "IM2760000742";
                    customsInformation.PreRegistrationType = "PRS";
                }


                // customsInformation.InvoiceNumber = ds.Tables[0].Rows[0]["Order Number"].ToString() + ds.Tables[0].Rows[0]["Position"].ToString();
                customsInformation.InvoiceNumber = ds.Tables[0].Rows[0]["ClientOrderID"].ToString() + ds.Tables[0].Rows[0]["Position"].ToString();

                customsInformation.InvoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
                customsInformation.ShippingCharges = "0.00";
                customsInformation.QuotedLandedCost = "20";

                #endregion custominfo
                RoyalCreateShipment createShipment = new RoyalCreateShipment();
                createShipment.Destination = destination;
                createShipment.Shipper = shipper;
                createShipment.ShipmentInformation = shipmentInformation;
                createShipment.customsInformation = customsInformation;


                return createShipment;

            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(@"H:\Applications\Absolute\Barcode Project\Project\log.txt", true);
                writer.WriteLine(ex.Message + DateTime.Now);
                writer.Close();
                //Operations.SendMail("Princy.matridtech@gmail.com", "Absolute Barcode Project", "In RoyalMailInternationalShipmentRequest Function  " + DateTime.Now + ex.Message);
                return null;
            }
        }

        public static Dictionary<string, string> RoyalMailProcessShipment(RoyalCreateShipment shipment, int OrderId, int Position, string OrderType)
        {

            string path = string.Empty;
            string TrackingID = string.Empty;
            String ShipmentID = string.Empty;
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                IRestResponse response = Shipment(shipment);
                IRestResponse documentresponse = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var res = JsonConvert.DeserializeObject<ShipmentReponse>(response.Content);

                    TrackingID = res.Packages[0].TrackingNumber;///res.properties.ConsignmentNumber.ToString();
                    if (EUCountries.Contains(shipment.Destination.CountryCode.ToUpper()) && shipment.Destination.CountryCode.ToUpper() != "GB")
                    {
                        PrintDocument pd = new PrintDocument();
                        pd.DocumentType = "CN23";
                        documentresponse = PrintLabelDocument(pd, res.Packages[0].UniqueId);

                        if (documentresponse.Content != null)
                        {
                            PrintDocumentResponse printresponse = JsonConvert.DeserializeObject<PrintDocumentResponse>(documentresponse.Content);
                            if (printresponse.DocumentType.Contains("CN23"))
                            {
                                if (!string.IsNullOrEmpty(OrderType) && OrderType == "ASDADelivery")
                                {
                                    if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString()))
                                        System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString());

                                    path = shipmentLabelPath + "/" + OrderId.ToString() + "/";
                                    byte[] bytes = Convert.FromBase64String(printresponse.DocumentImage);
                                    File.WriteAllBytes(path + OrderId + "cn23.pdf", bytes);
                                }
                                else
                                {
                                    if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString()))
                                        System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString());

                                    path = shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString() + "/";
                                    byte[] bytes = Convert.FromBase64String(printresponse.DocumentImage);
                                    File.WriteAllBytes(path + OrderId + Position.ToString() + "cn23.pdf", bytes);
                                }
                            }
                        }
                    }

                    if (res.LabelImageFormat.Contains("PDF"))
                    {
                        if (!string.IsNullOrEmpty(OrderType) && OrderType == "ASDADelivery")
                        {
                            if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString()))
                                System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString());

                            path = shipmentLabelPath + "/" + OrderId.ToString() + "/";
                            byte[] bytes = Convert.FromBase64String(res.LabelImages);
                            File.WriteAllBytes(path + OrderId + ".pdf", bytes);
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString()))
                                System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString());

                            path = shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString() + "/";
                            byte[] bytes = Convert.FromBase64String(res.LabelImages);
                            File.WriteAllBytes(path + OrderId + Position.ToString() + ".pdf", bytes);
                        }
                    }
                    else if (res.LabelImageFormat.Contains("PNG"))
                    {
                        if (!string.IsNullOrEmpty(OrderType) && OrderType == "ASDADelivery")
                        {
                            if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString()))
                                System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString());
                            path = shipmentLabelPath + "/" + OrderId.ToString() + "/";
                            byte[] bytes = Convert.FromBase64String(res.LabelImages);
                            File.WriteAllBytes(path + OrderId + ".png", bytes);
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString()))
                                System.IO.Directory.CreateDirectory(shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString());
                            path = shipmentLabelPath + "/" + OrderId.ToString() + Position.ToString() + "/";
                            byte[] bytes = Convert.FromBase64String(res.LabelImages);
                            File.WriteAllBytes(path + OrderId + Position.ToString() + ".png", bytes);
                        }
                    }

                    UpdateTrackingInfo(TrackingID, OrderId, Position, res.Message, res.Packages[0].TrackingUrl);

                    result.Add(res.Packages[0].TrackingUrl, path);
                }
            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(@"H:\Applications\Absolute\Barcode Project\Project\log.txt", true);
                writer.WriteLine(ex.Message + DateTime.Now);
                writer.Close();
                //Operations.SendMail("Princy.matridtech@gmail.com", "Absolute Barcode Project", "In RoyalMailProcessShipment Function " + DateTime.Now + ex.Message);
            }
            return result;
        }

        public static IRestResponse Shipment(RoyalCreateShipment shipment)
        {
            var json = JsonConvert.SerializeObject(shipment, Newtonsoft.Json.Formatting.Indented,
                          new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            var client = new RestClient("https://api.royalmail.net/shipping/v3/shipments");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rmg-auth-token", token);
            request.AddHeader("x-ibm-client-id", "7c16aebf-f723-44ae-8de4-bb2508640d09");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                SendMail(Ordernum + " " + response.Content);
            }
            return response;
        }
        public static IRestResponse PrintLabelDocument(PrintDocument printDocument, string ShipmentId)
        {
            var json = JsonConvert.SerializeObject(printDocument, Newtonsoft.Json.Formatting.Indented,
                          new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            var client = new RestClient("https://api.royalmail.net/shipping/v3/shipments/" + ShipmentId + "/printDocument");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rmg-auth-token", token);
            request.AddHeader("x-ibm-client-id", "7c16aebf-f723-44ae-8de4-bb2508640d09");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }
        public static bool SendMail(string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("relay-hosting.secureserver.net");
                SmtpClient SmtpServer = new SmtpClient("smtpout.secureserver.net");
                mail.From = new MailAddress("hello@matridtech.net", "Absolute Barcode Scan -Non Prime");
                mail.To.Add("poonam.matrid341@gmail.com");
                mail.To.Add("chris.millar@weblegs.co.uk");
                //mail.To.Add("jaspreet.matrid8899@gmail.com");
                mail.Subject = "Error Mail";
                mail.Body = body;
                //mail.IsBodyHtml = true;
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(reportpath);
                //mail.Attachments.Add(attachment);
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("hello@matridtech.net", "Inform@2020*");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
            }
            return true;
        }
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Helper class
        public class Shipper
        {
            public string AddressId { get; set; }
            public string ShipperReference { get; set; }
            public string ShipperDepartment { get; set; }
            public string CompanyName { get; set; }
            public string ContactName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string Town { get; set; }
            public string County { get; set; }
            public string CountryCode { get; set; }
            public string Postcode { get; set; }
            public string PhoneNumber { get; set; }
            public string EmailAddress { get; set; }
            public string VatNumber { get; set; }
        }

        public class Destination
        {
            public string AddressId { get; set; }
            public string CompanyName { get; set; }
            public string ContactName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string Town { get; set; }
            public string County { get; set; }
            public string CountryCode { get; set; }
            public string Postcode { get; set; }
            public string PhoneNumber { get; set; }
            public string EmailAddress { get; set; }
            public string VatNumber { get; set; }
        }

        public class ServiceOptions
        {
            public string PostingLocation { get; set; }
            public string ServiceLevel { get; set; }
            public string ServiceFormat { get; set; }
            public string Safeplace { get; set; }
            public bool SaturdayGuaranteed { get; set; }
            public string ConsequentialLoss { get; set; }
            public bool LocalCollect { get; set; }
            public string TrackingNotifications { get; set; }
            public bool RecordedSignedFor { get; set; }
        }
        public class Items
        {
            public string value { get; set; }
        }
        public partial class Package
        {
            public int PackageOccurrence { get; set; }
            public string PackagingId { get; set; }
            public double Weight { get; set; }
            public double Length { get; set; }//changes done in datatype
            public double Width { get; set; }
            public double Height { get; set; }
        }
        public class Packages
        {
            public string description { get; set; }
            public string type { get; set; }
            public Items items { get; set; }
            //public Xml xml { get; set; }
        }
        public class Xml3
        {
            public string name { get; set; }
        }
        public class Item
        {
            public string ItemId { get; set; }
            public int Quantity { get; set; }
            public string Description { get; set; }
            public double Value { get; set; }
            public double Weight { get; set; }
            public int PackageOccurrence { get; set; }
            public string HsCode { get; set; }
            public string SkuCode { get; set; }
            public string CountryOfOrigin { get; set; }
            public string ImageUrl { get; set; }
        }

        public class ShipmentInformation
        {
            public string ShipmentDate { get; set; }
            public string ServiceCode { get; set; }
            public ServiceOptions ServiceOptions { get; set; }
            public int TotalPackages { get; set; }
            public string Incoterms { get; set; }
            public double TotalWeight { get; set; }
            public string WeightUnitOfMeasure { get; set; }
            public string Product { get; set; }
            public string DescriptionOfGoods { get; set; }
            public string ReasonForExport { get; set; }
            public double Value { get; set; }
            public string Currency { get; set; }
            public string LabelFormat { get; set; }
            public string SilentPrintProfile { get; set; }
            public string ShipmentAction { get; set; }
            public List<Package> Packages { get; set; }
            public List<Item> Items { get; set; }
        }

        public class RoyalCreateShipment
        {
            public Shipper Shipper { get; set; }
            public Destination Destination { get; set; }
            public ShipmentInformation ShipmentInformation { get; set; }
            public CustomsInformation customsInformation { get; set; }
        }
        public class CustomsInformation
        {
            public string InvoiceNumber { get; set; }
            public string InvoiceDate { get; set; }
            public string QuotedLandedCost { get; set; }
            public string ShippingCharges { get; set; }
            public string PreRegistrationNumber { get; set; }
            public string PreRegistrationType { get; set; }

        }
        public class ConsignmentNumber
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class ConsignmentTrackingUrl
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class LabelImageFormat
        {
            public string description { get; set; }
            public List<string> @enum { get; set; }
            public string type { get; set; }
        }

        public class LabelImages
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class CustomsDocuments
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class ReturnLabelImageFormat
        {
            public string description { get; set; }
            public List<string> @enum { get; set; }
            public string type { get; set; }
        }

        public class ReturnLabelImages
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class HttpStatusCode
        {
            public string format { get; set; }
            public string description { get; set; }
            public string type { get; set; }
            public int example { get; set; }
        }

        public class HttpStatusDescription
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class Message
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }


        public class Errors
        {
            public string description { get; set; }
            public string type { get; set; }
            //  public Items2 items { get; set; }
            //  public Xml2 xml { get; set; }
        }

        public class Properties
        {
            public ConsignmentNumber ConsignmentNumber { get; set; }
            public ConsignmentTrackingUrl ConsignmentTrackingUrl { get; set; }
            public Packages Packages { get; set; }
            public LabelImageFormat LabelImageFormat { get; set; }
            public LabelImages LabelImages { get; set; }
            public CustomsDocuments CustomsDocuments { get; set; }
            public ReturnLabelImageFormat ReturnLabelImageFormat { get; set; }
            public ReturnLabelImages ReturnLabelImages { get; set; }
            public HttpStatusCode HttpStatusCode { get; set; }
            public HttpStatusDescription HttpStatusDescription { get; set; }
            public Message Message { get; set; }
            public Errors Errors { get; set; }
        }


        public class RoyalShipmentResponse
        {
            public List<string> required { get; set; }
            public string type { get; set; }
            public Properties properties { get; set; }
            public Xml3 xml { get; set; }
        }

        public class LabelPrint
        {
            public string LabelFormat { get; set; }
            public string SilentPrintProfile { get; set; }
        }
        public class CarrierCode
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }
        public class PackageOccurance
        {
            public string format { get; set; }
            public string description { get; set; }
            public int maximum { get; set; }
            public int minimum { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class UniqueId
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public int minLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class TrackingNumber
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public int minLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class TrackingUrl
        {
            public string description { get; set; }
            public int maxLength { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class Primary2DBarcodeImage
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class Primary2DBarcodeData
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class FormattedUniqueId
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class HighVolumeBarcodeData
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class HighVolumeBarcodeImage
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class HighVolumeSortCode
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class Primary1DBarcodeData
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }

        public class Primary1DBarcodeImage
        {
            public string description { get; set; }
            public string type { get; set; }
            public string example { get; set; }
        }
        public partial class Package
        {
            // public int PackageOccurance { get; set; }
            public string UniqueId { get; set; }
            public string TrackingNumber { get; set; }
            public string TrackingUrl { get; set; }
            public string CarrierCode { get; set; }
        }

        public class ShipmentReponse
        {
            public List<Package> Packages { get; set; }
            public string LabelImageFormat { get; set; }
            public string LabelImages { get; set; }
            public int HttpStatusCode { get; set; }
            public string HttpStatusDescription { get; set; }
            public string Message { get; set; }
        }
        public class PrintDocument
        {
            public string DocumentType { get; set; }
            public string SilentPrintProfile { get; set; }
        }
        public class PrintDocumentResponse
        {
            public string ShipmentId { get; set; }
            public string DocumentType { get; set; }
            public string DocumentImage { get; set; }
            public int HttpStatusCode { get; set; }
            public string HttpStatusDescription { get; set; }
            public string Message { get; set; }
        }
        public class Response
        {
            public string Error { get; set; }
            public string TrackingNumber { get; set; }
            public string Title { get; set; }
            public string SKU { get; set; }
            public int QTY { get; set; }
            public int OrderNumber { get; set; }
            public string LabelUrl { get; set; }
            public string LabelUrlDownload { get; set; }
        }
        public class Token
        {
            public string token { get; set; }
        }
        public class AbsoluteNonPrimesucessresponse
        {
            public string message { get; set; }

            public Response response1 { get; set; }
        }
        public class AbsoluteNonPrimefailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        class myConnection
        {
            public SqlConnection GetConnection()
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConn"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                return con;
            }
        }
        public class AesOperation
        {
            public static string EncryptString(string key, string plainText)
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }

            public static string DecryptString(string key, string cipherText)
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {

                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}