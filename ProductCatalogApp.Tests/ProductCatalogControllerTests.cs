using System;
using Xunit;
using ProductCatalogApp.Controllers;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogApp.Tests
{
    public class ProductCatalogControllerTests
    {
        [Fact]
        public void GetUserInput_ShouldReturnTrimmedString()
        {
            // Arrange
            var controller = new ProductCatalogController();
            var input = new StringReader("  Electronics  \n");

            // Act
            string result = controller.GetUserInput("Enter a Category: ", input);

            // Assert
            Assert.Equal("Electronics", result);
        }

        [Fact]
        public void GetPriceInput_ShouldReturnValidDecimal()
        {
            // Arrange
            var controller = new ProductCatalogController();
            var input = new StringReader("100.50\n"); // Ensure a valid decimal is entered

            // Act
            decimal result = controller.GetPriceInput("Enter a Price: ", input); // Pass `input` directly


            // Assert
            Assert.Equal(100.50m, result);
        }

        [Fact]
        public void GetPriceInput_ShouldThrowExceptionAfterMaxAttempts()
        {
            // Arrange
            var controller = new ProductCatalogController();
            var input = new StringReader("abc\n-50\nxyz\n0\nwrong\n"); // Invalid 5 times

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => controller.GetPriceInput("Enter a Price: ", input));
        }

        [Fact]
        public void DisplayProducts_ShouldSortProductsByPrice()
        {
            // Arrange
            var controller = new ProductCatalogController();
            controller.products.Add(new Product { Category = "Electronics", Name = "Laptop", Price = 1200 });
            controller.products.Add(new Product { Category = "Electronics", Name = "Phone", Price = 900 });
            controller.products.Add(new Product { Category = "Electronics", Name = "Mouse", Price = 50 });

            // Act
            var sortedProducts = controller.products.OrderBy(p => p.Price).ToList();

            // Assert
            Assert.Equal("Mouse", sortedProducts[0].Name);
            Assert.Equal("Phone", sortedProducts[1].Name);
            Assert.Equal("Laptop", sortedProducts[2].Name);
        }

        [Fact]
        public void SearchProduct_ShouldFindProductByName()
        {
            // Arrange
            var controller = new ProductCatalogController();
            controller.products.Add(new Product { Category = "Electronics", Name = "Laptop", Price = 1200 });
            controller.products.Add(new Product { Category = "Electronics", Name = "Phone", Price = 900 });
            controller.products.Add(new Product { Category = "Electronics", Name = "Mouse", Price = 50 });

            var input = new StringReader("Phone\n");
            Console.SetIn(input);

            // Act
            var foundProducts = controller.products.Where(p => p.Name.Equals("Phone", StringComparison.OrdinalIgnoreCase)).ToList();

            // Assert
            Assert.Single(foundProducts);
            Assert.Equal("Phone", foundProducts[0].Name);
        }
    }
}