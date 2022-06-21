using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Data.Levels
{
    public class LevelsRepository : ILevelRepositoryReader, ILevelRepositoryWriter
    {
        private readonly SortedDictionary<int, Level> _loadedLevels;

        public LevelsRepository()
        {
            _loadedLevels = new SortedDictionary<int, Level>();
        }

        public Level this[int id] => Load(id);

        public void Save(Level level)
        {
            var settings = new JsonSerializerSettings();
            var path = GetLevelsPath(level.Id);
            var data = JsonConvert.SerializeObject(level, Formatting.None, settings);

            using (StreamWriter streamWriter = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                streamWriter.Write(data);
            }
            
            
            //File.WriteAllText(path, data, Encoding.Default);

            _loadedLevels[level.Id] = level;
        }

        public Level Load(int levelId)
        {
            // TODO: Remove hardcode
            if (levelId > _loadedLevels.Count || levelId < 1)
            {
                levelId = 1;
            }
            
            if (_loadedLevels.TryGetValue(levelId, out var value) && value != null)
            {
                return value;
            }

            return new Level(levelId, null);
        }

        public IEnumerator LoadAllLevels()
        {
            for (int i = 1; i <= Game.MAX_LEVEL; i++)
            {
                yield return LoadLevel(i);
            }
        }
        
        private IEnumerator LoadLevel(int id)
        {
            var levelPath = GetLevelsPath(id);
            var request = UnityWebRequest.Get(levelPath);

            yield return request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error))
            {
                var settings = new JsonSerializerSettings();
                var data = request.downloadHandler.text;
                var level = JsonConvert.DeserializeObject<Level>(data, settings);

                _loadedLevels[id] = level;
            }

            yield return null;
        }

        private string GetLevelsPath(int id)
        {
            var levelName = GetLevelName(id);
            
#if UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, levelName);
#elif PLATFORM_ANDROID
            return Path.Combine("jar:file://" + Application.dataPath + "!/assets", levelName);
#endif
        }

        private string GetLevelName(int id)
        {
            return $"level_{id}.json";
        } 
    }
}
