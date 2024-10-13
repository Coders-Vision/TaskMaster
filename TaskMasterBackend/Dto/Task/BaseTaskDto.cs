namespace TaskMasterBackend.Dto.Task
{
    public abstract class BaseTaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
