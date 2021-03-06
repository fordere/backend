﻿using System;
using System.Collections.Generic;

namespace Fordere.RestService.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            var rnd = new Random();

            while (n > 1)
            {
                var k = (rnd.Next(0, n)%n);
                n--;

                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Pop<T>(this IList<T> list, int index)
        {
            T item = list[index];
            list.Remove(item);

            return item;
        }
    }
}