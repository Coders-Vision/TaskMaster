using AutoMapper;
using TaskMasterBackend.Dto.Task;
using TaskModel = TaskMasterBackend.Database.Task;
namespace TaskMasterBackend.Configuration
{
    public class MapperConfig: Profile
    {
        public MapperConfig() { 
        CreateMap<TaskModel, CreateTaskDto>().ReverseMap();
        CreateMap<TaskModel, GetAllTasksDto>().ReverseMap();
        CreateMap<TaskModel, GetTaskDto>().ReverseMap();
        CreateMap<TaskModel, UpdateTaskDto>().ReverseMap();
        }
    }
}
