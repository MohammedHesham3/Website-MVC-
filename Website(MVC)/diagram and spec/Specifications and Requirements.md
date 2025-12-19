# Software Requirements Specification
# Nutrition and BMI Tracking System

Version: 1.0  
Date: December 19, 2025  
Technology: ASP.NET Core 8.0 MVC  



# 1. Project Overview

# 1.1 Purpose
A web application that calculates BMI, personalized daily caloric needs, and macronutrient requirements. Also manages a food database with nutritional information.

# 1.2 Key Features
- BMI calculation and categorization
- Daily calorie needs calculation (TDEE)
- Macronutrient breakdown (Protein, Carbs, Fat)
- Food database management (CRUD operations)
- RESTful API with Swagger documentation



# 2. Functional Requirements

# 2.1 BMI Calculator

Calculate BMI
- Input: Weight (kg), Height (cm)
- Output: BMI value, Category (Underweight/Normal/Overweight/Obese)
- Formula: BMI = weight / (height in meters)²
- Categories:
  - Underweight: < 18.5
  - Normal: 18.5 - 24.9
  - Overweight: 25 - 29.9
  - Obese: ≥ 30

Calculate Macronutrients
- Additional Input: Age, Gender, Activity Level
- Output: Daily Calories, Protein (g), Carbs (g), Fat (g)
- Calculations:
  - BMR (Mifflin-St Jeor equation):
    - Male: (10 × weight) + (6.25 × height) - (5 × age) + 5
    - Female: (10 × weight) + (6.25 × height) - (5 × age) - 161
  - TDEE = BMR × Activity Multiplier
  - Protein = 2g per kg body weight
  - Fat = 25% of calories
  - Carbs = remaining calories

Activity Levels:
- Sedentary: 1.2
- Light: 1.375
- Moderate: 1.55 (default)
- Active: 1.725
- Very Active: 1.9

# 2.2 Food Database

Operations:
- Create new food item
- Get food by ID
- Get all foods
- Update existing food
- Delete food

Food Properties:
- Name (required)
- Serving Size (grams)
- Calories
- Protein (grams)
- Carbs (grams)
- Fat (grams)

