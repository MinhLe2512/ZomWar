using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletData _bulletData;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _lifeTime = 10f; // Time after which the bullet will be destroyed if not hit anything
    private ObjectPool<Bullet> _bulletPool;
    private float _timer = 0f;
    public void Init()
    {
        _timer = 0f;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    public void SetObjectPool(ObjectPool<Bullet> bulletPool)
    {
        _bulletPool = bulletPool;
    }
    public void Shoot(Vector3 direction, Vector3 position)
    {
        transform.position = position;
        transform.forward = direction.normalized;
        _rigidbody.AddForce(direction.normalized * _bulletData.Speed, ForceMode.Impulse);
    }
    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }
    private void Update()
    {
        if (_timer < _lifeTime)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _bulletPool.Release(this);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (other.attachedRigidbody.TryGetComponent<IDamagable>(out IDamagable idamagable))
        {
            idamagable.TakeDamage(transform, _bulletData.Damage);
             _bulletPool.Release(this);
        }
    }
}
