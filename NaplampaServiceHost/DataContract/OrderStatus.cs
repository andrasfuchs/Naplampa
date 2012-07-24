using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NaplampaService.DataContract
{
    [DataContract]
    public enum OrderStatus 
    { 
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        OrderPlaced = 1,

        [EnumMember]
        PaymentRequestSent = 2,

        [EnumMember]
        PaymentReceived = 4,

        [EnumMember]
        ReadyToPost = 8,

        [EnumMember]
        PackageSent = 16,

        [EnumMember]
        PackageReceived = 32,

        [EnumMember]
        SurveySent = 64,

        [EnumMember]
        WarrantyRequest = 128,

        [EnumMember]
        WarrantyOrder = 256,

        [EnumMember]
        Promotional = 512,

        [EnumMember]
        Donation = 1024,

        [EnumMember]
        Returned = 2048,

        [EnumMember]
        Refund = 4096,

        [EnumMember]
        Deleted = 65536
    }
}
