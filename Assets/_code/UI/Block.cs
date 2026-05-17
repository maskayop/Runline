using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runline
{
    public class Block : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;

        [Header("Alarm Lamps")]
        [SerializeField] int alarmChance = 10;
        [SerializeField] List<CheckCircleBase> alarmCheckCircles = new List<CheckCircleBase>();

        public void Init(string INname)
        {
            nameText.text = INname;

            for (int i = 0; i < alarmCheckCircles.Count; i++)
            {
                int rand = Random.Range(0, alarmChance);

                if (rand == 0)
                    alarmCheckCircles[i].Select(true);
                else
                    alarmCheckCircles[i].Select(false);
            }
        }
    }
}
