namespace TaskMasterBackend.Dto.Task
{
    public class GetAllTasksDto
    {
        public virtual Guid Id { get; set; }
        public string Name { get; set; }
        //  public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
