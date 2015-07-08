using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


/// This code was taken from http://thinketg.com/how-to-generate-better-random-numbers-in-c-net-2/
/// the built in random function was not proving random enough data, resulting in poor generation

namespace Terrerieh___Culminating
{

    class CryptoRandom : RandomNumberGenerator
    {
        private static RandomNumberGenerator r;

        public CryptoRandom()
        {
            r = RandomNumberGenerator.Create();
        }

        public override void GetBytes(byte[] buffer)
        {
            r.GetBytes(buffer);
        }

        public double NextDouble()
        {
            byte[] b = new byte[4];
            r.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            return Convert.ToInt32(Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue);
        }

        public int Next()
        {
            return Next(0, Int32.MaxValue);
        }

        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}
