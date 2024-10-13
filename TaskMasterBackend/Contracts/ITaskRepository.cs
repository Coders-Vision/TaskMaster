using TaskModel = TaskMasterBackend.Database.Task;

namespace TaskMasterBackend.Contracts
{
    public interface ITaskRepository : IGenericRepository<TaskModel>
    {
        Task<TaskModel> GetTaskById(Guid taskId);
    }
}
