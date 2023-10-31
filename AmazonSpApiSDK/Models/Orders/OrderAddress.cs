/* 
 * Selling Partner API for Orders
 *
 * The Selling Partner API for Orders helps you programmatically retrieve order information. These APIs let you develop fast, flexible, custom applications in areas like order synchronization, order research, and demand-based decision support tools.
 *
 * OpenAPI spec version: v0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Orders
{
    /// <summary>
    /// The shipping address for the order.
    /// </summary>
    [DataContract]
    public partial class OrderAddress : IEquatable<OrderAddress>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAddress" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public OrderAddress() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAddress" /> class.
        /// </summary>
        /// <param name="AmazonOrderId">An Amazon-defined order identifier, in 3-7-7 format. (required).</param>
        /// <param name="ShippingAddress">ShippingAddress.</param>
        public OrderAddress(string AmazonOrderId = default(string), Address ShippingAddress = default(Address))
        {
            // to ensure "AmazonOrderId" is required (not null)
            if (AmazonOrderId == null)
            {
                throw new InvalidDataException("AmazonOrderId is a required property for OrderAddress and cannot be null");
            }
            else
            {
                this.AmazonOrderId = AmazonOrderId;
            }
            this.ShippingAddress = ShippingAddress;
        }

        /// <summary>
        /// An Amazon-defined order identifier, in 3-7-7 format.
        /// </summary>
        /// <value>An Amazon-defined order identifier, in 3-7-7 format.</value>
        [DataMember(Name = "AmazonOrderId", EmitDefaultValue = false)]
        public string AmazonOrderId { get; set; }

        /// <summary>
        /// Gets or Sets ShippingAddress
        /// </summary>
        [DataMember(Name = "ShippingAddress", EmitDefaultValue = false)]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OrderAddress {\n");
            sb.Append("  AmazonOrderId: ").Append(AmazonOrderId).Append("\n");
            sb.Append("  ShippingAddress: ").Append(ShippingAddress).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as OrderAddress);
        }

        /// <summary>
        /// Returns true if OrderAddress instances are equal
        /// </summary>
        /// <param name="input">Instance of OrderAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OrderAddress input)
        {
            if (input == null)
                return false;

            return
                (
                    this.AmazonOrderId == input.AmazonOrderId ||
                    (this.AmazonOrderId != null &&
                    this.AmazonOrderId.Equals(input.AmazonOrderId))
                ) &&
                (
                    this.ShippingAddress == input.ShippingAddress ||
                    (this.ShippingAddress != null &&
                    this.ShippingAddress.Equals(input.ShippingAddress))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.AmazonOrderId != null)
                    hashCode = hashCode * 59 + this.AmazonOrderId.GetHashCode();
                if (this.ShippingAddress != null)
                    hashCode = hashCode * 59 + this.ShippingAddress.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
