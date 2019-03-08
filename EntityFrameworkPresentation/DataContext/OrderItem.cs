using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkPresentation.DataContext
{
    public partial class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal LinePrice { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
