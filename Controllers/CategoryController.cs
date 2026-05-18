using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Data.Configurations;
using MyApi.Dtos;
using MyApi.Models;

namespace MyApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(AppDbContext context, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> Get()
    {
        var categories = await context.Categories.ToListAsync();
        var categoryReturnDtos = mapper.Map<List<CategoryReturnDto>>(categories);
        return Ok(categoryReturnDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryReturnDto>> Get(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryReturnDto = mapper.Map<CategoryReturnDto>(category);
        return Ok(categoryReturnDto);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post(CategoryCreateDto categoryCreateDto)
    {
        if (await context.Categories.AnyAsync(c => c.Name == categoryCreateDto.Name))
        {
            return BadRequest("Category with the same name already exists.");
        }

        var category = mapper.Map<Category>(categoryCreateDto);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute]int id, [FromBody]CategoryPutDto categoryPutDto)
    {
        if (id != categoryPutDto.Id)
        {
            return BadRequest("Category ID mismatch.");
        }

        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        if (await context.Categories.AnyAsync(c => c.Name == categoryPutDto.Name))
        {
            return BadRequest("Category with the same name already exists.");
        }

        mapper.Map(categoryPutDto, existingCategory);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, CategoryPatchDto categoryPatchDto)
    {
        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }
        if (!string.IsNullOrEmpty(categoryPatchDto.Description))
        {
            existingCategory.Description = categoryPatchDto.Description;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
        return NoContent();
    }
}