using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogApp.Controllers
{
    public class ProductCatalogController
    {
        private readonly List<Product> products = new List<Product>();

        private static class Commands
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

                string category = GetUserInput("Enter a Category: ");
                if (category.Equals(Commands.Quit, StringComparison.OrdinalIgnoreCase))
                {
                    DisplayProducts();
                    break;
                }

                string name = GetUserInput("Enter a Product Name: ");
                if (string.IsNullOrWhiteSpace(name))
                {
                    PrintError("Product name cannot be empty.");
                    continue;
                }

                decimal price = GetPriceInput("Enter a Price: ");

                products.Add(new Product { Category = category, Name = name, Price = price });
                PrintSuccess("The product was successfully added!");
            }
        }

        private string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? "";
        }

        private decimal GetPriceInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal price) && price > 0)
                {
                    return price;
                }
                PrintError("Invalid price. Please enter a positive number.");
            }
        }

        private void DisplayProducts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Category\tProduct\tPrice");
            Console.ResetColor();

            var sortedProducts = products.OrderBy(p => p.Price).ToList();
            sortedProducts.ForEach(p => Console.WriteLine($"{p.Category}\t{p.Name}\t{p.Price}"));

            decimal total = sortedProducts.Sum(p => p.Price);
            Console.WriteLine($"\nTotal amount: {total}");

            Console.WriteLine($"\nTo enter a new product - enter: \"{Commands.AddProduct}\" | To search for a product - enter: \"{Commands.SearchProduct}\" | To quit - enter: \"{Commands.Quit}\"");
            HandleUserChoice();
        }

        private void HandleUserChoice()
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

        private void SearchProduct()
        {
            Console.Write("Enter a Product Name: ");
            string searchName = Console.ReadLine()?.Trim() ?? "";

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