using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionBarrels : MonoBehaviour, IDamagable
{
    [SerializeField] int _health = 50;
    [SerializeField] int _damage = 50;
    private int _currHealth = 0;
    [SerializeField] SpriteRenderer _warningRenderer;
    [SerializeField] GameObject[] _barrels;
    [SerializeField] Collider _triggerCollider, _hitCollider;
    [SerializeField] GameObject _explosionGO;
    private HashSet<Collider> _damagables = new();
    private bool _isExploded;

    private void Start()
    {
        _currHealth = _health;
        _isExploded = false;
        _warningRenderer.gameObject.SetActive(false);
    }
    public void ResetHealth()
    {
        _currHealth = _health;
    }
    public void TakeDamage(Transform damageDealer, int damage)
    {
        if (_currHealth == 0) return;
        _currHealth -= damage;

        if (_currHealth <= 0)
        {
            _currHealth = 0;
            StartCoroutine(FlashAnim(1.5f, 2f));
            //_isExploded = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isExploded) return;
        if (other.attachedRigidbody == null) return;
        if (_damagables.Contains(other)) return;
        _damagables.Add(other);
        if (other.attachedRigidbody.TryGetComponent<IDamagable>(out IDamagable idamagable))
        {
            idamagable.TakeDamage(transform, _damage);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (_damagables.Contains(other))
        {
            _damagables.Remove(other);
        }
    }
    IEnumerator FlashAnim(float duration, float power = 2f)
    {
        float time = 0f;
        _warningRenderer.gameObject.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(time / duration);
            var evilTime = Mathf.Lerp(0, 2 * Mathf.PI, normalizedTime);
            float acceleratedTime = Mathf.Pow(evilTime, power);
            float sineValue = (Mathf.Sin(acceleratedTime) + 1) / 2f;
            _warningRenderer.color = new Color(1f, 0f, 0f, sineValue);
            yield return null;
        }
        foreach (var barrel in _barrels)
        {
            barrel.SetActive(false);
        }
        _explosionGO.SetActive(true);
        _hitCollider.enabled = false;
        _warningRenderer.gameObject.SetActive(false);
        _isExploded = true;
        SoundManager.Instance.PlaySFX(SoundManager.EXPLOSION);
        yield return new WaitForSeconds(0.25f);
        _triggerCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
