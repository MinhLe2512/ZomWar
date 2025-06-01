using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shotgun : MonoBehaviour, IAttackable
{
    [SerializeField] ConeRaycaster _coneRaycaster;
    [SerializeField] AudioSource _shootAudioSource;
    [SerializeField] GunData _gunData;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _shootTransform;
    [SerializeField] Player _player;
    [SerializeField] GameObject _shootParticle;
    [SerializeField] Material _dissolveMaterial;
    [SerializeField] MeshRenderer _gunRenderer;
    [SerializeField] float _spreadAngle = 30f;
    [SerializeField] int _bulletCount = 5;
    private readonly float _dissolveDuration = 1f;
    private ObjectPool<Bullet> _bulletPool;
    private float _timer = 0f;
    private void Start()
    {
        _bulletPool = new ObjectPool<Bullet>(
            createFunc: () => {
                var bullet = Instantiate(_bulletPrefab);
                bullet.SetObjectPool(_bulletPool);
                return bullet;
            },
            actionOnGet: bullet => {
                bullet.Init();
                bullet.gameObject.SetActive(true);
                },
            actionOnRelease: bullet => bullet.gameObject.SetActive(false),
            actionOnDestroy: bullet => Destroy(bullet.gameObject),
            maxSize: 1000
        );
        _coneRaycaster = _player.GetComponentInChildren<ConeRaycaster>();
        _coneRaycaster.coneAngle = _gunData.ConeAngle;
        _coneRaycaster.coneRange = _gunData.Range;
        _coneRaycaster.ResetCone();
    }
    public void SetPlayer(Player player)
    {
        _player = player;
    }
    public void Attack()
    {
        if (_coneRaycaster.DetectedEnemies.Count == 0)
        {
            _shootParticle.SetActive(false);
            return;
        }
        var idamagable = _coneRaycaster.GetDetectedEnemies()[0].GetComponent<IDamagable>();
        if (idamagable == null) return;
        float halfAngle = _spreadAngle / 2f;

        for (int i = 0; i < _bulletCount; i++)
        {
            // Calculate angle for this bullet
            float angleStep = _spreadAngle / (_bulletCount - 1);
            float angle = -halfAngle + angleStep * i;

            // Get direction with angle offset
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * _shootTransform.rotation;
            Vector3 direction = rotation * Vector3.forward;

            // Instantiate bullet
            var bullet = _bulletPool.Get();
            bullet.Shoot(direction, _shootTransform.position);
        }
        _shootAudioSource.Play();
        _timer = _gunData.Cooldown;
    }
    private void Update()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _coneRaycaster.PerformConeRaycast();
            Attack();
        }
    }
    public void OnPlayerDie()
    {
        Destroy(gameObject);    
    }

}
