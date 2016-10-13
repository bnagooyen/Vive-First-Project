using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SteamVR_TrackedObject))]

public class ParentFixedJoint : MonoBehaviour
{

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    FixedJoint fixedJoint;


    public Transform sphere;
    public Rigidbody rigidBodyAttachPoint;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void FixedUpdate()
    {

        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {

            //set position, velocity, and angularVelocity to zero back to original location and movement. 
            sphere.transform.position = Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }
    }

    void OnTriggerStay(Collider col)
    {

        //fixedJoint is component is added, rigidBodyAttachpPoint is set outside
        if (fixedJoint == null && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {

            fixedJoint = col.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidBodyAttachPoint;

        } // 
        else if (fixedJoint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject go = fixedJoint.gameObject;
            Rigidbody rigidBody = go.GetComponent<Rigidbody>(); //rigidBody component to use when you
            Object.Destroy(fixedJoint);
            fixedJoint = null;
            ObjectThrow(rigidBody);



        }

    }

    void ObjectThrow(Rigidbody rigidbody)
    {

        //if trackedObj.origin true evaluate to origin else trackedObj.transform.parent
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;

        //Make sure that origin is not null 
        if (origin != null)
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