using EShop.Application.Services;

namespace EShop.Application.Tests.Services
{
    public class CreditCardServiceTest
    {
        [Fact]
        public void ValidateCard_CheckCardToShortLength_ReturnFalse()
        {
            // Arrange
            var creditCardService = new CreditCardService();
            string cardNumber = "1212";

            // Act
            var result = creditCardService.ValidateCardNumber(cardNumber);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void ValidateCard_CheckCardCorrectLength_ReturnTrue()
        {
            // Arrange
            var creditCardService = new CreditCardService();
            string cardNumber = "349779658312797";

            // Act
            var result = creditCardService.ValidateCardNumber(cardNumber);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCard_CheckCardToLongLength_ReturnFalse()
        {
            // Arrange
            var creditCardService = new CreditCardService();
            string cardNumber = "349779658312797349779658312797";

            // Act
            var result = creditCardService.ValidateCardNumber(cardNumber);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("3497 7965 8312 797")]
        [InlineData("345-470-784-783-010")]
        [InlineData("378523393817437")]
        [InlineData("4024-0071-6540-1778")]
        [InlineData("4532 2080 2150 4434")]
        [InlineData("4532289052809181")]
        [InlineData("5530016454538418")]
        [InlineData("5551561443896215")]
        [InlineData("5131208517986691")]
        public void ValidateCard_CheckCardValidator_ReturnTrue(string cardNumber)
        {
            // Arrange
            var creditCardService = new CreditCardService();

            // Act
            var result = creditCardService.ValidateCardNumber(cardNumber);

            // Assert
            Assert.True(result);
        }
    }
}
