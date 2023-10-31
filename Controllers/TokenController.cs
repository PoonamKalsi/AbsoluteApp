using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class TokenController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {

            try
            {
                sucessresponset sucessresponse = new sucessresponset();
                string accesstoken = string.Empty;

                Accesstoken accesstokens = GetAccessToken();
                accesstoken = accesstokens.access_token;
                sucessresponse.data = accesstokens;
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                failedresponset failedresponse = new failedresponset();
                failedresponse.code = 0;
                failedresponse.message = ex.Message;
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
                //return request.CreateResponse(HttpStatusCode.InternalServerError, Newtonsoft.Json.JsonConvert.SerializeObject(failedresponse));
            }
        }

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
    }
    #region RESPONSE CLASSES
 
    public class sucessresponset
    {
        public Accesstoken data { get; set; }
    }

    //public class Resultt
    //{
    //    public Accesstoken data { get; set; }

    //}

    public class Accesstoken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
    public class failedresponset
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    #endregion
}