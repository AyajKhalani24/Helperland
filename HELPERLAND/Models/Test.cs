using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HELPERLAND.Models
{
    [Keyless]
    [Table("Test")]
    public partial class Test
    {
        public int TestId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? TestName { get; set; }
    }
}
