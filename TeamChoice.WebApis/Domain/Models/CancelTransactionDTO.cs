using System.ComponentModel.DataAnnotations;

namespace TeamChoice.WebApis.Domain.Models
{
    public class CancelTransactionDTO
    {
        /// <summary>
        /// Tawakal transaction reference to be cancelled
        /// Example: TWK-REF-001234
        /// </summary>
        [Required(ErrorMessage = "Transaction reference cannot be blank")]
        public string TawakalTxnRef { get; set; }

        /// <summary>
        /// Location code from where the cancellation is requested
        /// Example: LOC-001
        /// </summary>
        [Required(ErrorMessage = "Location code cannot be blank")]
        public string LocationCode { get; set; }

        /// <summary>
        /// Reason for cancellation
        /// Example: Customer request
        /// </summary>
        [Required(ErrorMessage = "Reason cannot be blank")]
        public string Reason { get; set; }
    }
}
