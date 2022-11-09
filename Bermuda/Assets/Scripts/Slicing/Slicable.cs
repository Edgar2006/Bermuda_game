using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slicable : MonoBehaviour
{
    [HideInInspector] public MeshFilter MyMeshFilter;
    [HideInInspector] public MeshRenderer MyMeshRenderer;
    private void Awake()
    {
        MyMeshFilter = GetComponent<MeshFilter>();
        MyMeshRenderer = GetComponent<MeshRenderer>();
    }
    /*private void Start()
    {
        v = MyMeshFilter.mesh.vertices.ToList();
        t = MyMeshFilter.mesh.triangles.ToList();
        foreach (var a in MyMeshFilter.mesh.vertices)
        {
            Instantiate(SpherePrefab, transform.TransformPoint(a), Quaternion.identity).name = string.Format("{0}, {1}, {2}", a.x, a.y, a.z);
        }
    }*/
}
