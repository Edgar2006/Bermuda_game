using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    public MeshCollider collider;
    private void Awake()
    {
        collider = GetComponent<MeshCollider>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Mesh")
        {
            GameManager.instance.Lose();
        }
        else if(collision.gameObject.tag == "MeshEnd")
        {
            var mc = collision.gameObject.GetComponentInParent<MeshController>();
            mc.Done();
        }
    }
}