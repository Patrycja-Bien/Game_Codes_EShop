using Game_Codes_EShop_Application.Services;
using Game_Codes_EShop_Domain.Exceptions;
namespace Game_Codes_EShop_Application_Tests;

public class Verify_Card_Number_Tests
{

    [Fact]
    public void CreditCardTooShortLength_ThrowsCardNumberTooShortException()
    {
        // Arrange
        var card = new Credit_Card_Service();
        var number = "11111";

        // Act & Assert
        Assert.Throws<CardNumberTooShortException>(() => card.ValidateCardNumber(number));
    }

    [Fact]
    public void CardNumberTooLongLength_ThrowsCardNumberTooLongException()
    {
        //Arrange
        var card = new Credit_Card_Service();
        var number = "111111111111111111111111111111111111111111111111111111111111111";

        //Act & Assert
        Assert.Throws<CardNumberTooLongException>(() => card.ValidateCardNumber(number));
    }

    [Fact]
    public void CardNumberInvalid_ThrowsCardNumberInvalidException()
    {
        //Arrange
        var card = new Credit_Card_Service();
        var number = "553001%%%45&&418";

        //Act & Assert
        Assert.Throws<CardNumberInvalidException>(() => card.ValidateCardNumber(number));

    }

    [Theory]
    [InlineData("349779658312797")]
    [InlineData("4024007165401778")]
    [InlineData("345470784783010")]
    [InlineData("4532289052809181")]
    [InlineData("5530016454538418")]
    [InlineData("5131208517986691")]
    public void ValidateCard_CheckCardCorrectLength_ReturnsTrue(string cardNumber)
    {
        //Arrange
        var creditCardService = new Credit_Card_Service();

        //Act
        var result = creditCardService.ValidateCardNumber(cardNumber);

        //Assert
        Assert.True(result);

    }

    [Theory]
    [InlineData("3497-7965-8312-797")]
    [InlineData("4024-0071-6540-1778")]
    [InlineData("345-470-784-783-010")]
    public void ValidateCard_CheckCardWithDash_ReturnsTrue(string cardNumber)
    {
        //Arrange
        var creditCardService = new Credit_Card_Service();

        //Act
        var result = creditCardService.ValidateCardNumber(cardNumber);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("3497 7965 8312 797")]
    [InlineData("4024 0071 6540 1778")]
    [InlineData("4532 2080 2150 4434")]
    public void ValidateCard_CheckCardWithSpace_ReturnsTrue(string cardNumber)
    {
        //Arrange
        var creditCardService = new Credit_Card_Service();

        //Act
        var result = creditCardService.ValidateCardNumber(cardNumber);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("3497 7965 8312 797", "American Express")]
    [InlineData("4024-0071-6540-1778", "Visa")]
    [InlineData("5551561443896215", "MasterCard")]
    [InlineData("345-470-784-783-010", "American Express")]
    [InlineData("378523393817437", "American Express")]
    [InlineData("4532 2080 2150 4434", "Visa")]
    [InlineData("4532289052809181", "Visa")]
    [InlineData("5530016454538418", "MasterCard")]
    [InlineData("5131208517986691", "MasterCard")]

    public void GetCardType_ShouldReturnRightCardType_RightCardType(string cardNumber, string cardType)
    {
        // Arrange
        var cardService = new Credit_Card_Service();

        // Act
        var result = cardService.GetCardType(cardNumber);

        // Assert
        Assert.Equal(cardType, result);
    }

}