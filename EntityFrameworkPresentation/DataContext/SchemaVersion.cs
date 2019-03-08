using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPresentation.DataContext
{
    public partial class SchemaVersion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ScriptName { get; set; }

        public DateTime Applied { get; set; }
    }
}
