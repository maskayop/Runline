using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runline
{
    public class MonitorsPanel : MonoBehaviour
    {
        public static MonitorsPanel Instance { get; private set; }

        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform viewport;
        [SerializeField] RectTransform content;
        [SerializeField] RectTransform checkCirclesContainer;

        public ToggleGroup toggleGroup;

        [Header("Monitors")]
        [SerializeField] GameObject monitorPrefab;
        [SerializeField] GameObject checkCirclePrefab;

        [Header("Monitors")]
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
            viewport = scrollRect.viewport;
            content = scrollRect.content;
            toggleGroup = content.GetComponent<ToggleGroup>();

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

            ScrollToTarget(monitorToggles[id].GetComponent<RectTransform>());
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

        /// <summary>
        /// Сдвигает контент так, чтобы целевой элемент появился в видимой области
        /// </summary>
        public void ScrollToTarget(RectTransform targetElement)
        {
            // Принудительно обновляем лэйаут, чтобы размеры были актуальными
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            // Получаем позиции элементов в локальных координатах
            float contentLeftEdge = content.anchoredPosition.x;
            float viewportLeftEdge = -viewport.rect.width / 2f;
            float viewportRightEdge = viewport.rect.width / 2f;

            // Позиция целевого элемента относительно левого края контента
            float targetX = targetElement.anchoredPosition.x;
            float targetLeftEdge = targetX - targetElement.rect.width / 2f;
            float targetRightEdge = targetX + targetElement.rect.width / 2f;

            // Какая граница viewport'а ближе к элементу?
            float currentTargetViewportPos = targetX + contentLeftEdge;

            // Рассчитываем новый сдвиг контента
            float newContentX = contentLeftEdge;

            if (currentTargetViewportPos < viewportLeftEdge)
            {
                // Элемент слева за экраном → сдвигаем вправо
                newContentX = viewportLeftEdge - targetX + targetElement.rect.width / 2f;
            }
            else if (currentTargetViewportPos > viewportRightEdge)
            {
                // Элемент справа за экраном → сдвигаем влево
                newContentX = viewportRightEdge - targetX - targetElement.rect.width / 2f;
            }

            // Плавно или мгновенно
            content.anchoredPosition = new Vector2(newContentX, content.anchoredPosition.y);

            // Обновляем normalizedPosition для согласованности с ScrollRect
            UpdateNormalizedPosition();
        }

        /// <summary>
        /// Центрирует целевой элемент ровно посередине viewport'а
        /// </summary>
        public void CenterOnTarget(RectTransform targetElement)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            // Позиция элемента в координатах контента
            float targetCenterX = targetElement.anchoredPosition.x;

            // Сдвигаем контент так, чтобы элемент оказался по центру
            float newContentX = -targetCenterX + viewport.rect.width / 2f;

            // Ограничиваем, чтобы не уехать за границы
            float minX = Mathf.Min(0, viewport.rect.width - content.rect.width);
            float maxX = 0;
            newContentX = Mathf.Clamp(newContentX, minX, maxX);

            content.anchoredPosition = new Vector2(newContentX, content.anchoredPosition.y);
            UpdateNormalizedPosition();
        }

        void UpdateNormalizedPosition()
        {
            // Синхронизируем normalizedPosition ScrollRect'а (для корректной работы дотягивания)
            float totalWidth = content.rect.width;
            float visibleWidth = viewport.rect.width;

            if (totalWidth <= visibleWidth)
            {
                scrollRect.horizontalNormalizedPosition = 0f;
            }
            else
            {
                float scrollableWidth = totalWidth - visibleWidth;
                float currentScroll = -content.anchoredPosition.x;
                scrollRect.horizontalNormalizedPosition = currentScroll / scrollableWidth;
            }
        }
    }
}
