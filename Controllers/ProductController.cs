using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Dtos;
using MyApi.Models;

namespace MyApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController(AppDbContext context, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task <ActionResult<IEnumerable<Product>>> Get()
    {
        var products = await context.Products
            .ProjectTo<ProductReturnDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task <IActionResult> Get(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var productReturnDto = mapper.Map<ProductReturnDto>(product);
        return Ok(productReturnDto);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Post([FromBody] ProductCreateDto productCreateDto)
    {
        var existingCategory = await context.Categories.FindAsync(productCreateDto.CategoryId);
        if (existingCategory == null)
        {
            return BadRequest("Invalid CategoryId. Category does not exist.");
        }
        
        var product = mapper.Map<Product>(productCreateDto);
        
        await context.AddAsync(product);
        await context.SaveChangesAsync();
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Product product)
    {
        if(id != product.Id)
        {
            return BadRequest("Product ID mismatch.");
        }
        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        
        var existingCategory = await context.Products.FindAsync(product.CategoryId);
        if (existingCategory == null)
        {
            return BadRequest("Invalid CategoryId. Category does not exist.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.Price = product.Price;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        context.Remove(existingProduct);
        await context.SaveChangesAsync();
        return NoContent();
    }
}