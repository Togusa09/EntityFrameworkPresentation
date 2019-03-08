using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EntityFrameworkPresentation.DataContext;

namespace RepopulateData
{
    class Program
    {
        static void Main(string[] args)
        {
            var customerNames = File.ReadAllLines("CustomerNames.txt");
            var productNames = File.ReadAllLines("ProductNames.txt");

            using (var db = new CustomerOrderContext())
            {
                var random = new Random();
                

                foreach (var customerName in customerNames.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    db.Customers.Add(new Customer()
                    {
                        Name = customerName,
                        Balance =  (decimal)(Math.Round(random.NextDouble() * 1000, 2))
                    });
                }

                foreach (var productName in productNames.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    db.Products.Add(new Product
                    {
                        Description = productName,
                        ImageLocation = productName + ".png",
                        Price = (decimal)(Math.Round(random.NextDouble() * 333, 2))
                        // Need to populate the large data field
                    });
                }

                db.SaveChanges();

                var customers = db.Customers.Select(x => x.Id).ToArray();
                var products = db.Products.Select(x => x.Id).ToArray();
                var productPrices = db.Products.ToDictionary(x => x.Id, x => x.Price);

                for (var orderNo = 0; orderNo < 10000; orderNo++)
                {
                    var customerId = customers[random.Next(customers.Length)];

                    var order = new Order {CustomerId = customerId};

                    var orderLines = random.Next(10);
                    for (var lineNo = 0; lineNo < orderLines; lineNo++)
                    {
                        var productId = products[random.Next(products.Length)];
                        var quantity = random.Next(100);
                        order.OrderItems.Add(new OrderItem
                        {
                            Quantity = quantity,
                            ProductId = productId,
                            ItemPrice = productPrices[productId],
                            LinePrice = productPrices[productId] * quantity
                        });
                    }
                }
            }
        }
    }
}
