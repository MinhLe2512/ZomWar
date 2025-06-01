using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class AKMGun : MonoBehaviour, IAttackable
{
    [SerializeField] AudioSource _shootAudioSource;
    [SerializeField] ConeRaycaster _coneRaycaster;
    [SerializeField] GunData _gunData;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _shootTransform;
    [SerializeField] Player _player;
    [SerializeField] GameObject _shootParticle;
    [SerializeField] Material _dissolveMaterial;
    [SerializeField] MeshRenderer _gunRenderer;
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
        if (_coneRaycaster.DetectedEnemies.Count == 0){
            _shootParticle.SetActive(false);

            _shootAudioSource.Stop();
            return;
        }
        var idamagable = _coneRaycaster.GetDetectedEnemies()[0].GetComponent<IDamagable>();
        if (idamagable == null) return;
        var bullet = _bulletPool.Get();
        var direction = (_coneRaycaster.GetDetectedEnemies()[0].position - _shootTransform.position).normalized;
        direction.y = 0f;
        var angle = Vector3.Angle(_player.transform.forward, -direction) - 90;
        var value = Mathf.Clamp(angle, -45f, 45f);
        _player.RotateShootAngle(value / 45f);
        bullet.Shoot(direction, _shootTransform.position);
        _shootParticle.SetActive(true);
        _timer = _gunData.Cooldown;
        _shootAudioSource.Play();
    }
    public void OnPlayerDie()
    {
        _gunRenderer.material = _dissolveMaterial;
        StartCoroutine(DissolveAnimation());
        _coneRaycaster.gameObject.SetActive(false);
    }

    private IEnumerator DissolveAnimation()
    {
        float duration = _dissolveDuration;
        float elapsedTime = 0f;
        Material material = _gunRenderer.material;
        yield return new WaitForSeconds(0.5f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveAmount = Mathf.Clamp01(elapsedTime / duration);
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
        material.SetFloat("_DissolveAmount", 1);
        Destroy(gameObject);
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
}
