# Product Catalog

## Overview
Product Catalog is a **C# console application** that allows users to add, search, and display products in a structured format.

## Features
- **Add products** by specifying:
  - **Category** (e.g., Electronics, Clothing, etc.).
  - **Product Name** (free text input).
  - **Price** (valid positive decimal values).
- **Sorting & Display:**
  - Products are **sorted by price (ascending)** before being displayed.
  - A **total sum** of all product prices is displayed.
- **Search Functionality:**
  - Users can search for a product by name.
  - Matching results are **highlighted** for better visibility.
- **Command-based navigation:**
  - `P` - Add a new product.
  - `S` - Search for a product.
  - `Q` - Quit the application.
- **Error Handling:**
  - Invalid input for price prompts a retry (up to 5 attempts).
  - Ensures product names are not empty.

## How to Use
1. Run the application.
2. Follow the prompts to enter product details.
3. Type `Q` to quit and view all products.
4. Type `S` to search for a specific product.
5. Products are displayed in sorted order by price.

## Future Improvements
- Enhance unit test coverage for validation and search functionality.
- Implement persistent storage (e.g., saving products to a file or database).
- Introduce a graphical user interface (GUI).

## Author
Developed by **Jonni Akesson** as part of the **Lexicon C# Course**.