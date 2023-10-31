using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AbsoluteApp.Controllers
{
    public class RunProcessShipmentController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                UpdatePickssucessresponse sucessresponse = new UpdatePickssucessresponse();

                Process p = new Process();
                p.StartInfo.FileName = @"H:\Applications\Absolute\Barcode Project\ProcessShipment App\ProcessShipment.exe";
                //p.StartInfo.Arguments = i.ToString() + " " + type;
                p.Start();

                sucessresponse.message = "Please wait app is running!";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");

                response.Content = new StringContent(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(sucessresponse)).ToString(), Encoding.UTF8, "application/json");
                return response;
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

        #region RESPONSE CLASSES

        public class UpdatePickssucessresponse
        {
            public string message { get; set; }
        }


        public class UpdatePicksfailedresponse
        {
            public string message { get; set; }
            public int code { get; set; }
        }
        #endregion
    }
}