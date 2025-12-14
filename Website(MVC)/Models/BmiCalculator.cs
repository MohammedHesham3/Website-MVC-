namespace Website_MVC_.Models
{
    public class BmiCalculator
    {
        public double Weight { get; set; } // in kg
        public double Height { get; set; } // in cm
        public int Age { get; set; }
        public bool Gender { get; set; } // "T=male" or "F=female"
        public string ActivityLevel { get; set; } = "moderate"; // sedentary, light, moderate, active, very_active

        public double? Bmi { get; set; }
        public string? Category { get; set; }
        public double? DailyCalories { get; set; }
        public double? ProteinGrams { get; set; }
        public double? CarbsGrams { get; set; }
        public double? FatGrams { get; set; }

        public void CalculateBmi()
        {
            if (Height > 0 && Weight > 0)
            {
                double heightInMeters = Height / 100;
                Bmi = Weight / (heightInMeters * heightInMeters);
                Category = GetBmiCategory(Bmi.Value);
            }
        }

        public void CalculateMacros()
        {
            if (Height > 0 && Weight > 0 && Age > 0)
            {
                // Calculate BMR
                double bmr;
                if (Gender) // male
                {
                    bmr = (10 * Weight) + (6.25 * Height) - (5 * Age) + 5;
                }
                else // female
                {
                    bmr = (10 * Weight) + (6.25 * Height) - (5 * Age) - 161;
                }

                // Calculate TDEE
                double activityMultiplier = ActivityLevel.ToLower() switch
                {
                    "sedentary" => 1.2,
                    "light" => 1.375,
                    "moderate" => 1.55,
                    "active" => 1.725,
                    "very_active" => 1.9,
                    _ => 1.55
                };

                DailyCalories = Math.Round(bmr * activityMultiplier, 0);

                // Calculate macros 
                ProteinGrams = Math.Round(Weight * 2.0, 1); 

                // Fat: 25-30% of calories
                FatGrams = Math.Round((DailyCalories.Value * 0.25) / 9, 1);

                
                double proteinCalories = ProteinGrams.Value * 4; 
                double fatCalories = FatGrams.Value * 9;
                double carbCalories = DailyCalories.Value - proteinCalories - fatCalories;
                CarbsGrams = Math.Round(carbCalories / 4, 1); 
            }
        }

        private string GetBmiCategory(double bmi)
        {
            if (bmi < 18.5)
                return "Underweight";
            else if (bmi < 25)
                return "Normal weight";
            else if (bmi < 30)
                return "Overweight";
            else
                return "Obese";
        }
    }
}