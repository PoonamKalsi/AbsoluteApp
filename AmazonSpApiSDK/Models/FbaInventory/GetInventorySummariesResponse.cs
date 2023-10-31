/* 
 * Selling Partner API for FBA Inventory
 *
 * The Selling Partner API for FBA Inventory lets you programmatically retrieve information about inventory in Amazon's fulfillment network.
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.FbaInventory
{
    /// <summary>
    /// The Response schema.
    /// </summary>
    [DataContract]
    public partial class GetInventorySummariesResponse : IEquatable<GetInventorySummariesResponse>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetInventorySummariesResponse" /> class.
        /// </summary>
        /// <param name="Payload">The payload for the getInventorySummaries operation..</param>
        /// <param name="Pagination">Pagination.</param>
        /// <param name="Errors">One or more unexpected errors occurred during the getInventorySummaries operation..</param>
        public GetInventorySummariesResponse(GetInventorySummariesResult Payload = default(GetInventorySummariesResult), Pagination Pagination = default(Pagination), ErrorList Errors = default(ErrorList))
        {
            this.Payload = Payload;
            this.Pagination = Pagination;
            this.Errors = Errors;
        }
        public GetInventorySummariesResponse()
        {
            this.Payload = default(GetInventorySummariesResult);
            this.Pagination = default(Pagination);
            this.Errors = default(ErrorList);
        }

        /// <summary>
        /// The payload for the getInventorySummaries operation.
        /// </summary>
        /// <value>The payload for the getInventorySummaries operation.</value>
        [DataMember(Name = "payload", EmitDefaultValue = false)]
        public GetInventorySummariesResult Payload { get; set; }

        /// <summary>
        /// Gets or Sets Pagination
        /// </summary>
        [DataMember(Name = "pagination", EmitDefaultValue = false)]
        public Pagination Pagination { get; set; }

        /// <summary>
        /// One or more unexpected errors occurred during the getInventorySummaries operation.
        /// </summary>
        /// <value>One or more unexpected errors occurred during the getInventorySummaries operation.</value>
        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public ErrorList Errors { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetInventorySummariesResponse {\n");
            sb.Append("  Payload: ").Append(Payload).Append("\n");
            sb.Append("  Pagination: ").Append(Pagination).Append("\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
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
            return this.Equals(input as GetInventorySummariesResponse);
        }

        /// <summary>
        /// Returns true if GetInventorySummariesResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of GetInventorySummariesResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetInventorySummariesResponse input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Payload == input.Payload ||
                    (this.Payload != null &&
                    this.Payload.Equals(input.Payload))
                ) &&
                (
                    this.Pagination == input.Pagination ||
                    (this.Pagination != null &&
                    this.Pagination.Equals(input.Pagination))
                ) &&
                (
                    this.Errors == input.Errors ||
                    (this.Errors != null &&
                    this.Errors.Equals(input.Errors))
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
                if (this.Payload != null)
                    hashCode = hashCode * 59 + this.Payload.GetHashCode();
                if (this.Pagination != null)
                    hashCode = hashCode * 59 + this.Pagination.GetHashCode();
                if (this.Errors != null)
                    hashCode = hashCode * 59 + this.Errors.GetHashCode();
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
