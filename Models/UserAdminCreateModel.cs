using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserAdminCreateModel
    {
        [Required(ErrorMessage ="Kullanıcı Adı Giriniz")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Mail giriniz")]
        [Required(ErrorMessage = "Email Giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Cinsiyet Giriniz")]
        public string Gender { get; set; }
    }
}