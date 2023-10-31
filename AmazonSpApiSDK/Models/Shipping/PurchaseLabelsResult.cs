/* 
 * Selling Partner API for Shipping
 *
 * Provides programmatic access to Amazon Shipping APIs.
 *
 * OpenAPI spec version: v1
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

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Shipping
{
    /// <summary>
    /// The payload schema for the purchaseLabels operation.
    /// </summary>
    [DataContract]
    public partial class PurchaseLabelsResult : IEquatable<PurchaseLabelsResult>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseLabelsResult" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public PurchaseLabelsResult() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseLabelsResult" /> class.
        /// </summary>
        /// <param name="ShipmentId">ShipmentId (required).</param>
        /// <param name="ClientReferenceId">ClientReferenceId.</param>
        /// <param name="AcceptedRate">AcceptedRate (required).</param>
        /// <param name="LabelResults">LabelResults (required).</param>
        public PurchaseLabelsResult(ShipmentId ShipmentId = default(ShipmentId), ClientReferenceId ClientReferenceId = default(ClientReferenceId), AcceptedRate AcceptedRate = default(AcceptedRate), LabelResultList LabelResults = default(LabelResultList))
        {
            // to ensure "ShipmentId" is required (not null)
            if (ShipmentId == null)
            {
                throw new InvalidDataException("ShipmentId is a required property for PurchaseLabelsResult and cannot be null");
            }
            else
            {
                this.ShipmentId = ShipmentId;
            }
            // to ensure "AcceptedRate" is required (not null)
            if (AcceptedRate == null)
            {
                throw new InvalidDataException("AcceptedRate is a required property for PurchaseLabelsResult and cannot be null");
            }
            else
            {
                this.AcceptedRate = AcceptedRate;
            }
            // to ensure "LabelResults" is required (not null)
            if (LabelResults == null)
            {
                throw new InvalidDataException("LabelResults is a required property for PurchaseLabelsResult and cannot be null");
            }
            else
            {
                this.LabelResults = LabelResults;
            }
            this.ClientReferenceId = ClientReferenceId;
        }

        /// <summary>
        /// Gets or Sets ShipmentId
        /// </summary>
        [DataMember(Name = "shipmentId", EmitDefaultValue = false)]
        public ShipmentId ShipmentId { get; set; }

        /// <summary>
        /// Gets or Sets ClientReferenceId
        /// </summary>
        [DataMember(Name = "clientReferenceId", EmitDefaultValue = false)]
        public ClientReferenceId ClientReferenceId { get; set; }

        /// <summary>
        /// Gets or Sets AcceptedRate
        /// </summary>
        [DataMember(Name = "acceptedRate", EmitDefaultValue = false)]
        public AcceptedRate AcceptedRate { get; set; }

        /// <summary>
        /// Gets or Sets LabelResults
        /// </summary>
        [DataMember(Name = "labelResults", EmitDefaultValue = false)]
        public LabelResultList LabelResults { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PurchaseLabelsResult {\n");
            sb.Append("  ShipmentId: ").Append(ShipmentId).Append("\n");
            sb.Append("  ClientReferenceId: ").Append(ClientReferenceId).Append("\n");
            sb.Append("  AcceptedRate: ").Append(AcceptedRate).Append("\n");
            sb.Append("  LabelResults: ").Append(LabelResults).Append("\n");
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
            return this.Equals(input as PurchaseLabelsResult);
        }

        /// <summary>
        /// Returns true if PurchaseLabelsResult instances are equal
        /// </summary>
        /// <param name="input">Instance of PurchaseLabelsResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PurchaseLabelsResult input)
        {
            if (input == null)
                return false;

            return
                (
                    this.ShipmentId == input.ShipmentId ||
                    (this.ShipmentId != null &&
                    this.ShipmentId.Equals(input.ShipmentId))
                ) &&
                (
                    this.ClientReferenceId == input.ClientReferenceId ||
                    (this.ClientReferenceId != null &&
                    this.ClientReferenceId.Equals(input.ClientReferenceId))
                ) &&
                (
                    this.AcceptedRate == input.AcceptedRate ||
                    (this.AcceptedRate != null &&
                    this.AcceptedRate.Equals(input.AcceptedRate))
                ) &&
                (
                    this.LabelResults == input.LabelResults ||
                    (this.LabelResults != null &&
                    this.LabelResults.Equals(input.LabelResults))
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
                if (this.ShipmentId != null)
                    hashCode = hashCode * 59 + this.ShipmentId.GetHashCode();
                if (this.ClientReferenceId != null)
                    hashCode = hashCode * 59 + this.ClientReferenceId.GetHashCode();
                if (this.AcceptedRate != null)
                    hashCode = hashCode * 59 + this.AcceptedRate.GetHashCode();
                if (this.LabelResults != null)
                    hashCode = hashCode * 59 + this.LabelResults.GetHashCode();
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