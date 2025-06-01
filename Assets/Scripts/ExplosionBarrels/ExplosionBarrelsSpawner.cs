using UnityEngine;

public class ExplosionBarrelsSpawner : MonoBehaviour
{
    [SerializeField] ExplosionBarrels _explosionBarrels;
    private ExplosionBarrels _currentBarrels;
    public void SpawnExplosionBarrels()
    {
        if (_currentBarrels != null) 
        {
            _currentBarrels.ResetHealth(); 
            return;
        }
        _currentBarrels = Instantiate(_explosionBarrels, transform.position, Quaternion.identity);
    }
}
