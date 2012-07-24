using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public class ProductDiscountPrice
    {
        public Product Product;
        public Discount Discount;
        public int CurrencyId;
        public decimal MSRP;
        public decimal Price;
    }
}
