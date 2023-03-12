using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static int height = 25;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height];

    public int level = 1;
    public int lineCleared = 0;
    public int score = 0;
    public int combo = 0;
    public float falldelay;
    [SerializeField] float oldFalldelay;

    //for zone mechannic
    public float maxZone = 32;
    public float zone = 0;
    [SerializeField] bool inZoneMode = false;

    public Transform holdposition;
    public Transform previewPosition;
    public Transform blockSpawner;

    public bool canUseHold;

    //[SerializeField] IngameUI gameUI;
    [SerializeField] Canvas gameUI;
    [SerializeField] Canvas controlGuide;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameover;

    //improve logic
    public bool ispause;
    private bool canSpawn = true;
    [SerializeField] bool isGameOver;
    public bool stickDownLast = false;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        gameover.SetActive(false);
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        canUseHold = true;
        ispause = false;
        isGameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        oldFalldelay = falldelay;
    }
    IEnumerator DeletethenWait(int i)
    {
        DeleteLine(i);
        yield return new WaitForSeconds(0.5f);
        RowDown(i);
    }

    IEnumerator SwaptoUnder(int i)
    {
        yield return new WaitForSeconds(0.3f);
        FullLineDown(i);
    }
    IEnumerator spawnNextBlock(float input)
    {
        if(input == 0)
        {
            yield return null;
            if(canSpawn)
            FindObjectOfType<BlockSpawnner>().nextBlock();
        }
        else
        {
            yield return new WaitForSeconds(input);
            if(canSpawn)
            FindObjectOfType<BlockSpawnner>().nextBlock();
        }
    }
    IEnumerator delayShowup()
    {
        yield return new WaitForSeconds(2.5f);
        controlGuide.enabled = false;
        ispause = true;
        gameover.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return) && ispause == false)
            {
                Pause();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && ispause == true)
            {
                Continue();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            //prevent spamimng harddrop from holding up
            if (Input.GetAxisRaw("Vertical") > 0.5f && ispause == false)
            {
                if (!stickDownLast)
                {
                    GameObject.FindGameObjectWithTag("Current").GetComponent<ControlBlock>().Harddrop();
                    FindObjectOfType<BoxLerp>().Lerp();
                    stickDownLast = true;
                }

            }
            else if (Input.GetAxisRaw("Vertical") <= 0 && ispause == false)
            {
                stickDownLast = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && zone >= maxZone && inZoneMode == false)
            {
                zoneMode();
            }
        }
        //for zone calculation
        if(zone > maxZone)
        {
            zone = maxZone;
        }

        GameOver();
        Difficulty();
    }
    public void AddToGrid(Transform block)
    {
        block.parent = GameObject.FindGameObjectWithTag("Big Box").transform;
        foreach (Transform children in block.transform) // check all children (bevel blocks)
        {
            //get children position
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    public void CheckforLine(float input)
    {
        //check each row in the box
        for (int i = height-1; i >= 0 ; i--)
        {
            if (HasLine(i) && inZoneMode == false)
            {
                //if can delete a line, delay the spawn next block 0.5s
                input = 0.5f; 
                StartCoroutine(DeletethenWait(i));
                lineCleared++;
                combo++;
                zone++;
            }
            if (HasLine(i) && inZoneMode == true)
            {
                input = 0.01f;
                StartCoroutine(SwaptoUnder(i));
                if(inZoneMode == false)
                {

                }
            }
        }
        StartCoroutine(spawnNextBlock(input));
        input = 0;
        //FindObjectOfType<BlockSpawnner>().nextBlock();
        gameUI.GetComponent<IngameUI>().comboShow(combo);
        if(combo == 1)
        {
            score = score + 40 * level * combo;
        }
        else if(combo == 2)
        {
            score = score + 100 * level * combo;
        }
        else if (combo == 3)
        {
            score = score + 300 * level * combo;
        }
        else if (combo == 4)
        {
            score = score + 1200 * level * combo;
        }
        combo = 0;
        canUseHold = true;
    }
    bool HasLine(int i)
    {
        //check each index in the row
        for(int j = 0; j < width; j++)
        {
            //if one of them not have an object in the row, the row can not delete
            if(grid[j,i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int row)
    {
        //for every element in row, delete all of them
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,row].gameObject);
            grid[j, row] = null;
        }

    }

    void FullLineDown(int row)
    {
        //check every line in the box
        for (int y = row; y < height; y++)
        {
            //check every element in a line
            for (int j = 0; j < width; j++)
            {
                //check if any element is not null, go down 1 height
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position += new Vector3(0, 1, 0);
                }
            }
        }
    }
    void RowDown(int row)
    {
        //check every line in the box
        for (int y = row; y < height; y++)
        {
            //check every element in a line
            for (int j = 0; j < width; j++)
            {
                //check if any element is not null, go down 1 height
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0); 
                }
            }
        }
    }
    public void Difficulty()
    {
        if (lineCleared >= 10)
        {
            level = 2;
        }
        if (lineCleared >= 20)
        {
            level = 3;
        }
        if (lineCleared >= 30)
        {
            level = 4;
        }
        if (lineCleared >= 40)
        {
            level = 5;
        }
        if (lineCleared >= 50)
        {
            level = 6;
        }
        if (lineCleared >= 60)
        {
            level = 7;
        }
        if (lineCleared >= 70)
        {
            level = 8;
        }
        if (lineCleared >= 80)
        {
            level = 9;
        }
        if (lineCleared >= 90)
        {
            level = 10;
        }
        if (lineCleared >= 100)
        {
            level = 11;
        }

        if(level == 2)
        {
            falldelay = 0.9f;
            oldFalldelay = 0.9f;
        }
        if (level == 3)
        {
            falldelay = 0.8f;
            oldFalldelay = 0.8f;
        }
        if (level == 4)
        {
            falldelay = 0.7f;
            oldFalldelay = 0.7f;
        }
        if (level == 5)
        {
            falldelay = 0.6f;
            oldFalldelay = 0.6f;
        }
        if (level == 6)
        {
            falldelay = 0.5f;
            oldFalldelay = 0.5f;
        }
        if (level == 7)
        {
            falldelay = 0.4f;
            oldFalldelay = 0.4f;
        }
        if (level == 8)
        {
            falldelay = 0.3f;
            oldFalldelay = 0.3f;
        }
        if (level == 9)
        {
            falldelay = 0.2f;
            oldFalldelay = 0.2f;
        }
        if (level == 10)
        {
            falldelay = 0.1f;
            oldFalldelay = 0.1f;
        }
        if (level == 11)
        {
            falldelay = 0.05f;
            oldFalldelay = 0.05f;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0.00001f;
        gameUI.enabled = false;
        controlGuide.enabled = false;
        pauseMenu.SetActive(true);
        ispause = true;
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        gameUI.enabled = true;
        controlGuide.enabled = true;
        pauseMenu.SetActive(false);
        ispause = false;
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    void GameOver()
    {
        if (grid[4, 23] != null)
        {
            canSpawn = false;
            Destroy(GameObject.FindGameObjectWithTag("Ghost"));
            Destroy(GameObject.FindGameObjectWithTag("Current"));
            //for every element in row
            for (int j = 0; j < width; j++)
            {
                //for each element of column
                for (int k = 0; k < height; k++)
                {
                    if(grid[j, k] != null)
                    {
                        // unparent all of them
                        grid[j, k].gameObject.transform.parent = null;
                        grid[j, k].gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
                        grid[j, k].gameObject.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
                        //random launch direction and rotate with random force
                        grid[j, k] = null;
                    }
                }

            }
            isGameOver = true;
            StartCoroutine(delayShowup());
            Debug.Log("Game Over!");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    //IEnumerator endZone()
    //{
    //    inZoneMode = true;
    //    falldelay = 999;
    //    zone = 0;
    //    yield return new WaitForSeconds(10);
    //    falldelay = oldFalldelay;
    //    inZoneMode = false;
    //}

    IEnumerator endZone()
    {
        float timeElapsed = 0;
        while (timeElapsed < 10)
        {
            inZoneMode = true;
            falldelay = 999;
            zone = Mathf.Lerp(zone, 0, timeElapsed / 1f);
            timeElapsed += Time.deltaTime;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        zone = 0;
        falldelay = oldFalldelay;
        inZoneMode = false;
    }
    public void zoneMode()
    {
        StartCoroutine(endZone());
    }
}
