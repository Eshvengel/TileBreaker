using Assets.Scripts.Data.TilesData;
using Assets.Scripts.Gameplay;
using Assets.Scripts.Gameplay.Tiles;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Core.RuntimeEditor.Editor
{
    [CustomEditor(typeof(RuntimeLevelEditor))]

    public class RuntimeLevelEditorVisualizer : UnityEditor.Editor
    {
        public RuntimeLevelEditor RuntimeLevelEditor { get; private set; }

        private void OnEnable() 
        {
            RuntimeLevelEditor = target as RuntimeLevelEditor;
        }

        public override void OnInspectorGUI()
        {
            DrawGridParams();
            DrawBrushParams();
            DrawLoadLevel();
            DrawButtons();
            DrawParams();

            if (GUI.changed)
                SetObjectDirty(RuntimeLevelEditor.gameObject);
        }

        private void DrawLoadLevel()
        {
            RuntimeLevelEditor.LevelId = EditorGUILayout.IntField("LevelId ", RuntimeLevelEditor.LevelId);
        }

        private void DrawGridParams()
        { 
//            RuntimeLevelEditor.GridSize = EditorGUILayout.I("GridSize", (int)RuntimeLevelEditor.GridSize, 1, 7);
        }

        private void DrawBrushParams()
        {
            RuntimeLevelEditor.TileType = (TileType)EditorGUILayout.EnumPopup("Tile Type", RuntimeLevelEditor.TileType); 

            if (RuntimeLevelEditor.TileType == TileType.Common)
            {
                // TODO: Add params for common tiles...
            }

            if (RuntimeLevelEditor.TileType == TileType.Jump)
            {
                RuntimeLevelEditor.Direction = (Direction)EditorGUILayout.EnumPopup("Direction", RuntimeLevelEditor.Direction);
                RuntimeLevelEditor.JumpPower = EditorGUILayout.IntSlider("Jump Power", RuntimeLevelEditor.JumpPower, 1, 5);
            }

            if (RuntimeLevelEditor.TileType == TileType.Slide)
            {
                RuntimeLevelEditor.Direction = (Direction) EditorGUILayout.EnumPopup("Direction", RuntimeLevelEditor.Direction);
                RuntimeLevelEditor.SlidePower = EditorGUILayout.IntSlider("Slide Power", RuntimeLevelEditor.SlidePower, 1, 5);
            }

            if (RuntimeLevelEditor.TileType == TileType.Start)
            {

            }
        }

        private void DrawButtons()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Load"))
                RuntimeLevelEditor.LoadLevel();

            if (GUILayout.Button("Save"))
                RuntimeLevelEditor.SaveLevel();

            if (GUILayout.Button("Clear"))
                RuntimeLevelEditor.Clear();

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Play"))
                RuntimeLevelEditor.Play();

            if (GUILayout.Button("Stop"))
                RuntimeLevelEditor.Stop();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawParams()
        {
            RuntimeLevelEditor.EditorCamera = (Camera)EditorGUILayout.ObjectField("Editor Camera", RuntimeLevelEditor.EditorCamera, typeof(Camera), true);
//            RuntimeLevelEditor.TileFactory = (TileFactory)EditorGUILayout.ObjectField("Tile Creator", RuntimeLevelEditor.TileFactory, typeof(TileFactory), true);
//            RuntimeLevelEditor.PlayerActionHandler = (PlayerActionHandler)EditorGUILayout.ObjectField("Step Handler", RuntimeLevelEditor.PlayerActionHandler, typeof(PlayerActionHandler), true);
            RuntimeLevelEditor.Player = (Player)EditorGUILayout.ObjectField("Player", RuntimeLevelEditor.Player, typeof(Player), true);
        }

        private static void SetObjectDirty(GameObject go)
        {
            if (EditorApplication.isPlaying) 
                return;

            EditorUtility.SetDirty(go);
            EditorSceneManager.MarkSceneDirty(go.scene);
        }
    }
}
