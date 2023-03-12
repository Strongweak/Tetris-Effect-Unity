using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class MainMenuGameManager : MonoBehaviour
{

    [SerializeField] GameObject mainmenu;
    [SerializeField] GameObject areUSure;

    [SerializeField] GameObject setting;

    [SerializeField] Image image;
    [SerializeField] Color darkColor;
    private Color defaultColor;

    [SerializeField] bool inSetting;
    [SerializeField] bool areyousure;

    private void Start()
    {
        setting.SetActive(false);
        areUSure.SetActive(false);

    }
    private void Update()
    {
        if (inSetting == true || areyousure == true)
        {
            image.color = Color.Lerp(image.color, darkColor, Time.deltaTime * 2);
        }
        else if (inSetting == false || areyousure == false)
        {
            image.color = Color.Lerp(image.color, defaultColor, Time.deltaTime * 2);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Setting()
    {
        setting.SetActive(true);
        mainmenu.SetActive(false);
        inSetting = true;
    }
    public void Back()
    {
        setting.SetActive(false);
        mainmenu.SetActive(true);
        inSetting = false;
    }


    //for quitting
    public void Quit()
    {
        mainmenu.SetActive(false);
        areUSure.SetActive(true);
        areyousure = true;
    }

    public void Yes()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void No()
    {
        mainmenu.SetActive(true);
        areUSure.SetActive(false);
        areyousure = false;
    }


}
