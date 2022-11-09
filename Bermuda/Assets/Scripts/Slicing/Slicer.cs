using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slicer : MonoBehaviour
{
    //public Plane plane;
    public List<Transform> planesTransform = new List<Transform>();
    public List<Plane> planes;
    public Vector3[] verts;
    public int[] tris;
    private void OnTriggerEnter(Collider collision)
    {
        planes = new List<Plane>();
        foreach (var a in planesTransform)
        {
            var temp = new Plane(a.up, a.position);
            planes.Add(temp);
        }
        //plane = new Plane(transform.up, transform.position);
        Slicable other = collision.GetComponent<Slicable>();
        if(other != null)
        {
            Cut(other);
        }    
    }
    public void Cut(Slicable other)
    {
        for(int d = 0; d < planes.Count; d++)
        {
            Plane plane = planes[d];
            Mesh otherMesh = other.gameObject.GetComponent<MeshFilter>().mesh;
            //otherMesh.RecalculateBounds();
            verts = new Vector3[otherMesh.vertexCount];
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = other.transform.TransformPoint(otherMesh.vertices[i]);
            }
            tris = other.MyMeshFilter.mesh.triangles;
            int count = tris.Length;
            List<Vector3> cutPlaneVertices = new List<Vector3>();
            for (int i = 0; i < count; i += 3)
            {
                Vector3 p1 = verts[tris[i]];
                Vector3 p2 = verts[tris[i + 1]];
                Vector3 p3 = verts[tris[i + 2]];
                if (plane.SameSide(p1, p2) && plane.SameSide(p2, p3)) continue;
                if (plane.SameSide(p1, p3))
                {
                    Vector3 point1 = CutLineByPlane(p1, p2, plane);
                    Vector3 point2 = CutLineByPlane(p3, p2, plane);

                    Vector3[] ve = new Vector3[verts.Length + 2];
                    verts.CopyTo(ve, 0);
                    ve[ve.Length - 2] = point1;
                    cutPlaneVertices.Add(point1);
                    ve[ve.Length - 1] = point2;
                    cutPlaneVertices.Add(point2);
                    verts = new Vector3[ve.Length];
                    ve.CopyTo(verts, 0);
                    for (int j = 0; j < ve.Length; j++)
                    {
                        ve[j] = other.transform.InverseTransformPoint(ve[j]);
                    }
                    int[] tr = new int[tris.Length + 6];
                    tris.CopyTo(tr, 0);
                    tr[i + 1] = ve.Length - 2;
                    tr[i + 2] = ve.Length - 1;

                    tr[tr.Length - 6] = ve.Length - 1;
                    tr[tr.Length - 5] = ve.Length - 2;
                    tr[tr.Length - 4] = tris[i + 1];

                    tr[tr.Length - 3] = ve.Length - 1;
                    tr[tr.Length - 2] = tris[i + 2];
                    tr[tr.Length - 1] = tris[i];
                    tris = tr;
                    other.MyMeshFilter.mesh.vertices = ve;
                    other.MyMeshFilter.mesh.triangles = tr;
                }
                else if (plane.SameSide(p2, p3))
                {
                    Vector3 point1 = CutLineByPlane(p2, p1, plane);
                    Vector3 point2 = CutLineByPlane(p3, p1, plane);
                    Vector3[] ve = new Vector3[verts.Length + 2];
                    verts.CopyTo(ve, 0);
                    ve[ve.Length - 2] = point1;
                    cutPlaneVertices.Add(point1);
                    ve[ve.Length - 1] = point2;
                    cutPlaneVertices.Add(point2);
                    verts = new Vector3[ve.Length];
                    ve.CopyTo(verts, 0);
                    for (int j = 0; j < ve.Length; j++)
                    {
                        ve[j] = other.transform.InverseTransformPoint(ve[j]);
                    }
                    int[] tr = new int[tris.Length + 6];
                    tris.CopyTo(tr, 0);
                    tr[i + 1] = ve.Length - 2;
                    tr[i + 2] = ve.Length - 1;

                    tr[tr.Length - 6] = ve.Length - 1;
                    tr[tr.Length - 4] = tris[i + 1];
                    tr[tr.Length - 5] = ve.Length - 2;

                    tr[tr.Length - 3] = ve.Length - 1;
                    tr[tr.Length - 2] = tris[i + 1];
                    tr[tr.Length - 1] = tris[i + 2];
                    tris = tr;
                    other.MyMeshFilter.mesh.vertices = ve;
                    other.MyMeshFilter.mesh.triangles = tr;
                }
                else if (plane.SameSide(p1, p2))
                {
                    Vector3 point1 = CutLineByPlane(p1, p3, plane, false);
                    Vector3 point2 = CutLineByPlane(p2, p3, plane, false);

                    Vector3[] ve = new Vector3[verts.Length + 2];

                    verts.CopyTo(ve, 0);
                    ve[ve.Length - 2] = point1;
                    cutPlaneVertices.Add(point1);
                    ve[ve.Length - 1] = point2;
                    cutPlaneVertices.Add(point2);
                    verts = new Vector3[ve.Length];
                    ve.CopyTo(verts, 0);
                    for (int j = 0; j < ve.Length; j++)
                    {
                        ve[j] = other.transform.InverseTransformPoint(ve[j]);
                    }
                    int[] tr = new int[tris.Length + 6];
                    tris.CopyTo(tr, 0);
                    tr[i + 1] = ve.Length - 1;
                    tr[i + 2] = ve.Length - 2;

                    tr[tr.Length - 6] = ve.Length - 1;
                    tr[tr.Length - 5] = tris[i + 2];
                    tr[tr.Length - 4] = ve.Length - 2;

                    tr[tr.Length - 3] = tris[i];
                    tr[tr.Length - 2] = tris[i + 1];
                    tr[tr.Length - 1] = ve.Length - 1;
                    tris = tr;
                    other.MyMeshFilter.mesh.vertices = ve;
                    other.MyMeshFilter.mesh.triangles = tr;
                }
            }
            other.MyMeshFilter.mesh.RecalculateBounds();
            other.MyMeshFilter.mesh.RecalculateNormals();

            GameObject top = new GameObject(other.gameObject.name + " top");
            top.transform.position = other.transform.position;
            top.transform.rotation = other.transform.rotation;
            top.AddComponent<MeshFilter>().mesh = GetMeshBySide(other.MyMeshFilter.mesh, plane, true, other.transform, cutPlaneVertices);
            top.AddComponent<MeshRenderer>().material = other.MyMeshRenderer.material;
            top.AddComponent<MeshCollider>().convex = true;
            top.AddComponent<Slicable>();
            top.AddComponent<Rigidbody>();//.isKinematic = true;

            GameObject bottom = new GameObject(other.gameObject.name + " bottom");
            bottom.transform.position = other.transform.position;
            bottom.transform.rotation = other.transform.rotation;
            Mesh bmesh = GetMeshBySide(other.MyMeshFilter.mesh, plane, false, other.transform, cutPlaneVertices);
            bottom.AddComponent<MeshFilter>().mesh = bmesh;
            bottom.AddComponent<MeshRenderer>().material = other.MyMeshRenderer.material;
            bottom.AddComponent<MeshCollider>().convex = true;
            bottom.AddComponent<Rigidbody>();//.isKinematic = true;
            Destroy(other.gameObject);

            other = bottom.AddComponent<Slicable>();
        }
        Destroy(this);
    }
    private Vector3 CutLineByPlane(Vector3 p1, Vector3 p2, Plane plane, bool a = true)
    {
        float h1 = Mathf.Abs(plane.GetDistanceToPoint(p1));
        float h2 = Mathf.Abs(plane.GetDistanceToPoint(p2));
        float d = Vector3.Distance(p1, p2);
        float l1 = d * h1 / (h1 + h2);
        Vector3 point = Vector3.Lerp(p1, p2, l1 / d);
        return point;
    }
    private Mesh GetMeshBySide(Mesh mesh, Plane plane, bool isTop, Transform t, List<Vector3> cutPlane)
    {
    List<Vector3> newVertices;
    List<int> newTriangles;
    Mesh newMesh = new Mesh();
        if (isTop)
        {
            newVertices = new List<Vector3>();
            foreach (var a in mesh.vertices)
            {
                if (plane.GetSide(t.TransformPoint(a)))
                {
                    newVertices.Add(a);
                }
            }
            Vector3 center = Vector3.zero;
            foreach (var a in cutPlane)
            {
                center += a;
                newVertices.Add(t.InverseTransformPoint(a));
            }
            center /= cutPlane.Count;
            newVertices.Add(t.InverseTransformPoint(center));

            newMesh.vertices = newVertices.ToArray();
            newTriangles = new List<int>();
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                if (newVertices.IndexOf(mesh.vertices[mesh.triangles[i]]) != -1 &&
                    newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 1]]) != -1 &&
                    newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 2]]) != -1)
                {
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i]]));
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 1]]));
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 2]]));
                }
            }
            for (int i = newVertices.Count - cutPlane.Count - 1; i < newVertices.Count - 2; i += 2)
            {
                Plane pl = new Plane(newVertices[i], newVertices[i + 1], newVertices[newVertices.Count - 1]);
                if (Vector3.Angle(pl.normal, plane.normal) > 45f)
                {
                    newTriangles.Add(i);
                    newTriangles.Add(i + 1);
                    newTriangles.Add(newVertices.Count - 1);
                }
                else
                {
                    newTriangles.Add(newVertices.Count - 1);
                    newTriangles.Add(i + 1);
                    newTriangles.Add(i);
                }
            }
            newMesh.triangles = newTriangles.ToArray();
        }
        else
        {
            newVertices = new List<Vector3>();
            foreach (var a in mesh.vertices)
            {
                if (plane.flipped.GetSide(t.TransformPoint(a)))
                {
                    newVertices.Add(a);
                }
            }
            Vector3 center = Vector3.zero;
            foreach (var a in cutPlane)
            {
                center += a;
                newVertices.Add(t.InverseTransformPoint(a));
            }
            center /= cutPlane.Count;
            newVertices.Add(t.InverseTransformPoint(center));

            newMesh.vertices = newVertices.ToArray();
            newTriangles = new List<int>();
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                if (newVertices.IndexOf(mesh.vertices[mesh.triangles[i]]) != -1 &&
                    newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 1]]) != -1 &&
                    newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 2]]) != -1)
                {
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i]]));
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 1]]));
                    newTriangles.Add(newVertices.IndexOf(mesh.vertices[mesh.triangles[i + 2]]));
                }
            }
            for (int i = newVertices.Count - cutPlane.Count - 1; i < newVertices.Count - 2; i +=2)
            {
                Plane pl = new Plane(newVertices[i], newVertices[i + 1], newVertices[newVertices.Count - 1]);
                if (Vector3.Angle(pl.normal, plane.normal) < 45f)
                {
                    newTriangles.Add(i);
                    newTriangles.Add(i + 1);
                    newTriangles.Add(newVertices.Count - 1);
                }
                else
                {
                    newTriangles.Add(newVertices.Count - 1);
                    newTriangles.Add(i + 1);
                    newTriangles.Add(i);
                }
            }
            newMesh.triangles = newTriangles.ToArray();
        }
        NoShared(newMesh);
        return newMesh;
    }
    public void NoShared(Mesh mesh) // Code got from https://answers.unity.com/questions/798510/flat-shading.html
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