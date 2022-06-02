using IceCreamApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        projectContext _context;

        public RecipeController(projectContext context)
        {
            _context = context;
        }

        [HttpGet("/api/recipe")]
        public async Task<ActionResult<List<Recipe>>> findAll()
        {
            var recipe = await _context.Recipes.ToListAsync();
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }


        [HttpGet("/api/recipe/{id}")]
        public async Task<ActionResult<Recipe>> findByID(int id)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == id);
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [HttpGet("/api/recipe/category/{CategoryID}")]
        public async Task<ActionResult<List<Recipe>>> findRecipebyCategory(int CategoryID)
        {
            var recipe = await _context.Recipes.Where(r => r.CategoryID == CategoryID && r.Status == true).ToListAsync();
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [HttpGet("/api/recipe/latest/{CategoryID}/{number}")]
        public async Task<ActionResult<List<Recipe>>> findLatestRecipeByCategory(int CategoryID, int number)
        {
            var recipe = await _context.Recipes.Where(r => r.CategoryID == CategoryID).OrderByDescending(r => r.RecipeId).Take(number).ToListAsync();
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [HttpGet("/api/recipe/active")]
        public async Task<ActionResult<Recipe>> activeRecipeList()
        {
            var recipe = await _context.Recipes.Where(r => r.Status == true).ToListAsync();
            if (recipe != null)
            {
                return Ok(recipe);
            }
            return NotFound();
        }

        [HttpPost("/api/recipe")]
        public async Task<ActionResult> createRecipe(Recipe recipe)
        {
            if (recipe.Thumbnail == null || recipe.Thumbnail == "")
            {
                _context.Add(new Recipe
                {
                    Title = recipe.Title,
                    Ingredents = recipe.Ingredents,
                    Preparation = recipe.Preparation,
                    Description = recipe.Description,
                    CategoryID = recipe.CategoryID,
                    PayingRequired = Convert.ToBoolean(recipe.PayingRequired) ? true : false,
                    Status = Convert.ToBoolean(recipe.Status) ? true : false,
                    CreatedBy = recipe.CreatedBy,
                });

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return BadRequest();
            }

            _context.Add(new Recipe
            {
                Title = recipe.Title,
                Ingredents = recipe.Ingredents,
                Preparation = recipe.Preparation,
                Thumbnail = recipe.Thumbnail,
                Description = recipe.Description,
                CategoryID = recipe.CategoryID,
                PayingRequired = Convert.ToBoolean(recipe.PayingRequired) ? true : false,
                Status = Convert.ToBoolean(recipe.Status) ? true : false,
                CreatedBy = recipe.CreatedBy,
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("/api/recipe/{id}")]
        public async Task<ActionResult> updateRecipe(int id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }
            var updateRecipe = await _context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == id);
            if (recipe.Thumbnail == "")
            {
                updateRecipe.Title = recipe.Title;
                updateRecipe.Ingredents = recipe.Ingredents;
                updateRecipe.Preparation = recipe.Preparation;
                updateRecipe.Description = recipe.Description;
                updateRecipe.CategoryID = recipe.CategoryID;
                updateRecipe.PayingRequired = Convert.ToBoolean(recipe.PayingRequired) ? true : false;
                updateRecipe.Status = Convert.ToBoolean(recipe.Status) ? true : false;
                updateRecipe.CreatedBy = recipe.CreatedBy;
            }

            else
            {
                updateRecipe.Title = recipe.Title;
                updateRecipe.Ingredents = recipe.Ingredents;
                updateRecipe.Preparation = recipe.Preparation;
                updateRecipe.Thumbnail = recipe.Thumbnail;
                updateRecipe.Description = recipe.Description;
                updateRecipe.CategoryID = recipe.CategoryID;
                updateRecipe.PayingRequired = Convert.ToBoolean(recipe.PayingRequired) ? true : false;
                updateRecipe.Status = Convert.ToBoolean(recipe.Status) ? true : false;
                updateRecipe.CreatedBy = recipe.CreatedBy;
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("/api/recipe/status/{id}/{status}")]
        public async Task<ActionResult> updateRecipeStatus(int id, bool status)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == id);
            if (recipe != null)
            {
                recipe.Status = status;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpPut("/api/recipe/payingRequired/{id}/{payingRequired}")]
        public async Task<ActionResult> updateRecipePayingStatus(int id, bool payingRequired)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == id);
            if (recipe != null)
            {
                recipe.PayingRequired = payingRequired;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpDelete("/api/recipe/{id}")]
        public async Task<ActionResult> deleteRecipe(int id)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == id);
            if (recipe != null)
            {
                _context.Remove(recipe);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }
    }
}
