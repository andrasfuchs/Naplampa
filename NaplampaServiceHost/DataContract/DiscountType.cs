using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NaplampaWcfHost.DataContract
{
    [DataContract]
    public enum DiscountType
    { 
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        General = 1,

        [EnumMember]
        Quantity = 2,

        [EnumMember]
        Coupon = 4,

        [EnumMember]
        All = 7
    }
}
