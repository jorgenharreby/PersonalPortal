using Microsoft.AspNetCore.Mvc;
using PersonalPortal.API.Data;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController : ControllerBase
{
    private readonly IChecklistRepository _repository;

    public ChecklistsController(IChecklistRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Checklist>>> GetAll()
    {
        var checklists = await _repository.GetAllAsync();
        return Ok(checklists);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Checklist>> GetById(Guid id)
    {
        var checklist = await _repository.GetByIdAsync(id);
        if (checklist == null)
            return NotFound();
        
        return Ok(checklist);
    }

    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<Checklist>>> GetLatest(int count)
    {
        var checklists = await _repository.GetLatestAsync(count);
        return Ok(checklists);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Checklist>>> GetByType(string type)
    {
        var checklists = await _repository.GetByTypeAsync(type);
        return Ok(checklists);
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<Checklist>>> Search(string searchTerm)
    {
        var checklists = await _repository.SearchAsync(searchTerm);
        return Ok(checklists);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] Checklist checklist)
    {
        var id = await _repository.CreateAsync(checklist);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] Checklist checklist)
    {
        checklist.Id = id;
        var success = await _repository.UpdateAsync(checklist);
        if (!success)
            return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var success = await _repository.DeleteAsync(id);
        if (!success)
            return NotFound();
        
        return NoContent();
    }
}
