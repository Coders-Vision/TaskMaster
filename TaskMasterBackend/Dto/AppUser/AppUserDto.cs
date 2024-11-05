using System.ComponentModel.DataAnnotations;

namespace TaskMasterBackend.Dto.Task
{
    public  class AppUserDto: LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

    }
}