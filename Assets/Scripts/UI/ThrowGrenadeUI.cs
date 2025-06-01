using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ThrowGrenadeUI : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _cooldownImage;
    [Inject]
    private readonly Player _player;
    public void OnClickButton()
    {
        _player.ThrowGrenade();
        StartCoroutine(CooldownAnimation());
    }
    private IEnumerator CooldownAnimation()
    {
        _cooldownImage.fillAmount = 1f;
        float cooldownDuration = 6f;
        float elapsedTime = 0f;
        _button.interactable = false;
        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            _cooldownImage.fillAmount = Mathf.Clamp01(elapsedTime / cooldownDuration);
            yield return null;
        }
        _cooldownImage.fillAmount = 0f;
        _button.interactable = true; // Reset to full after cooldown
    }
}
