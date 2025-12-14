using Microsoft.AspNetCore.Mvc;
using Website_MVC_.Models;

namespace Website_MVC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BmiController : ControllerBase
    {
        [HttpPost]
        public IActionResult CalculateBmi(BmiDto bmiDto)
        {
            if (bmiDto.Weight <= 0 || bmiDto.Height <= 0 || bmiDto.Age <= 0)
            {
                return BadRequest(new { error = "Weight, height, and age must be greater than 0" });
            }

            var calculator = new BmiCalculator
            {
                Weight = bmiDto.Weight,
                Height = bmiDto.Height,
                Age = bmiDto.Age,
                Gender = bmiDto.Gender ?? true,
                ActivityLevel = bmiDto.ActivityLevel ?? "moderate"
            };

            calculator.CalculateBmi();
            calculator.CalculateMacros();

            return Ok(calculator);
        }

        [HttpGet]
        public IActionResult CalculateBmiGet(
            double weight,
            double height,
            int age,
            bool gender = true,
            string activityLevel = "moderate")
        {
            if (weight <= 0 || height <= 0 || age <= 0)
            {
                return BadRequest(new { error = "Weight, height, and age must be greater than 0" });
            }

            var calculator = new BmiCalculator
            {
                Weight = weight,
                Height = height,
                Age = age,
                Gender = gender,
                ActivityLevel = activityLevel
            };

            calculator.CalculateBmi();
            calculator.CalculateMacros();

            return Ok(calculator);
        }
    }

    public class BmiDto
    {
        public double Weight { get; set; } // kg
        public double Height { get; set; } // cm
        public int Age { get; set; }
        public bool? Gender { get; set; } // True= Male , False = Female
        public string? ActivityLevel { get; set; } 
    }
}