using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class LoginUserViewModel
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        /// <value></value>
        [Required]
        public string Password { get; set; }
    }
}