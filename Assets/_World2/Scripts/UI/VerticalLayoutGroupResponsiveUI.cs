using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class VerticalLayoutGroupResponsiveUI : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

        private void Awake()
        {
            if (_verticalLayoutGroup == null)
                _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        public void SetPadding(float top, float bottom, float left, float right)
        {
            _verticalLayoutGroup.padding.top = Mathf.RoundToInt(top);
            _verticalLayoutGroup.padding.bottom = Mathf.RoundToInt(bottom);
            _verticalLayoutGroup.padding.left = Mathf.RoundToInt(left);
            _verticalLayoutGroup.padding.right = Mathf.RoundToInt(right);
            _verticalLayoutGroup.SetLayoutHorizontal();
            _verticalLayoutGroup.SetLayoutVertical();
        }
    }
}