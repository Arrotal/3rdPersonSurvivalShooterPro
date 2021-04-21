using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamagable
{
    public enum EnemyState
    { 
        Idle,
        Chase,
        Attack
    }

    private CharacterController _enemyControl;
    private Player _player;
    private float _gravity = -10f,_moveSpeed=2f, _attackDelay =1.5f, _lastAttack=0f;
    private Vector3 _eVelocity,_direction;
    [SerializeField]private EnemyState _currentState;

    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public void Damage(int damage)
    {
        Health -= damage;
        Debug.Log(transform.name + " Health: " + Health);
        if (Health < 1)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        _enemyControl = GetComponent<CharacterController>();
        _player = FindObjectOfType<Player>();
        MaxHealth = 10;
        Health = MaxHealth;
        _currentState = EnemyState.Chase;
    }

    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Chase:
                Movement();
                break;
            case EnemyState.Idle:
                break;

        }
        if (_lastAttack > 0)
        {
            _lastAttack -= Time.deltaTime;
        }

    }

    private void Attack()
    {
        if (_lastAttack <= 0f)
        {
            _lastAttack = _attackDelay;
            IDamagable hit = _player.GetComponent<IDamagable>();
            if (hit == null)
            {
                Debug.Log("Cannot find player");
            }
            if (hit != null && _currentState == EnemyState.Attack)
            {
                hit.Damage(2);
            }
        }
    }

    private void Movement()
    {
        if (_enemyControl.isGrounded)
        {
            _direction = _player.transform.position - transform.position;
            _direction.Normalize();
            _direction.y = 0;
            _eVelocity = _direction * _moveSpeed;
        }
        _eVelocity.y += _gravity;

        if (_currentState == EnemyState.Chase)
        {
            _enemyControl.Move(_eVelocity * Time.deltaTime);

            transform.localRotation = Quaternion.LookRotation(_direction);
        }
    }

    public void AttackState(bool isIt)
    {
        if (isIt)
        {
            _currentState = EnemyState.Attack;
        }
        else
        {
            _currentState = EnemyState.Chase;
        }
    }

   



}
