using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public enum ValidationType
    {
        Unknown = 0,

        RequiredField = 1,

        RegularExpression = 2,

        IntegerRange = 4,

        StringLength = 8,
        
        Custom = 16
    }
}
