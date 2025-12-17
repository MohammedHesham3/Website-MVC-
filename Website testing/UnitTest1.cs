using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_MVC_.Controllers;
using Website_MVC_.Data;
using Website_MVC_.Models;
using Xunit;

namespace Website_MVC_.Tests.Controllers
{
    public class FoodControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FoodController _controller;

        public FoodControllerTests()
        {
            // Create in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            
            // Seed initial test data
            _context.Foods.Add(new Food 
            { 
                FoodId = 1, 
                Name = "Apple", 
                ServingSize = 100, 
                Calories = 52, 
                Protein = 0.3f, 
                Carbs = 13.8f, 
                Fat = 0.2f 
            });
            _context.SaveChanges();

            _controller = new FoodController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllFoods_ReturnsOkResult_WithListOfFoods()
        {
            var result = await _controller.GetAllFoods();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var foods = Assert.IsAssignableFrom<List<Food>>(okResult.Value);
            Assert.NotEmpty(foods);
        }

        [Fact]
        public async Task GetFoodById_ReturnsOkResult_WhenFoodExists()
        {
            int existingId = 1;

            var result = await _controller.GetFoodById(existingId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var food = Assert.IsType<Food>(okResult.Value);
            Assert.Equal(existingId, food.FoodId);
        }

        [Fact]
        public async Task GetFoodById_ReturnsNotFound_WhenFoodDoesNotExist()
        {
            int nonExistingId = 999;

            var result = await _controller.GetFoodById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateFood_ReturnsCreatedAtAction_WithNewFood()
        {
            var newFood = new Food
            {
                Name = "Banana",
                ServingSize = 120,
                Calories = 89,
                Protein = 1.1f,
                Carbs = 22.8f,
                Fat = 0.3f
            };

            var result = await _controller.CreateFood(newFood);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var food = Assert.IsType<Food>(createdResult.Value);
            Assert.Equal("Banana", food.Name);
            Assert.True(food.FoodId > 0);
        }

        [Fact]
        public async Task UpdateFood_ReturnsNoContent_WhenFoodExists()
        {
            int existingId = 1;
            var updatedFoodDto = new FoodDto
            {
                Name = "Updated Apple",
                ServingSize = 110,
                Calories = 60,
                Protein = 0.4f,
                Carbs = 15.0f,
                Fat = 0.3f
            };

            var result = await _controller.UpdateFood(existingId, updatedFoodDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateFood_ReturnsNotFound_WhenFoodDoesNotExist()
        {
            int nonExistingId = 999;
            var updatedFoodDto = new FoodDto
            {
                Name = "Non-existent Food",
                ServingSize = 100,
                Calories = 50,
                Protein = 0.5f,
                Carbs = 10.0f,
                Fat = 0.2f
            };

            var result = await _controller.UpdateFood(nonExistingId, updatedFoodDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFoodById_ReturnsNoContent_WhenFoodExists()
        {
            var foodToDelete = new Food
            {
                Name = "Orange",
                ServingSize = 130,
                Calories = 47,
                Protein = 0.9f,
                Carbs = 11.8f,
                Fat = 0.1f
            };
            var createResult = await _controller.CreateFood(foodToDelete) as CreatedAtActionResult;
            var createdFood = createResult?.Value as Food;

            var result = await _controller.DeleteFoodById(createdFood!.FoodId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFoodById_ReturnsNotFound_WhenFoodDoesNotExist()
        {
            int nonExistingId = 999;

            var result = await _controller.DeleteFoodById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateFood_AssignsCorrectFoodId()
        {
            var food1 = new Food
            {
                Name = "Food1",
                ServingSize = 100,
                Calories = 50,
                Protein = 1.0f,
                Carbs = 10.0f,
                Fat = 0.5f
            };
            var food2 = new Food
            {
                Name = "Food2",
                ServingSize = 100,
                Calories = 50,
                Protein = 1.0f,
                Carbs = 10.0f,
                Fat = 0.5f
            };

            var result1 = await _controller.CreateFood(food1) as CreatedAtActionResult;
            var result2 = await _controller.CreateFood(food2) as CreatedAtActionResult;

            var createdFood1 = result1?.Value as Food;
            var createdFood2 = result2?.Value as Food;

            Assert.NotNull(createdFood1);
            Assert.NotNull(createdFood2);
            Assert.True(createdFood2.FoodId > createdFood1.FoodId);
        }
    }

    public class BmiCalculatorTests
    {
        [Theory]
        [InlineData(70, 175, 22.86, "Normal weight")]
        [InlineData(50, 160, 19.53, "Normal weight")]
        [InlineData(100, 180, 30.86, "Obese")]
        [InlineData(45, 170, 15.57, "Underweight")]
        [InlineData(85, 175, 27.76, "Overweight")]
        public void CalculateBmi_ReturnsCorrectBmiAndCategory(
            double weight, double height, double expectedBmi, string expectedCategory)
        {
            var calculator = new BmiCalculator
            {
                Weight = weight,
                Height = height
            };

            calculator.CalculateBmi();

            Assert.NotNull(calculator.Bmi);
            Assert.Equal(expectedBmi, calculator.Bmi.Value, 2);
            Assert.Equal(expectedCategory, calculator.Category);
        }

        [Fact]
        public void CalculateBmi_WithZeroHeight_DoesNotCalculate()
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 0
            };

            calculator.CalculateBmi();

            Assert.Null(calculator.Bmi);
            Assert.Null(calculator.Category);
        }

        [Fact]
        public void CalculateBmi_WithZeroWeight_DoesNotCalculate()
        {
            var calculator = new BmiCalculator
            {
                Weight = 0,
                Height = 175
            };

            calculator.CalculateBmi();

            Assert.Null(calculator.Bmi);
            Assert.Null(calculator.Category);
        }

        [Theory]
        [InlineData(70, 175, 25, true, "moderate", 2679)]
        [InlineData(60, 165, 30, false, "moderate", 2066)]
        [InlineData(80, 180, 40, true, "sedentary", 2234)]
        [InlineData(55, 160, 20, false, "active", 2475)]
        public void CalculateMacros_ReturnsCorrectDailyCalories(
            double weight, double height, int age, bool gender, string activityLevel, double expectedCalories)
        {
            var calculator = new BmiCalculator
            {
                Weight = weight,
                Height = height,
                Age = age,
                Gender = gender,
                ActivityLevel = activityLevel
            };

            calculator.CalculateMacros();

            Assert.NotNull(calculator.DailyCalories);
            Assert.Equal(expectedCalories, calculator.DailyCalories.Value, 0);
        }

        [Fact]
        public void CalculateMacros_ReturnsCorrectProteinGrams()
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = true,
                ActivityLevel = "moderate"
            };

            calculator.CalculateMacros();

            Assert.NotNull(calculator.ProteinGrams);
            Assert.Equal(140.0, calculator.ProteinGrams.Value, 1);
        }

        [Fact]
        public void CalculateMacros_ReturnsCorrectMacronutrients()
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = true,
                ActivityLevel = "moderate"
            };

            calculator.CalculateMacros();

            Assert.NotNull(calculator.DailyCalories);
            Assert.NotNull(calculator.ProteinGrams);
            Assert.NotNull(calculator.FatGrams);
            Assert.NotNull(calculator.CarbsGrams);

            // Verify macros add up to daily calories
            double totalCalories = (calculator.ProteinGrams.Value * 4) +
                                   (calculator.FatGrams.Value * 9) +
                                   (calculator.CarbsGrams.Value * 4);

            Assert.Equal(calculator.DailyCalories.Value, totalCalories, 1);
        }

        [Theory]
        [InlineData("sedentary")]
        [InlineData("light")]
        [InlineData("moderate")]
        [InlineData("active")]
        [InlineData("very_active")]
        public void CalculateMacros_HandlesAllActivityLevels(string activityLevel)
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = true,
                ActivityLevel = activityLevel
            };

            calculator.CalculateMacros();

            Assert.NotNull(calculator.DailyCalories);
            Assert.True(calculator.DailyCalories > 0);
        }

        [Fact]
        public void CalculateMacros_WithZeroAge_DoesNotCalculate()
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 0,
                Gender = true
            };

            calculator.CalculateMacros();

            Assert.Null(calculator.DailyCalories);
            Assert.Null(calculator.ProteinGrams);
        }

        [Fact]
        public void CalculateMacros_MaleVsFemale_DifferentResults()
        {
            var male = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = true,
                ActivityLevel = "moderate"
            };

            var female = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = false,
                ActivityLevel = "moderate"
            };

            male.CalculateMacros();
            female.CalculateMacros();

            Assert.NotNull(male.DailyCalories);
            Assert.NotNull(female.DailyCalories);
            Assert.True(male.DailyCalories > female.DailyCalories);
        }

        [Fact]
        public void CalculateMacros_DefaultActivityLevel_UsesModerate()
        {
            var calculator = new BmiCalculator
            {
                Weight = 70,
                Height = 175,
                Age = 25,
                Gender = true
            };

            calculator.CalculateMacros();

            Assert.NotNull(calculator.DailyCalories);
            Assert.True(calculator.DailyCalories > 0);
        }
    }
}