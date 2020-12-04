namespace FinalFantasyXI.Context
{
    public class Item
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public int Flag { get; set; }
        public int Price { get; set; }
        public byte[] Extra { get; set; }
    }
}