using System.Collections;
using System.Collections.Generic;
using MyUtils;
using UnityEngine;
using UnityEngine.Pool;

public class FXPooling : Singleton<FXPooling>
{
    public List<ParticleSystem> ParticlePrefabLs = new();
    public Dictionary<string, ObjectPool<ParticleSystem>> FxMap = new();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        foreach (var fxPrefab in ParticlePrefabLs)
        {
            FxMap.Add(fxPrefab.name, CreateObjectPool(fxPrefab));
        }
    }
    private ObjectPool<ParticleSystem> CreateObjectPool(ParticleSystem psPrefab)
    {
        return new ObjectPool<ParticleSystem>(
            () => CreateParticleSystem(psPrefab),
            OnGetParticle,
            OnReleaseParticle,
            OnDestroyParticle);
    }
    private ParticleSystem CreateParticleSystem(ParticleSystem psPrefab)
    {
        var ps = Instantiate(psPrefab);
        ps.gameObject.AddComponent<ReturnToPool>().pool = FxMap[psPrefab.name];
        ps.transform.SetParent(transform);
        return ps;
    }
    private void OnGetParticle(ParticleSystem ps)
    {
        ps.gameObject.SetActive(true);
    }
    private void OnReleaseParticle(ParticleSystem ps)
    {
        ps.gameObject.SetActive(false);
    }
    private void OnDestroyParticle(ParticleSystem ps)
    {
        Destroy(ps.gameObject);
    }

    public const string METAL_EXPLOSION = "MetalExplosion";
    public const string BLOOD_SPLAT = "BloodSplatDirectional";
}
