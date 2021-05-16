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
        public async Task<ActionResult<SubProject>> Get(string projectCode, int id)
        {
            try
            {
                return Ok(await _projectRepository.GetSubProjectAsync(projectCode, id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
        }

        // POST api/projects/{projectCode}/<ProjectsController>
        [HttpPost]
        public async Task<ActionResult<SubProjectModel>> Post( string projectCode, [FromBody] SubProject subProject)
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
                var newEntity = _mapper.Map<SubProject>(subProject);

                _projectRepository.AddEntity(newEntity);

                _projectRepository.UpdateProjectState(existingProject, subProject.State);

                if (await _projectRepository.SaveChangesAsync())
                {

                    return Created($"/api/projects/{projectCode}/{subProject.Code}", subProject);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return BadRequest();
        } 

        // PUT api/projects/{projectCode}/<ProjectsController>/5
        [HttpPut("{code}")]
        public async Task<ActionResult<SubProjectModel>> Put(string projectCode, string code,[FromBody] SubProject subProject)
        {
            try
            {
                var oldSubProject = await _projectRepository.GetSubProjectAsync(projectCode, subProject.Id);
                if (oldSubProject == null) return NotFound($"Project {subProject.Code} not exists.");
                var project = await _projectRepository.GetProjectAsync(projectCode);


                _mapper.Map(subProject, oldSubProject);

                _projectRepository.UpdateProjectState(project, subProject.State);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok(subProject);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return BadRequest();

        }

        // DELETE api/projects/{projectCode}/<ProjectsController>/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var project = await _projectRepository.GetProjectAsync(code, true, true);
                if (project == null) return NotFound();

                _projectRepository.DeleteEntity(project);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok();
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
