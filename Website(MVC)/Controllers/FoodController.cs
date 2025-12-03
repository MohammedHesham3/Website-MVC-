using Microsoft.AspNetCore.Mvc;
using Website_MVC_.Models;

namespace Website_MVC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly FoodContext _context;

        public FoodController(FoodContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateFood(FoodDto foodDto)
        {
            var food = new Food
            {
                Name = foodDto.Name,
                ServingSize = foodDto.ServingSize,
                Calories = foodDto.Calories,
                Protein = foodDto.Protein,
                Carbs = foodDto.Carbs,
                Fat = foodDto.Fat
            };
            
            _context.Foods.Add(food);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetFoodById), new { id = food.FoodId }, food);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            var food = _context.Foods.Find(id);
            if (food == null)
                return NotFound();
            return Ok(food);
        }

        [HttpGet]
        public IActionResult GetAllFoods()
        {
            return Ok(_context.Foods.ToList());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood(int id, FoodDto updatedFood)
        {
            var food = _context.Foods.Find(id);
            if (food == null)
                return NotFound();
            food.Name = updatedFood.Name;
            food.ServingSize = updatedFood.ServingSize;
            food.Calories = updatedFood.Calories;
            food.Protein = updatedFood.Protein;
            food.Carbs = updatedFood.Carbs;
            food.Fat = updatedFood.Fat;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFoodById(int id)
        {
            var food = _context.Foods.Find(id);
            if (food == null)
                return NotFound();
            _context.Foods.Remove(food);
            _context.SaveChanges();
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
