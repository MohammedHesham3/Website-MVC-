using Microsoft.AspNetCore.Mvc;
using Website_MVC_.Models;

namespace Website_MVC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private static List<Food> _Food = new List<Food>
        {
            new Food { FoodId = 1, Name = "Apple", ServingSize = 100, Calories = 52, Protein = 0.3f, Carbs = 13.8f, Fat = 0.2f }
        };

        [HttpPost]
        public IActionResult CreateFood(Food food)
        {
            food.FoodId = _Food.Max(f => f.FoodId) + 1;
            _Food.Add(food);
            return CreatedAtAction(nameof(GetFoodById), new { id = food.FoodId }, food);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            var food = _Food.FirstOrDefault(f => f.FoodId == id);
            if (food == null)
                return NotFound();
            return Ok(food);
        }

        [HttpGet]
        public IActionResult GetAllFoods()
        {
            return Ok(_Food);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood( int id, FoodDto updatedFood)
        {
            var food = _Food.FirstOrDefault(f => f.FoodId == id);
            if (food == null)
                return NotFound();
            food.Name = updatedFood.Name;
            food.ServingSize = updatedFood.ServingSize;
            food.Calories = updatedFood.Calories;
            food.Protein = updatedFood.Protein;
            food.Carbs = updatedFood.Carbs;
            food.Fat = updatedFood.Fat;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFoodById(int id) 
        {
            var food = _Food.FirstOrDefault(f => f.FoodId == id);

            if (food == null)
                return NotFound();
            _Food.Remove(food);
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
