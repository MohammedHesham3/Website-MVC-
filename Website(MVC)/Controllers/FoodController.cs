using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_MVC_.Data;
using Website_MVC_.Models;

namespace Website_MVC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFoodById), new { id = food.FoodId }, food);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound();
            return Ok(food);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _context.Foods.ToListAsync();
            return Ok(foods);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(int id, FoodDto updatedFood)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound();

            food.Name = updatedFood.Name;
            food.ServingSize = updatedFood.ServingSize;
            food.Calories = updatedFood.Calories;
            food.Protein = updatedFood.Protein;
            food.Carbs = updatedFood.Carbs;
            food.Fat = updatedFood.Fat;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodById(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
                return NotFound();

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class FoodDto
    {
        public required string Name { get; set; }
        public short ServingSize { get; set; }
        public short Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fat { get; set; }
    }
}
