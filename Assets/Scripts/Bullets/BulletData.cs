using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData", order = 1)]
public class BulletData : ScriptableObject
{
    [SerializeField] int _damage;
    [SerializeField] float _speed;
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
}
