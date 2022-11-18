using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Giriniz")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Şifre Giriniz")]
        public string Password { get; set; }
    }
}