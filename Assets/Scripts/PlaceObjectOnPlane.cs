using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager raycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject gameOjbect;

    private List<GameObject> placedObjects = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                bool isOverUI = touch.position.isPointOverUIObject();


                if (placedObjects.Count < 3 && !isOverUI && raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    var rotation = new Quaternion(hitPose.rotation.x, hitPose.rotation.y + 90, hitPose.rotation.z, hitPose.rotation.w);
                    var obj = Instantiate(gameOjbect, hitPose.position, rotation);
                    placedObjects.Add(obj);
                    FindObjectOfType<AudioManager>().Play("Music");
                }
            }
        }
    }

    public void removeObject()
    {
        foreach(GameObject plObj in placedObjects)
        {
            Destroy(plObj);
        }
        placedObjects = new List<GameObject>();
        FindObjectOfType<AudioManager>().Stop("Music");
    }

    public void changeSpeed(float value)
    {
        foreach (GameObject plObj in placedObjects)
        {
            plObj.GetComponentInChildren<Animator>().speed = value;
        }
    }
}
