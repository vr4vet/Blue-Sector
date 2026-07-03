using System.Collections;
using System.Linq;
using UnityEngine;


public class MeshHighlight : MonoBehaviour
{
    [SerializeField] private MeshType meshType;
    [SerializeField] private float outlineThickness = 1f;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material alternativeMeshMaterial;
    [SerializeField] private bool enabledOnStart = false;

    private GameObject _duplicateMeshObject = null;
    private Mesh _mesh;
    private Vector3[] _vertices; // array storing outline duplicate mesh vertices
    private bool _highlightReady = false;
    private bool _enabled = false;

    private Color _opaque, _transparent;

    private void OnEnable()
    {
        _mesh = new Mesh();
        _opaque = new Color(highlightMaterial.color.r, highlightMaterial.color.g, highlightMaterial.color.b, 1);
        _transparent = new Color(highlightMaterial.color.r, highlightMaterial.color.g, highlightMaterial.color.b, 0);

        StartCoroutine(nameof(MakeHighlight));
    }

    private void Update()
    {
        // pulsing highlight material at provided speed
        if (_enabled && _highlightReady)
        {
            float lerp = Mathf.PingPong(Time.time, pulseSpeed) / pulseSpeed;
            highlightMaterial.color = Color.Lerp(_transparent, _opaque, lerp);
        }
    }

    /// <summary>
    /// Creates an upscaled of mesh by moving vertices outwards along their normals. Results in a highlight.
    /// The computationally expensive operation of doing this for all vertices are spread over several frames,
    /// by operating on chunks of vertices at a time, to prevent the main thread from stalling. 
    /// This can result in a brief moment without any highlight after instantiation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeHighlight()
    {
        _highlightReady = false;

        // update vertex positions for mesh and mesh collider in case blend shape has been adjusted
        if (meshType == MeshType.SkinnedMeshRenderer)
        {
            GetComponent<SkinnedMeshRenderer>().BakeMesh(_mesh); // bake current state of mesh
            GetComponent<MeshCollider>().sharedMesh = _mesh; // use that bake as mesh for mesh collider
        }
        else // a "normal" Mesh Filter can not have blend shapes, so simply assign it to variable
            _mesh = GetComponent<MeshFilter>().mesh;

        _vertices = new Vector3[_mesh.vertexCount];

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

        // creating duplicate model using the vertices created above to create highlight
        if (_duplicateMeshObject == null)
        {
            _duplicateMeshObject = new("DuplicateMesh", typeof(MeshFilter), typeof(MeshRenderer));
            _duplicateMeshObject.transform.SetParent(transform, false);
        }
        
        Mesh duplicateMesh = new()
        {
            vertices = _vertices.ToArray(),
            triangles = _mesh.triangles
        };
        duplicateMesh.RecalculateBounds();

        // applying mesh and material
        _duplicateMeshObject.GetComponent<MeshFilter>().mesh = duplicateMesh;
        _duplicateMeshObject.GetComponent<MeshRenderer>().material = highlightMaterial;
        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = enabledOnStart;

        _highlightReady = true;
    }

    private void GenerateVertex(int index)
    {
        Vector3 normal = _mesh.normals[index];
        Vector3 newVertex = _mesh.vertices[index] + (normal.normalized * (outlineThickness) / 1000);
        _vertices[index] = newVertex;
    }

    public void EnableHighlight() => StartCoroutine(ToggleHighlight(true));

    public void DisableHighlight() => StartCoroutine(ToggleHighlight(false));

    private IEnumerator ToggleHighlight(bool enable)
    {
        while (!_highlightReady)
            yield return null;

        _duplicateMeshObject.GetComponent<MeshRenderer>().enabled = enable;
        _enabled = enable;
    }

    /// <summary>
    /// Generates the highlight effect again. Used after BlendShape(s) has moved vertices to update highlight and collider vertex positions.
    /// </summary>
    public void RemakeHighlight() => StartCoroutine(nameof(MakeHighlight));


}
