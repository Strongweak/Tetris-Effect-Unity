using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    [SerializeField] MainScreenUI controller;
    [SerializeField] int thisIndex;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] string des;

    [SerializeField] float delayActive;

    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }
    IEnumerator delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        button.onClick.Invoke();
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
                StartCoroutine(delay(delayActive));
            }
        }
        else
        {
            image.enabled = false;
        }
    }
}
