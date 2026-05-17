using UnityEngine;

namespace Runline
{
    public class MainCanvas : MonoBehaviour
    {
        public static MainCanvas Instance { get; private set; }

        [Header("Main Panel")]
        [SerializeField] GameObject collapseButton;
        [SerializeField] GameObject expandButton;

        MonitorsPanel monitorsPanel;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create MainCanvas");
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
            monitorsPanel = MonitorsPanel.Instance;

            ShowMonitorsPanel(true);
        }

        public void ShowMonitorsPanel(bool state)
        {
            monitorsPanel?.gameObject.SetActive(state);
            collapseButton?.SetActive(state);
            expandButton?.SetActive(!state);
        }
    }
}
