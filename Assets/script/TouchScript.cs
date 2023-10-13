using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TouchScript : MonoBehaviour
{
    /// <summary>
    /// The last loaded GameObject.
    /// </summary>
    public Transform modeltarget;
    private Transform target;
    bool isselected = false;

    //scaling variables
    private float initialScale;
    private float initialDistance;
    private float targetScale;
    private float scale;
    const float speedZoom = 0.001f;
    float maxScale = 1.0f;
    float minScale = 0.25f;
    const float dampingZoom = 10.0f;
    private Vector3 DefaultScale;

    //rotation
    bool isrotating = false;

    /// <summary>
    /// Called when Touchscript is enabled 
    /// set target of the scale to 1
    /// Getting reference of the Gamobject which needs to scaled(Modeltarget)
    /// </summary>
    void Start()
    {
        targetScale = 1;
        DefaultScale = modeltarget.transform.localScale;
    }

    /// <summary>
    ///  called per frame, when touch count is greater than one, 
    ///  Raycast on the screen to find gameobject with tag"ARObj" and sets it as a target to scale and rotate
    ///  when touch count is 2 and both phase moved scalling/rotation is done based on user finger distance
    /// </summary>
    void Update()
    {
        
        if (Input.touchCount > 1)
        {
            //Raycast on the screen
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Raycst hit");
                //if hit object has a tag
                if (hit.collider.gameObject.tag == "ArObj")
                {
                    target = hit.transform;//Assigning hit gameobject to target for scale/rotate
                    isselected = true;//target is selected
                }
            }
            //if two finger is on the screen,moved and the target gameobject is selected
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved && target != null && isselected)
            {
                //gets touch of each finger
                UnityEngine.Touch t0 = Input.GetTouch(0);
                UnityEngine.Touch t1 = Input.GetTouch(1);

                //gets touch deltaposition
                Vector2 dt0 = t0.deltaPosition;
                Vector2 dt1 = t1.deltaPosition;

                //gets finger position on the screen
                Vector3 f0 = new Vector3(t0.deltaPosition.x, t0.deltaPosition.y, 0);
                Vector3 f1 = new Vector3(t1.deltaPosition.x, t1.deltaPosition.y, 0);

                //find finger moves in same or different direction
                float dotProduct = Vector2.Dot(dt0.normalized, dt1.normalized);

                //if dot product is greater than -0.7f scalling is done
                if (dotProduct < -0.7f)
                {
                    initialDistance = Vector3.Distance((t0.position - dt0), (t1.position - dt1));
                    initialScale = targetScale;
                    float curDistance = Vector3.Distance(t0.position, t1.position);
                    {
                        targetScale = Mathf.Clamp(initialScale + ((curDistance - initialDistance) * speedZoom), minScale, maxScale);
                        Debug.Log("tar scale" + targetScale);
                    }

                }

                //if dot product is greater than 0.7f rotation is done
                if (dotProduct > 0.7)
                {
                    if (f0.magnitude > 2 || f1.magnitude > 2)
                    {
                        if (t0.deltaPosition.x > 0 && t1.deltaPosition.x > 0)
                        {

                            Rotate3DObject(t0, t1);//Calling Rotate3DObject
                            isrotating = true;
                        }
                        else if (t0.deltaPosition.x < 0 && t1.deltaPosition.x < 0)
                        {
                            Rotate3DObject(t0, t1);//Calling Rotate3DObject
                            isrotating = true;
                        }
                    }
                }
                //scale target 
                scale = Mathf.Lerp(scale, targetScale, dampingZoom * Time.deltaTime);
                target.transform.localScale = scale * DefaultScale;
            }
            //if touch is ended 
            if (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(1).phase == TouchPhase.Ended)
            {

                isrotating = false;
                isselected = false;
                target=null;

            }
        }
    }

    /// <summary>
    ///  called to rotate around y axis
    /// </summary>
    void Rotate3DObject(UnityEngine.Touch touch1, UnityEngine.Touch touch2)
    {
        if (target != null)
        {
            target.RotateAround(target.position, Vector3.up, -touch1.deltaPosition.x / 2);
        }
    }
}
