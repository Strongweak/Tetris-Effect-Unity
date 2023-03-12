using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum Type
{
    L,
    ReverseL,
    S,
    ReverseS,
    I,
    T,
    Square
}
public class ControlBlock : MonoBehaviour
{
    [SerializeField] Type type;
    //rotaion
    [SerializeField] Vector3 rotation_point;

    //vertical movement
    private float previousTime;
    private float fallTime = 1f;

    //horizontal movement
    private float HorizontalTime;
    float horizontalMovement = 0.05f;



    //limit movement in box (move to gamemanager)
    [SerializeField] GameManager gameManager;

    // Start is called before the first frame update

    private void Awake()
    {
        this.transform.parent = GameObject.FindGameObjectWithTag("Big Box").transform;
        gameManager = FindObjectOfType<GameManager>();
        fallTime = gameManager.falldelay;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fallTime = gameManager.falldelay;
        Hold();
        // move left right
        if (gameManager.ispause == false && Input.GetAxisRaw("Horizontal") < -0.2)
        {
            HorizontalTime += Time.deltaTime;
            if (HorizontalTime > horizontalMovement)
            {
                transform.position += new Vector3(-1, 0, 0);
                HorizontalTime = 0;
            }
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        if (gameManager.ispause == false && Input.GetAxisRaw("Horizontal") > 0.2)
        {
            HorizontalTime += Time.deltaTime;
            if (HorizontalTime > horizontalMovement)
            {
                transform.position += new Vector3(1, 0, 0);
                HorizontalTime = 0;
            }
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }


        //rotate counter-clockwise
        if (gameManager.ispause == false && Input.GetKeyDown(KeyCode.Z))
        {
            transform.RotateAround(transform.TransformPoint(rotation_point), new Vector3(0, 0, 1), 90);

            if (!ValidMove())
            {
                //reverse the action, cancel the rotation
                transform.RotateAround(transform.TransformPoint(rotation_point), new Vector3(0, 0, 1), -90);
            }

        }
        //rotate clockwise
        if (gameManager.ispause == false && Input.GetKeyDown(KeyCode.X))
        {
            transform.RotateAround(transform.TransformPoint(rotation_point), new Vector3(0, 0, 1), -90);

            if (!ValidMove())
            {
                //reverse the action, cancel the rotation
                transform.RotateAround(transform.TransformPoint(rotation_point), new Vector3(0, 0, 1), 90);

            }
        }
        //hard drop

        //if (gameManager.ispause == false && gameManager.stickDownLast == false && Input.GetAxisRaw("Vertical") > 0.2)
        //{   
        //}


        //auto falldown
        //if down arrow key is hold down, falltime is divide to 10, if not then falltime is default
        //previousTime += Time.deltaTime;
        if (gameManager.ispause == false && (Time.time - previousTime > (Input.GetAxisRaw("Vertical") < -0.5 ? fallTime / 12 : fallTime)))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                //disable control on this block when touched the ground
                gameObject.tag = "Untagged";
                gameManager.AddToGrid(this.transform);
                gameManager.CheckforLine(0);
                this.enabled = false;
            }
            //previousTime = 0;
            previousTime = Time.time;
        }

    }
    public bool ValidMove()
    {
        foreach (Transform children in transform) // check all children (bevel blocks)
        {
            //get children position
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            //if one children position is larger than box size
            if (roundedX < 0 || roundedX >= GameManager.width || roundedY < 0 || roundedY >= GameManager.height)
            {
                return false;
            }

            //if children position is overlaping exsisted block
            if (GameManager.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    //put block to hold position and get the next block
    void Hold()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.ispause == false)
        {
            //if hold position have not any child, add current block to it
            if (gameManager.holdposition.childCount == 0)
            {
                //get the current object to the hold position, turn off control and game logic
                this.transform.position = gameManager.holdposition.position;
                this.transform.parent = gameManager.holdposition;
                this.gameObject.tag = "Untagged";
                //reset rotation to 0 0 0 
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                this.enabled = false;

                FindObjectOfType<BlockSpawnner>().nextBlock();

                //prevent from spamming hold to cheat death
                gameManager.canUseHold = false;
            }
            //if hold position have any child
            else if (gameManager.holdposition.childCount > 0 && gameManager.canUseHold == true)
            {
                //get the current object to the hold position, turn off control and game logic
                this.transform.position = gameManager.holdposition.position;
                this.transform.parent = gameManager.holdposition;
                this.gameObject.tag = "Untagged";
                //reset rotation to 0 0 0
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                this.enabled = false;

                //get the first child out, move to block spawnner position
                gameManager.holdposition.GetChild(0).position = gameManager.blockSpawner.position;
                //turn on control and game logic
                gameManager.holdposition.GetChild(0).GetComponent<ControlBlock>().enabled = true;
                gameManager.holdposition.GetChild(0).tag = "Current";
                FindObjectOfType<BlockSpawnner>().spawnGhost(gameManager.holdposition.GetChild(0).gameObject);
                //unparent from hold position and become the child of box
                gameManager.holdposition.GetChild(0).parent = GameObject.FindGameObjectWithTag("Big Box").transform;


                gameManager.canUseHold = false;
            }
        }
    }
    public void Harddrop()
    {
        // Call your event function here.
        transform.position = FindObjectOfType<GhostPiece>().transform.position;
        gameObject.tag = "Untagged";
        gameManager.AddToGrid(this.transform);
        gameManager.CheckforLine(0.2f);
        this.enabled = false;
        Destroy(GameObject.FindGameObjectWithTag("Ghost"));
    }
}
