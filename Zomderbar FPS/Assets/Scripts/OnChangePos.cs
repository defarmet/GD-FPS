using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangePos : MonoBehaviour
{
    public PolygonCollider2D Hole2Dcollisions;
    public PolygonCollider2D ground2DCollisions;
    public MeshCollider genMeshcollider;
    Mesh genMesh;

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            Hole2Dcollisions.transform.position = new Vector2(transform.position.x, transform.position.z);
        }
    }

    private void  makeHole()
    {
        Vector2[] Pointpos = Hole2Dcollisions.GetPath(0);

        for (int i = 0; i < Pointpos.Length; i++)
        {
            Pointpos[i] += (Vector2)Hole2Dcollisions.transform.position;
        }

        ground2DCollisions.pathCount = 2;
        ground2DCollisions.SetPath(1, Pointpos);
    }

    private void make3dMeshCollision()
    {
        genMesh = ground2DCollisions.CreateMesh(true, true);
        genMeshcollider.sharedMesh = genMesh;
    }
}
