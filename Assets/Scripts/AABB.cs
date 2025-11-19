using UnityEngine;
using UnityEngine.UIElements;

public class AABB : MonoBehaviour
{
    private Vector3 localCenter;
    private Vector3 localExtents;

    void Start()
    {
        // Calcular una sola vez en local space
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh; // usar sharedMesh en editor
        Bounds localBounds = mesh.bounds;

        localCenter = localBounds.center;
        localExtents = localBounds.extents;
    }

    void OnDrawGizmos()
    {
        if (GetComponent<MeshFilter>() == null) return;

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return;

        if (localExtents == Vector3.zero) // si aún no se inicializó (modo editor)
        {
            Bounds localBounds = mesh.bounds;
            localCenter = localBounds.center;
            localExtents = localBounds.extents;
        }

        // Matriz de transformación actual
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        // Centro en mundo
        Vector3 worldCenter = localToWorld.MultiplyPoint3x4(localCenter);

        // Extraemos ejes
        Vector3 right = new Vector3(localToWorld.m00, localToWorld.m01, localToWorld.m02);
        Vector3 up = new Vector3(localToWorld.m10, localToWorld.m11, localToWorld.m12);
        Vector3 forward = new Vector3(localToWorld.m20, localToWorld.m21, localToWorld.m22);

        // Valor absoluto
        right = new Vector3(Mathf.Abs(right.x), Mathf.Abs(right.y), Mathf.Abs(right.z));
        up = new Vector3(Mathf.Abs(up.x), Mathf.Abs(up.y), Mathf.Abs(up.z));
        forward = new Vector3(Mathf.Abs(forward.x), Mathf.Abs(forward.y), Mathf.Abs(forward.z));

        // Extents en mundo
        Vector3 worldExtents =
            right * localExtents.x +
            up * localExtents.y +
            forward * localExtents.z;

        // Bounds en mundo
        Bounds worldBounds = new Bounds(worldCenter, worldExtents * 2f);

        // Dibujar
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
    }

    //private Vector3 worldMin;
    //private Vector3 worldMax;

    //// centro y extents en espacio local (defínelos según tu modelo)
    //// aquí ejemplo: un cubo unitario centrado en (0,0,0)
    //[SerializeField] private Vector3 localCenter = Vector3.zero;
    //[SerializeField] private Vector3 localExtents = Vector3.one * 0.5f;

    //void Update()
    //{
    //    ComputeWorldAABB(transform.localToWorldMatrix, localCenter, localExtents,
    //                     out worldMin, out worldMax);

    //    Debug.Log($"AABB World Min: {worldMin}, Max: {worldMax}");
    //}

    //void OnDrawGizmos()
    //{
    //    // dibujar caja para depuración
    //    Vector3 size = worldMax - worldMin;
    //    Vector3 center = (worldMax + worldMin) * 0.5f;

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(center, size);
    //}

    ///// <summary>
    ///// Calcula la AABB en espacio mundo usando el "absolute matrix trick"
    ///// </summary>
    //private void ComputeWorldAABB(Matrix4x4 M, Vector3 c, Vector3 e,
    //                              out Vector3 worldMin, out Vector3 worldMax)
    //{
    //    // 1. Centro en mundo
    //    Vector3 worldCenter = M.MultiplyPoint3x4(c);

    //    // 2. Matriz 3x3 con valor absoluto
    //    Vector3 right = new Vector3(Mathf.Abs(M.m00), Mathf.Abs(M.m01), Mathf.Abs(M.m02));
    //    Vector3 up = new Vector3(Mathf.Abs(M.m10), Mathf.Abs(M.m11), Mathf.Abs(M.m12));
    //    Vector3 forward = new Vector3(Mathf.Abs(M.m20), Mathf.Abs(M.m21), Mathf.Abs(M.m22));

    //    // 3. Extents en mundo
    //    Vector3 worldExtents =
    //        right * e.x +
    //        up * e.y +
    //        forward * e.z;

    //    // 4. Min y Max
    //    worldMin = worldCenter - worldExtents;
    //    worldMax = worldCenter + worldExtents;
    //}
}
