using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RoleCreateModel
    {
         [Required(ErrorMessage ="Kullanıcı Adı Giriniz")]
        public string Name { get; set; }
    }
}