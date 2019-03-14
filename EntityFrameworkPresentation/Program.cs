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
                //SimpleQueries.Query1(db);
                //SimpleQueries.AddData1(db);
                //NoTrackingQueries.Query1(db);
                //NoTrackingQueries.AddData1(db);
                //IncludedQueries.Query1(db);
                //IncludedQueries.AddData1(db);
                //AnonymousQueries.Query1(db);
            }

        }
    }

    public class SimpleQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            var orders = db.Orders.ToList();

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

        public static void Delete1(CustomerOrderContext db)
        {
            var customers = db.Customers.OrderByDescending(x => x.Id).Take(100).ToList();
            foreach (var customer in customers)
            {
                db.Customers.Remove(customer);
            }
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
                Console.WriteLine("Order number: " + order.Id);

                // And queries database again for each order to get the order items
                foreach (var orderItem in order.OrderItems)
                {
                    var quantity = orderItem.Quantity;
                    // And again for each product on each order item
                    var description = orderItem.Product.Description;

                    Console.WriteLine($"Product: {description}", quantity);
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

        public static void Delete1(CustomerOrderContext db)
        {
            var customers = db.Customers.OrderByDescending(x => x.Id).Take(100).ToList();
            db.Customers.RemoveRange(customers);
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

        public static void AddData1(CustomerOrderContext db)
        {
            for (int id = 0; id < 100; id++)
            {
                var balance = id;
                // EF detects each of these as a discrete change, and processes in the insert statements one at a time
                var customer = new Customer
                {
                    Name = $"Customer Name",
                    Balance = balance
                };

                db.Customers.Attach(customer);
                db.Entry(customer).State = EntityState.Added;
                db.Entry(customer).State = EntityState.Modified;
                db.Entry(customer).State = EntityState.Deleted;
            }

            db.SaveChanges();
        }

        public static void Delete1(CustomerOrderContext db)
        {
            var customers = db.Customers
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Take(100)
                .ToList();

            foreach (var customer in customers)
            {
                db.Customers.Attach(customer);
                db.Entry(customer).State = EntityState.Deleted;
            }
        }
    }

    public class AnonymousQueries
    {
        public static void Query1(CustomerOrderContext db)
        {
            // Queries database
            // By querying to anonymous types, you essentially define a custom query that will only return the fields you 
            // ask for. This is advantageous in situations where where the normal model includes a lot of unnecessary data

            var orders = db.Orders
                .Select(order => new {
                    order.Id,
                    OrderItems = order.OrderItems.Select(orderItem => 
                        new {
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


        public static void Delete1(CustomerOrderContext db)
        {
            var customerIds = db.Customers
                .OrderByDescending(x => x.Id)
                .Select(x => x.Id)
                .Take(100)
                .ToList();

            foreach (var id in customerIds)
            {
                var customer = new Customer() {Id = id};
                db.Customers.Attach(customer);
                db.Entry(customer).State = EntityState.Deleted;
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
