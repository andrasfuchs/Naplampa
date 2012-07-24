using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Collections;
using System.Threading;

namespace NaplampaAdmin.Util
{
    public static class ResourceManagerAdapter
    {
        public static KeyValuePair<string, string>[] ToArray(this ResourceManager resourceManager, string prefix)
        {
            List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();

            foreach (DictionaryEntry entry in resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true))
            {
                string key = entry.Key as string;
                string value = entry.Value as string;

                if (!String.IsNullOrEmpty(prefix))
                {
                    if (!key.StartsWith(prefix)) continue;
                    key = key.Substring(prefix.Length);
                }

                kvpList.Add(new KeyValuePair<string,string>(key, value));
            }

            return kvpList.ToArray();
        }

        public static KeyValuePair<int, string>[] ToIntArray(this ResourceManager resourceManager, string prefix)
        {
            List<KeyValuePair<int, string>> kvpList = new List<KeyValuePair<int, string>>();

            foreach (KeyValuePair<string, string> kvp in resourceManager.ToArray(prefix))
            {
                kvpList.Add(new KeyValuePair<int, string>(Int32.Parse(kvp.Key), kvp.Value));
            }

            return kvpList.ToArray();
        }
    }
}
