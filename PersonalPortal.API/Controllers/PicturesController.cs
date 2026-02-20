using Microsoft.AspNetCore.Mvc;
using PersonalPortal.API.Data;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IPictureRepository _repository;

    public PicturesController(IPictureRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Picture>>> GetAll()
    {
        var pictures = await _repository.GetAllAsync();
        return Ok(pictures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Picture>> GetById(Guid id)
    {
        var picture = await _repository.GetByIdAsync(id);
        if (picture == null)
            return NotFound();
        
        return Ok(picture);
    }

    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<Picture>>> GetLatest(int count)
    {
        var pictures = await _repository.GetLatestAsync(count);
        return Ok(pictures);
    }

    [HttpGet("recipe/{recipeId}")]
    public async Task<ActionResult<IEnumerable<Picture>>> GetByRecipeId(Guid recipeId)
    {
        var pictures = await _repository.GetByRecipeIdAsync(recipeId);
        return Ok(pictures);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] Picture picture)
    {
        var id = await _repository.CreateAsync(picture);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] Picture picture)
    {
        picture.Id = id;
        var success = await _repository.UpdateAsync(picture);
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
