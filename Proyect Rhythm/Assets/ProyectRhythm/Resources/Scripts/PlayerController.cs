using DG.Tweening;
using RP.Core;
using RP.Grid;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [FoldoutGroup("References"), SerializeField] private BaseHexagon _tile;
    [SerializeField] private float rayLength = 5f;
    private PlayerControls _playerControls;
    private void Awake()
    {
        #region Inputs
        _playerControls = new PlayerControls();
        #endregion
    }
    private void OnEnable()
    {
        #region Inputs
        _playerControls.Enable();
        #endregion
    }
    private void OnDestroy()
    {
        #region Inputs
        _playerControls.Disable();
        _playerControls.Player.Movement.performed -= MovePlayer;
        _playerControls.Player.Movement.canceled -= StopMovement;
        #endregion
    }
    void Start()
    {
        #region Inputs
        _playerControls.Player.Movement.performed += MovePlayer;
        _playerControls.Player.Movement.canceled += StopMovement;

        _playerControls.Player.Attack.performed += AttackSystem;
        #endregion
    }
    #region Core
    public void Set(BaseHexagon tile)
    {
        _tile = tile;
       // _tile.MakeOccupied();
        SetPos(false);
    }
    private void SetPos(bool tween = true)
    {
        var newPos = new Vector3(_tile.Coords.Pos.x, 0, _tile.Coords.Pos.y);
        if (!tween)
            transform.position = newPos;
        else
            transform.DOMove(newPos, 1f);
    }
    private void SetAttack(BaseHexagon baseHexagon)
    {
        GameManager.Instance.TempoController.GetCurrentTempo();
    }
    private BaseHexagon PerformRaycast(Vector2 direction, GameObject origin)
    {
        Vector3 rayDirection = new Vector3(direction.x, 0, direction.y);
        if (Physics.Raycast(origin.transform.position, rayDirection, out RaycastHit hit, rayLength))
        {
            BaseHexagon tile = hit.collider.gameObject.GetComponent<BaseHexagon>();
            if (tile != null && tile.Walkable)
                return hit.collider.gameObject.GetComponent<BaseHexagon>();
        }
        Debug.DrawRay(origin.transform.position, rayDirection * rayLength, Color.red, 1f);
        return null;
    }
    #endregion
    #region Inputs
    private void MovePlayer(InputAction.CallbackContext context)
    {
        _tile.MakeEmpty();
        Vector2 dir = context.ReadValue<Vector2>();
        //print(dir);
        var newTile = PerformRaycast(dir,_tile.gameObject);
        if (newTile == null) return;
        
        _tile = newTile;
       // _tile.MakeOccupied();
        SetPos();
    }
    private void StopMovement(InputAction.CallbackContext context)
    {
       
    }
    private void AttackSystem(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        var tile = PerformRaycast(dir, _tile.gameObject);
        SetAttack(tile);
    }

    #endregion
   
    #region Getters
    public BaseHexagon GetTile() { return _tile; }
    #endregion
}
