using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> input, Action<T> action)
        {
            foreach (var element in input)
                action(element);
        }

        public static void Each<T>(this IEnumerable<T> input, Action<T, int> action)
        {
            int counter = 0;
            foreach (var element in input)
                action(element, counter++);
        }
    }
}
