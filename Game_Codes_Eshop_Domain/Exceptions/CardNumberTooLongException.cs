using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Codes_EShop_Domain.Exceptions;

public class CardNumberTooLongException : Exception
{
    public CardNumberTooLongException() : base("Card Number is too long") { }

    public CardNumberTooLongException(Exception innerException) : base("Card Number is too long", innerException) { }
    public CardNumberTooLongException(string message, Exception innerException) { }
}
