using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WeCollect.App.Extensions
{
    public static class Wei
    {
        private static decimal _weiInEth = 1000000000000000000;

        public static BigInteger FromEth(decimal eth)
        {
            var wei = eth * _weiInEth;
            return new BigInteger(wei);
        }
    }
}
