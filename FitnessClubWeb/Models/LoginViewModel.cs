using System.ComponentModel.DataAnnotations;

namespace FitnessClubWeb.Models
{
    /// <summary>
    /// класс, который содержит данные пользователя, вошедшего на сайт
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
