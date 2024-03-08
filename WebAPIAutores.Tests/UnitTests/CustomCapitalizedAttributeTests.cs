using System.ComponentModel.DataAnnotations;
using WebAppAutores.Validations;

namespace WebAPIAutores.Tests.UnitTests
{
    [TestClass]
    public class CustomCapitalizedAttributeTests
    {
        [TestMethod]
        public void When_TheFirstCharacterIsLowerCase_AnErrorIsThrown()
        {
            // Arrange
            var customCapitalized = new CustomCapitalizedAttribute();
            var value = "name";
            var valContext = new ValidationContext(new {Nombre = value});

            // Act
            var result = customCapitalized.GetValidationResult(value, valContext);

            // Assert
            Assert.AreEqual("The field must be Capitalized (Attribute)", result.ErrorMessage);
        }
    }
}