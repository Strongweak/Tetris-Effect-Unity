using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameOver;
    [SerializeField] Image image;
    [SerializeField] GameObject resultAppear;
    [SerializeField] GameObject option;

    [SerializeField] Color currentColor;
    [SerializeField] Color goaltextColor;
    [SerializeField] Color goalreverseTextColor;
    [SerializeField] Color imageColor;
    [SerializeField] int rank = 0;


    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI line;
    [SerializeField] TextMeshProUGUI score;

    private IngameUI inGameUI;
    private void Start()
    {
        inGameUI = FindObjectOfType<IngameUI>();
        time.text = inGameUI.time.text;
        level.text = inGameUI.level.text;
        line.text = inGameUI.lineCleared.text;
        score.text = inGameUI.score.text;

        gameOver.enabled = true;

        resultAppear.SetActive(false);
        option.SetActive(false);
    }

    IEnumerator LerpColor()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1.5)
        {
            currentColor = Color.Lerp(currentColor, goaltextColor, timeElapsed / 1f);
            timeElapsed += Time.deltaTime;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        currentColor = goaltextColor;
    }

    IEnumerator LerpReverseColor()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1.5)
        {
            currentColor = Color.Lerp(currentColor, goalreverseTextColor, timeElapsed / 1f);
            timeElapsed += Time.deltaTime;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        currentColor = goalreverseTextColor;
        //yield return new WaitForSeconds(0.5f);
        //gameOver.gameObject.SetActive(false);

    }

    IEnumerator LerpImage()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1.5)
        {
            image.color = Color.Lerp(image.color, imageColor, timeElapsed / 1f);
            timeElapsed += Time.deltaTime;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        image.color = imageColor;
    }

    IEnumerator LerpResult()
    {
        foreach (Transform child in resultAppear.transform)
        {
            //yield return new WaitForSeconds(0.5f);
            float timeElapsed = 0;
            while (timeElapsed < 0.5)
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(child.GetComponent<TextMeshProUGUI>().color, goaltextColor, timeElapsed / 1f);
                timeElapsed += Time.deltaTime;
                //wait for the next frame and continue execution the Lerp
                yield return null;
            }
            child.gameObject.GetComponent<TextMeshProUGUI>().color = goaltextColor;
        }
    }

    IEnumerator LerpOption()
    {
        foreach (Transform child in option.transform)
        {
            //yield return new WaitForSeconds(0.5f);
            float timeElapsed = 0;
            while (timeElapsed < 0)
            {
                if(child.gameObject.GetComponent<TextMeshProUGUI>() != null)
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(child.GetComponent<TextMeshProUGUI>().color, goaltextColor, timeElapsed / 1f);
                }
                if (child.gameObject.GetComponent<Image>() != null)
                {
                    child.gameObject.GetComponent<Image>().color = Color.Lerp(child.GetComponent<TextMeshProUGUI>().color, goaltextColor, timeElapsed / 1f);
                }
                timeElapsed += Time.deltaTime;
                //wait for the next frame and continue execution the Lerp
                yield return null;
            }
            if (child.gameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                child.gameObject.GetComponent<TextMeshProUGUI>().color = goaltextColor;
            }
            if (child.gameObject.GetComponent<Image>() != null)
            {
                child.gameObject.GetComponent<Image>().color = goaltextColor;
            }
        }
    }
    IEnumerator switchText()
    {
        gameOver.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        resultAppear.SetActive(true);
        StartCoroutine(LerpResult());
    }
    IEnumerator switchText2()
    {
        StopCoroutine(LerpResult());
        resultAppear.SetActive(false);
        yield return new WaitForSeconds(1);
        option.SetActive(true);
        StartCoroutine(LerpOption());
    }
    // Update is called once per frame
    void Update()
    {
        gameOver.color = currentColor;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            rank++;
            if(rank > 3)
            {
                rank--;
            }
        }
        if(rank == 0)
        {
            StartCoroutine(LerpColor());
            //StartCoroutine(LerpColorInput(image.color,imageColor));
            StartCoroutine(LerpImage());
        }
        else if(rank == 1)
        {
            StartCoroutine(switchText());
        }
        else if (rank == 2)
        {
            StartCoroutine(switchText2());
        }
    }
}
