using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public class OrderCosts
    {
        public int TotalWeight;
        public int CurrencyId;
        public decimal TransactionCost;
        public decimal PackageCost;
        public decimal SendingCost;
        public decimal ProductCost;
        public decimal InsuranceCost;
        public decimal QuantityDiscount;
        public decimal CouponDiscount;
        public decimal Total;
    }
}
