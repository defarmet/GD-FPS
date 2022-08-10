using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    [Range(1, 10)] public int range;
    public Transform holdArea;
    GameObject currentWeapon;
    private string weaponTag = "Weapon";

    public bool inRange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkRange();
        if (inRange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                PickUp();
            }
        }
    }

    public bool checkRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, range))
        {
            if (hit.transform.tag == weaponTag)
            {
                currentWeapon = hit.transform.gameObject;
                inRange = true;
                return true;
            }
            inRange = false;
            return false;
        }
        else
        {
            inRange = false;
            return false;
        }
    }

    private void PickUp()
    {
        currentWeapon.transform.position = holdArea.position;
        currentWeapon.transform.parent = holdArea;
        currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 180f);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
    }
}

