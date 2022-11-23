using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Giriniz")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Şifre Giriniz")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}