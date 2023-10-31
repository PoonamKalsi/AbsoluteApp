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
    /// An item of a shipment, refund, guarantee claim, or chargeback.
    /// </summary>
    [DataContract]
    public partial class ShipmentItem : IEquatable<ShipmentItem>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentItem" /> class.
        /// </summary>
        /// <param name="SellerSKU">The seller SKU of the item. The seller SKU is qualified by the seller&#39;s seller ID, which is included with every call to the Selling Partner API..</param>
        /// <param name="OrderItemId">An Amazon-defined order item identifier..</param>
        /// <param name="OrderAdjustmentItemId">An Amazon-defined order adjustment identifier defined for refunds, guarantee claims, and chargeback events..</param>
        /// <param name="QuantityShipped">The number of items shipped..</param>
        /// <param name="ItemChargeList">A list of charges associated with the shipment item..</param>
        /// <param name="ItemChargeAdjustmentList">A list of charge adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events..</param>
        /// <param name="ItemFeeList">A list of fees associated with the shipment item..</param>
        /// <param name="ItemFeeAdjustmentList">A list of fee adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events..</param>
        /// <param name="ItemTaxWithheldList">A list of taxes withheld information for a shipment item..</param>
        /// <param name="PromotionList">PromotionList.</param>
        /// <param name="PromotionAdjustmentList">A list of promotion adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events..</param>
        /// <param name="CostOfPointsGranted">The cost of Amazon Points granted for a shipment item..</param>
        /// <param name="CostOfPointsReturned">The cost of Amazon Points returned for a shipment item. This value is only returned for refunds, guarantee claims, and chargeback events..</param>
        public ShipmentItem(string SellerSKU = default(string), string OrderItemId = default(string), string OrderAdjustmentItemId = default(string), int? QuantityShipped = default(int?), ChargeComponentList ItemChargeList = default(ChargeComponentList), ChargeComponentList ItemChargeAdjustmentList = default(ChargeComponentList), FeeComponentList ItemFeeList = default(FeeComponentList), FeeComponentList ItemFeeAdjustmentList = default(FeeComponentList), TaxWithheldComponentList ItemTaxWithheldList = default(TaxWithheldComponentList), PromotionList PromotionList = default(PromotionList), PromotionList PromotionAdjustmentList = default(PromotionList), Currency CostOfPointsGranted = default(Currency), Currency CostOfPointsReturned = default(Currency))
        {
            this.SellerSKU = SellerSKU;
            this.OrderItemId = OrderItemId;
            this.OrderAdjustmentItemId = OrderAdjustmentItemId;
            this.QuantityShipped = QuantityShipped;
            this.ItemChargeList = ItemChargeList;
            this.ItemChargeAdjustmentList = ItemChargeAdjustmentList;
            this.ItemFeeList = ItemFeeList;
            this.ItemFeeAdjustmentList = ItemFeeAdjustmentList;
            this.ItemTaxWithheldList = ItemTaxWithheldList;
            this.PromotionList = PromotionList;
            this.PromotionAdjustmentList = PromotionAdjustmentList;
            this.CostOfPointsGranted = CostOfPointsGranted;
            this.CostOfPointsReturned = CostOfPointsReturned;
        }

        /// <summary>
        /// The seller SKU of the item. The seller SKU is qualified by the seller&#39;s seller ID, which is included with every call to the Selling Partner API.
        /// </summary>
        /// <value>The seller SKU of the item. The seller SKU is qualified by the seller&#39;s seller ID, which is included with every call to the Selling Partner API.</value>
        [DataMember(Name = "SellerSKU", EmitDefaultValue = false)]
        public string SellerSKU { get; set; }

        /// <summary>
        /// An Amazon-defined order item identifier.
        /// </summary>
        /// <value>An Amazon-defined order item identifier.</value>
        [DataMember(Name = "OrderItemId", EmitDefaultValue = false)]
        public string OrderItemId { get; set; }

        /// <summary>
        /// An Amazon-defined order adjustment identifier defined for refunds, guarantee claims, and chargeback events.
        /// </summary>
        /// <value>An Amazon-defined order adjustment identifier defined for refunds, guarantee claims, and chargeback events.</value>
        [DataMember(Name = "OrderAdjustmentItemId", EmitDefaultValue = false)]
        public string OrderAdjustmentItemId { get; set; }

        /// <summary>
        /// The number of items shipped.
        /// </summary>
        /// <value>The number of items shipped.</value>
        [DataMember(Name = "QuantityShipped", EmitDefaultValue = false)]
        public int? QuantityShipped { get; set; }

        /// <summary>
        /// A list of charges associated with the shipment item.
        /// </summary>
        /// <value>A list of charges associated with the shipment item.</value>
        [DataMember(Name = "ItemChargeList", EmitDefaultValue = false)]
        public ChargeComponentList ItemChargeList { get; set; }

        /// <summary>
        /// A list of charge adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.
        /// </summary>
        /// <value>A list of charge adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.</value>
        [DataMember(Name = "ItemChargeAdjustmentList", EmitDefaultValue = false)]
        public ChargeComponentList ItemChargeAdjustmentList { get; set; }

        /// <summary>
        /// A list of fees associated with the shipment item.
        /// </summary>
        /// <value>A list of fees associated with the shipment item.</value>
        [DataMember(Name = "ItemFeeList", EmitDefaultValue = false)]
        public FeeComponentList ItemFeeList { get; set; }

        /// <summary>
        /// A list of fee adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.
        /// </summary>
        /// <value>A list of fee adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.</value>
        [DataMember(Name = "ItemFeeAdjustmentList", EmitDefaultValue = false)]
        public FeeComponentList ItemFeeAdjustmentList { get; set; }

        /// <summary>
        /// A list of taxes withheld information for a shipment item.
        /// </summary>
        /// <value>A list of taxes withheld information for a shipment item.</value>
        [DataMember(Name = "ItemTaxWithheldList", EmitDefaultValue = false)]
        public TaxWithheldComponentList ItemTaxWithheldList { get; set; }

        /// <summary>
        /// Gets or Sets PromotionList
        /// </summary>
        [DataMember(Name = "PromotionList", EmitDefaultValue = false)]
        public PromotionList PromotionList { get; set; }

        /// <summary>
        /// A list of promotion adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.
        /// </summary>
        /// <value>A list of promotion adjustments associated with the shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.</value>
        [DataMember(Name = "PromotionAdjustmentList", EmitDefaultValue = false)]
        public PromotionList PromotionAdjustmentList { get; set; }

        /// <summary>
        /// The cost of Amazon Points granted for a shipment item.
        /// </summary>
        /// <value>The cost of Amazon Points granted for a shipment item.</value>
        [DataMember(Name = "CostOfPointsGranted", EmitDefaultValue = false)]
        public Currency CostOfPointsGranted { get; set; }

        /// <summary>
        /// The cost of Amazon Points returned for a shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.
        /// </summary>
        /// <value>The cost of Amazon Points returned for a shipment item. This value is only returned for refunds, guarantee claims, and chargeback events.</value>
        [DataMember(Name = "CostOfPointsReturned", EmitDefaultValue = false)]
        public Currency CostOfPointsReturned { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ShipmentItem {\n");
            sb.Append("  SellerSKU: ").Append(SellerSKU).Append("\n");
            sb.Append("  OrderItemId: ").Append(OrderItemId).Append("\n");
            sb.Append("  OrderAdjustmentItemId: ").Append(OrderAdjustmentItemId).Append("\n");
            sb.Append("  QuantityShipped: ").Append(QuantityShipped).Append("\n");
            sb.Append("  ItemChargeList: ").Append(ItemChargeList).Append("\n");
            sb.Append("  ItemChargeAdjustmentList: ").Append(ItemChargeAdjustmentList).Append("\n");
            sb.Append("  ItemFeeList: ").Append(ItemFeeList).Append("\n");
            sb.Append("  ItemFeeAdjustmentList: ").Append(ItemFeeAdjustmentList).Append("\n");
            sb.Append("  ItemTaxWithheldList: ").Append(ItemTaxWithheldList).Append("\n");
            sb.Append("  PromotionList: ").Append(PromotionList).Append("\n");
            sb.Append("  PromotionAdjustmentList: ").Append(PromotionAdjustmentList).Append("\n");
            sb.Append("  CostOfPointsGranted: ").Append(CostOfPointsGranted).Append("\n");
            sb.Append("  CostOfPointsReturned: ").Append(CostOfPointsReturned).Append("\n");
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
            return this.Equals(input as ShipmentItem);
        }

        /// <summary>
        /// Returns true if ShipmentItem instances are equal
        /// </summary>
        /// <param name="input">Instance of ShipmentItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ShipmentItem input)
        {
            if (input == null)
                return false;

            return
                (
                    this.SellerSKU == input.SellerSKU ||
                    (this.SellerSKU != null &&
                    this.SellerSKU.Equals(input.SellerSKU))
                ) &&
                (
                    this.OrderItemId == input.OrderItemId ||
                    (this.OrderItemId != null &&
                    this.OrderItemId.Equals(input.OrderItemId))
                ) &&
                (
                    this.OrderAdjustmentItemId == input.OrderAdjustmentItemId ||
                    (this.OrderAdjustmentItemId != null &&
                    this.OrderAdjustmentItemId.Equals(input.OrderAdjustmentItemId))
                ) &&
                (
                    this.QuantityShipped == input.QuantityShipped ||
                    (this.QuantityShipped != null &&
                    this.QuantityShipped.Equals(input.QuantityShipped))
                ) &&
                (
                    this.ItemChargeList == input.ItemChargeList ||
                    (this.ItemChargeList != null &&
                    this.ItemChargeList.Equals(input.ItemChargeList))
                ) &&
                (
                    this.ItemChargeAdjustmentList == input.ItemChargeAdjustmentList ||
                    (this.ItemChargeAdjustmentList != null &&
                    this.ItemChargeAdjustmentList.Equals(input.ItemChargeAdjustmentList))
                ) &&
                (
                    this.ItemFeeList == input.ItemFeeList ||
                    (this.ItemFeeList != null &&
                    this.ItemFeeList.Equals(input.ItemFeeList))
                ) &&
                (
                    this.ItemFeeAdjustmentList == input.ItemFeeAdjustmentList ||
                    (this.ItemFeeAdjustmentList != null &&
                    this.ItemFeeAdjustmentList.Equals(input.ItemFeeAdjustmentList))
                ) &&
                (
                    this.ItemTaxWithheldList == input.ItemTaxWithheldList ||
                    (this.ItemTaxWithheldList != null &&
                    this.ItemTaxWithheldList.Equals(input.ItemTaxWithheldList))
                ) &&
                (
                    this.PromotionList == input.PromotionList ||
                    (this.PromotionList != null &&
                    this.PromotionList.Equals(input.PromotionList))
                ) &&
                (
                    this.PromotionAdjustmentList == input.PromotionAdjustmentList ||
                    (this.PromotionAdjustmentList != null &&
                    this.PromotionAdjustmentList.Equals(input.PromotionAdjustmentList))
                ) &&
                (
                    this.CostOfPointsGranted == input.CostOfPointsGranted ||
                    (this.CostOfPointsGranted != null &&
                    this.CostOfPointsGranted.Equals(input.CostOfPointsGranted))
                ) &&
                (
                    this.CostOfPointsReturned == input.CostOfPointsReturned ||
                    (this.CostOfPointsReturned != null &&
                    this.CostOfPointsReturned.Equals(input.CostOfPointsReturned))
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
                if (this.SellerSKU != null)
                    hashCode = hashCode * 59 + this.SellerSKU.GetHashCode();
                if (this.OrderItemId != null)
                    hashCode = hashCode * 59 + this.OrderItemId.GetHashCode();
                if (this.OrderAdjustmentItemId != null)
                    hashCode = hashCode * 59 + this.OrderAdjustmentItemId.GetHashCode();
                if (this.QuantityShipped != null)
                    hashCode = hashCode * 59 + this.QuantityShipped.GetHashCode();
                if (this.ItemChargeList != null)
                    hashCode = hashCode * 59 + this.ItemChargeList.GetHashCode();
                if (this.ItemChargeAdjustmentList != null)
                    hashCode = hashCode * 59 + this.ItemChargeAdjustmentList.GetHashCode();
                if (this.ItemFeeList != null)
                    hashCode = hashCode * 59 + this.ItemFeeList.GetHashCode();
                if (this.ItemFeeAdjustmentList != null)
                    hashCode = hashCode * 59 + this.ItemFeeAdjustmentList.GetHashCode();
                if (this.ItemTaxWithheldList != null)
                    hashCode = hashCode * 59 + this.ItemTaxWithheldList.GetHashCode();
                if (this.PromotionList != null)
                    hashCode = hashCode * 59 + this.PromotionList.GetHashCode();
                if (this.PromotionAdjustmentList != null)
                    hashCode = hashCode * 59 + this.PromotionAdjustmentList.GetHashCode();
                if (this.CostOfPointsGranted != null)
                    hashCode = hashCode * 59 + this.CostOfPointsGranted.GetHashCode();
                if (this.CostOfPointsReturned != null)
                    hashCode = hashCode * 59 + this.CostOfPointsReturned.GetHashCode();
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
