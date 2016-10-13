using UnityEngine;
using System.Collections;



//For whatever Object this script is a component of make sure you require that Object
[RequireComponent(typeof(SteamVR_TrackedObject))]

public class PickUpParent : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public Transform sphere;
    [Space(5)]

    [Header("Haptic Values")]
    public int pulseCount;
    [Range(0.0f, 2.0f)]
    public float pulseLength;
    [Range(0.0f, 2.0f)]
    public float pulseBreakLength;
    

    // Use this for initialization
    void Awake () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();
	
	}
	
	//Update is called at a fixed rate 
	void FixedUpdate () {

        device = SteamVR_Controller.Input((int)trackedObj.index);
        
        //reset the ball's location 
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {

            //set position, velocity, and angularVelocity to zero back to original location and movement. 
            sphere.transform.position =  Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }

	}

    void OnTriggerEnter(Collider col)
    {

        StartCoroutine(HapticSinglePulse(0.2f));

    }
    

    //run separate from frame by frame.  If put all this code in OnTriggerEnter, then it will only run once and time period will not be respected.  
    IEnumerator HapticSinglePulse(float length)
    {

        for (float i = 0; i < length; i += Time.deltaTime)
        {

            device.TriggerHapticPulse();
            yield return null;

        }

    }

    IEnumerator HapticMultiplePulse(int pulseCount, float pulseLength, float pulseBreakLength)
    {

        for (int i = 0; i < pulseCount; i++)
        {
            //when going into alot of different threads and multiprocessing, think of each Coroutine as a "unit of work"
            yield return StartCoroutine(HapticSinglePulse(pulseLength));
            yield return new WaitForSeconds(pulseBreakLength);

        }

    }
    
   
    void OnTriggerStay(Collider col){


        Debug.Log(col.name);

        //set collided object's transform parent to the controller 
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {

            //String comes first in order to avoid null exception in col.gameObject.Tag, "train" in which col, Gameobject, or tag can be null.  
            //actually not best practice to define interactability in a controller script, not logical, used for demonstration.  Best practice would be to encapsulate behavior in item being picked up.  
            if ("Interactable".Equals(col.gameObject.tag))
            {
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(this.gameObject.transform);
            }

        }

        if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger))
        {

            Debug.Log("touched up!");
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;
            
            

        }

        ObjectThrow(col.attachedRigidbody);

    }



    void ObjectThrow(Rigidbody rigidbody)
    {

        //if trackedObj.origin true evaluate to origin else trackedObj.transform.parent
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;

        //Make sure that origin is not null 
        if(origin != null)
        {
            //Transform device.velocity and device.angularVelocity from local space to (in-game) world space from the origin. 
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);

        }
        else
        {

            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;

        }

        
        

    }

   
}
