using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Codes_EShop_Domain.Exceptions;

public class CardNumberTooShortException : Exception
{
    public CardNumberTooShortException() : base("Card Number is too short") { }

    public CardNumberTooShortException(Exception innerException) : base("Card Number is too short", innerException) { }
}
