using System;
using System.Collections.Generic;
using System.Threading;

namespace MTGinator.Extensions
{
    public static  class ShuffleExtensions
    {
        public static void ShuffleManyTimes<T>(this IList<T> list, int nbOfShuffles)
        {
            for (int i = 0; i < nbOfShuffles; i++)
            {
                list.Shuffle();
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static class ThreadSafeRandom
        {
            [ThreadStatic] private static Random Local;

            public static Random ThisThreadsRandom
            {
                get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)); }
            }
        }
    }
}
