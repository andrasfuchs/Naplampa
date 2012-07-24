using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaplampaWcfHost.DataContract
{
    public class ValidationResult
    {
        public string PropertyName;
        public ValidationType Type;
        public ValidationResultType ResultType;
        public object[] Parameters;
    }
}
