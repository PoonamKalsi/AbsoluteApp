using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteApp
{
   
        public class BuyerAddress
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string name { get; set; }
            public object company { get; set; }
            public string street1 { get; set; }
            public object street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string mode { get; set; }
            public object carrier_facility { get; set; }
            public object residential { get; set; }
            public object federal_tax_id { get; set; }
            public object state_tax_id { get; set; }
            public Verifications verifications { get; set; }
        }

        public class FromAddress
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string name { get; set; }
            public object company { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string mode { get; set; }
            public object carrier_facility { get; set; }
            public object residential { get; set; }
            public object federal_tax_id { get; set; }
            public object state_tax_id { get; set; }
            public Verifications verifications { get; set; }
        }

        public class Message
        {
            public string carrier { get; set; }
            public string carrier_account_id { get; set; }
            public string type { get; set; }
            public string message { get; set; }
        }

        public class Options
        {
            public string currency { get; set; }
            public Payment payment { get; set; }
            public int date_advance { get; set; }
        }

        public class Parcel
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public object predefined_package { get; set; }
            public double weight { get; set; }
            public string mode { get; set; }
        }

        public class Payment
        {
            public string type { get; set; }
        }

        public class Rate
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string mode { get; set; }
            public string service { get; set; }
            public string carrier { get; set; }
            public string rate { get; set; }
            public string currency { get; set; }
            public object retail_rate { get; set; }
            public object retail_currency { get; set; }
            public object list_rate { get; set; }
            public object list_currency { get; set; }
            public string billing_type { get; set; }
            public int delivery_days { get; set; }
            public object delivery_date { get; set; }
            public object delivery_date_guaranteed { get; set; }
            public int est_delivery_days { get; set; }
            public string shipment_id { get; set; }
            public string carrier_account_id { get; set; }
        }

        public class ReturnAddress
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string name { get; set; }
            public object company { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string mode { get; set; }
            public object carrier_facility { get; set; }
            public object residential { get; set; }
            public object federal_tax_id { get; set; }
            public object state_tax_id { get; set; }
            public Verifications verifications { get; set; }
        }

        public class root
        {
            public DateTime created_at { get; set; }
            public bool is_return { get; set; }
            public List<Message> messages { get; set; }
            public string mode { get; set; }
            public Options options { get; set; }
            public object reference { get; set; }
            public string status { get; set; }
            public object tracking_code { get; set; }
            public DateTime updated_at { get; set; }
            public object batch_id { get; set; }
            public object batch_status { get; set; }
            public object batch_message { get; set; }
            public object customs_info { get; set; }
            public FromAddress from_address { get; set; }
            public object insurance { get; set; }
            public object order_id { get; set; }
            public Parcel parcel { get; set; }
            public object postage_label { get; set; }
            public List<Rate> rates { get; set; }
            public object refund_status { get; set; }
            public object scan_form { get; set; }
            public object selected_rate { get; set; }
            public object tracker { get; set; }
            public ToAddress to_address { get; set; }
            public int usps_zone { get; set; }
            public ReturnAddress return_address { get; set; }
            public BuyerAddress buyer_address { get; set; }
            public List<object> forms { get; set; }
            public List<object> fees { get; set; }
            public string id { get; set; }
            public string @object { get; set; }
        }

        public class ToAddress
        {
            public string id { get; set; }
            public string @object { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string name { get; set; }
            public object company { get; set; }
            public string street1 { get; set; }
            public object street2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string mode { get; set; }
            public object carrier_facility { get; set; }
            public object residential { get; set; }
            public object federal_tax_id { get; set; }
            public object state_tax_id { get; set; }
            public Verifications verifications { get; set; }
        }

        public class Verifications
        {
        }
    
}
