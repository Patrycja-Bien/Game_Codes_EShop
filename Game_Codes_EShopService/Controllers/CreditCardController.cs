using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Game_Codes_EShop_Domain.Exceptions;
using Game_Codes_EShop_Domain.Enums;
using Game_Codes_EShop_Application.Services;

namespace Game_Codes_EShopService.Controllers;

public class CreditCardController : ControllerBase
{
    protected ICredit_Card_Service _creditCardService;

    public CreditCardController(ICredit_Card_Service creditCardService)
    {
        _creditCardService = creditCardService;
    }

    [HttpGet]
    public IActionResult Get(string cardNumber)
    {
        try
        {
            _creditCardService.ValidateCardNumber(cardNumber);
            return Ok(new { cardProvider = _creditCardService.GetCardType(cardNumber) });
        }
        catch (CardNumberTooLongException ex)
        {
            return StatusCode((int)HttpStatusCode.RequestUriTooLong, new { error = ex.Message, code = (int)HttpStatusCode.RequestUriTooLong });
        }
        catch (CardNumberTooShortException ex)
        {
            return BadRequest(new { error = ex.Message, code = (int)HttpStatusCode.BadRequest });
        }
        catch (CardNumberInvalidException ex)
        {
            return BadRequest(new { error = ex.Message, code = (int)HttpStatusCode.BadRequest });
        }
    }

}
