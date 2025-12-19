using Microsoft.AspNetCore.Mvc;
using Website_MVC_.Models;

namespace Website_MVC_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BmiCalculatorController : ControllerBase
    {
        [HttpPost("calculate-macros")]
        public IActionResult CalculateMacros([FromBody] MacrosRequest request)
        {
            var calculator = new BmiCalculator
            {
                Weight = request.Weight,
                Height = request.Height,
                Age = request.Age,
                Gender = request.Gender,
                ActivityLevel = request.ActivityLevel ?? "moderate"
            };

            calculator.CalculateBmi();
            calculator.CalculateMacros();

            return Ok(new MacrosResponse
            {
                Weight = calculator.Weight,
                Height = calculator.Height,
                Age = calculator.Age,
                Gender = calculator.Gender,
                ActivityLevel = calculator.ActivityLevel,
                Bmi = calculator.Bmi,
                Category = calculator.Category,
                DailyCalories = calculator.DailyCalories,
                ProteinGrams = calculator.ProteinGrams,
                CarbsGrams = calculator.CarbsGrams,
                FatGrams = calculator.FatGrams
            });
        }
    }

    public class MacrosRequest
    {
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string? ActivityLevel { get; set; }
    }

    public class MacrosResponse
    {
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string ActivityLevel { get; set; } = "moderate";
        public double? Bmi { get; set; }
        public string? Category { get; set; }
        public double? DailyCalories { get; set; }
        public double? ProteinGrams { get; set; }
        public double? CarbsGrams { get; set; }
        public double? FatGrams { get; set; }
    }
}
