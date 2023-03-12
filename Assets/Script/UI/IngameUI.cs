using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public TextMeshProUGUI lineCleared;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI score;
    [SerializeField] Slider zoneMeter;
    [SerializeField] TextMeshProUGUI combo;

    private float timeCount;

    private void Start()
    {
        zoneMeter.maxValue = gameManager.maxZone;
        zoneMeter.value = gameManager.zone;
    }

    //visible and hold the animation for 'delay' time then turn off
    IEnumerator Hide(float delay)
    {
        combo.GetComponent<Animator>().SetBool("show", true);
        yield return new WaitForSeconds(delay);
        combo.GetComponent<Animator>().SetBool("show", false);
    }

    // Update is called once per frame
    void Update()
    {
        timeCount = Time.timeSinceLevelLoad;
        lineCleared.text = gameManager.lineCleared.ToString();
        level.text = gameManager.level.ToString();

        //format hour and minute
        float minutes = Mathf.FloorToInt(timeCount / 60);
        float seconds = Mathf.FloorToInt(timeCount % 60);
        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        score.text = gameManager.score.ToString();


        zoneMeter.value = Mathf.Lerp(zoneMeter.value, gameManager.zone, Time.deltaTime * 3);
    }


    public void comboShow(int input)
    {
        if (input == 2)
        {
            combo.text = "2 IN A ROW";
            StartCoroutine(Hide(1.5f));
        }
        else if (input == 3)
        {
            combo.text = "3 IN A ROW";
            StartCoroutine(Hide(1.5f));

        }
        else if (input == 4)
        {
            combo.text = "TETRIS";
            StartCoroutine(Hide(1.5f));

        }
    }
}
