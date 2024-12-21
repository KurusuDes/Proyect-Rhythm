using DamageNumbersPro;
using RP.core;
using RP.Entitys;
using RP.Grid;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

namespace RP.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [FoldoutGroup("References"), SerializeField] private PlayerController _playerPrefab;
        [FoldoutGroup("References"), SerializeField] private MobBase _mobBasePrefab;
        [FoldoutGroup("References"), SerializeField] private GridGenerator _gridGenerator;
        [FoldoutGroup("References"), SerializeField] private TempoController _tempoControllers;

        [FoldoutGroup("Settings"),MinMaxSlider(0,100), SerializeField] private int _perfectPercentage;
        [FoldoutGroup("Settings"), MinMaxSlider(0, 100), SerializeField] private int _AveragePercentage;
        [FoldoutGroup("Settings"), MinMaxSlider(0, 100), SerializeField] private int _LoosePercentage;

        [FoldoutGroup("TEST"), SerializeField] private bool SpawnRandomEnemy;
        [FoldoutGroup("TEST"), SerializeField] private DamageNumber _TestTextScore;

        //[SerializeField] private BaseHexagon _playerPos = null;

        private PlayerController _currentPlayer;

        #region TEST
        [Button("Spawn score")]
        public void SpawnScore()
        {
            _tempoControllers.GetCurrentTempo();
            DamageNumber scoreTemp = _TestTextScore.Spawn(_currentPlayer.transform.position);
            scoreTemp.SetFollowedTarget(_currentPlayer.transform);
        }
        #endregion

        #region Getters
        public GridGenerator GridGenerator => _gridGenerator;
        public TempoController TempoController => _tempoControllers;

        #endregion

        void Awake() 
        {
            Instance = this;        
        }      
        void Start()
        {         
            SpawnPlayer();
            if(SpawnRandomEnemy)
            {
                List<BaseHexagon> baseHexagons = GridGenerator.Tiles.Values.ToList();
                SpawnMob(new Vector2( baseHexagons[baseHexagons.Count-1].Coords.Q, baseHexagons[baseHexagons.Count-1].Coords.R));
            }
        }
        public List<BaseHexagon> GetPlayerRoute(BaseHexagon startPoint)
        {
            if (_currentPlayer == null || _currentPlayer.GetTile() == null)
            {
                Debug.LogError("player pos is null");
                return null;
            }
            return Pathfinder.FindPath(startPoint, _currentPlayer.GetTile());

        }
        [Button("Spawn random player")]
        public void SpawnPlayer()
        {
            
            int length = (int)GridGenerator.GetWidthHeight().x;
            SpawnPlayer(new Vector2(UnityEngine.Random.Range(0, length), 0));
        }
        [Button("Spawn Objective")]
        public void SpawnPlayer(Vector2 pos)
        {
            var tile = GridGenerator.GetTileAtPosition(pos);
            if (tile == null)
            {
                Debug.LogError("Tile donsent exists");
                return;
            }
            var player = Instantiate(_playerPrefab);
            _currentPlayer = player;
            player.Set(tile);
           

        }
        [Button("Spawn Mob")]
        public void SpawnMob(Vector2 pos)
        {
            var tile = GridGenerator.GetTileAtPosition(pos);
            if (tile == null)
            {
                Debug.LogError("Tile donsent exists");
                return;
            }
            var mob = Instantiate(_mobBasePrefab);
            mob.Set(tile);
        }
    }
}
