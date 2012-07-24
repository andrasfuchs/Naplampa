using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public class OrderReview
    {
        public Order Order;
        public Person Payer;
        public string PaymentMethod;
    }
}
