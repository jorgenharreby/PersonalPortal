using Microsoft.AspNetCore.Mvc;
using PersonalPortal.API.Data;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _repository;

    public RecipesController(IRecipeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetAll()
    {
        var recipes = await _repository.GetAllAsync();
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetById(Guid id)
    {
        var recipe = await _repository.GetByIdAsync(id);
        if (recipe == null)
            return NotFound();
        
        return Ok(recipe);
    }

    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetLatest(int count)
    {
        var recipes = await _repository.GetLatestAsync(count);
        return Ok(recipes);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetByType(string type)
    {
        var recipes = await _repository.GetByTypeAsync(type);
        return Ok(recipes);
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<Recipe>>> Search(string searchTerm)
    {
        var recipes = await _repository.SearchAsync(searchTerm);
        return Ok(recipes);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] Recipe recipe)
    {
        var id = await _repository.CreateAsync(recipe);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] Recipe recipe)
    {
        recipe.Id = id;
        var success = await _repository.UpdateAsync(recipe);
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
