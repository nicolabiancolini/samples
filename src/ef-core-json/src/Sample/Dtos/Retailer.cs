namespace Sample.Dtos
{
    public class Retailer
    {
        public Retailer(string name)
        {
            this.Name = name;
        }

        private Retailer()
        {
        }

        public string Name { get; }
    }
}
