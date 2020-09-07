using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static T AsJson<T>(this string input, bool errorInstantiatesNew = true) where T : new()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new Exception();
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception e)
            {
                if (errorInstantiatesNew)
                    return new T();
                throw e;
            }
        }
    }
}