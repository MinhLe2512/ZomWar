using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SelectWeapon : MonoBehaviour
{
    [Inject]
    private readonly GunFactory _gunFactory;
    [SerializeField] Button _leftButton, _rightButton;
    [SerializeField] TextMeshProUGUI _gunTypeText;
    private int _currGunTypeValue = 0;
    private int _maxIndex = 1;
    public void Init()
    {
        //_leftButton.onClick.AddListener(OnLeftButtonClick);
        //_rightButton.onClick.AddListener(OnRightButtonClick);
        //_maxIndex = System.Enum.GetValues(typeof(GunType)).Length;
        var gunType = (GunType)_currGunTypeValue;
        _gunTypeText.text = gunType.ToString();
        _gunFactory.SpawnGun(gunType);
    }
    public void OnLeftButtonClick()
    {
        _currGunTypeValue--;
        if (_currGunTypeValue < 0)
        {
            _currGunTypeValue = _maxIndex;
        }
        var gunType = (GunType)_currGunTypeValue;
        _gunTypeText.text = gunType.ToString();
        _gunFactory.SpawnGun(gunType);
    }
    public void OnRightButtonClick()
    {
        _currGunTypeValue++;
        if (_currGunTypeValue > _maxIndex)
        {
            _currGunTypeValue = 0;
        }
        var gunType = (GunType)_currGunTypeValue;
        _gunTypeText.text = gunType.ToString();
        _gunFactory.SpawnGun(gunType);
    }
    private void OnDestroy()
    {
        //_leftButton.onClick.RemoveAllListeners();
        //_rightButton.onClick.RemoveAllListeners();
    }
}
