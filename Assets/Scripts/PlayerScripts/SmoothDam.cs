using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDam : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public float speed = 10f;

    void LateUpdate()
    {
        Vector3 targetPosition = _target.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        transform.rotation = Quaternion.Euler(_target.transform.rotation.eulerAngles);
    }
}
