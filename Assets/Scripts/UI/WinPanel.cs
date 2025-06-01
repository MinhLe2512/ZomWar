using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : BasePanel
{
    [SerializeField] CanvasGroup _canvasGroup;
    public const string PanelId = "WinPanel";
    public float FadeDuration = 1f;
    public override void Show(float delay = 0)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeAnimation(true, delay));
    }
    public override void Hide(float delay = 0)
    {
        StartCoroutine(FadeAnimation(false, delay));
    }
    public void OnRetryClick()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    private IEnumerator FadeAnimation(bool isShow, float delay)
    {
        _canvasGroup.alpha = isShow ? 0f : 1f;
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0f;
        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            var targetAlpha = isShow ? elapsedTime / FadeDuration : 1 - elapsedTime / FadeDuration;
            _canvasGroup.alpha = Mathf.Clamp01(targetAlpha);
            yield return null;
        }
        _canvasGroup.alpha = isShow ? 1f : 0f;
        gameObject.SetActive(isShow);// Ensure it's fully visible at the end
    }
    public override string GetId()
    {
        return PanelId;
    }
}
