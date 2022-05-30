using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Field.FieldPresenter
{
    public class LeftDownToRightUpGameFieldPresenter : IGameFieldPresenter
    {
       public IEnumerator Present(GameField gameField, Action callback)
        {
            var tiles = gameField.GetTiles();
            var xMax = tiles.Max(tile => tile.X);
            var yMax = tiles.Max(tile => tile.Y);
            var index = 0;

            while (index <= xMax || index <= yMax)
            {
                if (index <= yMax) // Check Y line;
                {
                    for (int i = 0; i < index; i++)
                    {
                        var tile = gameField[i, Math.Min(index, yMax)];
                        tile?.Show();
                    }
                }

                if (index <= xMax) // Check X line;
                {
                    for (int i = 0; i < index; i++)
                    {
                        var tile = gameField[Math.Min(index, xMax), i];
                        tile?.Show();
                    }
                }

                var rightCornerTile = gameField[index, index];
                rightCornerTile?.Show();

                yield return new WaitForSeconds(0.1f);

                index++;
            }

            callback?.Invoke();
        }
    }
}
