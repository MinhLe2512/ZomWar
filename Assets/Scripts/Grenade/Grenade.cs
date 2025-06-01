using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] SpriteRenderer _warningRenderer;
    [SerializeField] Collider _triggerCollider, _hitCollider;
    [SerializeField] GameObject _explosionGO;
    private HashSet<Collider> _damagables = new();
    [SerializeField] int _damage = 50;
    private bool _isExploded;
    public void OnThrow(Vector3 throwForce)
    {
        _rigidbody.AddForce(throwForce, ForceMode.Impulse);
        StartCoroutine(FlashAnim(1f));
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
    void Update()
    {
        _warningRenderer.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        _explosionGO.transform.rotation = Quaternion.identity;
    }
     IEnumerator FlashAnim(float duration, float power = 2f)
    {
        yield return new WaitForSeconds(2f);
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
        _explosionGO.SetActive(true);
        _warningRenderer.gameObject.SetActive(false);
        _isExploded = true;
        SoundManager.Instance.PlaySFX(SoundManager.EXPLOSION);
        yield return new WaitForSeconds(0.25f);
        _triggerCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
