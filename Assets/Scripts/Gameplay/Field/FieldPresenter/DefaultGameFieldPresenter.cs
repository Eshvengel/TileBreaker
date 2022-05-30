using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Field.FieldPresenter
{
    public class DefaultGameFieldPresenter : IGameFieldPresenter
    {
        public IEnumerator Present(GameField gameField, Action callback)
        {
            var tiles = gameField.GetTiles();

            foreach (var tile in tiles)
            {
                tile?.Show();
            }
            
            yield return new WaitForNextFrameUnit();
            
            callback?.Invoke();
        }
    }
}