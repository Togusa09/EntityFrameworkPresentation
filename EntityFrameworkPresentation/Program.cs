using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkPresentation.DataContext;

namespace EntityFrameworkPresentation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CustomerOrderContext())
            {
                SimpleQueries.Query1(db);
            }
        }
    }

    public class SimpleQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            var orders = db.Orders.Take(10).ToList();

            foreach (var order in orders)
            {
                Console.WriteLine("Order number: " + order);

                // And queries database again for each order to get the order items
                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = orderItem.Quantity;
                    // And again for each product on each order item
                    var description = orderItem.Product.Description;

                    Console.Write($"Product: {description}", quantity);
                }
            }
        }

        public static void AddData1(CustomerOrderContext db)
        {
            for (int i = 0; i < 100; i++)
            {
                // EF detects each of these as a discrete change, and processes in the insert statements one at a time
                db.Customers.Add(new Customer
                {
                    Name = $"Customer {i}",
                    Balance =  i
                });
            }

            db.SaveChanges();
        }
    }

    public class IncludedQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            var orders = db.Orders
                // Explicitly include other tables. EF then queries all information as a single joined query
                .Include(x => x.OrderItems)
                .Include(x => x.OrderItems.Select(oi => oi.Product))
                .ToList();

            foreach (var order in orders)
            {
                Console.WriteLine("Order number: " + order);

                // And queries database again for each order to get the order items
                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = orderItem.Quantity;
                    // And again for each product on each order item
                    var description = orderItem.Product.Description;

                    Console.Write($"Product: {description}", quantity);
                }
            }
        }

        public static void AddData1(CustomerOrderContext db)
        {
            var customers = new List<Customer>();
            for (int i = 0; i < 100; i++)
            {
                // EF detects each of these as a discrete change, and processes in the insert statements one at a time
                customers.Add(new Customer
                {
                    Name = $"Customer {i}",
                    Balance = i
                });
            }

            // When you use AddRange, all the inserts are done as part of the one operation, dramatically reducing the overhead
            db.Customers.AddRange(customers);
            db.SaveChanges();
        }
    }

    public class NoTrackingQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            var orders = db.Orders
                // Explicitly include other tables. EF then queries all information as a single joined query
                .Include(x => x.OrderItems)
                .Include(x => x.OrderItems.Select(oi => oi.Product))
                // Adding `AsNoTracking` means that EF doesn't do any change tracking on the entities. Which makes querying
                // and manipulation of the returned results faster
                .AsNoTracking()
                .ToList();

            foreach (var order in orders)
            {
                Console.WriteLine("Order number: " + order);

                // And queries database again for each order to get the order items
                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = orderItem.Quantity;
                    // And again for each product on each order item
                    var description = orderItem.Product.Description;

                    Console.Write($"Product: {description}", quantity);
                }
            }
        }
    }

    public class AnonymousQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            var orders = db.Orders
                // By querying to anonymous types, you essentially define a custom query that will only return the fields you 
                // ask for. This is advantageous in situations where where the normal model includes a lot of unnecessary data
                .Select(order => new {
                    order.Id,
                    OrderItems = order.OrderItems.Select(orderItem => new
                    {
                        orderItem.Quantity,
                        orderItem.Product.Description
                    })
                }).ToList();

            foreach (var order in orders)
            {
                Console.WriteLine("Order number: " + order);

                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = orderItem.Quantity;
                    var description = orderItem.Description;

                    Console.Write($"Product: {description}", quantity);
                }
            }
        }
    }

    public class AdditionaNotes
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Database indexes are also useful. Table records are stored in the order of their clustered index, which by default is created
            // on the primary key, by can be set to another column if needed. ie date.
            // Indexes provide a quick lookup of information by a key value, which avoids the need for a full table scan (table scan is bad)
            // Indexes can also 
        }
    }
}
