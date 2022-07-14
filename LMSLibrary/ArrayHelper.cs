using System;
using System.Collections.Generic;

namespace LMSLibrary
{
    public static class ArrayHelper
    {
        public static IList<T> Shuffle<T>(this IList<T> array)
        {
            Random rand = new Random();
            for (int i = 0; i < array.Count; i++)
            {
                array.Swap(rand.Next(0, array.Count), rand.Next(0, array.Count));
            }

            return array;
        }

        public static void Swap<T>(this IList<T> array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
