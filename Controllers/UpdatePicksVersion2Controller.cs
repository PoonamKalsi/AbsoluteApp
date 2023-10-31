using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class UpdatePicksVersion2Controller : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request, string BatchId, string SKU, string OrderNumber, string type, bool updateall = false, string UserId = null, bool IsHold = false, bool IsAttEANIncluded = false)
        {
            try
            {
                UpdatePickssucessresponse sucessresponse = new UpdatePickssucessresponse();

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnJadlam"].ToString();

                if ((string.IsNullOrEmpty(BatchId) || string.IsNullOrEmpty(SKU)) && !updateall)
                {
                    UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
                    failedresponse.code = 0;
                    failedresponse.message = "Batch Id or SKU is empty";
                    var response1 = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response1.Headers.Add("Access-Control-Allow-Origin", "*");
                    response1.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response1;
                }
                else
                {
                    #region FOR SIW AND SSMQW
                    if (type != "MSMQW")
                    {
                        // STEP 1: Check if we need to Validate the complete picklist or not 
                        // IF WE DON'T NEED TO VALIDATE THE COMPLETE PICKLIST THEN 
                        if (!updateall)
                        {
                            // CHECK IF WE NEED TO KEEP THE ORDER ON HOLD OR NOT
                            // IN THIS CASE RAHUL WILL CHECK THE ATTRIBUTE(EAN) INCLUSION
                            //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                            // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                            if (!IsHold)
                            {
                                using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true', ValidatedByUser='" + UserId + "', IsHold='false' where [Order Number] in (" + OrderNumber + ") and  BatchId='" + BatchId + "'", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }

                            //-----------IF TO KEEP ON HOLD THEN -------------------
                            // USER ID = SENT BY RAHUL , ISHOLD= TRUE
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set  ValidatedByUser='" + UserId + "', IsHold='true' where [Order Number] in (" + OrderNumber + ") and  BatchId='" + BatchId + "'", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                        }
                        // IF WE NEED TO VALIDATE THE COMPLETE PICKLIST THEN 
                        else
                        {
                            // IN THIS CASE WE HAVE TO CHECK THE ATTRIBUTE(EAN)
                            //------------IF WE DON'T NEED TO INCLUDE THE ATTRIBUTE-------------                            
                            if (!IsAttEANIncluded)
                            {
                                //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                                // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                if (!IsHold)
                                {
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' , ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "' and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                //-----------IF TO KEEP ON HOLD THEN -------------------
                                // CASE 1:  WHOSE EAN IS EMPTY : USER ID = SENT BY RAHUL , ISHOLD= TRUE
                                // CASE 2:  WHOSE EAN IS NOT EMPTY : VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                else
                                {
                                    //-----------------------------CASE 1----------------------------
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set  ValidatedByUser='" + UserId + "', IsHold='true' where BatchId='" + BatchId + "' and [Order Number] in (select pic.[Order Number] from PicklistOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING jdean on pic.SKU=jdean.Sku where pic.BatchId='" + BatchId + "' and jdean.EAN is null )", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    //-----------------------------CASE 2----------------------------
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' , ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "' and [Order Number] in (select pic.[Order Number] from PicklistOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING jdean on pic.SKU=jdean.Sku where pic.BatchId='" + BatchId + "' and jdean.EAN is not  null  and IsHold is null)", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }

                                }
                            }

                            // ------------IF WE NEED TO INCLUDE THE ATTRIBUTE-------------
                            else
                            {
                                //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                                // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                if (!IsHold)
                                {
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true'  , ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "'  and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                //-----------IF TO KEEP ON HOLD THEN -------------------
                                // CASE 1:  WHOSE EAN IS EMPTY : USER ID = SENT BY RAHUL , ISHOLD= TRUE
                                // CASE 2:  WHOSE EAN IS NOT EMPTY : VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                else
                                {
                                    //-----------------------------CASE 1----------------------------
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set  ValidatedByUser='" + UserId + "', IsHold='true' where BatchId='" + BatchId + "' and [Order Number] in (select pic.[Order Number] from PicklistOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING jdean on pic.SKU=jdean.Sku where pic.BatchId='" + BatchId + "' and jdean.EAN is null and jdean.AttEAN is null)", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    //-----------------------------CASE 2----------------------------
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' , ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "' and [Order Number] in (select pic.[Order Number] from PicklistOrdersForApp pic inner join Absolute.dbo.JADLAM_EAN_SKU_MAPPING jdean on pic.SKU=jdean.Sku where pic.BatchId='" + BatchId + "' and jdean.EAN is not  null and jdean.AttEAN not is null  and IsHold is null))", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                            }
                        }
                        // Updating Picked SKU , Partial Picked SKU and Picked Orders -- there's no such case of partial Order in SIW and SSMQW
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PickedSKUs=( select count(*) from ( (SELECT t.sku,r.MaxTime FROM ( SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and [Shipping Status]<>'Shipped' and BatchId='" + BatchId + "' GROUP BY SKU,[Profile Id] ) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku and [Shipping Status]<>'Shipped' and BatchId='" + BatchId + "' group by t.sku,r.MaxTime having count(*)>= r.MaxTime )) as [Picked SKU] ) where BatchId='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PartialSKUs=(select count(*) from((SELECT t.sku, r.MaxTime FROM(SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA = 'true' and[Shipping Status] <> 'Shipped' and BatchId = '" + BatchId + "' GROUP BY SKU,[Profile Id]) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku and IsShippedOnCA = 'false' and[Shipping Status] <> 'Shipped' and BatchId='" + BatchId + "' group by t.sku, r.MaxTime)) as [Picked SKU]) where BatchId = '" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PickedOrders= ( select top 1 COUNT(*) OVER () AS TotalRecords from PicklistOrdersForApp pic where BatchId ='" + BatchId + "' and IsShippedOnCA='true' and [Shipping Status]<>'Shipped' group by [Order Number]) where BatchId ='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }


                    }
                    #endregion

                    #region  FOR MSMQW
                    else
                    {
                        // STEP 1: Check if we need to Validate the complete picklist or not 
                        // IF WE DON'T NEED TO VALIDATE THE COMPLETE PICKLIST THEN  
                        if (!updateall)
                        {
                            // CHECK IF WE NEED TO KEEP THE ORDER ON HOLD OR NOT
                            // IN THIS CASE RAHUL WILL CHECK THE ATTRIBUTE(EAN) INCLUSION

                            //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                            // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                            if (!IsHold)
                            {
                                using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true',  ValidatedByUser='" + UserId + "', IsHold='false' where BatchId='" + BatchId + "' and sku='" + SKU + "' and [Order Number]='" + OrderNumber + "' ", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }

                            //-----------IF TO KEEP ON HOLD THEN -------------------
                            // USER ID = SENT BY RAHUL , ISHOLD= TRUE
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set  ValidatedByUser = '" + UserId + "', IsHold = 'true' where BatchId = '" + BatchId + "' and sku = '" + SKU + "' and[Order Number] = '" + OrderNumber + "' ", con))
                                {
                                    if (con.State == ConnectionState.Closed)
                                        con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                        }

                        // IF WE NEED TO VALIDATE THE COMPLETE PICKLIST THEN 
                        else
                        {
                            // IN THIS CASE WE HAVE TO CHECK THE ATTRIBUTE(EAN)
                            //------------IF WE DON'T NEED TO INCLUDE THE ATTRIBUTE------------- 
                            if (!IsAttEANIncluded)
                            {
                                //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                                // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                if (!IsHold)
                                {
                                    using (SqlCommand cmd = new SqlCommand("UPDATE A  SET IsShippedOnCA = 'true',  ValidatedByUser='" + UserId + "', IsHold='false' from PicklistOrdersForApp A  INNER JOIN Absolute.dbo.JADLAM_EAN_SKU_MAPPING B ON A.SKU = B.Sku where   A.BatchId = '" + BatchId + "'  and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                //-----------IF TO KEEP ON HOLD THEN -------------------
                                // CASE 1:  WHOSE EAN IS EMPTY : USER ID = SENT BY RAHUL , ISHOLD= TRUE
                                // CASE 2:  WHOSE EAN IS NOT EMPTY : VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                else
                                {
                                    //----------------------CASE 1--------------------------------------
                                    using (SqlCommand cmd = new SqlCommand("UPDATE A  SET   ValidatedByUser='" + UserId + "', IsHold='true' from PicklistOrdersForApp A  INNER JOIN Absolute.dbo.JADLAM_EAN_SKU_MAPPING B ON A.SKU = B.Sku where   A.BatchId = '" + BatchId + "' and B.EAN is null ", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    //---------------------CASE 2-----------------------------------------
                                    using (SqlCommand cmd = new SqlCommand("UPDATE A  SET IsShippedOnCA = 'true',  ValidatedByUser='" + UserId + "', IsHold='false' from PicklistOrdersForApp A  INNER JOIN Absolute.dbo.JADLAM_EAN_SKU_MAPPING B ON A.SKU = B.Sku where   A.BatchId = '" + BatchId + "' and B.EAN is not null  and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                            }

                            // ------------IF WE NEED TO INCLUDE THE ATTRIBUTE-------------
                            else
                            {
                                //-----------IF NOT TO KEEP ON HOLD THEN -------------------
                                // VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                if (!IsHold)
                                {
                                    using (SqlCommand cmd = new SqlCommand("Update PicklistOrdersForApp set IsShippedOnCA = 'true' ,  ValidatedByUser='" + UserId + "' , IsHold='false' where BatchId='" + BatchId + "'  and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                //-----------IF TO KEEP ON HOLD THEN -------------------
                                // CASE 1:  WHOSE EAN IS EMPTY : USER ID = SENT BY RAHUL , ISHOLD= TRUE
                                // CASE 2:  WHOSE EAN IS NOT EMPTY : VALIDATED = TRUE , USER ID = SENT BY RAHUL , ISHOLD= FALSE
                                else
                                {
                                    //----------------------CASE 1--------------------------------------
                                    using (SqlCommand cmd = new SqlCommand("UPDATE A  SET   ValidatedByUser='" + UserId + "', IsHold='true' from PicklistOrdersForApp A  INNER JOIN Absolute.dbo.JADLAM_EAN_SKU_MAPPING B ON A.SKU = B.Sku where   A.BatchId = '" + BatchId + "' and B.EAN is null and B.AttEAN is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    //---------------------CASE 2-----------------------------------------
                                    using (SqlCommand cmd = new SqlCommand("UPDATE A  SET IsShippedOnCA = 'true',  ValidatedByUser='" + UserId + "', IsHold='false' from PicklistOrdersForApp A  INNER JOIN Absolute.dbo.JADLAM_EAN_SKU_MAPPING B ON A.SKU = B.Sku where   A.BatchId = '" + BatchId + "' and B.EAN is not null and B.AttEAN is not null  and IsHold is null", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                            }

                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PickedSKUs=(select count(*) from((SELECT t.sku, r.MaxTime FROM(SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA = 'true' and[Shipping Status] <> 'Shipped' and BatchId = '" + BatchId + "' GROUP BY SKU,[Profile Id]) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku group by t.sku, r.MaxTime having count(*) >= r.MaxTime)) as [Picked SKU]) where BatchId ='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PartialSKUs=(select count(*) from((SELECT t.sku, r.MaxTime FROM(SELECT SKU, count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA = 'true' and[Shipping Status] <> 'Shipped' and BatchId = '" + BatchId + "' GROUP BY SKU,[Profile Id]) r INNER JOIN PicklistOrdersForApp t ON t.sku = r.sku and IsShippedOnCA = 'false' and[Shipping Status] <> 'Shipped'  group by t.sku, r.MaxTime)) as [Picked SKU]) where BatchId = '" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PickedOrders =( select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and [Shipping Status]<>'Shipped' and BatchId='" + BatchId + "' GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] where IsShippedOnCA='true' group by t.[Order Number],r.MaxTime )) as [Picked Orders] ) where BatchId ='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set PartialOrders =(select count(*) from ( (SELECT t.[Order Number],r.MaxTime FROM ( SELECT [Order Number], count(*) as MaxTime FROM PicklistOrdersForApp where IsShippedOnCA='true' and [Shipping Status]<>'Shipped' and BatchId='" + BatchId + "' GROUP BY [Order Number] ) r INNER JOIN PicklistOrdersForApp t ON t.[Order Number] = r.[Order Number] where IsShippedOnCA='false' group by t.[Order Number],r.MaxTime) ) as [Picked Orders]) where BatchId ='" + BatchId + "'", con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    #endregion

                    using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='Complete' where BatchId in ( select BatchId from JadlamPickList where [Request Type]='MSMQW' AND TotalOrders=PickedOrders and PartialOrders=0 )", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }


                    using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='Complete' where BatchId in ( select BatchId from JadlamPickList where [Request Type]<>'MSMQW' AND TotalSKUs=PickedSKUs and PartialSKUs=0 )", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set  [Status To Display]='In Progress'where BatchId in (select BatchId from JadlamPickList where [Request Type] = 'MSMQW' AND PickedOrders <> 0  and PickedOrders < TotalOrders and PickListStatus is null)", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("Update JadlamPickList set [Status To Display]='In Progress'where BatchId in ( select BatchId from JadlamPickList where [Request Type] <> 'MSMQW' AND PickedSKUs <> 0 and PickedSKUs = TotalSkus and PickListStatus is null and PartialSKUs <> 0)", con))
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //#region Getting access token
                    //string accesstoken = string.Empty;
                    //Accesstoken accesstokens = GetAccessToken();
                    //accesstoken = accesstokens.access_token;
                    //#endregion


                    //#region set flags for picklist
                    //if (updateOnCA)
                    //{
                    //    foreach (var i in OrderNumber.Split(','))
                    //    {
                    //        string ordnum = string.Empty;
                    //        var data = new Dictionary<string, string>
                    //        {
                    //            {"FlagID","5"},
                    //            {"FlagDescription","Picklist Validated"}
                    //        };
                    //        var jsonData = JsonConvert.SerializeObject(data);
                    //        var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    //        string urltosetflag = "https://api.channeladvisor.com/v1/Orders(" + i.ToString() + ")?access_token=" + accesstoken;
                    //        string res = string.Empty;

                    //        var client = new RestClient(urltosetflag);
                    //        var request1 = new RestSharp.RestRequest();
                    //        request1.Method = RestSharp.Method.PUT;
                    //        request1.Parameters.Clear();
                    //        request1.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                    //        var result = client.Execute(request1);
                    //        if (!result.IsSuccessful)
                    //        {
                    //            SendErrorMail("Flag not updated for order number " + OrderNumber.ToString());
                    //        }
                    //    }
                    //}
                    //if (partialupdate)
                    //{
                    //    foreach (var i in OrderNumber.Split(','))
                    //    {
                    //        string ordnum = string.Empty;
                    //        var data = new Dictionary<string, string>
                    //        {
                    //            {"FlagID","5"},
                    //            {"FlagDescription","Picklist Partially Validated"}
                    //        };
                    //        var jsonData = JsonConvert.SerializeObject(data);
                    //        var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    //        string urltosetflag = "https://api.channeladvisor.com/v1/Orders(" + i.ToString() + ")?access_token=" + accesstoken;
                    //        string res = string.Empty;

                    //        var client = new RestClient(urltosetflag);
                    //        var request1 = new RestSharp.RestRequest();
                    //        request1.Method = RestSharp.Method.PUT;
                    //        request1.Parameters.Clear();
                    //        request1.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                    //        var result = client.Execute(request1);
                    //        if (!result.IsSuccessful)
                    //        {
                    //            SendErrorMail("Flag not updated for order number " + OrderNumber.ToString());
                    //        }
                    //    }
                    //}
                    //#endregion

                    if (!string.IsNullOrEmpty(UserId))
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = @"H:\Applications\Jadlam\Jadlam App - Private Notes Updation\PickListValidatedNotes\JadlamApp_PrivateNotes_ValidationNotes.exe";
                        p.StartInfo.Arguments = "Order:" +( String.IsNullOrEmpty(OrderNumber)?"": OrderNumber.Trim()) + " " + "Batch:" + BatchId.Trim().Replace(" ","**") + " " + "sku:" + (String.IsNullOrEmpty(SKU) ? "" : SKU);
                        p.Start();
                    }

                    sucessresponse.message = "updated!";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");

                    response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                    return response;
                }

            }

            catch (Exception ex)
            {
                UpdatePicksfailedresponse failedresponse = new UpdatePicksfailedresponse();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }

        #region SEND ERROR MAIL
        public bool SendErrorMail(string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtpout.secureserver.net");

                mail.From = new MailAddress("hello@matridtech.net", "JADLAM APP- SET FLAG FOR VALIDATE PICKLIST");
                mail.To.Add(new MailAddress("poonam.matrid341@gmail.com"));
                mail.To.Add("rahulkumar.matrid55779@gmail.com");
                mail.Subject = "Message from jadlam_sku_ean_mapping_app - " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                mail.Body = body;
                mail.IsBodyHtml = true;


                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("hello@matridtech.net", "Inform@2020*");
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

        #region ACCESS TOKEN METHODS
        static Accesstoken GetAccessToken()
        {
            string URL = "oauth2/token";
            //string refreshtoken = "a7ux0ZzwOaF4CSksc36kYmJCqTYdiuBTT47Tka94-uE";
            string refreshtoken = "bsNYTWti-r8BttlV-cf7DL_Vf3QxDYpHHNE4iQv6iaE";
            string applicationid = "aka7vw99tpzu12igo9x3x9ty18mkdw94";
            //string applicationid = "gpiwrw4f9jhn7zkz0f7m9v7l1pdurgyq";
            string secretid = "BeIncReW4keeG4P9YjpfrA";
            //string secretid = "24N9ocV6AEaii4IfGWDMJw";
            string authorize = applicationid + ":" + secretid;
            string encode = EncodeTo64(authorize);
            string encodeauthorize = "Basic " + encode;
            Accesstoken accesstoken = PostForAccesstoken(URL, refreshtoken, encodeauthorize);
            return accesstoken;
        }

        static Accesstoken PostForAccesstoken(string URL, string refreshtoken, string encodeauthorize)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            string URI = "https://api.channeladvisor.com/" + URL;
            var client = new RestClient(URI);
            var request = new RestSharp.RestRequest();
            request.Method = RestSharp.Method.POST;
            request.Parameters.Clear();
            request.AddHeader("Authorization", encodeauthorize);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshtoken);
            JsonDeserializer deserial = new JsonDeserializer();
            Accesstoken x = deserial.Deserialize<Accesstoken>(client.Execute<Accesstoken>(request));
            return x;
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