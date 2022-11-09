using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class Test : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public void Do()
    {
        // Perform boolean operation
        Model result = CSG.Subtract(obj2, obj1);

        // Create a gameObject to render the result
        var composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        Model result2 = CSG.Intersect(obj1, obj2);

        // Create a gameObject to render the result
        var composite2 = new GameObject();
        composite2.AddComponent<MeshFilter>().sharedMesh = result2.mesh;
        composite2.AddComponent<MeshRenderer>().sharedMaterials = result2.materials.ToArray();
        Debug.Log(composite2.GetComponent<MeshFilter>().sharedMesh.subMeshCount);
        
    }
}
