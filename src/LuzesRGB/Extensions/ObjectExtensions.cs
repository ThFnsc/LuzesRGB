using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object input) =>
            JsonConvert.SerializeObject(input);
    }
}
