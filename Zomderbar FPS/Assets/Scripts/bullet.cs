using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] bool slowEffect;
    [SerializeField] bool hasShake = false;
    public int damage;
    public int speed;
    public int destroyTime;
    [SerializeField] private bool destroyOnImpact = true;

    [SerializeField] float slowFactor = 1;
    [SerializeField] float slowTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed;
        if(hasShake)
        {
            StartCoroutine(CameraShake.Instance.ShakeCamera(0.65f, .45f));
        }
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().takeDamage(damage);
            if(slowEffect)
            {
                StartCoroutine(other.GetComponent<playerController>().SlowPlayer(slowFactor, slowTime));
            }
            damage = 0; //After the first tick of damage the particle effect will remain but cause no more damage.
        }

        if (destroyOnImpact)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, slowTime);
        }
    }
}
