/* 
 * Selling Partner API for Pricing
 *
 * The Selling Partner API for Pricing helps you programmatically retrieve product pricing and offer information for Amazon Marketplace products.
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

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.ProductPricing
{
    /// <summary>
    /// The number of offer listings with the specified condition.
    /// </summary>
    [DataContract]
    public partial class OfferListingCountType : IEquatable<OfferListingCountType>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfferListingCountType" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public OfferListingCountType() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OfferListingCountType" /> class.
        /// </summary>
        /// <param name="Count">The number of offer listings. (required).</param>
        /// <param name="Condition">The condition of the item. (required).</param>
        public OfferListingCountType(int? Count = default(int?), string Condition = default(string))
        {
            // to ensure "Count" is required (not null)
            if (Count == null)
            {
                throw new InvalidDataException("Count is a required property for OfferListingCountType and cannot be null");
            }
            else
            {
                this.Count = Count;
            }
            // to ensure "Condition" is required (not null)
            if (Condition == null)
            {
                throw new InvalidDataException("Condition is a required property for OfferListingCountType and cannot be null");
            }
            else
            {
                this.Condition = Condition;
            }
        }

        /// <summary>
        /// The number of offer listings.
        /// </summary>
        /// <value>The number of offer listings.</value>
        [DataMember(Name = "Count", EmitDefaultValue = false)]
        public int? Count { get; set; }

        /// <summary>
        /// The condition of the item.
        /// </summary>
        /// <value>The condition of the item.</value>
        [DataMember(Name = "condition", EmitDefaultValue = false)]
        public string Condition { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OfferListingCountType {\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  Condition: ").Append(Condition).Append("\n");
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
            return this.Equals(input as OfferListingCountType);
        }

        /// <summary>
        /// Returns true if OfferListingCountType instances are equal
        /// </summary>
        /// <param name="input">Instance of OfferListingCountType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OfferListingCountType input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Count == input.Count ||
                    (this.Count != null &&
                    this.Count.Equals(input.Count))
                ) &&
                (
                    this.Condition == input.Condition ||
                    (this.Condition != null &&
                    this.Condition.Equals(input.Condition))
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
                if (this.Count != null)
                    hashCode = hashCode * 59 + this.Count.GetHashCode();
                if (this.Condition != null)
                    hashCode = hashCode * 59 + this.Condition.GetHashCode();
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
