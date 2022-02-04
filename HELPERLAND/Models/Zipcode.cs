using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HELPERLAND.Models
{
    [Table("Zipcode")]
    public partial class Zipcode
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string ZipcodeValue { get; set; } = null!;
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("Zipcodes")]
        public virtual City City { get; set; } = null!;
    }
}
