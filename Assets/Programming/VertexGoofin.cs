using UnityEngine;
using System.Collections;

public class VertexGoofin : MonoBehaviour
{
    public float smooth = 0.01f;
    private Mesh mesh;
    private void Start()
    {
        try {
            GetComponent<SkinnedMeshRenderer>().BakeMesh(mesh);
        } catch {}

        try {
            mesh = GetComponent<MeshFilter>().mesh;
        } catch {}
    }
    void Update()
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int i = 0;
        while (i < vertices.Length)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time) * smooth;
            i++;
        }
        mesh.vertices = vertices;
    }
}