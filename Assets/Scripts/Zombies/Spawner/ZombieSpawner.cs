using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] Zombie _zombiePrefab;
    [SerializeField] List<WaveData> _waveDatas;
    [Inject]
    private readonly Player _player;
    private readonly List<Zombie> _zombies = new();
    private bool _isSpawning = false;   
    public void Init()
    {
    }
    public void StartSpawning(int waveIndex)
    {
        StartCoroutine(SpawnWave(_waveDatas[waveIndex]));
    }
    public void StopSpawning()
    {
        StopAllCoroutines();
    }
    public bool IsEndWave()
    {
        bool hasZombies = false;
        foreach (var zombie in _zombies)
        {
            if (zombie != null)
            {
                hasZombies = true;
                break;
            }
        }
        if (!hasZombies)
        {
            _zombies.Clear();
        }
        return !_isSpawning && !hasZombies;
    }
    public IEnumerator SpawnWave(WaveData waveData)
    {
        _isSpawning = true;
        for (int i = 0; i < waveData.StartingZombies; i++)
        {
            var zombie = SpawnZombie();
            _zombies.Add(zombie);
        }
        var zombieCount = 0;
        while (zombieCount < waveData.MaxZombies)
        {
            yield return new WaitForSeconds(waveData.SpawnInterval);
            for (int i = 0; i < waveData.SpawnCount; i++)
            {
                if (zombieCount < waveData.MaxZombies)
                {
                    var zombie = SpawnZombie();
                    _zombies.Add(zombie);
                    zombieCount++;
                }
            }
        }
        _isSpawning = false;

    }
    Zombie SpawnZombie()
    {
        var randomInCircle = Random.insideUnitCircle;
        var spawnPosition = new Vector3(randomInCircle.x, 0, randomInCircle.y) * 2.5f + transform.position;
        var zombie = Instantiate(_zombiePrefab, spawnPosition, Quaternion.identity);
        zombie.SetTarget(_player);
        return zombie;
    }
}
[System.Serializable]
public class WaveData
{
    public int StartingZombies = 5;
    public float SpawnInterval = 2f;
    public int MaxZombies = 20;
    public int SpawnCount = 1;
}