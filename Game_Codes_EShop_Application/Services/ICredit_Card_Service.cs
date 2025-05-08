namespace Game_Codes_EShop_Application.Services;

public interface ICredit_Card_Service
{
    public bool ValidateCardNumber(string cardNumber);

    public string GetCardType(string cardNumber);
}