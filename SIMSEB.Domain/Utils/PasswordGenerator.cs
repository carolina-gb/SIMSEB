using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Utils
{
    public static class PasswordGenerator
    {
        private static readonly string _chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@$%#?";

        public static string Generate(int length)
        {
            var random = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => _chars[random.Next(_chars.Length)]).ToArray());
        }
    }
}
