using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using RP.Grid;
using RP.Core;
using DG.Tweening;
using RP.core;

namespace RP.Entitys
{
    public class MobBase : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private BaseHexagon _currentPos;

        public void Start()
        {
            GameManager.Instance.TempoController.Tick += StartIteration;
        }
        public void Set(BaseHexagon initialPos)
        {
            _currentPos = initialPos;
            _currentPos.MakeOccupied();
            SetPos(true);
        }
        [Button("StartIteration")]
        public void StartIteration()
        {
            List<BaseHexagon> path = GameManager.Instance.GetPlayerRoute(_currentPos);
            //print(path.Count);
            path.Reverse();
            if (path.Count <= 1) return;

            StartCoroutine(startIterationCoroutine(path));
        }
        private IEnumerator startIterationCoroutine(List<BaseHexagon> path)
        {
            _currentPos.MakeEmpty();
            yield return new WaitForSeconds(0.1f);

            //GameManager.Instance.GridGenerator.CleanTiles();           
            _currentPos = path[0];
            _currentPos.MakeOccupied();
            SetPos();
            
        }
        private void SetPos(bool tween = false)
        {
            var newPos = new Vector3(_currentPos.Coords.Pos.x, 0, _currentPos.Coords.Pos.y);
            if (tween)
                transform.position = newPos;
            else
                transform.DOMove(newPos, 1f);
        }
    }
    

}
   