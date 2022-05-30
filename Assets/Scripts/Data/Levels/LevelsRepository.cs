using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Data.Levels
{
    public class LevelsRepository
    {
        private readonly SortedDictionary<int, Level> _loadedLevels;

        public LevelsRepository()
        {
            _loadedLevels = new SortedDictionary<int, Level>();
        }

        public void Save(Level level)
        {
            var settings = new JsonSerializerSettings();
            
            var path = GetPath(level.Id);
            var data = JsonConvert.SerializeObject(level, Formatting.None, settings);
            
            File.WriteAllText(path, data);

            _loadedLevels[level.Id] = null;
        }

        public Level Load(int id)
        {
            if (_loadedLevels.TryGetValue(id, out var value) && value != null)
                return value;

            var path = GetPath(id);

            if (!File.Exists(path))
                return new Level(id, null);
            
            var settings = new JsonSerializerSettings();
            var data = File.ReadAllText(path);
            var level = JsonConvert.DeserializeObject<Level>(data, settings);

            _loadedLevels[level.Id] = level;

            return level;
        }

        private string GetPath(int id)
        {
            var levelName = GetLevelName(id);

            return $"{Application.streamingAssetsPath}/{levelName}";
        }

        private string GetLevelName(int id) => $"level_{id}.json";
    }
}
