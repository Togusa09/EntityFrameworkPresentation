using System.IO;
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
                foreach (var customerName in customerNames)
                {
                    db.Customers.Add(new Customer()
                    {
                        Name = customerName
                    });
                }
            }
        }
    }
}
