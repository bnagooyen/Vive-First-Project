using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour {

    //Gameobject that is hit
    //RaycastHit is the information of the hit object
    GameObject hitObject; 
    RaycastHit hit;
    public Reticle reticle;


	void Update () {
	
        //params of Physics.Raycast(starting Raycast Position, Max distance, 
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)){


            reticle.SetPosition(hit);
            //access the hitObject through the information of the actual hit.  
            hitObject = hit.transform.gameObject;
            Interactability interactable = hitObject.GetComponent<Interactability>();
            
            if (interactable != null)
            {
                interactable.isTargeted = true;
            }

        } else
          {
            reticle.SetPosition();
          }


	}
}
