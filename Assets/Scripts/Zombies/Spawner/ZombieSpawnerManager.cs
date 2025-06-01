using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VContainer;

public class ZombieSpawnerManager : MonoBehaviour
{
    [Inject]
    private readonly UIManager _uiManager;
    [SerializeField] ZombieSpawner _zombieSpawnerPrefab;
    [SerializeField] ZombieSpawner[] _zombieSpawners;
    [SerializeField] ExplosionBarrelsSpawner[] _explosionBarrelsSpawners;
    [SerializeField] int _maxWaveCount = 3;
    private bool _isInWave = false;
    private int _currentWaveIndex = 0;
    private Coroutine _startSpawningCoroutine;
    private Coroutine _checkIfHasSpawnLeftCoroutine;
    private bool _hasClearLevel = false;
    public bool HasClearLevel => _hasClearLevel;
    private WaveDisplayPanel _waveDisplayPanel;

#if UNITY_EDITOR
    [ContextMenu("Create Zombie Spawner")]
    public void CreateZombieSpawner()
    {
        PrefabUtility.InstantiatePrefab(_zombieSpawnerPrefab, transform);
    }
    [ContextMenu("Get Zombie Spawners")]
    public void GetZombieSpawners()
    {
        _zombieSpawners = GetComponentsInChildren<ZombieSpawner>();
    }
#endif
    private void Start()
    {
        foreach (var spawner in _zombieSpawners)
        {
            spawner.Init();
        }
        var ingamePanel = _uiManager.GetPanel(IngamePanel.PanelId) as IngamePanel;
        _waveDisplayPanel = ingamePanel.WaveDisplayPanel;
        _waveDisplayPanel.Show();
        _waveDisplayPanel.SetText($"Wave {_currentWaveIndex + 1}");
        StartCoroutine(StartSpawning());
    }
    public IEnumerator StartSpawning()
    {
        foreach (var spawner in _zombieSpawners)
        {
            spawner.StartSpawning(_currentWaveIndex);
        }
        foreach (var explosionBarrelsSpawner in _explosionBarrelsSpawners)
        {
            explosionBarrelsSpawner.SpawnExplosionBarrels();
        }
        yield return new WaitForSeconds(1f);
        _isInWave = true;
        if (_checkIfHasSpawnLeftCoroutine != null)
        {
            StopCoroutine(_checkIfHasSpawnLeftCoroutine);
        }
        _checkIfHasSpawnLeftCoroutine = StartCoroutine(CheckIfHasSpawnLeft());
    }
    private IEnumerator CheckIfHasSpawnLeft()
    {
        while (true)
        {
            bool hasZombieLeft = false;
            foreach (var spawner in _zombieSpawners)
            {
                if (!spawner.IsEndWave())
                {
                    hasZombieLeft = true;
                    break;
                }
            }
            if (!hasZombieLeft)
            {
                _isInWave = false;
                Debug.Log($"End wave{_currentWaveIndex + 1}");

                _currentWaveIndex++;
                _waveDisplayPanel.Show();
                if (_currentWaveIndex == _maxWaveCount - 1)
                {
                    _waveDisplayPanel.SetText("Final Wave!");
                }
                else
                {
                    _waveDisplayPanel.SetText($"Wave {_currentWaveIndex + 1}");
                }
                yield return new WaitForSeconds(1.5f);
                if (_currentWaveIndex == _maxWaveCount)
                {
                    _hasClearLevel = true;
                    Debug.Log("All waves completed!");
                    yield break; // All waves completed, exit the coroutine
                }
                else
                {
                    Debug.Log($"Start wave{_currentWaveIndex + 1}");
                    if (_startSpawningCoroutine != null)
                    {
                        StopCoroutine(_startSpawningCoroutine);
                    }
                    _startSpawningCoroutine = StartCoroutine(StartSpawning());
                }

                yield break;
            }
            yield return null;
        }
    }
}
