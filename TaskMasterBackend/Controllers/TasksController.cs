using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TaskMasterBackend.Contracts;
using TaskMasterBackend.Database;
using TaskMasterBackend.Dto.Task;
using TaskModel = TaskMasterBackend.Database.Task;

namespace TaskMasterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TasksController(IMapper mapper, ITaskRepository taskRepository)
        {
           this._taskRepository = taskRepository;
           this._mapper = mapper;   
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllTasksDto>>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var getAllTasks = _mapper.Map<IList<GetAllTasksDto>>(tasks);
            return Ok(getAllTasks);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaskDto>> GetTask(Guid id)
        {
            var task = await _taskRepository.GetTaskById(id);

            var getTask = _mapper.Map<GetTaskDto>(task);

            if (getTask == null)
            {
                return NotFound();
            }

            return getTask;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(Guid id, UpdateTaskDto taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }


            var getTask = await _taskRepository.GetAsync(id);   

            if (getTask == null)
            {
                return NotFound();
            }

             _mapper.Map(taskDto, getTask);


            try
            {
                await _taskRepository.UpdateAsync(getTask);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskModel>> PostTask(CreateTaskDto taskDto)
        {
      
            var newTask= _mapper.Map<TaskModel>(taskDto);
            await _taskRepository.AddAsync(newTask);
            return CreatedAtAction("GetTask", new { id = newTask.Id }, newTask);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                return NotFound();
            }

          await  _taskRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> TaskExists(Guid id)
        {
            return await _taskRepository.Exists(id);
        }
    }
}
