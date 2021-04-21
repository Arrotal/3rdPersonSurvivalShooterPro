using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private EnemyAI _ai;
    private void Start()
    {
        _ai = GetComponentInParent<EnemyAI>();
        if (_ai == null)
        {
            Debug.Log("Missing EnemyAi");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _ai.AttackState(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _ai.AttackState(false);
        }

    }
}
