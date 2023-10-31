/* 
 * Selling Partner API for Merchant Fulfillment
 *
 * The Selling Partner API for Merchant Fulfillment helps you build applications that let sellers purchase shipping for non-Prime and Prime orders using Amazon’s Buy Shipping Services.
 *
 * OpenAPI spec version: v0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;


namespace FikaAmazonAPI.AmazonSpApiSDK.Models.MerchantFulfillment
{
    /// <summary>
    /// The dimensions of a package contained in a shipment.
    /// </summary>
    [DataContract]
    public partial class PackageDimensions :  IEquatable<PackageDimensions>, IValidatableObject
    {
        /// <summary>
        /// The unit of measurement. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Unit.
        /// </summary>
        /// <value>The unit of measurement. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Unit.</value>
        [DataMember(Name="Unit", EmitDefaultValue=false)]
        public UnitOfLength? Unit { get; set; }
        /// <summary>
        /// Gets or Sets PredefinedPackageDimensions
        /// </summary>
        [DataMember(Name="PredefinedPackageDimensions", EmitDefaultValue=false)]
        public PredefinedPackageDimensions? PredefinedPackageDimensions { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDimensions" /> class.
        /// </summary>
        /// <param name="length">The length dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Length..</param>
        /// <param name="width">The width dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Width..</param>
        /// <param name="height">The height dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Height..</param>
        /// <param name="unit">The unit of measurement. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Unit..</param>
        /// <param name="predefinedPackageDimensions">predefinedPackageDimensions.</param>
        public PackageDimensions(double? length = default(double?), double? width = default(double?), double? height = default(double?), UnitOfLength? unit = default(UnitOfLength?), PredefinedPackageDimensions? predefinedPackageDimensions = default(PredefinedPackageDimensions?))
        {
            this.Length = length;
            this.Width = width;
            this.Height = height;
            this.Unit = unit;
            this.PredefinedPackageDimensions = predefinedPackageDimensions;
        }
        
        /// <summary>
        /// The length dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Length.
        /// </summary>
        /// <value>The length dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Length.</value>
        [DataMember(Name="Length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// The width dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Width.
        /// </summary>
        /// <value>The width dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Width.</value>
        [DataMember(Name="Width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// The height dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Height.
        /// </summary>
        /// <value>The height dimension. If you don&#39;t specify PredefinedPackageDimensions, you must specify the Height.</value>
        [DataMember(Name="Height", EmitDefaultValue=false)]
        public double? Height { get; set; }



        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PackageDimensions {\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Unit: ").Append(Unit).Append("\n");
            sb.Append("  PredefinedPackageDimensions: ").Append(PredefinedPackageDimensions).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
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
            return this.Equals(input as PackageDimensions);
        }

        /// <summary>
        /// Returns true if PackageDimensions instances are equal
        /// </summary>
        /// <param name="input">Instance of PackageDimensions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PackageDimensions input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Length == input.Length ||
                    (this.Length != null &&
                    this.Length.Equals(input.Length))
                ) && 
                (
                    this.Width == input.Width ||
                    (this.Width != null &&
                    this.Width.Equals(input.Width))
                ) && 
                (
                    this.Height == input.Height ||
                    (this.Height != null &&
                    this.Height.Equals(input.Height))
                ) && 
                (
                    this.Unit == input.Unit ||
                    (this.Unit != null &&
                    this.Unit.Equals(input.Unit))
                ) && 
                (
                    this.PredefinedPackageDimensions == input.PredefinedPackageDimensions ||
                    (this.PredefinedPackageDimensions != null &&
                    this.PredefinedPackageDimensions.Equals(input.PredefinedPackageDimensions))
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
                if (this.Length != null)
                    hashCode = hashCode * 59 + this.Length.GetHashCode();
                if (this.Width != null)
                    hashCode = hashCode * 59 + this.Width.GetHashCode();
                if (this.Height != null)
                    hashCode = hashCode * 59 + this.Height.GetHashCode();
                if (this.Unit != null)
                    hashCode = hashCode * 59 + this.Unit.GetHashCode();
                if (this.PredefinedPackageDimensions != null)
                    hashCode = hashCode * 59 + this.PredefinedPackageDimensions.GetHashCode();
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