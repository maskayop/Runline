using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vopere.Common.UI;

namespace Runline
{
    public class MonitorsPanel : MonoBehaviour
    {
        public static MonitorsPanel Instance { get; private set; }

        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform content;
        [SerializeField] RectTransform checkCirclesContainer;

        ToggleGroup toggleGroup;
        UIScrollRectExtensions scrollRectExtensions;

        [Header("Monitors")]
        [SerializeField] GameObject monitorPrefab;
        [SerializeField] GameObject checkCirclePrefab;

        [Header("Data")]
        [SerializeField] List<Data_Monitor> dataset = new List<Data_Monitor>();

        List<MonitorToggle> monitorToggles = new List<MonitorToggle>();
        List<CheckCircle> checkCircles = new List<CheckCircle>();

        int currentMonitor = -1;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create MonitorsPanel");
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
            content = scrollRect.content;
            toggleGroup = content.GetComponent<ToggleGroup>();
            scrollRectExtensions = scrollRect.GetComponent<UIScrollRectExtensions>();

            CreateMonitors();
            CreateMonitorsCheckCircles();

            SelectMonitor(0);

            scrollRect.horizontalNormalizedPosition = 0f;
        }

        void CreateMonitors()
        {
            foreach (Transform t in content)
                Destroy(t.gameObject);

            for (int i = 0; i < dataset.Count; i++)
            {
                GameObject go = Instantiate(monitorPrefab, content);
                MonitorToggle mt = go.GetComponent<MonitorToggle>();
                mt.Init(toggleGroup, dataset[i].monitorName, i);
                go.name = "Monitor - " + dataset[i].monitorName;
                monitorToggles.Add(mt);
            }
        }

        void CreateMonitorsCheckCircles()
        {
            checkCircles.Clear();

            foreach (Transform t in checkCirclesContainer)
                Destroy(t.gameObject);

            for (int i = 0; i < dataset.Count; i++)
            {
                GameObject go = Instantiate(checkCirclePrefab, checkCirclesContainer);
                CheckCircle cc = go.GetComponent<CheckCircle>();
                cc.id = i;
                cc.Init();
                cc.Select(false);
                go.name = "Monitor Check - " + dataset[i].monitorName;
                checkCircles.Add(cc);
            }
        }

        public void SelectMonitor(int id)
        {
            currentMonitor = id;
            monitorToggles[id].Select();

            for (int i = 0; i < checkCircles.Count; i++)
            {
                if (i == id)
                    checkCircles[i].Select(true);
                else
                    checkCircles[i].Select(false);
            }

            scrollRectExtensions.ScrollToTarget(monitorToggles[id].GetComponent<RectTransform>());
        }

        public void SelectPrevNextMonitor(bool isNext)
        {
            if (isNext)
                currentMonitor++;
            else
                currentMonitor--;

            if (currentMonitor >= monitorToggles.Count)
                currentMonitor = monitorToggles.Count - 1;
            else if (currentMonitor < 0)
                currentMonitor = 0;

            SelectMonitor(currentMonitor);
        }
    }
}
