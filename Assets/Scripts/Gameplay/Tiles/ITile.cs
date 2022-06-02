using Assets.Scripts.Data.TilesData;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tiles
{
    public interface ITile
    {
        int X { get; }
        int Y { get; }
        bool Interactable { get; }
        Vector3 WorldPosition { get; }
        
        void Show();
        void Hide();
        void Destroy();
        bool Equal(ITile tile);
        bool Equal(int x, int y);
        void OnPlayerExit(Player player);
        void OnPlayerEnter(Player player);
        TileData GetTileData();
    }
}
