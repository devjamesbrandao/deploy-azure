using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}