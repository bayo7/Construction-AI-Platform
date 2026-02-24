using System.ComponentModel.DataAnnotations;

namespace Construction.Web.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Lütfen Rol Adını Boş Geçmeyiniz")]
        public string Name { get; set; }
    }
}
