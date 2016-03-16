namespace RequestLibrary
{
    class Product
    {
        public string Name { get; protected set; }
        public float Price { get; protected set; }
        int ProductionPlace { get; set; }

        public Product(string name, float price, int productionPlace)
        {
            Name = name;
            Price = price;
            ProductionPlace = productionPlace;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}