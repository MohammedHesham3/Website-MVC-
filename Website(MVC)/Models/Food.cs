namespace Website_MVC_.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public required string Name { get; set; }
        public short ServingSize { get; set; } 
        public short Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fat { get; set; }
    }
}
