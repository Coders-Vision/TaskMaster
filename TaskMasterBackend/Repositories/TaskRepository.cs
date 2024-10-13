using TaskMasterBackend.Contracts;
using TaskMasterBackend.Database;
using TaskModel = TaskMasterBackend.Database.Task;

namespace TaskMasterBackend.Repositories
{
    public class TaskRepository : GenericRepository<TaskModel>, ITaskRepository
    {
        private readonly TaskManagerDbContext _context;
        public TaskRepository(TaskManagerDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<TaskModel> GetTaskById(Guid taskId) { 
            var task = await _context.Tasks.FindAsync(taskId);
            return task;
        }
    }
}
