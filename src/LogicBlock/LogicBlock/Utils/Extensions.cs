using System;
using System.Collections.Generic;

namespace LogicBlock.Utils.Extensions
{
    public static class Extensions
    {
        private static Random _random = new Random();
        public static void Shuffle(this List<int> wordSequence)
        {
            for (int i = wordSequence.Count - 1; i >= 0; i--)
            {
                var j = _random.Next(i);
                var buffer = wordSequence[i];
                wordSequence[i] = wordSequence[j];
                wordSequence[j] = buffer;
            }
        }
    }
}