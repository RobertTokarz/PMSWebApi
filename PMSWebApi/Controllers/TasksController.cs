using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSWebApi.Data;
using PMSWebApi.DTOEntities;
using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Controllers
{


    [Route("api/projects/{projectCode}/[controller]")]
    [Route("api/projects/{projectCode}/subprojects/{subProjectId}/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public TasksController(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        // GET: api/projects/{projectCode}/<TaskController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> Get(string projectCode, bool includeSubTasks = false)
        {
            try
            {
                return Ok(await _projectRepository.GetTasksAsync(projectCode, includeSubTasks));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }

        }

        // GET api/projects/{projectCode}/<TaskContrloller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> Get(string projectCode, int id, bool includeSubTasks = false)
        {
            try
            {
                var result = await _projectRepository.GetTaskAsync(projectCode, id, includeSubTasks);
                if (result == null) return NotFound();
                return base.Ok(_mapper.Map<TaskModel>(result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
        }

        // POST api/projects/{projectCode}/<TaskController>
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> Post(string projectCode, [FromBody] TaskModel task, int subProjectId = -1)
        {
            try
            {
                string url;
                var existingProject = await _projectRepository.GetProjectAsync(projectCode, true, true);
                if (existingProject == null)
                {
                    return BadRequest("Project not exists.");
                }
                task.ProjectId = existingProject.Id;
                if (subProjectId == -1)
                {
                    task.ProjectType = ProjectType.Main;
                    if (existingProject.Tasks.Count() > 0)
                    {
                        if (existingProject.Tasks.Any(x => x.Id == task.Id)) return BadRequest($"Task {task.Id} alredy exists.");
                    }
                   
                }
                else
                {
                    var existingSubProject = await _projectRepository.GetSubProjectAsync(projectCode, subProjectId, true);
                    task.ProjectType = ProjectType.Sub;
                    task.SubProjectId = existingSubProject.Id;
                    if (existingSubProject.Tasks.Count() > 0)
                    {
                        if (existingSubProject.Tasks.Any(x => x.Id == task.Id)) return BadRequest($"Task {task.Id} alredy exists.");
                    }
                }
               
                var newTask = _mapper.Map<DTOEntities.Task>(task);

                _projectRepository.AddEntity(newTask);

                _projectRepository.UpdateProjectState(existingProject, task.State);

                if (await _projectRepository.SaveChangesAsync())
                {
                    if(subProjectId == -1)
                    {
                        url = $"/api/projects/{projectCode}/tasks/{newTask.Id}";
                    }
                    else
                    {
                        url = url = $"/api/projects/{projectCode}/subprojects/{subProjectId}/tasks/{newTask.Id}";                        
                    }
                    return Created(url, _mapper.Map<ProjectModel>(existingProject));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return BadRequest();
        }

        // PUT api/projects/{projectCode}/<ProjectsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectModel>> Put(string projectCode, int id, [FromBody] SubProjectModel subProject)
        {
            try
            {
                var oldSubProject = await _projectRepository.GetSubProjectAsync(projectCode, id);
                if (oldSubProject == null) return NotFound($"Project {subProject.Code} not exists.");
                var project = await _projectRepository.GetProjectAsync(projectCode, true, true);
                _mapper.Map(subProject, oldSubProject);
                _projectRepository.UpdateProjectState(project, subProject.State);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<ProjectModel>(project));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return BadRequest();

        }

        // DELETE api/projects/{projectCode}/<TaskController]/5
        // DELETE api/projects/{projectCode}/subprojects/{subProjectId}/<TaskController]/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string projectCode, int id)
        {
            try
            {
                var task = await _projectRepository.GetTaskAsync(projectCode, id, true);
                if (task == null) return NotFound();

                _projectRepository.DeleteEntity(task);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok($"Task {id} from Project {projectCode} deleted");
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }

            return BadRequest("Failed to delete the project");

        }

    }
}