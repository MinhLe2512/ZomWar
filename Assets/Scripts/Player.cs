using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour, IDamagable
{
    public Transform GunPlaceholder;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Animator _characterAnimator;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] Grenade _grenadePrefab;
    [SerializeField] float _speed = 5f;
    [SerializeField] int _maxHealth = 100;
    [SerializeField] IAttackable _gun;
    private float _currHealth;
    private GameObject _weaponGO;
    private void Start()
    {
        _currHealth = _maxHealth;
    }
    public void SetWeapon(GameObject weaponGO)
    {
        _weaponGO = weaponGO;
        _gun = weaponGO.GetComponent<IAttackable>();
    }
    public void OnSwitchWeapon()
    {
        if (_weaponGO != null)
            Destroy(_weaponGO);
    }
    private void OnEnable()
    {
        FloatingJoystick.OnJoystickDrag += Move;
    }
    void Move(Vector2 direction)
    {
        var moveDirection = new Vector3(direction.x, 0, direction.y);
        // Only rotate if there is movement input
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            // Rotate to face the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = targetRotation;
        }
        _rigidbody.MovePosition(transform.position + moveDirection * _speed * Time.deltaTime);
        //transform.position += moveDirection * _speed * Time.deltaTime;
        _characterAnimator.SetFloat("Speed", moveDirection.magnitude * 6);
    }
    private void OnDisable()
    {
        FloatingJoystick.OnJoystickDrag -= Move;
    }
    public void RotateShootAngle(float shootDirectionX)
    {
        _characterAnimator.SetFloat("DirectionX", shootDirectionX);
    }
    [ContextMenu("Throw Grenade")]
    public void ThrowGrenade()
    {
        if (_grenadePrefab == null) return;
        var grenade = Instantiate(_grenadePrefab, GunPlaceholder.position, Quaternion.identity);
        grenade.OnThrow(new Vector3(transform.forward.x * 3, 2.5f, transform.forward.z * 3));
    }
    public void TakeDamage(Transform damageDealer, int damage)
    {
        if (_currHealth == 0) return;
        _currHealth -= damage;
        var bloodSplat = FXPooling.Instance.FxMap[FXPooling.BLOOD_SPLAT].Get();
        bloodSplat.transform.position = transform.position;
        bloodSplat.transform.forward = (transform.position - damageDealer.transform.position).normalized;
        if (_currHealth <= 0)
        {
            _currHealth = 0; // Disable joystick input on death
            _characterAnimator.SetTrigger("Die");
            _gun.OnPlayerDie();
        }
        _healthBar.SetHealth(_currHealth * 1.0f / _maxHealth);
    }
    public bool IsAlive()
    {
        return _currHealth > 0;
    }
}
