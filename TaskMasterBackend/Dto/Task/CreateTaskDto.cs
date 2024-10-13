using System.ComponentModel.DataAnnotations;

namespace TaskMasterBackend.Dto.Task
{
    public class CreateTaskDto:BaseTaskDto
    {
        [Required]
        public string Name { get; set; }


    }
}
