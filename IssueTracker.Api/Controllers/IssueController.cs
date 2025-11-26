using IssueTracker.Api.Services;
using IssueTracker.Api.Entities;
using IssueTracker.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IssueService _service;

        public IssuesController(IssueService service)
        {
            _service = service;
        }

        // GET: api/issues
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = await _service.GetAllIssuesAsync();
            return Ok(issues);
        }

        // GET: api/issues/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var issue = await _service.GetIssueByIdAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            return Ok(issue);
        }

        // GET api/issues/type/{type}
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(IssueType type)
        {
            var issues = await _service.GetIssuesByTypeAsync(type);
            return Ok(issues);
        }

        // GET api/issues/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(IssueStatus status)
        {
            var issues = await _service.GetIssuesByStatusAsync(status);
            return Ok(issues);
        }

        // GET api/issues/priority/{priority}
        [HttpGet("priority/{priority}")]
        public async Task<IActionResult> GetByPriority(IssuePriority priority)
        {
            var issues = await _service.GetIssuesByPriorityAsync(priority);
            return Ok(issues);
        }

        // POST api/issues
        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDTO dto)
        {
            var newIssueId = await _service.AddIssueAsync(
                dto.Title,
                dto.Description ?? "",
                dto.Priority,
                dto.Type,
                dto.Status
            );

            return CreatedAtAction(nameof(GetById), new { id = newIssueId }, null);
        }

        // PUT api/issues/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateIssueStatusDTO dto)
        {
            var success = await _service.UpdateIssueStatusAsync(id, dto.Status);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT api/issues/{id}/description
        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateIssueDescriptionDTO dto)
        {
            var success = await _service.UpdateIssueDescriptionAsync(id, dto.Description);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT api/issues/{id}/priority
        [HttpPut("{id}/priority")]
        public async Task<IActionResult> UpdatePriority(int id, [FromBody] UpdateIssuePriorityDTO dto)
        {
            var success = await _service.UpdateIssuePriorityAsync(id, dto.Priority);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/issues/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteIssueAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
