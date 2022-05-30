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
        bool Equal(int x, int y);
        bool Equal(ITile tile);
        void OnPlayerExit(Player player);
        void OnPlayerEnter(Player player);
    }
}
