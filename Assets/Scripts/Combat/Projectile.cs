using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] float speed = 1;

    void Update()
    {
        if (target == null) return;
        transform.LookAt(GetAimLoc());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Vector3 GetAimLoc()
    {
        CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        if (targetCollider == null) return target.position;
        return target.position + Vector3.up * targetCollider.height / 2;
    }
}
