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

        [Theory]
        [InlineData("3497 7965 8312 797", "American Express")]
        [InlineData("345-470-784-783-010", "American Express")]
        [InlineData("378523393817437", "American Express")]
        [InlineData("4024-0071-6540-1778", "Visa")]
        [InlineData("4532 2080 2150 4434", "Visa")]
        [InlineData("4532289052809181", "Visa")]
        [InlineData("5530016454538418", "MasterCard")]
        [InlineData("5551561443896215", "MasterCard")]
        [InlineData("5131208517986691", "MasterCard")]
        [InlineData("", "Unknown")]
        public void ValidateCard_CheckGetCardType_ReturnTrue(string cardNumber, string cardType)
        {
            // Arrange
            var creditCardService = new CreditCardService();

            // Act
            var result = creditCardService.GetCardType(cardNumber);

            // Assert
            Assert.Equal(result, cardType);
        }
    }
}
