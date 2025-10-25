namespace Stellarfarer
{
    [System.Serializable]
    public class SerializedDictionary<TKay, TValue> : System.Collections.Generic.Dictionary<TKay, TValue>, UnityEngine.ISerializationCallbackReceiver
    {
        [UnityEngine.SerializeField] private System.Collections.Generic.List<TKay> _keyList = new();
        [UnityEngine.SerializeField] private System.Collections.Generic.List<TValue> _valueList = new();

        public void OnBeforeSerialize()
        {
            _keyList.Clear();
            _valueList.Clear();

            foreach (var kvp in this)
            {
                _keyList.Add(kvp.Key);
                _valueList.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < _keyList.Count && i < _valueList.Count; i++)
                this[_keyList[i]] = _valueList[i];
        }
    }
}