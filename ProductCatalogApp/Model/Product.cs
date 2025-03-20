using System;


namespace ProductCatalogApp
{
    public class Product
    {
        public required string Category { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}