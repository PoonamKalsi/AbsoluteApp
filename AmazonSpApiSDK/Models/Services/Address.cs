/* 
 * Selling Partner API for Services
 *
 * With the Services API, you can build applications that help service providers get and modify their service orders.
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

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Services
{
    /// <summary>
    /// The shipping address for the service job.
    /// </summary>
    [DataContract]
    public partial class Address : IEquatable<Address>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Address" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public Address() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Address" /> class.
        /// </summary>
        /// <param name="Name">The name of the person, business, or institution. (required).</param>
        /// <param name="AddressLine1">The first line of the address. (required).</param>
        /// <param name="AddressLine2">Additional address information, if required..</param>
        /// <param name="AddressLine3">Additional address information, if required..</param>
        /// <param name="City">The city..</param>
        /// <param name="County">The county..</param>
        /// <param name="District">The district..</param>
        /// <param name="StateOrRegion">The state or region..</param>
        /// <param name="PostalCode">The postal code. This can contain letters, digits, spaces, and/or punctuation..</param>
        /// <param name="CountryCode">The two digit country code, in ISO 3166-1 alpha-2 format..</param>
        /// <param name="Phone">The phone number..</param>
        public Address(string Name = default(string), string AddressLine1 = default(string), string AddressLine2 = default(string), string AddressLine3 = default(string), string City = default(string), string County = default(string), string District = default(string), string StateOrRegion = default(string), string PostalCode = default(string), string CountryCode = default(string), string Phone = default(string))
        {
            // to ensure "Name" is required (not null)
            if (Name == null)
            {
                throw new InvalidDataException("Name is a required property for Address and cannot be null");
            }
            else
            {
                this.Name = Name;
            }
            // to ensure "AddressLine1" is required (not null)
            if (AddressLine1 == null)
            {
                throw new InvalidDataException("AddressLine1 is a required property for Address and cannot be null");
            }
            else
            {
                this.AddressLine1 = AddressLine1;
            }
            this.AddressLine2 = AddressLine2;
            this.AddressLine3 = AddressLine3;
            this.City = City;
            this.County = County;
            this.District = District;
            this.StateOrRegion = StateOrRegion;
            this.PostalCode = PostalCode;
            this.CountryCode = CountryCode;
            this.Phone = Phone;
        }

        /// <summary>
        /// The name of the person, business, or institution.
        /// </summary>
        /// <value>The name of the person, business, or institution.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The first line of the address.
        /// </summary>
        /// <value>The first line of the address.</value>
        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Additional address information, if required.
        /// </summary>
        /// <value>Additional address information, if required.</value>
        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Additional address information, if required.
        /// </summary>
        /// <value>Additional address information, if required.</value>
        [DataMember(Name = "addressLine3", EmitDefaultValue = false)]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        /// <value>The city.</value>
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string City { get; set; }

        /// <summary>
        /// The county.
        /// </summary>
        /// <value>The county.</value>
        [DataMember(Name = "county", EmitDefaultValue = false)]
        public string County { get; set; }

        /// <summary>
        /// The district.
        /// </summary>
        /// <value>The district.</value>
        [DataMember(Name = "district", EmitDefaultValue = false)]
        public string District { get; set; }

        /// <summary>
        /// The state or region.
        /// </summary>
        /// <value>The state or region.</value>
        [DataMember(Name = "stateOrRegion", EmitDefaultValue = false)]
        public string StateOrRegion { get; set; }

        /// <summary>
        /// The postal code. This can contain letters, digits, spaces, and/or punctuation.
        /// </summary>
        /// <value>The postal code. This can contain letters, digits, spaces, and/or punctuation.</value>
        [DataMember(Name = "postalCode", EmitDefaultValue = false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// The two digit country code, in ISO 3166-1 alpha-2 format.
        /// </summary>
        /// <value>The two digit country code, in ISO 3166-1 alpha-2 format.</value>
        [DataMember(Name = "countryCode", EmitDefaultValue = false)]
        public string CountryCode { get; set; }

        /// <summary>
        /// The phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [DataMember(Name = "phone", EmitDefaultValue = false)]
        public string Phone { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Address {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  AddressLine1: ").Append(AddressLine1).Append("\n");
            sb.Append("  AddressLine2: ").Append(AddressLine2).Append("\n");
            sb.Append("  AddressLine3: ").Append(AddressLine3).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  County: ").Append(County).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  StateOrRegion: ").Append(StateOrRegion).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
            sb.Append("  Phone: ").Append(Phone).Append("\n");
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
            return this.Equals(input as Address);
        }

        /// <summary>
        /// Returns true if Address instances are equal
        /// </summary>
        /// <param name="input">Instance of Address to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Address input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.AddressLine1 == input.AddressLine1 ||
                    (this.AddressLine1 != null &&
                    this.AddressLine1.Equals(input.AddressLine1))
                ) &&
                (
                    this.AddressLine2 == input.AddressLine2 ||
                    (this.AddressLine2 != null &&
                    this.AddressLine2.Equals(input.AddressLine2))
                ) &&
                (
                    this.AddressLine3 == input.AddressLine3 ||
                    (this.AddressLine3 != null &&
                    this.AddressLine3.Equals(input.AddressLine3))
                ) &&
                (
                    this.City == input.City ||
                    (this.City != null &&
                    this.City.Equals(input.City))
                ) &&
                (
                    this.County == input.County ||
                    (this.County != null &&
                    this.County.Equals(input.County))
                ) &&
                (
                    this.District == input.District ||
                    (this.District != null &&
                    this.District.Equals(input.District))
                ) &&
                (
                    this.StateOrRegion == input.StateOrRegion ||
                    (this.StateOrRegion != null &&
                    this.StateOrRegion.Equals(input.StateOrRegion))
                ) &&
                (
                    this.PostalCode == input.PostalCode ||
                    (this.PostalCode != null &&
                    this.PostalCode.Equals(input.PostalCode))
                ) &&
                (
                    this.CountryCode == input.CountryCode ||
                    (this.CountryCode != null &&
                    this.CountryCode.Equals(input.CountryCode))
                ) &&
                (
                    this.Phone == input.Phone ||
                    (this.Phone != null &&
                    this.Phone.Equals(input.Phone))
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
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.AddressLine1 != null)
                    hashCode = hashCode * 59 + this.AddressLine1.GetHashCode();
                if (this.AddressLine2 != null)
                    hashCode = hashCode * 59 + this.AddressLine2.GetHashCode();
                if (this.AddressLine3 != null)
                    hashCode = hashCode * 59 + this.AddressLine3.GetHashCode();
                if (this.City != null)
                    hashCode = hashCode * 59 + this.City.GetHashCode();
                if (this.County != null)
                    hashCode = hashCode * 59 + this.County.GetHashCode();
                if (this.District != null)
                    hashCode = hashCode * 59 + this.District.GetHashCode();
                if (this.StateOrRegion != null)
                    hashCode = hashCode * 59 + this.StateOrRegion.GetHashCode();
                if (this.PostalCode != null)
                    hashCode = hashCode * 59 + this.PostalCode.GetHashCode();
                if (this.CountryCode != null)
                    hashCode = hashCode * 59 + this.CountryCode.GetHashCode();
                if (this.Phone != null)
                    hashCode = hashCode * 59 + this.Phone.GetHashCode();
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
