using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProductCatalogApp.Controllers
{
    public class ProductCatalogController
    {
        // Public list to store products, allowing access for unit testing
        public readonly List<Product> products = new List<Product>();

        // Command constants for user interaction
        public static class Commands
        {
            public const string AddProduct = "P";
            public const string SearchProduct = "S";
            public const string Quit = "Q";
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\"");
                Console.ResetColor();

                string category = GetUserInput("Enter a Category: ", Console.In) ?? string.Empty;

                // Exit immediately if user enters 'Q'
                if (category.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
                {
                    DisplayProducts();
                    return;
                }

                string name = GetUserInput("Enter a Product Name: ", Console.In);
                if (string.IsNullOrWhiteSpace(name))
                {
                    PrintError("Product name cannot be empty.");
                    continue;
                }

                decimal price = GetPriceInput("Enter a Price: ", Console.In);

                // Add valid product entry to the list
                products.Add(new Product { Category = category, Name = name, Price = price });
                PrintSuccess("The product was successfully added!");
            }
        }

        // Reads and returns trimmed user input
        public string GetUserInput(string prompt, TextReader inputReader)
        {
            Console.Write(prompt);
            return inputReader.ReadLine()?.Trim() ?? "";
        }

        // Ensures valid price input with a max number of attempts
        public decimal GetPriceInput(string prompt, TextReader inputReader)
        {
            int attempts = 0;
            int maxAttempts = 5;

            while (attempts < maxAttempts)
            {
                Console.Write(prompt);
                string input = inputReader.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price) && price > 0)
                {
                    return price;
                }

                PrintError("Invalid price. Please enter a positive number.");
                attempts++;
            }

            throw new InvalidOperationException("Maximum retry attempts exceeded.");
        }

        public void DisplayProducts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            // Sort products by price in ascending order
            var sortedProducts = products.OrderBy(p => p.Price).ToList();

            // Display sorted product list
            sortedProducts.ForEach(p => Console.WriteLine($"{p.Category}\t{p.Name}\t{p.Price}"));

            // Calculate and display total price
            decimal total = sortedProducts.Sum(p => p.Price);
            Console.WriteLine($"\nTotal amount: {total}");

            Console.WriteLine($"\nTo enter a new product - enter: \"{Commands.AddProduct}\" | To search for a product - enter: \"{Commands.SearchProduct}\" | To quit - enter: \"{Commands.Quit}\"");
            HandleUserChoice();
        }

        public void HandleUserChoice()
        {
            while (true)
            {
                string choice = (Console.ReadLine() ?? "").Trim().ToUpper();

                switch (choice)
                {
                    case Commands.AddProduct:
                        Run();
                        return;
                    case Commands.SearchProduct:
                        SearchProduct();
                        return;
                    case Commands.Quit:
                        Environment.Exit(0);
                        return;
                    default:
                        PrintError("Invalid choice. Please try again.");
                        Console.Write("\nEnter a valid command: ");
                        break;
                }
            }
        }

        public void SearchProduct()
        {
            Console.Write("Enter a Product Name: ");
            string searchName = Console.ReadLine()?.Trim() ?? "";

            // Exit search if user enters 'Q'
            if (searchName.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Find products matching the search name
            var foundProducts = products.Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            if (!foundProducts.Any())
            {
                // Display message if no products were found
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo products found matching your search.");
                Console.ResetColor();
            }
            else
            {
                // Display matching products with distinct color
                foreach (var product in foundProducts)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{product.Category}\t{product.Name}\t{product.Price}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine($"\nTo enter a new product - enter: \"{Commands.AddProduct}\" | To search for a product - enter: \"{Commands.SearchProduct}\" | To quit - enter: \"{Commands.Quit}\"");
            HandleUserChoice();
        }

        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
