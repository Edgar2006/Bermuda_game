using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Threading.Tasks;
using System.Collections;

public class MeshGenerator : MonoBehaviour
{
    public Material Material;
    public Material Material2;

    public int playerSides;
    public float playerSize;

    public int meshCount;
    public float meshSizeMin;
    public float meshSizeMax;
    public int meshSidesMin;
    public int meshSidesMax;

    public float meshThickness;

    public int[] tris;
    public Vector3[] verts;
    //List<Vector3> vertices = new();

    List<GameObject> Polygons = new List<GameObject>();
    private void Start()
    {
        StartCoroutine(GenerateTest());
    }
    IEnumerator GenerateTest()
    {
        while(true)
        {
            Generate();
            yield return new WaitForSeconds(3f);
        }
    }
    public async void Generate()
    {
        //await Task.Run(() =>
        //{

        Polygons.AddRange(GenerateSecondaries());
        GameObject mesh = GeneratePolygon(5, playerSize + 3, meshThickness, false);
        mesh.transform.position = Vector3.zero;
        mesh.transform.rotation = Quaternion.identity;
        GameObject player = GeneratePolygon(playerSides, playerSize, meshThickness + 1);
        Polygons.Add(player);
        player.transform.rotation = Quaternion.identity;
        Polygon central = player.GetComponent<Polygon>();
        //foreach (var a in central.mainPolygonPoints)
        //{
        //vertices.Add(central.transform.TransformPoint(a));
        //}
        for (int i = Polygons.Count - 2; i > 0;)
        {
            for (int j = 0; j < central.polygonPoints.Count - 1; j++, i--)
            {
                if (i < 0)
                    break;
                Vector3 position = Vector3.Lerp(central.polygonPoints[j], central.polygonPoints[j + 1], Random.Range(0, 1f));
                position.z = 0;
                Polygons[i].transform.position = player.transform.TransformPoint(position) - player.transform.position;
            //await Task.Yield();
            }
        }
        var meshF = mesh.GetComponent<MeshFilter>();
        var meshR = mesh.GetComponent<MeshRenderer>();
        for (int k = 0; k < Polygons.Count; k++)
        {
            Model result = CSG.Subtract(mesh, Polygons[k]);
            Destroy(Polygons[k]);
            meshF.sharedMesh = result.mesh;
            meshR.sharedMaterials = result.materials.ToArray();
            //
            //await Task.Yield();
        }
        
        var myFilter = GetComponent<MeshFilter>();
        var myRenderer = GetComponent<MeshRenderer>();
        if (myFilter == null)
            gameObject.AddComponent<MeshFilter>().sharedMesh = meshF.sharedMesh;
        else
            myFilter.sharedMesh = meshF.sharedMesh;

        if (myRenderer == null)
            gameObject.AddComponent<MeshRenderer>().sharedMaterials = meshR.sharedMaterials;
        else
            myRenderer.sharedMaterials = meshR.sharedMaterials;
        Destroy(mesh);
        Polygons.Clear();
        Plane plane = new Plane(transform.forward, transform.position);
        tris = meshF.sharedMesh.triangles;
        verts = meshF.sharedMesh.vertices;
        int count = tris.Length;
    }
   
    public List<GameObject> GenerateSecondaries()
    {
        List<GameObject> pols = new List<GameObject>();  
        for (int i = 0; i < meshCount; i++)
        {
            GameObject newPolygonObj = GeneratePolygon(Random.Range(meshSidesMin, meshSidesMax), Random.Range(meshSizeMin, meshSizeMax), meshThickness);
            newPolygonObj.transform.parent = transform;
            pols.Add(newPolygonObj);
        }
        return pols;
    }
    public GameObject GeneratePolygon(int sides, float size, float thickness, bool main = true)
    {
        GameObject pol = new GameObject();
        if (main)
        {
            pol.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
            pol.AddComponent<Polygon>().Generate(sides, size, thickness + 1);
        }
        else
        {
            pol.AddComponent<Polygon>().Generate(sides, size, thickness);
        }
        if (main)
            pol.GetComponent<MeshRenderer>().material = Material2;
        else
            pol.GetComponent<MeshRenderer>().material = Material;
        return pol;
    }
    private Vector3 CutLineByPlane(Vector3 p1, Vector3 p2, Plane plane)
    {
        float h1 = Mathf.Abs(plane.GetDistanceToPoint(p1));
        float h2 = Mathf.Abs(plane.GetDistanceToPoint(p2));
        float d = Vector3.Distance(p1, p2);
        float l1 = d * h1 / (h1 + h2);
        Vector3 point = Vector3.Lerp(p1, p2, l1 / d);
        return point;
    }
}
