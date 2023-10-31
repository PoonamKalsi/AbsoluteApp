/* 
 * Selling Partner API for Fulfillment Inbound
 *
 * The Selling Partner API for Fulfillment Inbound lets you create applications that create and update inbound shipments of inventory to Amazon's fulfillment network.
 *
 * OpenAPI spec version: v0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.FulfillmentInbound
{
    /// <summary>
    /// Indicates who will prepare the item.
    /// </summary>
    /// <value>Indicates who will prepare the item.</value>

    [JsonConverter(typeof(StringEnumConverter))]

    public enum PrepOwner
    {

        /// <summary>
        /// Enum AMAZON for value: AMAZON
        /// </summary>
        [EnumMember(Value = "AMAZON")]
        AMAZON = 1,

        /// <summary>
        /// Enum SELLER for value: SELLER
        /// </summary>
        [EnumMember(Value = "SELLER")]
        SELLER = 2
    }

}
