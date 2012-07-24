using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NaplampaWcfHost.DataContract
{
    [DataContract]
    public class ProductQuantity
    {
        [DataMember]
        public int ProductId;
        [DataMember]
        public int Quantity;
    }
}
