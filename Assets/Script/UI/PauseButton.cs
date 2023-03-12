using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] PauseUI controller;
    [SerializeField] int thisIndex;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] string des;

    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.index == thisIndex)
        {
            image.enabled = true;
            description.text = des;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //do something
                button.onClick.Invoke();
            }
        }
        else
        {
            image.enabled = false;
        }
    }
}
