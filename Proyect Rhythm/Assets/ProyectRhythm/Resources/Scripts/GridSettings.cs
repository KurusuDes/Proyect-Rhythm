using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Tiles;

namespace RP.Grid
{
    [CreateAssetMenu(fileName ="Grid Settings", menuName ="RP/Grid"),InlineEditor]
    public class GridSettings : SerializedScriptableObject
    {
        [FoldoutGroup("References"),SerializeField] private BaseHexagon _baseHexagon;
        [FoldoutGroup("Settings"), SerializeField, Range(0, 6)] private int _obstacleWeight = 3;
        [FoldoutGroup("Settings"), SerializeField, Range(1, 50)] private int _gridWidth = 16;
        [FoldoutGroup("Settings"), SerializeField, Range(1, 50)] private int _gridDepth = 9;

        public int GridWidth => _gridWidth;
        public int GridDepth => _gridDepth;
        public Dictionary<Vector2, BaseHexagon> GenerateHexGrid()
        {
            var tiles = new Dictionary<Vector2, BaseHexagon>();
            var grid = new GameObject
            {
                name = "Grid"
            };
            for (var r = 0; r < _gridDepth; r++)
            {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < _gridWidth - rOffset; q++)
                {
                    BaseHexagon tile = Instantiate(_baseHexagon, grid.transform);
                    tile.Init(DecideIfObstacle(), new HexaCoords(q, r));
                    tiles.Add(tile.Coords.Pos, tile);
                }
            }

            return tiles;
        }
        protected bool DecideIfObstacle() => Random.Range(1, 20) > _obstacleWeight;
    }
}
