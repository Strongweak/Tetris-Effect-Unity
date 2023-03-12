using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    //[SerializeField] GameObject trackingBlock;

    private GameObject daddy;

    private void Awake()
    {
        transform.parent = GameObject.FindGameObjectWithTag("Big Box").transform;
        gameObject.tag = "Ghost";
        daddy = GameObject.FindGameObjectWithTag("Current");
        //for each child in tetris block
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = daddy.transform.position /*- new Vector3(0,GameManager.height,0)*/;
        transform.rotation = daddy.transform.rotation;
        //Movedown();
        FollowCurrentTetromino();
    }

    //get position of current tetris block
    void FollowCurrentTetromino()
    {
        while (CheckIsValidPosition())
        {
            transform.position += new Vector3(0, -1f, 0);
        }
        //if it can not move down, move up
        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(0, 1f, 0);
        }
    }

    bool CheckIsValidPosition()
    {
        if (!InsideBox())
        {
            return false;
        }
        if (InsideBox() && daddy.GetComponentInParent<ControlBlock>().ValidMove())
        {
            return true;
        }
        //if (InsideBox() && daddy.transform != transform.parent)
        //{
        //    return false;
        //}
        return true;
    }
    public bool InsideBox()
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
            if (GameManager.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool checkGrid()
    {
        foreach (Transform children in transform) // check all children (bevel blocks)
        {
            //get children position
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            //if children position is overlaping exsisted block
            if (GameManager.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
}
