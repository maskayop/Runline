using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runline
{
    public class BlocksPanel : MonoBehaviour
    {
        public static BlocksPanel Instance { get; private set; }

        [SerializeField] GridLayoutGroup containerGrid;
        [SerializeField] GameObject blockPrefab;
        [SerializeField] Data_Block data;
        [SerializeField] int amount;

        List<Block> dataset = new List<Block>();

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create BlocksPanel");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            CreateBlocks();
        }

        void CreateBlocks()
        {
            dataset.Clear();

            foreach (Transform t in containerGrid.transform)
                Destroy(t.gameObject);

            for (int i = 0; i < amount; i++)
            {
                GameObject go = Instantiate(blockPrefab, containerGrid.transform);
                Block b = go.GetComponent<Block>();

                int randName = Random.Range(data.blockNumberRange.x, data.blockNumberRange.y);
                int randTypeName = Random.Range(data.blockTypeNumberRange.x, data.blockTypeNumberRange.y);
                string blockName = data.blockName + " " + randName + " " + data.blockTypeName + randTypeName;

                b.Init(blockName);
                go.name = "Block - " + blockName;
                dataset.Add(b);
            }
        }
    }
}
