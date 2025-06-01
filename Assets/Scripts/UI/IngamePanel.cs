using UnityEngine;

public class IngamePanel : BasePanel
{
    [SerializeField] SelectWeapon _selectWeapon;
    [SerializeField] WaveDisplayPanel _waveDisplayPanel;
    public WaveDisplayPanel WaveDisplayPanel => _waveDisplayPanel;
    public const string PanelId = "IngamePanel";

    public override void Init()
    {
        base.Init();
        _selectWeapon = GetComponentInChildren<SelectWeapon>();
        _selectWeapon.Init();
    }
    public override string GetId()
    {
        return PanelId;
    }

    public override void Hide(float delay = 0)
    {
        gameObject.SetActive(false);
    }

    public override void Show(float delay = 0)
    {
        gameObject.SetActive(true);
        //StartCoroutine(FadeAnimation(true, delay));
    }
}
