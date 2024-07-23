using UnityEngine;

namespace DDStudio.Visualization
{
    public class ShowBoundingBox : MonoBehaviour
    {
        [Tooltip("The color of the bounding box lines.")]
        [SerializeField]
        private Color _lineColor = new(1.0f, 1.0f, 1.0f, 0.25f);

        private Material lineMaterial;

        private void Start()
        {
            // Create a simple shader for the GL lines
            Shader shader = Shader.Find("Hidden/Internal-Colored");

            lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }

        private void OnRenderObject()
        {
            // Return if no Renderer component has been found
            if (!TryGetComponent<Renderer>(out var renderer)) return;

            // Apply the line material
            lineMaterial.SetPass(0);

            // Get the bounds of the renderer
            Bounds bounds = renderer.bounds;

            // Calculate the corners of the bounding box
            Vector3 boundsCenter = bounds.center;
            Vector3 boundsExtents = bounds.extents;

            Vector3[] boundsCorners = new Vector3[8];
            boundsCorners[0] = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);
            boundsCorners[1] = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);
            boundsCorners[2] = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);
            boundsCorners[3] = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);
            boundsCorners[4] = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);
            boundsCorners[5] = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);
            boundsCorners[6] = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);
            boundsCorners[7] = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);

            // Draw the lines
            GL.Begin(GL.LINES);
            GL.Color(_lineColor);

            DrawLine(boundsCorners[0], boundsCorners[1]);
            DrawLine(boundsCorners[1], boundsCorners[2]);
            DrawLine(boundsCorners[2], boundsCorners[3]);
            DrawLine(boundsCorners[3], boundsCorners[0]);
            DrawLine(boundsCorners[4], boundsCorners[5]);
            DrawLine(boundsCorners[5], boundsCorners[6]);
            DrawLine(boundsCorners[6], boundsCorners[7]);
            DrawLine(boundsCorners[7], boundsCorners[4]);
            DrawLine(boundsCorners[0], boundsCorners[4]);
            DrawLine(boundsCorners[1], boundsCorners[5]);
            DrawLine(boundsCorners[2], boundsCorners[6]);
            DrawLine(boundsCorners[3], boundsCorners[7]);

            GL.End();
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            GL.Vertex(start);
            GL.Vertex(end);
        }
    }
}