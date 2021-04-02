using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    private int _layerMask;
    [SerializeField] private float _initialRadius;
    [SerializeField] private float _radius;
    [SerializeField] private float _increaseRate;

    private void OnEnable()
    {
        
        GameCamera.clickAndHold += IncreaseDestructionRadius;
        Player.createBullet += ResetRadius;
    }
    private void OnDisable()
    {
        
        GameCamera.clickAndHold -= IncreaseDestructionRadius;
        Player.createBullet -= ResetRadius;
    }

    private void Start()
    {
        _layerMask = 1 << 10;
        _radius = _initialRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            KillNeighbors(transform.position, _radius);
        }
    }

    private void KillNeighbors(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, _layerMask);
        foreach (var hitCollider in hitColliders)
        {
            Destroy(hitCollider.gameObject);
        }
    }

    private void IncreaseDestructionRadius()
    {
        _radius *= _increaseRate;
    }

    private void ResetRadius()
    {
        _radius = _initialRadius;
    }
}
