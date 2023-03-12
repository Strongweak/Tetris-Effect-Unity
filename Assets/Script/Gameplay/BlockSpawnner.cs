using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnner : MonoBehaviour
{
    [SerializeField] GameObject[] tetrisBlock;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject ghostBlock;
    private GameObject preview;
    private GameObject current;

    private void Awake()
    {
        //spawn a block to preview position also turn off that block's game logic
        preview = Instantiate(tetrisBlock[Random.Range(0, tetrisBlock.Length - 1)], gameManager.previewPosition.position, Quaternion.identity);
        preview.GetComponent<ControlBlock>().enabled = false;
        preview.tag = "Untagged";

        //spawn a block at gameplay zone (duh)
        current = Instantiate(tetrisBlock[Random.Range(0, tetrisBlock.Length - 1)], transform.position, Quaternion.identity);
        current.tag = "Current";

        spawnGhost(current);
    }
    private void Start()
    {

    }

    public void nextBlock()
    {
        //get the preview block in the preview position to gameplay and turn on control and game logic
        preview.transform.position = transform.position;
        current = preview;
        current.GetComponent<ControlBlock>().enabled = true;
        current.tag = "Current";

        //spawn a block to preview position also turn off that block's game logic
        preview = Instantiate(tetrisBlock[Random.Range(0, tetrisBlock.Length - 1)], gameManager.previewPosition.position, Quaternion.identity);
        preview.GetComponent<ControlBlock>().enabled = false;

        spawnGhost(current);
    }

    public void spawnGhost(GameObject blockToFollow)
    {
        if(GameObject.FindGameObjectWithTag ("Ghost") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Ghost"));

        }
        ghostBlock = (GameObject)Instantiate(blockToFollow, blockToFollow.transform.position, Quaternion.identity);
        //remove component from ghost piece
        Destroy(ghostBlock.GetComponent<ControlBlock>());
        //add ghost logic to block
        ghostBlock.AddComponent<GhostPiece>();
       
    }
}
