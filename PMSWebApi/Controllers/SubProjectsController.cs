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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PMSWebApi.Controllers
{
    [Route("api/projects/{projectCode}/[controller]")]
    [ApiController]
    public class SubProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public SubProjectsController(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        // GET: api/projects/{projectCode}/<ProjectsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubProjectModel>>> Get(string projectCode)
        {
            try
            {
                return Ok(await _projectRepository.GetSubProjectsAsync(projectCode));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }

        }

        // GET api/projects/{projectCode}/<ProjectsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubProjectModel>> Get(string projectCode, int id, bool includeTasks)
        {          
            try
            {
                var result = await _projectRepository.GetSubProjectAsync(projectCode, id, includeTasks);
                if (result == null) return NotFound();
                return base.Ok(_mapper.Map<SubProjectModel>(result));
             }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
        }

        // POST api/projects/{projectCode}/<ProjectsController>
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> Post( string projectCode, [FromBody] SubProjectModel subProject)
        {
            try
            {
                var existingProject = await _projectRepository.GetProjectAsync(projectCode, true, true);
                if (existingProject == null)
                {
                    return BadRequest("Project not exists.");
                }
                
                subProject.ProjectId = existingProject.Id;

                if (existingProject.SubProjects != null)
                {
                    if(existingProject.SubProjects.Any(x => x.Code == subProject.Code)) return BadRequest($"Subproject {subProject.Code} alredy exists.");
                }
                var newSubProject = _mapper.Map<SubProject>(subProject);

                _projectRepository.AddEntity(newSubProject);

                _projectRepository.UpdateProjectState(existingProject, subProject.State);

                if (await _projectRepository.SaveChangesAsync())
                {

                    return Created($"/api/projects/{projectCode}/subprojects/{newSubProject.Id}", _mapper.Map<ProjectModel>(existingProject));
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
        public async Task<ActionResult<ProjectModel>> Put(string projectCode, int id,[FromBody] SubProjectModel subProject)
        {
            try
            {
                var oldSubProject = await _projectRepository.GetSubProjectAsync(projectCode, id);
                if (oldSubProject == null) return NotFound($"Project {subProject.Code} not exists.");
                var project = await _projectRepository.GetProjectAsync(projectCode,true,true);
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

        // DELETE api/projects/{projectCode}/<ProjectsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string projectCode, int id)
        {
            try
            {
                var project = await _projectRepository.GetSubProjectAsync(projectCode, id, true);
                if (project == null) return NotFound();

                _projectRepository.DeleteEntity(project);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok($"Subproject {projectCode} deleted");
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
