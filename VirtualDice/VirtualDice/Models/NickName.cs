using System.ComponentModel.DataAnnotations;

namespace VirtualDice.Models
{
    public class NickName
    {
        [Required(ErrorMessage = "The field is required!")]
        [MinLength(2, ErrorMessage = "Invalid symbols count!")]
        [MaxLength(10, ErrorMessage = "Invalid symbols count!")]
        public string Name { get; set; }

        public int[] Score { get; set; }

        public bool isAdmin { get; set; }
    }
}
