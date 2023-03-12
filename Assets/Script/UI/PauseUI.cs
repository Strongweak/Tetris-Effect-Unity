//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    private Animator _animator;

    private GameManager gameManager;

    //for choosing choice
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.ispause == true)
        {
            _animator.SetBool("isPause", true);
        }
        else
        {
            _animator.SetBool("isPause", false);
        }
        pauseControl();
    }
    void pauseControl()
    {
        //check if user pressing up or down, joystick up or down 
        if (Input.GetAxis("Vertical") != 0)

        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0 || Input.GetKey(KeyCode.DownArrow))
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                    //add sound effect in here

                }
                else if (Input.GetAxis("Vertical") > 0 || Input.GetKey(KeyCode.UpArrow))
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                    //add sound effect in here

                }
                keyDown = true;

            }
        }
        else
        {
            keyDown = false;
        }
    }

}
