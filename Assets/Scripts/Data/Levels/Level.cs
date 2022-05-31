using Assets.Scripts.Data.TilesData;

namespace Assets.Scripts.Data.Levels
{
    public class Level
    {
        public int Id { get; set; }
        public TileData[] Data { get; set; }

        public Level(int id, TileData[] data)
        {
            Id = id;
            Data = data;
        }
    }
}