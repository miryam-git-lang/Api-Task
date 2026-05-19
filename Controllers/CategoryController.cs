using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public async Task<ActionResult> Get([FromQuery]int page= 1, int take = 2)
    {
        var data = await context.Categories.
        ProjectTo<CategoryReturnDto>(mapper.ConfigurationProvider)
            .Skip((page - 1) * take)
            .Take(take)
            .ToListAsync();
        var categoryPaginationDto = new CategoryPaginationDto()
        {
            Page = page,
            Take = take,
            TotalCount = await context.Categories.CountAsync(),
            Categories = data
        };
        return Ok(categoryPaginationDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryReturnDto>> Get(int id)
    {
        var request = HttpContext.Request;
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var categoryReturnDto = mapper.Map<CategoryReturnDto>(category);
        return Ok(categoryReturnDto);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post([FromForm]CategoryCreateDto categoryCreateDto)
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
    public async Task<IActionResult> Put([FromRoute]int id, [FromBody]CategoryUpdateDto categoryUpdateDto)
    {
        if (id != categoryUpdateDto.Id)
        {
            return BadRequest("Category ID mismatch.");
        }

        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        if (await context.Categories.AnyAsync(c => c.Name == categoryUpdateDto.Name))
        {
            return BadRequest("Category with the same name already exists.");
        }

        mapper.Map(categoryUpdateDto, existingCategory);
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
    public async Task<IActionResult> Patch(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
    {
        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }
        if (!string.IsNullOrEmpty(categoryUpdateDto.Description))
        {
            existingCategory.Description = categoryUpdateDto.Description;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
        return NoContent();
    }
}