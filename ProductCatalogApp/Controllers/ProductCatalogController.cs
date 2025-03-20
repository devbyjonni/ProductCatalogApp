using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProductCatalogApp.Controllers
{
    public class ProductCatalogController
    {
        // Made public to allow unit tests to insert and verify product data.
        public readonly List<Product> products = new List<Product>();

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
                if (category.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
                {
                    DisplayProducts();
                    break;
                }

                string name = GetUserInput("Enter a Product Name: ", Console.In);
                if (string.IsNullOrWhiteSpace(name))
                {
                    PrintError("Product name cannot be empty.");
                    continue;
                }

                decimal price = GetPriceInput("Enter a Price: ", Console.In);

                products.Add(new Product { Category = category, Name = name, Price = price });
                PrintSuccess("The product was successfully added!");
            }
        }

        public string GetUserInput(string prompt, TextReader inputReader)
        {
            Console.Write(prompt);
            return inputReader.ReadLine()?.Trim() ?? "";
        }

        public decimal GetPriceInput(string prompt, TextReader inputReader)
        {
            int attempts = 0;
            int maxAttempts = 5;

            while (attempts < maxAttempts)
            {
                Console.Write(prompt);
                string input = inputReader.ReadLine() ?? string.Empty;

                // Debugging line to verify input received during testing
                Console.WriteLine($"DEBUG: Read input '{input}'");

                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price) && price > 0)
                {
                    Console.WriteLine($"DEBUG: Parsed valid price '{price}'");
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

            // LINQ: Sorting the products list by price in ascending order
            var sortedProducts = products.OrderBy(p => p.Price).ToList();

            // LINQ: Using .ForEach() to iterate and print each product
            sortedProducts.ForEach(p => Console.WriteLine($"{p.Category}\t{p.Name}\t{p.Price}"));

            // LINQ: Calculating the total price of all products
            decimal total = sortedProducts.Sum(p => p.Price);
            Console.WriteLine($"\nTotal amount: {total}");

            Console.WriteLine($"\nTo enter a new product - enter: \"{Commands.AddProduct}\" | To search for a product - enter: \"{Commands.SearchProduct}\" | To quit - enter: \"{Commands.Quit}\"");
            HandleUserChoice();
        }

        public void HandleUserChoice()
        {
            string choice = (Console.ReadLine() ?? "").Trim().ToUpper();

            switch (choice)
            {
                case Commands.AddProduct:
                    Run();
                    break;
                case Commands.SearchProduct:
                    SearchProduct();
                    break;
                case Commands.Quit:
                    Environment.Exit(0);
                    break;
                default:
                    PrintError("Invalid choice. Please try again.");
                    HandleUserChoice();
                    break;
            }
        }

        public void SearchProduct()
        {
            Console.Write("Enter a Product Name: ");
            string searchName = Console.ReadLine()?.Trim() ?? "";

            // LINQ: Filtering products based on user input
            var foundProducts = products.Where(p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            foreach (var product in products)
            {
                if (foundProducts.Contains(product))
                    Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{product.Category}\t{product.Name}\t{product.Price}");
                Console.ResetColor();
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
