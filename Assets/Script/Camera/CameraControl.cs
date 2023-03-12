using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //zooming
    [SerializeField] float zoomScale;
    [SerializeField] float distance;
    private float oldassDistance;
    [SerializeField] float minDis = 10f, maxDis = 30f;

    //sentivity, mouse input
    public float senX, senY;
    private float mouseX, mouseY;

    //all for rotation
    [SerializeField] Transform pivot;
    [SerializeField] float maxDegreeY = 30f;
    [SerializeField] float maxDegreeX = 40f;
    private float roteX = 0f;
    private float roteY = 0f;
    private float valueToLerp;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        //current angles of camera (make sure it's looking at what you want
        //to rotate around)
        roteY = transform.eulerAngles.y;
        roteX = transform.eulerAngles.x;

        Vector3 initialVector = transform.position - pivot.position;
        distance = Vector3.Magnitude(initialVector);
        oldassDistance = distance;
    }

    //Linear interpolation
    IEnumerator Lerp(float startValueX, float endValueX , float startValueY , float endValueY, float currentDis, float defultDis)
    {
        float timeElapsed = 0;
        while (timeElapsed < 0.25f)
        {
            roteX = Mathf.Lerp(startValueX, endValueX, timeElapsed / 0.15f);
            roteY = Mathf.Lerp(startValueY, endValueY, timeElapsed / 0.15f);
            distance = Mathf.Lerp(currentDis, defultDis, timeElapsed / 0.15f);
            timeElapsed += Time.deltaTime;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        roteX = endValueX;
        roteY = endValueY;
        distance = defultDis;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(pivot);   
        //zoom camera with mouse scrollwheel
        if (gameManager.ispause == false && Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            distance -= zoomScale;
        }
        else if (gameManager.ispause == false && Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            distance += zoomScale;
        }
        if (distance > maxDis)
        {
            distance = maxDis;
        }
        else if (distance < minDis)
        {
            distance = minDis;
        }
        //move camera
        if (gameManager.ispause == false && Input.GetMouseButton(0))
        {
            mouseInput();
            //transform.RotateAround(pivot.transform.position, Vector3.up, roteX);
        }
        //reset camera position, zooming distance
        if (gameManager.ispause == false && Input.GetMouseButtonDown(1))
        {
            //roteX = 0f;
            //roteY = 0f;
            //distance = oldassDistance;

            StartCoroutine(Lerp(roteX,0, roteY, 0, distance, oldassDistance));
            //transform.RotateAround(pivot.transform.position, Vector3.left, mouseY);
        }
    }

    private void LateUpdate()
    {
        //transform.RotateAround(pivot.transform.position, Vector3.up, roteX);
        //and apply!
        // convert it to quaternions
        Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        Quaternion toRotation = Quaternion.Euler(roteX, roteY, 0);
        Quaternion rotation = toRotation;

        //figure out what your distance should be (so that it's rotating around 
        //not just rotating)
        Vector3 negDistance = new Vector3(0, 0, -distance);
        Vector3 position = rotation * negDistance + pivot.position;

        //and apply!
        transform.rotation = rotation;
        transform.position = position;
    }

    void mouseInput()
    {
        //get mouse input
        mouseX = Input.GetAxisRaw("Mouse X") * senX;
        mouseY = Input.GetAxisRaw("Mouse Y") * senY;

        roteX -= mouseY;
        roteY += mouseX;

        //clamp the angle
        roteX = ClampAngle(roteX, -maxDegreeY, maxDegreeY);
        roteY = ClampAngle(roteY, -maxDegreeX, maxDegreeX);

    }

    //clamp angle from before
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
