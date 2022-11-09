using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Polygon : MonoBehaviour
{
    public void Generate(int sides, float size, float thickness, bool visible = true)
    {
        polygonSides = sides;
        meshSize = size;
        meshThickness = thickness;
        isVisible = visible;
        Draw();
    }
    public int polygonSides;
    public float meshSize;
    public float meshThickness;
    public bool isVisible;
    public List<Vector3> polygonPoints = new List<Vector3>();
    public List<Vector3> mainPolygonPoints = new List<Vector3>();
    List<int> polygonTriangles = new List<int>();
    //polygon properties
    public void Draw()
    {
        Mesh mesh = new Mesh();
        MeshFilter meshf = GetComponent<MeshFilter>();
        if (meshf != null)
            meshf.mesh = mesh;
        else
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
        if (GetComponent<MeshRenderer>() == null)
           gameObject.AddComponent<MeshRenderer>();
        //var meshR = gameObject.GetComponent<MeshRenderer>();
        //meshR.enabled = isVisible;

        DrawFilled(mesh, polygonSides, meshSize, 0);
    }
    void DrawFilled(Mesh mesh, int sides, float radius, float y)
    {
        mesh.Clear();
        GetCircumferencePoints(sides, radius, y + meshThickness / 2);
        DrawFilledTriangles(polygonPoints, 0);
        //Vector3[] temp = new Vector3[polygonPoints.Count];
        //polygonPoints.CopyTo(temp);
        mainPolygonPoints = polygonPoints.ToList();
        GetCircumferencePoints(sides, radius, y - meshThickness / 2);
        DrawFilledReversedTriangles(polygonPoints, sides);
        

        for (int i = 0; i < sides - 1; i++)
        {
            polygonTriangles.Add(i); polygonTriangles.Add(i + sides); polygonTriangles.Add(i + sides + 1);
            polygonTriangles.Add(i); polygonTriangles.Add(i + sides + 1); polygonTriangles.Add(i + 1);
        }
        polygonTriangles.Add(sides - 1); polygonTriangles.Add((sides * 2) - 1); polygonTriangles.Add(0);
        polygonTriangles.Add(0); polygonTriangles.Add((sides * 2) - 1); polygonTriangles.Add(sides);
        
        mesh.Clear();
        mesh.vertices = polygonPoints.ToArray();
        mesh.triangles = polygonTriangles.ToArray();
        NoShared(mesh);
        //gameObject.AddComponent<MeshCollider>();
    }
    void GetCircumferencePoints(int sides, float radius, float y)
    {
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            polygonPoints.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, y));
        }
    }

    void DrawFilledTriangles(List<Vector3> points, int size)
    {
        int triangleAmount = points.Count - 2;
        for (int i = size; i < triangleAmount; i++)
        {
            polygonTriangles.Add(size);
            polygonTriangles.Add(i + 1);
            polygonTriangles.Add(i + 2);
        }
    }
    void DrawFilledReversedTriangles(List<Vector3> points, int size)
    {
        int triangleAmount = points.Count - 2;
        for (int i = size; i < triangleAmount; i++)
        {
            polygonTriangles.Add(i + 1);
            polygonTriangles.Add(size);
            polygonTriangles.Add(i + 2);
        }
    }
    public void NoShared(Mesh mesh)
    {
        Vector3[] oldVerts = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}