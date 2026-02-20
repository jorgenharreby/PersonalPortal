using Microsoft.AspNetCore.Mvc;
using PersonalPortal.API.Data;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextNotesController : ControllerBase
{
    private readonly ITextNoteRepository _repository;

    public TextNotesController(ITextNoteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TextNote>>> GetAll()
    {
        var notes = await _repository.GetAllAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TextNote>> GetById(Guid id)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note == null)
            return NotFound();
        
        return Ok(note);
    }

    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<TextNote>>> GetLatest(int count)
    {
        var notes = await _repository.GetLatestAsync(count);
        return Ok(notes);
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<TextNote>>> Search(string searchTerm)
    {
        var notes = await _repository.SearchAsync(searchTerm);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] TextNote note)
    {
        var id = await _repository.CreateAsync(note);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] TextNote note)
    {
        note.Id = id;
        var success = await _repository.UpdateAsync(note);
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
