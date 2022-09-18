using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPallet : MonoBehaviour
{
    [SerializeField] Transform particlePos;
    [SerializeField] GameObject particle;
    // Start is called before the first frame update

    bool doOnce = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(doOnce == false && other.tag == "Boss" || other.tag == "EnemyBullet")
        {
            doOnce = true;
            Destroy();
        }
    }
    public void Destroy()
    {
        MeshRenderer[] children = this.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer item in children)
        {
            item.enabled = false;
        }
        Instantiate(particle, particlePos);
        Destroy(gameObject, 3f);

    }

}
