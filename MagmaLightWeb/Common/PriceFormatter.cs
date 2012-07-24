using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagmaLightWeb.NaplampaService;
using System.Text;

namespace MagmaLightWeb.Common
{
    public class PriceFormatter
    {
        public static string Format(decimal value, Currency currency)
        {
            return currency.Prefix + value.ToString(("#0." + LoopChar('0', currency.DefaultDecimalPlaces))) + " " + currency.Postfix;
        }

        private static string LoopChar(char ch, int iteration)
        {
            if ((ch == '0') && (iteration == 0)) return "";
            if ((ch == '0') && (iteration == 1)) return "0";
            if ((ch == '0') && (iteration == 2)) return "00";
            if ((ch == '0') && (iteration == 3)) return "000";
            if ((ch == '0') && (iteration == 4)) return "0000";
            if ((ch == '0') && (iteration == 5)) return "00000";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iteration; i++)
            {
                sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
