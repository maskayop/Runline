using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runline
{
    public class Block : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;

        [Header("Border")]
        [SerializeField] Image borderImage;
        [SerializeField] Color defaultBorderColor = Color.white;
        [SerializeField] Color alarmBorderColor = Color.white;

        [Header("Alarm Lamps")]
        [SerializeField] int alarmChance = 10;
        [SerializeField] List<CheckCircleBase> alarmCheckCircles = new List<CheckCircleBase>();

        [Header("Fan")]
        [SerializeField] GameObject fanObject;
        [SerializeField] Color defaultFanColor = Color.white;
        [SerializeField] Color wokingFanColor = Color.white;
        [SerializeField] Color alarmFanColor = Color.white;
        [SerializeField] int fanAlarmChance = 10;
        [SerializeField] string fanWorkingAnimState;
        [SerializeField] string fanNotWorkingAnimState;

        Image fanImage;
        Animator fanAnimator;

        [Header("Connection")]
        [SerializeField] int noConnectionChance = 10;
        [SerializeField] TextMeshProUGUI connectionUpdateText;
        [SerializeField] GameObject connectionUpdatePanel;
        [SerializeField] GameObject noConnectionPanel;
        [SerializeField] Vector2 connectionUpdateTimeRange = new Vector2(0, 1);

        float currentUpdateTime = 0;
        bool noConnection = false;

        [Header("Frequency")]
        [SerializeField] TextMeshProUGUI frequencyText;
        [SerializeField] Vector2 frequencyRange = new Vector2(0, 1);

        void Update()
        {
            currentUpdateTime -= Time.deltaTime;

            if (currentUpdateTime < 0)
            {
                currentUpdateTime = Random.Range(connectionUpdateTimeRange.x, connectionUpdateTimeRange.y);
                OnInit();
            }
        }

        public void Init(string INname)
        {
            nameText.text = INname;

            fanImage = fanObject.GetComponent<Image>();
            fanAnimator = fanObject.GetComponent<Animator>();
            fanAnimator.Play(fanNotWorkingAnimState);

            OnInit();
        }

        void OnInit()
        {
            UpdateAlarmCircles();
            UpdateConnection();
            UpdateFan();
        }

        void UpdateAlarmCircles()
        {
            for (int i = 0; i < alarmCheckCircles.Count; i++)
            {
                int rand = Random.Range(0, alarmChance);

                if (rand == 0 && !noConnection)
                    alarmCheckCircles[i].Select(true);
                else
                    alarmCheckCircles[i].Select(false);
            }
        }

        void UpdateConnection()
        {
            int rand = Random.Range(0, noConnectionChance);

            if (rand == 0)
            {
                connectionUpdatePanel.SetActive(false);
                noConnectionPanel.SetActive(true);
                noConnection = true;
                SetBorderColor(alarmBorderColor);
            }
            else
            {
                connectionUpdatePanel.SetActive(true);
                noConnectionPanel.SetActive(false);
                noConnection = false;
                SetBorderColor(defaultBorderColor);
            }

            connectionUpdateText.text = System.DateTime.Now.ToString();
        }

        void SetBorderColor(Color c)
        {
            borderImage.color = c;
        }

        void UpdateFan()
        {
            int rand = Random.Range(0, fanAlarmChance);

            if (noConnection)
            {
                fanAnimator.Play(fanNotWorkingAnimState);
                fanImage.color = defaultFanColor;
                frequencyText.text = "0";
                return;
            }

            if (rand == 0)
            {
                fanAnimator.Play(fanNotWorkingAnimState);
                fanImage.color = alarmFanColor;
                frequencyText.text = "0";
            }
            else
            {
                fanAnimator.Play(fanWorkingAnimState);
                fanImage.color = wokingFanColor;

                float randFreq = Random.Range(frequencyRange.x, frequencyRange.y);

                frequencyText.text = randFreq.ToString("F2");
            }
        }
    }
}
