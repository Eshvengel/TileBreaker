using System.Collections;

namespace Assets.Scripts.Data.Levels
{
    public interface ILevelRepositoryReader
    {
        Level Load(int id);
        IEnumerator LoadAllLevels();
    }
}