using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Codes_EShop_Domain.Exceptions;

public class CardNumberInvalidException : Exception
{
    public CardNumberInvalidException() : base("Card Number is invalid") { }

    public CardNumberInvalidException(Exception innerException) : base("Card Number is invalid", innerException) { }
}
