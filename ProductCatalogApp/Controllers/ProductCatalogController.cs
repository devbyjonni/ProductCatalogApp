using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using ProductCatalogApp.Model;

namespace ProductCatalogApp.Controllers
{
    public class ProductCatalogController
    {
        // Public list of products (accessible for testing)
        public readonly List<Product> products = new List<Product>();

        // Command constants used for menu navigation
        public static class Commands
        {
            public const string AddProduct = "P";
            public const string SearchProduct = "S";
            public const string Quit = "Q";
        }

        // Main program loop
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("To enter a new product - follow the steps | To quit - enter: \"Q\"");
                Console.ResetColor();

                // Get category input or return if user quits
                string? category = GetUserInput("Enter a Category: ", Console.In);
                if (category == null)
                {
                    DisplayProducts();
                    return;
                }

                // Get product name input or return if user quits
                string? name = GetUserInput("Enter a Product Name: ", Console.In);
                if (name == null)
                {
                    DisplayProducts();
                    return;
                }

                // Get and validate price input or return if user quits
                decimal? price = ReadValidPrice("Enter a Price: ", Console.In);
                if (price == null)
                {
                    DisplayProducts();
                    return;
                }

                // Add the product to the catalog
                products.Add(new Product { Category = category, Name = name, Price = price.Value });

                PrintSuccess("The product was successfully added!");
            }
        }

        // Prompts user for input and returns trimmed result or null if user quits
        public string? GetUserInput(string prompt, TextReader inputReader)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = inputReader.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    PrintError("Input cannot be empty.");
                    continue;
                }

                if (input.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return input;
            }
        }

        // Prompts the user for a valid price or returns null if they choose to quit
        public decimal? ReadValidPrice(string prompt, TextReader inputReader)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = inputReader.ReadLine()?.Trim() ?? string.Empty;

                // Allow the user to exit by typing "Q"
                if (input.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                // Try to parse the input as a positive decimal
                if (IsValidPrice(input, out decimal price))
                {
                    return price;
                }

                // Show error and re-prompt
                PrintError("Invalid price. Please enter a positive number.");
            }
        }

        // Validates price input and ensures it’s a positive decimal
        public bool IsValidPrice(string input, out decimal price)
        {
            return decimal.TryParse(
                input,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out price
            ) && price > 0;
        }

        // Displays all products in the catalog, sorted by price
        public void DisplayProducts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            var sortedProducts = products.OrderBy(p => p.Price).ToList();

            sortedProducts.ForEach(p => Console.WriteLine($"{p.Category}\t{p.Name}\t{p.Price}"));

            decimal total = sortedProducts.Sum(p => p.Price);
            Console.WriteLine($"\nTotal amount: {total}");

            PrintCommandMenu();
            HandleUserChoice();
        }

        // Handles user input for the main menu
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

        // Searches the product list by name
        public void SearchProduct()
        {
            Console.Write("Enter a Product Name: ");
            string searchName = Console.ReadLine()?.Trim() ?? "";

            // Exit if user enters "Q"
            if (searchName.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var foundProducts = products
                .Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            if (!foundProducts.Any())
            {
                PrintError("\nNo products found matching your search.");
            }
            else
            {
                foreach (var product in foundProducts)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{product.Category}\t{product.Name}\t{product.Price}");
                    Console.ResetColor();
                }
            }

            PrintCommandMenu();
            HandleUserChoice();
        }

        // Displays error messages in red
        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Displays success messages in green
        private void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Displays menu command options
        private void PrintCommandMenu()
        {
            Console.WriteLine($"\nTo enter a new product - enter: \"{Commands.AddProduct}\" | To search for a product - enter: \"{Commands.SearchProduct}\" | To quit - enter: \"{Commands.Quit}\"");
        }
    }
}
