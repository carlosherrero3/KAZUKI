using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimingZone : MonoBehaviour
{
    public RectTransform indicator;
    public RectTransform perfectZone;
    public TMP_Text resultText;
    public string nextSceneName = "Nivel 2b";

    public IndicatorMover indicatorMover; // <-- Referencia al otro script

    private int streak = 0;

    public float speedIncrease = 50f; // cuánto sube la velocidad por acierto

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float distance = Mathf.Abs(indicator.anchoredPosition.x - perfectZone.anchoredPosition.x);
            float zoneWidth = perfectZone.rect.width / 2;

            if (distance < zoneWidth * 0.3f)
            {
                resultText.text = "PERFECTO";
                streak++;
                IncreaseSpeed();
            }
            else if (distance < zoneWidth)
            {
                resultText.text = "Bien";
                streak++;
                IncreaseSpeed();
            }
            else
            {
                resultText.text = "Mala";
                streak = 0;
            }

            if (streak >= 3)
            {
                LevelLoader.LoadLevel(nextSceneName);
            }
        }
    }

    void IncreaseSpeed()
    {
        if (indicatorMover != null)
        {
            indicatorMover.speed += speedIncrease;
        }
    }
}