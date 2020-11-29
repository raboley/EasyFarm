namespace EasyFarm.Mapping
{
    public interface ISpot
    {
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }
        int F { get; set; }
        int G { get; set; }
        int H { get; set; }
        System.Collections.Generic.List<Spot> Neighbors { get; set; }
        Spot Previous { get; set; }
        bool IsWall { get; set; }
    }
}