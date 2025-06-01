using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnZombieAttack : StateMachineBehaviour
{
    private Zombie _zombie;
    private bool _hasAttacked = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_zombie == null)
        {
            _zombie = animator.GetComponent<Zombie>();
        }
        _hasAttacked = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_hasAttacked && stateInfo.normalizedTime >= 0.25f)
        {
            _zombie.HitTarget();
            _hasAttacked = true;
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _zombie.OnDoneAttack();
    }
}
