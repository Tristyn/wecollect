using System.Numerics;

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
