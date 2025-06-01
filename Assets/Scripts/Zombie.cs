using Stateless;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamagable
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Collider _collider;
    [SerializeField] Player _targetPlayer;
    [SerializeField] Animator _zombieAnimator;
    [SerializeField] int _health = 100;
    [SerializeField] Material _dissolveMaterial;
    [SerializeField] SkinnedMeshRenderer _zombieMeshRenderer;
    [SerializeField] ZombieData _zombieData;
    private StateMachine<State, Trigger> _stateMachine;
    private readonly float _dissolveDuration = 2f;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _zombieAnimator = GetComponent<Animator>();
        _zombieMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _navMeshAgent.speed = _zombieData.WalkingSpeed;
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        _stateMachine = new StateMachine<State, Trigger>(State.Walking);
        _stateMachine.Configure(State.Walking)
            .OnActivate(() =>
            {
            })
            .OnEntry(() =>
            {
            })
            .Permit(Trigger.Attack, State.Attacking)
            .Permit(Trigger.Die, State.Dead);
        _stateMachine.Configure(State.Attacking)
            .OnEntry(() =>
            {
                _zombieAnimator.SetTrigger("Attack");   
            })
            .Permit(Trigger.Walk, State.Walking)
            .Permit(Trigger.Die, State.Dead)
            .Ignore(Trigger.Attack);
        _stateMachine.Configure(State.Dead)
            .OnEntry(() =>
            {
                _navMeshAgent.isStopped = true;
                _zombieAnimator.SetTrigger("Die");
                _collider.enabled = false;
            })
            .Ignore(Trigger.Walk)
            .Ignore(Trigger.Attack);
        _stateMachine.Activate();
    }

    public void TakeDamage(Transform damageDealer, int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _stateMachine.Fire(Trigger.Die);
            _zombieMeshRenderer.material = _dissolveMaterial;
            StartCoroutine(DissolveAnimation());
            //Destroy(gameObject);
        }
        SoundManager.Instance.PlayZombieHurtSound();
        FXPooling.Instance.FxMap[FXPooling.METAL_EXPLOSION].Get().transform.position = transform.position;
    }
    private IEnumerator DissolveAnimation()
    {
        float duration = _dissolveDuration;
        float elapsedTime = 0f;
        Material material = _zombieMeshRenderer.material;

        SoundManager.Instance.PlaySFX(SoundManager.ZOMBIE_DEATH);
        yield return new WaitForSeconds(0.5f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveAmount = Mathf.Clamp01(elapsedTime / duration);
            material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
        material.SetFloat("_DissolveAmount", 1);
        Destroy(gameObject);
    }
    public void SetTarget(Player targetPlayer)
    {
        _targetPlayer = targetPlayer;
    }

    private void Update()
    {
        if (!_targetPlayer.IsAlive())
        {
            _navMeshAgent.isStopped = true;
            return;
        }
        if (!_stateMachine.IsInState(State.Walking))
        {
           transform.LookAt(_targetPlayer.transform.position);
        }
        else
        {
            _navMeshAgent.destination = _targetPlayer.transform.position;
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                OnStartAttack();
            }
        }
    }
    public void OnStartAttack()
    {
        _stateMachine.Fire(Trigger.Attack);
        //_targetPlayer.TakeDamage(_zombieData.AttackDamage);
    }
    public void OnDoneAttack()
    {
        _stateMachine.Fire(Trigger.Walk);
    }
    public void HitTarget()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ZOMBIE_BITE);
        _targetPlayer.TakeDamage(transform, _zombieData.AttackDamage);
    }
    public enum State
    {
        Walking,
        Attacking,
        Dead
    }
    public enum Trigger
    {
        Walk,
        Attack,
        Die
    }
}
