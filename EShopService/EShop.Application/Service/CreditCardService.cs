
using System.Text.RegularExpressions;

namespace EShop.Application.Services
{
    public class CreditCardService
    {

        public Boolean ValidateCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
            if (!cardNumber.All(char.IsDigit))
                return false;

            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }

    }
}
