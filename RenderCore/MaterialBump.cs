using System.Collections.Generic;

namespace WPRenderer
{
    public class MaterialBump : Material
    {
        public Texture normalTexture;
        public float bumpScale = 5f;

        Vector3 tangentSpaceLight;

        public MaterialBump(Texture mainTexture, Color mainColor, Texture normalTexture)
            : base(mainTexture, mainColor)
        {
            this.normalTexture = normalTexture;
        }

        public override Vector4 CallVertexStage(ref Vertex vertex)
        {
            Vector3 normal = vertex.normal.normalized;
            Vector3 tangent = vertex.tangent.normalized;
            Vector3 binormal = Vector3.Cross(normal, tangent);

            Matrix4x4 objectToTangentSpace = new Matrix4x4(
                tangent.x, tangent.y, tangent.z, 0,
                binormal.x, binormal.y, binormal.z, 0,
                normal.x, normal.y, normal.z, 0,
                0, 0, 0, 1
            );

            // Note that use Matrix4x4.Mul3x3 rotation light direction(inverse).
            tangentSpaceLight = objectToTangentSpace * Matrix4x4.Mul3x3(currentInverseM, -currentLight.direction);

            return base.CallVertexStage(ref vertex);
        }

        public override Color CallFragmentStage(ref Vertex vertex)
        {
            Color color = vertex.color * mainColor * Tex2D(mainTexture, vertex.uv.x, vertex.uv.y);
            Color normalC = Tex2D(normalTexture, vertex.uv.x, vertex.uv.y);
            Vector3 normal = new Vector3(normalC.r, normalC.g, normalC.b);
            // unpack normal (same work with Unity builtin UnpackNormal)
            normal = normal * 2 - Vector3.one;

            // scale normal
            normal.x *= bumpScale;
            normal.y *= bumpScale;
            normal.Normalize();

            // lambert
            //float diffuse = currentLight.intensity * Mathf.Max(0, Vector3.Dot(tangentSpaceLight, normal));
            // half lambert
            float diffuse = 0.5f * currentLight.intensity * Mathf.Max(0, Vector3.Dot(tangentSpaceLight, normal)) + 0.5f;

            // alpha dont't need apply calculation
            float alpha = color.a;

            // finally colors
            color *= currentLight.color * diffuse;
            color.a = alpha;
            return color;
        }
    }
}
