using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum MeshType
{
    MeshFilter, SkinnedMeshRenderer
}

public class MeshOutline : MonoBehaviour
{
    [SerializeField] private MeshType meshType;
    [SerializeField] private float outlineThickness = 1f;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private bool enabledOnStart = false;

    private GameObject _duplicateMeshObject;
    private Mesh _mesh;

    // Start is called before the first frame update
    void Start()
    {
        if (meshType == MeshType.MeshFilter)
            _mesh = GetComponent<MeshFilter>().mesh;
        else
            _mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;

        // creating an upscaled version of mesh by moving vertices outwards along their normals
        List<Vector3> vertices = new();
        for (int i = 0; i < _mesh.vertices.Count(); i++)
        {
            Vector3 normal = _mesh.normals[i];
            Vector3 newVertex = _mesh.vertices[i] + (normal.normalized * (outlineThickness / 1000));
            vertices.Add(newVertex);
        }
        
        // creating duplicate model with flipped normals, using the vertices created above, to create highlight/outline
        _duplicateMeshObject = new("DuplicateMesh", typeof(MeshFilter), typeof(MeshRenderer));
        _duplicateMeshObject.transform.SetParent(transform, false);
        Mesh duplicateMesh = new()
        {
            vertices = vertices.ToArray(),
            triangles = _mesh.triangles.Reverse().ToArray()
        };
        duplicateMesh.RecalculateBounds();
        
        // applying mesh and material
        _duplicateMeshObject.GetComponent<MeshFilter>().mesh = duplicateMesh;
        _duplicateMeshObject.GetComponent<MeshRenderer>().material = outlineMaterial;
        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = enabledOnStart;
    }

    public void EnableOutline()
    {
        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DisableOutline()
    {
        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
