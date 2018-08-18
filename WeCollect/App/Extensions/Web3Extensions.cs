using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nethereum.Web3
{
    public static class Web3Extensions
    {
        public static async Task UnlockServerAccount(this Web3 web3, string serverAddress, string serverPrivateKey, ulong? durationInSeconds)
        {
            bool unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(
                serverAddress,
                serverPrivateKey,
                durationInSeconds);
            if (!unlockAccountResult)
            {
                //throw new Exception("Failed to unlock server account");
            }
        }
    }
}
