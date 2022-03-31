namespace Crafter.RecipeComposition.Services
{
    public sealed class RecipeService
    {
        public IEnumerable<string> IdentifyPossibilities()
        {
            var random = new Random();
            var recipes = new[]
            {
                "Asian Coleslaw",
                "Road Beet Salad",
                "Cesar Salad Supreme",
                "Good for You Geek Salad",
                "Chef John's Raw Kale Salad",
                "Pasta Salad"
            };

            var first = random.Next(recipes.Length - 1);
            return recipes.Skip(first).Take(random.Next(recipes.Length - first));
        }
    }
}
