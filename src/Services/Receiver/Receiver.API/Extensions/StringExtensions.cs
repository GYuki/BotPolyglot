using System;
using System.Linq;

namespace Receiver.API.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharCapitalize(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
    }
}