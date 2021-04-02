using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _player;

    public static Action clickAndHold;
    public static Action clickRelease;
    public static Action clickAndRelease;
    public static Action<Vector3> gotAWayPoint;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        BallClicker();
        WayPointer();
    }

    private void BallClicker()
    {
        Ray _rayOrigin = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hitInfo;

        if (Physics.Raycast(_rayOrigin, out _hitInfo))
        {
            if (_hitInfo.collider.CompareTag("Player") && Input.GetMouseButton(0))
            {
                clickAndHold?.Invoke();
            }
            else if (_hitInfo.collider.CompareTag("Player") && Input.GetMouseButtonDown(0))
            {
                clickAndRelease?.Invoke();
            }
            else if (_hitInfo.collider.CompareTag("Player") & Input.GetMouseButtonUp(0))
            {
                clickRelease?.Invoke();
            }
        }
    }

    private void WayPointer()
    {
        Ray _rayOrigin = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hitInfo;

        if (Physics.Raycast(_rayOrigin, out _hitInfo))
        {
            if (_hitInfo.collider.CompareTag("Ground") && Input.GetMouseButtonDown(0))
            {
                if (_player != null)
                {
                    float playersYCoordinate = _player.transform.position.y;
                    Vector3 wayPoint = new Vector3(_hitInfo.point.x, playersYCoordinate, _hitInfo.point.z);
                    gotAWayPoint?.Invoke(wayPoint);
                }
            }
        }
    }

    public void ReloadScene()
    {
        Player.gameOverShown = false;
        SceneManager.LoadScene(0);
    }
}
