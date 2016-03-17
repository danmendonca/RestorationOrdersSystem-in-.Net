namespace RequestLibrary
{
    public class Product
    {
        public string Name { get; protected set; }
        public float Price { get; protected set; }
        PreparationRoomID PreparationSource { get; set; }

        public Product(string name, float price, PreparationRoomID PreparationSource)
        {
            Name = name;
            Price = price;
            this.PreparationSource = PreparationSource;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}