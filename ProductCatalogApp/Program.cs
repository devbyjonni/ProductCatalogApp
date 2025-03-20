using System;

using ProductCatalogApp.Controllers;

namespace ProductCatalogApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductCatalogController productController = new ProductCatalogController();
            productController.Run();
        }
    }
}