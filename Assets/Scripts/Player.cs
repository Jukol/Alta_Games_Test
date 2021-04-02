using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float _scaleRate;
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _door;
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] private float _crititicalScale = 2f;
    [SerializeField] private GameObject _gameOverPanel;
    private bool _bulletCreated;
    private Tween _jumps;
    private bool _canJump = true;
    private float _updatedScale;
    private float _initialScale = 0;
    private bool _initialScaleReceived = false;
    public static bool gameOverShown;


    public static Action createBullet;


    private void Start()
    {
        transform.LookAt(_door.transform);
        _updatedScale = transform.localScale.x;
        _gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameCamera.clickAndHold += CreateBullet;
        GameCamera.clickAndHold += GrowBulletAndShrink;
        GameCamera.clickRelease += ReleaseBullet;
        GameCamera.clickRelease += ResetScale;
        GameCamera.gotAWayPoint += GoTo;
    }

    private void OnDisable()
    {
        GameCamera.clickAndHold -= CreateBullet;
        GameCamera.clickAndHold -= GrowBulletAndShrink;
        GameCamera.clickRelease -= ReleaseBullet;
        GameCamera.clickRelease -= ResetScale;
        GameCamera.gotAWayPoint -= GoTo;
    }

    private void CreateBullet()
    {
        if (!_bulletCreated)
        {
            Instantiate(_bullet, this.transform, false);
            _bulletCreated = true;
            createBullet?.Invoke();
        }
    }

    private void GrowBulletAndShrink()
    {
     
        if (_initialScaleReceived == false)
        {
            _initialScale = transform.localScale.x;
            _initialScaleReceived = true;
        }
        
        
        if (!gameOverShown)
        {
            transform.localScale *= _scaleRate;
        }
        
        if (transform.localScale.x < _crititicalScale)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (!gameOverShown)
        {
            _gameOverPanel.SetActive(true);
            gameOverShown = true;
        }
    }

    private void ReleaseBullet()
    {
        _bulletCreated = false;
    }

    private void GoTo(Vector3 wayPoint)
    {
        transform.LookAt(_door.transform);
        if (_canJump)
        {
            _jumps = _rigidBody.DOJump(wayPoint, 3, 1, 1);
            _canJump = false;
            _jumps.OnComplete(() => _canJump = true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hurdle"))
        {
            GameOver();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _jumps.Kill();
    }

    private void ResetScale()
    {
        _initialScaleReceived = false;
    }

}
