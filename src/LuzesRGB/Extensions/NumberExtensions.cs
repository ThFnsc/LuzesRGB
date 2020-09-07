using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class NumberExtensions
    {
        public static byte Map(this byte x, byte in_min, byte in_max, byte out_min, byte out_max) =>
            (byte)((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
    }
}
