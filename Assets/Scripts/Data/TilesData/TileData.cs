using System;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Data.TilesData
{
    [Serializable]
    public class TileData
    {
        public TileType Type { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        [JsonConverter(typeof(Vec3Conv))]
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Vector3 WorldPosition { get; set; }
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Direction JumpDirection { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int JumpPower { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Direction SlideDirection { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int SlidePower { get; set; }

        public bool Solid { get; set; }
    }

    [Serializable]
    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
}
