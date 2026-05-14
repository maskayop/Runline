using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runline
{
    [RequireComponent(typeof(Toggle))]
    public class MonitorToggle : MonoBehaviour
    {
        public int id = -1;

        [SerializeField] TextMeshProUGUI monitorName;
        [SerializeField] GameObject checkmark;

        Toggle toggle;
        MonitorsPanel monitorsPanel;

        public void Init(ToggleGroup toggleGroup, string INmonitorName, int INid)
        {
            toggle = GetComponent<Toggle>();
            monitorsPanel = MonitorsPanel.Instance;

            toggle.group = toggleGroup;
            monitorName.text = INmonitorName;
            id = INid;
        }

        public void UpdateState()
        {
            checkmark.SetActive(toggle.isOn);

            if (toggle.isOn)
                SelectMonitor();
        }

        public void Select()
        {
            toggle.isOn = true;
        }

        void SelectMonitor()
        {
            monitorsPanel.SelectMonitor(id);
        }
    }
}
