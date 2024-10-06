using System.ComponentModel.DataAnnotations;

namespace TaskMasterBackend.Database
{
    public class Task
    {
        [Key]
        public virtual Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        // public DateTime CompletedAt { get; set; }
        //  public DateTime CreatedAt { get; set; }
        //  public DateTime UpdatedAt { get; set; }

    }
}
