using Xunit;
using ProductCatalogApp;
using System;

namespace ProductCatalogApp.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Program_Output_ShouldBeCorrect()
        {
            // Arrange
            var expectedOutput = "Dotnet Console App is Running! 🚀";

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw); // Redirect Console output to StringWriter

                // Act
                Program.Main(); // Run the Main method

                // Assert
                var result = sw.ToString().Trim(); // Capture output and trim spaces
                Assert.Equal(expectedOutput, result); // Ensure the output matches
            }
        }
    }
}

