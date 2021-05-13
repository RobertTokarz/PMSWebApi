using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSWebApi.Data;
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

        
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        } 

        // GET: api/<ProjectsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            try
            {
                return Ok(await _projectRepository.GetProjectsAsync());
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
            
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Project>> Get(string code)
        {
            try
            {
                return Ok(await _projectRepository.GetProjectAsync(code));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }
        }

        // POST api/<ProjectsController>
        [HttpPost]
        public async Task<ActionResult<Project>> Post([FromBody] Project project)
        {
            try
            {
                var existing = await _projectRepository.GetProjectAsync(project.Code);
                if (existing != null)
                {
                    return BadRequest("Project already exists.");
                }
                 _projectRepository.AddEntity(project);
                
                if (await _projectRepository.SaveChangesAsync())
                {
                    return Created($"/api/projects/{project.Code}", project);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB Issue");
            }

            return BadRequest();
        }

        // PUT api/<ProjectsController>/5
        [HttpPut("{code}")]
        public void Put(string code, [FromBody] Project project)
        {

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
