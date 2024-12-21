using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid.Scriptables;
using System.Linq;
using Tarodev_Pathfinding._Scripts;

namespace RP.Grid
{
    public class GridGenerator : MonoBehaviour
    {
        [FoldoutGroup("Settings"),SerializeField]public static GridGenerator Instance;
        [SerializeField] private GridSettings _scriptableGrid;

        public Dictionary<Vector2, BaseHexagon> Tiles { get; private set; }

        void Awake() => Instance = this;
        void Start()
        {
            StartSequence();
            //Tiles.
        }
        [Button("Start")]
        public void StartSequence()
        {
            Tiles = _scriptableGrid.GenerateHexGrid();

            foreach (var tile in Tiles.Values) tile.CacheNeighbors();

            BaseHexagon.OnHoverTile += OnCreateObtacle;
        }
        private void OnCreateObtacle(BaseHexagon baseHex)
        {
            baseHex.MakeObstacle();
        }
        public Vector2 GetWidthHeight()
        {
            return new Vector2(_scriptableGrid.GridWidth,_scriptableGrid.GridDepth);
        }
        /*
        private void OnTileHover(BaseHexagon baseHex)
        {
            if (_startNode == null)
            {
                _startNode = baseHex;
                return;
            }
            _goalNode = baseHex;

            CleanTiles();// limpiar antes de dibujar

            var path = Pathfinder.FindPath(_startNode, _goalNode);

        }*/
        /*[Button("Revert")]
        private void RevertHexagaon()
        {
            foreach (var t in Tiles.Values) t.RevertTile();

            _startNode = null;
            _goalNode = null;
        }*/
        public void CleanTiles()
        {
            foreach (var t in Tiles.Values) t.RevertTile();
        }
        public BaseHexagon GetTileAtPosition(Vector2 pos)
        {
            foreach (var tile in Tiles.Values)
            {
                if(tile.Coords.Q == pos.x && tile.Coords.R == pos.y)
                    return tile;
            }
            return null;
        }
    }
}
