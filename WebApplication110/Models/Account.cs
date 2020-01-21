using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication110.Models
{

    public class Account
    {
        
        [Required]
        [Key]
        public int accountNumber { get; set; }


       [Required(ErrorMessage = "Please select Account Types.")]
        [Display(Name = "Types")]
        public Types AccountTypes { get; set; }


        public double InterestRate { get; set; }
        [Display(Name = "Balance")]
        [Required(ErrorMessage = "Please Enter the initial balance.")]
        public double Balance { get; set; }

        [Display(Name = "Opening Date")]
        [DataType(DataType.Date)]
        public DateTime createdAt { get; set; }

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

    }
        
        public enum Types
        {
            Checking,
            Business,
            Loan,
            Term
        }
    

}
