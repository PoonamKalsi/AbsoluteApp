/* 
 * Selling Partner API for Finances
 *
 * The Selling Partner API for Finances helps you obtain financial information relevant to a seller's business. You can obtain financial events for a given order, financial event group, or date range without having to wait until a statement period closes. You can also obtain financial event groups for a given date range.
 *
 * OpenAPI spec version: v0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Finances
{
    /// <summary>
    /// The payload for the listFinancialEvents operation.
    /// </summary>
    [DataContract]
    public partial class ListFinancialEventsPayload : IEquatable<ListFinancialEventsPayload>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListFinancialEventsPayload" /> class.
        /// </summary>
        /// <param name="NextToken">When present and not empty, pass this string token in the next request to return the next response page..</param>
        /// <param name="FinancialEvents">FinancialEvents.</param>
        public ListFinancialEventsPayload(string NextToken = default(string), FinancialEvents FinancialEvents = default(FinancialEvents))
        {
            this.NextToken = NextToken;
            this.FinancialEvents = FinancialEvents;
        }

        /// <summary>
        /// When present and not empty, pass this string token in the next request to return the next response page.
        /// </summary>
        /// <value>When present and not empty, pass this string token in the next request to return the next response page.</value>
        [DataMember(Name = "NextToken", EmitDefaultValue = false)]
        public string NextToken { get; set; }

        /// <summary>
        /// Gets or Sets FinancialEvents
        /// </summary>
        [DataMember(Name = "FinancialEvents", EmitDefaultValue = false)]
        public FinancialEvents FinancialEvents { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ListFinancialEventsPayload {\n");
            sb.Append("  NextToken: ").Append(NextToken).Append("\n");
            sb.Append("  FinancialEvents: ").Append(FinancialEvents).Append("\n");
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
            return this.Equals(input as ListFinancialEventsPayload);
        }

        /// <summary>
        /// Returns true if ListFinancialEventsPayload instances are equal
        /// </summary>
        /// <param name="input">Instance of ListFinancialEventsPayload to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ListFinancialEventsPayload input)
        {
            if (input == null)
                return false;

            return
                (
                    this.NextToken == input.NextToken ||
                    (this.NextToken != null &&
                    this.NextToken.Equals(input.NextToken))
                ) &&
                (
                    this.FinancialEvents == input.FinancialEvents ||
                    (this.FinancialEvents != null &&
                    this.FinancialEvents.Equals(input.FinancialEvents))
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
                if (this.NextToken != null)
                    hashCode = hashCode * 59 + this.NextToken.GetHashCode();
                if (this.FinancialEvents != null)
                    hashCode = hashCode * 59 + this.FinancialEvents.GetHashCode();
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
