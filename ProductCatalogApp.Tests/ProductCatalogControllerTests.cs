using System;
using Xunit;
using ProductCatalogApp.Controllers;
using System.IO;

namespace ProductCatalogApp.Tests
{
    public class ProductCatalogControllerTests
    {
        [Fact]
        public void GetUserInput_ShouldReturnTrimmedString()
        {
            var controller = new ProductCatalogController();
            var input = new StringReader("  Electronics  \n");

            string? result = controller.GetUserInput("Enter a Category: ", input);

            Assert.Equal("Electronics", result);
        }

        [Fact]
        public void GetUserInput_ShouldReturnNull_WhenInputIsQ()
        {
            var controller = new ProductCatalogController();
            var input = new StringReader("Q\n");

            string? result = controller.GetUserInput("Enter a Category: ", input);

            Assert.Null(result);
        }

        [Fact]
        public void ReadValidPrice_ShouldReturnDecimal_WhenValidInputProvided()
        {
            var controller = new ProductCatalogController();
            var input = new StringReader("199.99\n");

            decimal? result = controller.ReadValidPrice("Enter a Price: ", input);

            Assert.Equal(199.99m, result);
        }

        [Fact]
        public void ReadValidPrice_ShouldReturnNull_WhenInputIsQ()
        {
            var controller = new ProductCatalogController();
            var input = new StringReader("Q\n");

            decimal? result = controller.ReadValidPrice("Enter a Price: ", input);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("100", true)]
        [InlineData("0", false)]
        [InlineData("-10", false)]
        [InlineData("abc", false)]
        public void IsValidPrice_ShouldReturnExpectedResult(string input, bool expected)
        {
            var controller = new ProductCatalogController();

            bool result = controller.IsValidPrice(input, out decimal price);

            Assert.Equal(expected, result);
        }
    }
}
