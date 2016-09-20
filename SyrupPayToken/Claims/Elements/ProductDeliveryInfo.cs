using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SyrupPayToken.exception;
using System;

namespace SyrupPayToken.Claims
{
    [JsonObject(MemberSerialization.Fields)]
    public sealed class ProductDeliveryInfo : Element
    {
        [JsonConverter(typeof(StringEnumConverter))]
        private DeliveryType deliveryType = DeliveryType.UNDEFINED;
        private string deliveryName;
        private bool defaultDeliveryCostApplied;
        private bool additionalDeliveryCostApplied;
        private bool shippingAddressDisplay;

        public ProductDeliveryInfo SetDeliveryType(DeliveryType deliveryType)
        {
            this.deliveryType = deliveryType;
            return this;
        }

        public ProductDeliveryInfo SetDeliveryName(string deliveryName)
        {
            this.deliveryName = deliveryName;
            return this;
        }

        public ProductDeliveryInfo SetDefaultDeliveryCostApplied(bool defaultDeliveryCostApplied)
        {
            this.defaultDeliveryCostApplied = defaultDeliveryCostApplied;
            return this;
        }

        public ProductDeliveryInfo SetAdditionalDeliveryCostApplied(bool additionalDeliveryCostApplied)
        {
            this.additionalDeliveryCostApplied = additionalDeliveryCostApplied;
            return this;
        }

        public ProductDeliveryInfo SetShippingAddressDisplay(bool shippingAddressDisplay)
        {
            this.shippingAddressDisplay = shippingAddressDisplay;
            return this;
        }

        public void ValidRequired()
        {
            if (deliveryType == DeliveryType.UNDEFINED || String.IsNullOrEmpty(deliveryName))
            {
                throw new IllegalArgumentException("ProductDeliveryInfo object couldn't be with null fields. deliveryType : " + deliveryType + ", deliveryName : " + deliveryName);
            }
        }
    }
}
