using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public class ProductDiscountCalculationResult
    {
        public decimal OriginalPrice;
        public decimal PriceAfterQuantityDiscount;
        public decimal PriceAfterCouponDiscount;
        public decimal FinalPrice;
    }
}
