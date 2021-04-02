using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _scaleRate;
    [SerializeField] GameObject _player;
    private Transform _target;
    private Tween _moveTween;
    private bool _canGrow = true;
    private bool _canFly = false;
    [SerializeField] private float _speed = 5f;

    private void Start()
    {
        transform.parent = null;
        transform.localScale = new Vector3(1, 1, 1);
        _target = GameObject.FindGameObjectWithTag("Door").GetComponent<Transform>();
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(_player.transform);
    }

    private void Update()
    {
        if (_canFly && !Player.gameOverShown)
        {   
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
    }
    private void OnEnable()
    {
        GameCamera.clickAndHold += ScaleUp;
        GameCamera.clickRelease += Fly;
    }
    private void OnDisable()
    {
        GameCamera.clickAndHold -= ScaleUp;
        GameCamera.clickRelease -= Fly;
    }

    private void ScaleUp()
    {
        if (_canGrow && !Player.gameOverShown)
        {
            transform.localScale *= _scaleRate;
        }
        
    }

    private void Fly()
    {
        transform.LookAt(_target.transform);
        _canFly = true;
    }

    private void OnDestroy()
    {
        _moveTween.Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hurdle"))
        {
            Destroy(gameObject);
        }
    }


}
