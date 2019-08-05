using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class EditMesh : MonoBehaviour
{
    void Update()
    {
        Mesh mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
        }

        mesh.vertices = vertices;
    }
}