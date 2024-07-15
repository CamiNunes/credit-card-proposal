using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProposalCreditCard.Application.Utils
{
    public class CreditCardNumberGenerator
    {
        private static Random random = new Random();

        public static string GenerateCreditCardNumber()
        {
            const int length = 16;
            string cardNumber = string.Empty;
            for (int i = 0; i < length; i++)
            {
                cardNumber += random.Next(0, 10).ToString();
            }
            return cardNumber;
        }
    }
}
