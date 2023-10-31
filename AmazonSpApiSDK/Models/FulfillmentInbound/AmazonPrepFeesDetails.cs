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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.FulfillmentInbound
{
    /// <summary>
    /// The fees for Amazon to prep goods for shipment.
    /// </summary>
    [DataContract]
    public partial class AmazonPrepFeesDetails : IEquatable<AmazonPrepFeesDetails>, IValidatableObject
    {
        /// <summary>
        /// Gets or Sets PrepInstruction
        /// </summary>
        [DataMember(Name = "PrepInstruction", EmitDefaultValue = false)]
        public PrepInstruction? PrepInstruction { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPrepFeesDetails" /> class.
        /// </summary>
        /// <param name="PrepInstruction">PrepInstruction.</param>
        /// <param name="FeePerUnit">The fee for Amazon to prepare 1 unit..</param>
        public AmazonPrepFeesDetails(PrepInstruction? PrepInstruction = default(PrepInstruction?), Amount FeePerUnit = default(Amount))
        {
            this.PrepInstruction = PrepInstruction;
            this.FeePerUnit = FeePerUnit;
        }


        /// <summary>
        /// The fee for Amazon to prepare 1 unit.
        /// </summary>
        /// <value>The fee for Amazon to prepare 1 unit.</value>
        [DataMember(Name = "FeePerUnit", EmitDefaultValue = false)]
        public Amount FeePerUnit { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AmazonPrepFeesDetails {\n");
            sb.Append("  PrepInstruction: ").Append(PrepInstruction).Append("\n");
            sb.Append("  FeePerUnit: ").Append(FeePerUnit).Append("\n");
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
            return this.Equals(input as AmazonPrepFeesDetails);
        }

        /// <summary>
        /// Returns true if AmazonPrepFeesDetails instances are equal
        /// </summary>
        /// <param name="input">Instance of AmazonPrepFeesDetails to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AmazonPrepFeesDetails input)
        {
            if (input == null)
                return false;

            return
                (
                    this.PrepInstruction == input.PrepInstruction ||
                    (this.PrepInstruction != null &&
                    this.PrepInstruction.Equals(input.PrepInstruction))
                ) &&
                (
                    this.FeePerUnit == input.FeePerUnit ||
                    (this.FeePerUnit != null &&
                    this.FeePerUnit.Equals(input.FeePerUnit))
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
                if (this.PrepInstruction != null)
                    hashCode = hashCode * 59 + this.PrepInstruction.GetHashCode();
                if (this.FeePerUnit != null)
                    hashCode = hashCode * 59 + this.FeePerUnit.GetHashCode();
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
