using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public enum MeshType
{
    MeshFilter, SkinnedMeshRenderer
}

public class MeshOutline : MonoBehaviour
{
    [SerializeField] private MeshType meshType;
    [SerializeField] private float outlineThickness = 1f;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material alternativeMeshMaterial;
    [SerializeField] private bool enabledOnStart = false;

    private GameObject _duplicateMeshObject;
    private Mesh _mesh;
    private Material _material;
    private Vector3[] _vertices; // array storing outline duplicate mesh vertices
    private bool _outlineReady;

    private void OnEnable()
    {
        if (meshType == MeshType.MeshFilter)
        {
            _mesh = GetComponent<MeshFilter>().mesh;
            _material = GetComponent<MeshRenderer>().material;
        }
        else
        {
            _mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
            _material = GetComponent<SkinnedMeshRenderer>().material;
        }

        _vertices = new Vector3[_mesh.vertexCount];
        StartCoroutine(nameof(MakeOutline));
    }

    /// <summary>
    /// Creates an upscaled and inverted version of mesh by moving vertices outwards along their normals. Results in a cell-shaded look (outline around original model).
    /// The computationally expensive operation of doing this for all vertices are spread over several frames,
    /// by operating on chunks of vertices at a time, to prevent the main thread from stalling. 
    /// This can result in a brief moment without any outline after instantiation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeOutline()
    {
        int index = 0;
        int vertexCount = _mesh.vertexCount;
        while (index < vertexCount)
        {
            // process a chunk of the array
            int end = Mathf.Min(index + 10, vertexCount);
            for (int i = index; i < end; i++)
            {
                // process each element in the chunk
                GenerateVertex(i);
            }

            // move to the next chunk
            index = end;

            // yield control to the main thread
            yield return null;
        }

        // creating duplicate model with flipped normals, using the vertices created above, to create highlight/outline
        _duplicateMeshObject = new("DuplicateMesh", typeof(MeshFilter), typeof(MeshRenderer));
        _duplicateMeshObject.transform.SetParent(transform, false);
        Mesh duplicateMesh = new()
        {
            vertices = _vertices.ToArray(),
            triangles = _mesh.triangles.Reverse().ToArray()
        };
        duplicateMesh.RecalculateBounds();

        // applying mesh and material
        _duplicateMeshObject.GetComponent<MeshFilter>().mesh = duplicateMesh;
        _duplicateMeshObject.GetComponent<MeshRenderer>().material = outlineMaterial;
        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = enabledOnStart;

        _outlineReady = true;
    }

    private void GenerateVertex(int index)
    {
        Vector3 normal = _mesh.normals[index];
        Vector3 newVertex = _mesh.vertices[index] + (normal.normalized * (outlineThickness) / 1000);
        _vertices[index] = newVertex;
    }

    public void EnableOutline() => StartCoroutine(ToggleOutline(true));

    public void DisableOutline() => StartCoroutine(ToggleOutline(false));

    private IEnumerator ToggleOutline(bool enable)
    {
        while (!_outlineReady)
            yield return null;

        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = enable;
    }

    //public void EnableAlternativeMaterial() => return;

    public void ToggleAlternativeMaterial(bool enable)
    {
        if (meshType == MeshType.MeshFilter)
            GetComponent<MeshRenderer>().material = enable ? alternativeMeshMaterial : _material;
        else
            GetComponent<SkinnedMeshRenderer>().material = enable ? alternativeMeshMaterial : _material;
    }
}
