using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 1;
    public float Speed;
    public Transform Target;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3.MoveTowards(transform.position, Target.position, Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}