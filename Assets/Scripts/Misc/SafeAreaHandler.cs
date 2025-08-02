using UnityEngine;

namespace CapstoneProj.MiscSystem
{
    public class SafeAreaHandler : MonoBehaviour
    {
        private void Awake()
            => ApplySafeArea();

        private void Update()
            => ApplySafeArea();

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            // Bottom-left corner of safe area
            Vector3 safeMin = Camera.main.ScreenToWorldPoint(new Vector3(safeArea.xMin, safeArea.yMin, Camera.main.nearClipPlane));

            // Top-right corner of safe area
            Vector3 safeMax = Camera.main.ScreenToWorldPoint(new Vector3(safeArea.xMax, safeArea.yMax, Camera.main.nearClipPlane));

            // Center position of the safe area
            Vector3 safeCenter = (safeMin + safeMax) / 2f;

            // Optional: Resize object to fit within the safe area
            Vector3 safeSize = safeMax - safeMin;

            // Move this object to center of safe area
            transform.position = new Vector3(safeCenter.x, safeCenter.y, transform.position.z);

            // Optionally scale to fit (for demonstration)
            transform.localScale = new Vector3(safeSize.x, safeSize.y, 1f);
        }
    }
}