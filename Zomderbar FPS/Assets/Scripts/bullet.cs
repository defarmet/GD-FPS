using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public int damage;
    public int speed;
    public int destroyTime;
    [SerializeField] private bool destroyOnImpact = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().takeDamage(damage);
            damage = 0; //After the first tick of damage the particle effect will remain but cause no more damage.
            gameManager.instance.d_angle = transform.position - other.transform.position;
        }

        if (destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }
}
