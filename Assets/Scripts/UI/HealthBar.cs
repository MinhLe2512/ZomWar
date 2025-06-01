using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] Gradient _gradient;
    private float _currPercentage = 1f;
    private Coroutine _updateHealthCoroutine;
    private void Start()
    {
        SetHealth(1);
    }
    public void SetHealth(float percentage)
    {
        if (_updateHealthCoroutine != null)
        {
            StopCoroutine(_updateHealthCoroutine);
        }
        _updateHealthCoroutine = StartCoroutine(UpdateHealthColor(percentage));
    }
    private IEnumerator UpdateHealthColor(float percentage)
    {
        while (Mathf.Abs(_currPercentage - percentage) > 0.01f)
        {
            _currPercentage = Mathf.MoveTowards(_currPercentage, percentage, Time.deltaTime * 0.5f);
            _fillImage.color = _gradient.Evaluate(_currPercentage);
            _fillImage.fillAmount = _currPercentage;
            yield return null;
        }
        _currPercentage = percentage;
        _fillImage.fillAmount = percentage;
        _fillImage.color = _gradient.Evaluate(percentage);
    }
}
