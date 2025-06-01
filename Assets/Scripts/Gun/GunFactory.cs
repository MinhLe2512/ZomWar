using UnityEngine;
using VContainer;

public class GunFactory : MonoBehaviour
{
    [SerializeField] AKMGun _AKMGun;
    [SerializeField] Shotgun _shotGun;
    [Inject]
    private readonly Player _player;
    public IAttackable SpawnGun(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.AKM:
                {
                    return SpawnAKM();
                }
            case GunType.Shotgun:
                {
                    return SpawnShotgun();
                }
            default:
                {
                    return SpawnAKM();
                }
        }
    }

    private IAttackable SpawnShotgun()
    {
        var shotGun = Instantiate(_shotGun);
        shotGun.SetPlayer(_player);
        shotGun.transform.SetParent(_player.GunPlaceholder);
        shotGun.transform.localPosition = Vector3.zero;
        shotGun.transform.localRotation = Quaternion.identity;
        _player.OnSwitchWeapon();
        _player.SetWeapon(shotGun.gameObject);
        return shotGun;
    }

    private IAttackable SpawnAKM()
    {
        var akmGun = Instantiate(_AKMGun);
        akmGun.SetPlayer(_player);
        akmGun.transform.SetParent(_player.GunPlaceholder);
        akmGun.transform.localPosition = Vector3.zero;
        akmGun.transform.localRotation = Quaternion.identity;
        _player.OnSwitchWeapon();
        _player.SetWeapon(akmGun.gameObject);
        return akmGun;
    }
}
[System.Serializable]
public enum GunType
{
    AKM = 0,
    Shotgun = 1,
    Flamethrower = 2
}
