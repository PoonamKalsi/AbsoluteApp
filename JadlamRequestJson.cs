using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadlamPrimeOrderIntegration
{
    public class FromAddress
    {
        public string name { get; set; }
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class Parcel
    {
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
    }

    public class JadlamRequestJson
    {
        public Shipment shipment { get; set; }
    }

    public class Shipment
    {
        public ToAddress to_address { get; set; }
        public FromAddress from_address { get; set; }
        public Parcel parcel { get; set; }
    }

    public class ToAddress
    {
        public string name { get; set; }
        public string street1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

}
