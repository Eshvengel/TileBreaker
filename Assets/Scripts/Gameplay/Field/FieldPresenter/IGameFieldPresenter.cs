using System;
using System.Collections;

namespace Assets.Scripts.Gameplay.Field.FieldPresenter
{
    public interface IGameFieldPresenter
    {
        IEnumerator Present(GameField gameField, Action callback);
    }
}
