/* 
 * Selling Partner API for Solicitations
 *
 * With the Solicitations API you can build applications that send non-critical solicitations to buyers. You can get a list of solicitation types that are available for an order that you specify, then call an operation that sends a solicitation to the buyer for that order. Buyers cannot respond to solicitations sent by this API, and these solicitations do not appear in the Messaging section of Seller Central or in the recipient's Message Center. The Solicitations API returns responses that are formed according to the <a href=https://tools.ietf.org/html/draft-kelly-json-hal-08>JSON Hypertext Application Language</a> (HAL) standard.
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

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Solicitations
{
    /// <summary>
    /// A Link object.
    /// </summary>
    [DataContract]
    public partial class LinkObject : IEquatable<LinkObject>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkObject" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public LinkObject() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkObject" /> class.
        /// </summary>
        /// <param name="Href">A URI for this object. (required).</param>
        /// <param name="Name">An identifier for this object..</param>
        public LinkObject(string Href = default(string), string Name = default(string))
        {
            // to ensure "Href" is required (not null)
            if (Href == null)
            {
                throw new InvalidDataException("Href is a required property for LinkObject and cannot be null");
            }
            else
            {
                this.Href = Href;
            }
            this.Name = Name;
        }

        /// <summary>
        /// A URI for this object.
        /// </summary>
        /// <value>A URI for this object.</value>
        [DataMember(Name = "href", EmitDefaultValue = false)]
        public string Href { get; set; }

        /// <summary>
        /// An identifier for this object.
        /// </summary>
        /// <value>An identifier for this object.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LinkObject {\n");
            sb.Append("  Href: ").Append(Href).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
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
            return this.Equals(input as LinkObject);
        }

        /// <summary>
        /// Returns true if LinkObject instances are equal
        /// </summary>
        /// <param name="input">Instance of LinkObject to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LinkObject input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Href == input.Href ||
                    (this.Href != null &&
                    this.Href.Equals(input.Href))
                ) &&
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
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
                if (this.Href != null)
                    hashCode = hashCode * 59 + this.Href.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
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
