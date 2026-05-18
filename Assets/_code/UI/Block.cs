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
        [SerializeField] Color workingFanColor = Color.white;
        [SerializeField] Color alarmFanColor = Color.white;
        [SerializeField] int fanAlarmChance = 10;
        [SerializeField] string fanWorkingAnimState;
        [SerializeField] string fanNotWorkingAnimState;

        Image fanImage;
        Animator fanAnimator;

        [Header("Pressure")]
        [SerializeField] TextMeshProUGUI pressureInTop;
        [SerializeField] TextMeshProUGUI pressureInBottom;
        [SerializeField] TextMeshProUGUI pressureOutTop;
        [SerializeField] TextMeshProUGUI pressureOutBottom;
        [SerializeField] Vector2 pressureRange = new Vector2(0f, 1f);

        [Header("Fill Indicator")]
        [SerializeField] Image fillImage;
        [SerializeField] TextMeshProUGUI fillValueText;
        [SerializeField] Color minFillColor = Color.white;
        [SerializeField] Color maxFillColor = Color.white;

        [Header("Connection")]
        [SerializeField] int noConnectionChance = 10;
        [SerializeField] TextMeshProUGUI connectionUpdateText;
        [SerializeField] GameObject connectionUpdatePanel;
        [SerializeField] GameObject noConnectionPanel;
        [SerializeField] Vector2 connectionUpdateTimeRange = new Vector2(0f, 1f);

        float currentUpdateTime = 0;
        bool noConnection = false;

        [Header("Frequency")]
        [SerializeField] TextMeshProUGUI frequencyText;
        [SerializeField] Vector2 frequencyRange = new Vector2(0f, 1f);

        [Header("Temperature")]
        [SerializeField] TextMeshProUGUI temperatureText;
        [SerializeField] Vector2 temperatureRange = new Vector2(0f, 1f);

        void Update()
        {
            currentUpdateTime -= Time.deltaTime;

            if (currentUpdateTime < 0)
                OnInit();
        }

        public void Init(string INname)
        {
            nameText.text = INname;

            fanImage = fanObject.GetComponent<Image>();
            fanAnimator = fanObject.GetComponent<Animator>();
            fanAnimator.StopPlayback();

            OnInit();
        }

        void OnInit()
        {
            currentUpdateTime = Random.Range(connectionUpdateTimeRange.x, connectionUpdateTimeRange.y);

            UpdateAlarmCircles();
            UpdateConnection();
            UpdateFan();
            UpdatePressure();
            UpdateFillIndicator();
            UpdateTemperature();
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
                fanImage.color = workingFanColor;

                float randFreq = Random.Range(frequencyRange.x, frequencyRange.y);

                frequencyText.text = randFreq.ToString("F2");
            }
        }

        void UpdatePressure()
        {
            if (noConnection)
            {
                pressureInTop.text = "0";
                pressureInBottom.text = "0";
                pressureOutTop.text = "0";
                pressureOutBottom.text = "0";
                return;
            }

            float ranTI = Random.Range(pressureRange.x, pressureRange.y);
            float ranBI = Random.Range(pressureRange.x, pressureRange.y);
            float ranTO = Random.Range(pressureRange.x, pressureRange.y);
            float ranBO = Random.Range(pressureRange.x, pressureRange.y);

            pressureInTop.text = ranTI.ToString("F2");
            pressureInBottom.text = ranBI.ToString("F2");
            pressureOutTop.text = ranTO.ToString("F2");
            pressureOutBottom.text = ranBO.ToString("F2");
        }

        void UpdateFillIndicator()
        {
            if (noConnection)
            {
                fillValueText.text = "0";
                fillImage.gameObject.SetActive(false);
                return;
            }

            float rand = Random.Range(0f, 1f);

            fillImage.gameObject.SetActive(true);
            fillImage.color = Color.Lerp(minFillColor, maxFillColor, rand);
            fillImage.fillAmount = rand;

            if (rand == 0)
                fillValueText.text = "0";
            else
                fillValueText.text = rand.ToString("F2");
        }

        void UpdateTemperature()
        {
            if (noConnection)
            {
                temperatureText.text = "-";
                return;
            }

            float rand = Random.Range(temperatureRange.x, temperatureRange.y);

            if (rand == 0)
                temperatureText.text = "-";
            else
                temperatureText.text = rand.ToString("F1") + " °";
        }
    }
}
