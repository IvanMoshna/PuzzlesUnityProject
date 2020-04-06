using UnityEngine;

namespace Common
{
    public class GizmoSphere : MonoBehaviour
    {
        public float Radius;
        public Color Color;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}