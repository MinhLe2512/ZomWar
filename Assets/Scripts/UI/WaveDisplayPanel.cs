using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveDisplayPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _displayTextMesh;
    [SerializeField] RectTransform _rectTransform;

    public void Show()
    {
        gameObject.SetActive(true);
        var seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).From(Vector3.zero));
        seq.AppendInterval(1f);
        seq.Append(_rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

    }
    public void SetText(string text)
    {
        _displayTextMesh.text = text;
    }
}
