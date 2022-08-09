using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    [Header("PickUp Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectRB;
    [SerializeField] private float pickupRange = 1;
    [SerializeField] private float pickupForce = 50;

    private void Start()
    {
        heldObject = null;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickUp(hit.transform.gameObject);
                }
            }
        }
        if (heldObject != null)
        {
            MoveObject();
        }

        if (Input.GetButtonDown("Drop"))
        {
            if (heldObject == null)
            {
                return;
            }
            else
            {
                Drop();
            }


        }
    }

    void PickUp(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjectRB = pickObj.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea;
            heldObject = pickObj;
        }
    }
    void Drop()
    {
        if (Input.GetButtonDown("Drop"))
        {
            heldObjectRB.useGravity = true;
            heldObjectRB.drag = 1;
            heldObjectRB.constraints = RigidbodyConstraints.None;

            heldObject.transform.parent = null;
            heldObject = null;
        }

    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }
}
