using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runline
{
    [RequireComponent(typeof(Toggle))]
    public class MonitorToggle : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI monitorName;
        [SerializeField] GameObject checkmark;

        Toggle toggle;
        MonitorsPanel monitorsPanel;

        void Start()
        {
            toggle = GetComponent<Toggle>();
            monitorsPanel = MonitorsPanel.Instance;

            Init();
        }

        public void Init()
        {
            //monitorName.text = "";

            UpdateState();
        }

        public void UpdateState()
        {
            checkmark.SetActive(toggle.isOn);
        }
    }
}
