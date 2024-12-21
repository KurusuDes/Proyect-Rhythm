using _Scripts.Tiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using System.Linq;
using System;
using TMPro;
//using System.Drawing;

namespace RP.Grid
{
    public class BaseHexagon : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private GameObject _visual;

        [FoldoutGroup("Settings"), SerializeField] private List<Color> OptionalStyles;
        [FoldoutGroup("Settings"), SerializeField] private Color Obstacle;//funcion para llamar obstaculos en un futuro
        #region Pathfinding
        [FoldoutGroup("Settings/Pathfinding"), SerializeField] private TextMeshPro _fCostText;
        [FoldoutGroup("Settings/Pathfinding"), SerializeField] private TextMeshPro _gCostText;
        [FoldoutGroup("Settings/Pathfinding"), SerializeField] private TextMeshPro _hCostText;
        #endregion
        [ShowInInspector]public HexaCoords Coords;
        [ShowInInspector] public List<BaseHexagon> Neighbors { get; protected set; }

        public float GetDistance(BaseHexagon other) => Coords.GetDistance(other.Coords);
        public bool Walkable { get; private set; }
        private bool _selected;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public virtual void Init(bool walkable, HexaCoords coords)
        {
            Walkable = walkable;

            if (_visual == walkable)
             PicRandomVisual(); 
            else
             SetColor(Obstacle);//make random color
            //_defaultColor = _renderer.color;

            OnHoverTile += OnOnHoverTile;

            Coords = coords;
            transform.position = Coords.Pos;
            transform.position = new Vector3(Coords.Pos.x, 0, Coords.Pos.y);
        }
        public void MakeObstacle()
        {
            Walkable = false;
            SetColor(Obstacle);
        }
        public void MakeOccupied()
        {
            Walkable = false;
        }
        public void MakeEmpty()
        {
            Walkable = true;
        }
        public static event Action<BaseHexagon> OnHoverTile;
        private void OnEnable() => OnHoverTile += OnOnHoverTile;
        private void OnDisable() => OnHoverTile -= OnOnHoverTile;
        private void OnOnHoverTile(BaseHexagon selected) => _selected = selected == this;
        public void PicRandomVisual()
        {
            SetColor( OptionalStyles[UnityEngine.Random.Range(0,OptionalStyles.Count)]);
        }
        public void CacheNeighbors()
        {
            Neighbors = GridGenerator.Instance.Tiles.Where(t => Coords.GetDistance(t.Value.Coords) == 1).Select(t => t.Value).ToList();
        }
        
        protected virtual void OnMouseDown()
        {
            if (!Walkable) return;
            OnHoverTile?.Invoke(this);

            print("click on me:"+ gameObject.name);
        }
        public void RevertTile()
        {
            PicRandomVisual();
            //_visual.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        public void SetColor(Color color)
        {
            _visual.GetComponent<MeshRenderer>().material.color = color;
        }
        #region Pathfinding
        public BaseHexagon Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public void SetConnection(BaseHexagon nodeBase)
        {
            Connection = nodeBase;
        }

        public void SetG(float g)
        {
            G = g;
            SetText();
        }

        public void SetH(float h)
        {
            H = h;
            SetText();
        }

        private void SetText()
        {
            if (_selected) return;
            _gCostText.text = G.ToString();
            _hCostText.text = H.ToString();
            _fCostText.text = F.ToString();
        }
        #endregion
    }
    public struct HexaCoords
    {
        [FoldoutGroup("Settings"), SerializeField] private  int _q;
        [FoldoutGroup("Settings"), SerializeField] private  int _r;

        public int Q => _q;
        public  int R => _r;

        public HexaCoords(int q, int r)
        {
            _q = q;
            _r = r;
            Pos = _q * new Vector2(Sqrt3, 0) + _r * new Vector2(Sqrt3 / 2, 1.5f);
        }

        public float GetDistance(HexaCoords other) => (this - other).AxialLength();
        private static readonly float Sqrt3 = Mathf.Sqrt(3);
        public Vector2 Pos { get; set; }

        private int AxialLength()
        {
            if (_q == 0 && _r == 0) return 0;
            if (_q > 0 && _r >= 0) return _q + _r;
            if (_q <= 0 && _r > 0) return -_q < _r ? _r : -_q;
            if (_q < 0) return -_q - _r;
            return -_r > _q ? -_r : _q;
        }
        public static HexaCoords operator -(HexaCoords a, HexaCoords b)
        {
            return new HexaCoords(a._q - b._q, a._r - b._r);
        }
    }
}
