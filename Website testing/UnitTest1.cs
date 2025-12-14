using Microsoft.AspNetCore.Mvc;
using Website_MVC_.Controllers;
using Website_MVC_.Models;
using Xunit;

namespace Website_MVC_.Tests.Controllers
{
    public class FoodControllerTests : IDisposable
    {
        private readonly FoodController _controller;

        public FoodControllerTests()
        {
            _controller = new FoodController();
            ResetFoodList();
        }

        public void Dispose()
        {
            ResetFoodList();
        }

        private void ResetFoodList()
        {
            var field = typeof(FoodController).GetField("_Food",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var initialList = new List<Food>
            {
                new Food { FoodId = 1, Name = "Apple", ServingSize = 100, Calories = 52, Protein = 0.3f, Carbs = 13.8f, Fat = 0.2f }
            };

            field?.SetValue(null, initialList);
        }

        [Fact]
        public void GetAllFoods_ReturnsOkResult_WithListOfFoods()
        {
            var result = _controller.GetAllFoods();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var foods = Assert.IsAssignableFrom<List<Food>>(okResult.Value);
            Assert.NotEmpty(foods);
        }

        [Fact]
        public void GetFoodById_ReturnsOkResult_WhenFoodExists()
        {
            int existingId = 1;

            var result = _controller.GetFoodById(existingId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var food = Assert.IsType<Food>(okResult.Value);
            Assert.Equal(existingId, food.FoodId);
        }

        [Fact]
        public void GetFoodById_ReturnsNotFound_WhenFoodDoesNotExist()
        {
            int nonExistingId = 999;

            var result = _controller.GetFoodById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateFood_ReturnsCreatedAtAction_WithNewFood()
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

            var result = _controller.CreateFood(newFood);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var food = Assert.IsType<Food>(createdResult.Value);
            Assert.Equal("Banana", food.Name);
            Assert.True(food.FoodId > 0);
        }

        [Fact]
        public void UpdateFood_ReturnsNoContent_WhenFoodExists()
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

            var result = _controller.UpdateFood(existingId, updatedFoodDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateFood_ReturnsNotFound_WhenFoodDoesNotExist()
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

            var result = _controller.UpdateFood(nonExistingId, updatedFoodDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteFoodById_ReturnsNoContent_WhenFoodExists()
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
            var createResult = _controller.CreateFood(foodToDelete) as CreatedAtActionResult;
            var createdFood = createResult?.Value as Food;

            var result = _controller.DeleteFoodById(createdFood!.FoodId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteFoodById_ReturnsNotFound_WhenFoodDoesNotExist()
        {
            int nonExistingId = 999;

            var result = _controller.DeleteFoodById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateFood_AssignsCorrectFoodId()
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

            var result1 = _controller.CreateFood(food1) as CreatedAtActionResult;
            var result2 = _controller.CreateFood(food2) as CreatedAtActionResult;

            var createdFood1 = result1?.Value as Food;
            var createdFood2 = result2?.Value as Food;

            Assert.NotNull(createdFood1);
            Assert.NotNull(createdFood2);
            Assert.True(createdFood2.FoodId > createdFood1.FoodId);
        }
    }
}