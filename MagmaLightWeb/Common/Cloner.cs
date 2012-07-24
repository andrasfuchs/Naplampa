using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace MagmaLightWeb.Common
{
    public class Cloner
    {
        public static T[] CloneAll<T>(object[] source) where T : new()
        {
            T[] result = new T[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                object sourceItem = source[i];
                Type sourceType = sourceItem.GetType();

                T destItem = new T();
                Type destType = destItem.GetType();

                if (sourceType.FullName != destType.FullName) throw new Exception("The source and destination types must be the same.");

                foreach (PropertyInfo pi in sourceType.GetProperties())
                {
                    pi.SetValue(destItem, pi.GetValue(sourceItem, null), null);
                }

                result[i] = destItem;
            }

            return result;
        }
    }
}
