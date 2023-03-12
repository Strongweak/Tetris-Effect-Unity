using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLerp : MonoBehaviour
{
    [SerializeField] Vector3 oldPos;
    [SerializeField] float translationMultiply = 1f;
    [SerializeField] AnimationCurve bounce;
    [SerializeField] AnimationCurve returnToDefault;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, oldPos, returnToDefault.Evaluate(Time.deltaTime * 2));
    }
    IEnumerator LerpTransition()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1f)
        {
            //transform.position = new Vector3(oldPos.x,oldPos.y * timeElapsed * Mathf.PI, oldPos.z);
            transform.position = Vector3.Slerp(this.transform.position, new Vector3(this.transform.position.x, -1.5f, this.transform.position.z), bounce.Evaluate(timeElapsed));
            timeElapsed += Time.deltaTime * translationMultiply;
            //wait for the next frame and continue execution the Lerp
            yield return null;
        }
        //transform.position = new Vector3(this.transform.position.x, -1.5f, this.transform.position.z);

        
    }

    public void Lerp()
    {
        //transform.position = new Vector3(this.transform.position.x, -1.5f, this.transform.position.z);
        StartCoroutine(LerpTransition());
    }
}
