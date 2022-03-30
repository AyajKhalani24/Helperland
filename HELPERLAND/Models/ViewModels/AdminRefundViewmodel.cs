using System.ComponentModel.DataAnnotations;

namespace HELPERLAND.Models.ViewModels
{
    public class AdminRefundViewModel
    {
#nullable disable
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public decimal RefundAmount { get; set; }
        [Required]
        public string RefundReason { get; set; }
    }
}