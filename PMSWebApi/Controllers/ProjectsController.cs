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
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        // GET: api/<ProjectsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> Get(bool includeTasks, bool includeSubProjects)
        {
            try
            {
                var results = await _projectRepository.GetProjectsAsync(includeTasks, includeSubProjects);
                if (!results.Any()) return NotFound();

                return  base.Ok(_mapper.Map<IEnumerable<ProjectModel>>(results));

            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
            
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{code}")]
        public async Task<ActionResult<ProjectModel>> Get(string code, bool includeTasks, bool includeSubProjects)
        {
            try
            {
                var result = await _projectRepository.GetProjectAsync(code, includeTasks, includeSubProjects);
                if (result == null) return NotFound();

                return base.Ok(_mapper.Map<ProjectModel>(result));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
        }

        // POST api/<ProjectsController>
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> Post([FromBody] ProjectModel project)
        {
            try
            {
                var existing = await _projectRepository.GetProjectAsync(project.Code);
                if (existing != null)
                {
                    return BadRequest("Project already exists.");
                }

                _projectRepository.AddEntity(_mapper.Map<DTOEntities.Project>(project));
                
                if (await _projectRepository.SaveChangesAsync())
                {
                    return Created($"/api/projects/{project.Code}", project);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }

            return BadRequest();
        }

        // PUT api/<ProjectsController>/5
        [HttpPut("{code}")]
        public async Task<ActionResult<ProjectModel>> Put(string code, [FromBody] ProjectModel project)
        {
            try
            {
                var oldProject = await _projectRepository.GetProjectAsync(code);

                if (oldProject == null) return NotFound($"Project {code} not exists.");

                _mapper.Map(project, oldProject);

                if (await _projectRepository.SaveChangesAsync())
                {
                    return Ok(project);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return BadRequest();

        }

        // DELETE api/<ProjectsController>/5
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
