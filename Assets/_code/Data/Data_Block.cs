using UnityEngine;

namespace Runline
{
    [CreateAssetMenu(fileName = "Data Block", menuName = "Runline/Data Block")]
    public class Data_Block : ScriptableObject
    {
        public string blockName;
        public Vector2Int blockNumberRange;
        public string blockTypeName;
        public Vector2Int blockTypeNumberRange;
    }
}
