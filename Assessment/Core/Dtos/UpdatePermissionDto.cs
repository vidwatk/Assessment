using System.ComponentModel.DataAnnotations;

namespace Assessment.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
    }
}
