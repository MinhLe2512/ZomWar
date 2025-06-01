using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObjects/ZombieData", order = 1)]
public class ZombieData : ScriptableObject
{
    public float WalkingSpeed = 2f;
    public int AttackDamage = 1;
}
