using UnityEngine;
using System.Collections;

public class Interactability : MonoBehaviour {

    private Color startColor;
    public bool isTargeted = false;
    private Material material; 

	void Start () {

        material = GetComponent<Renderer>().material;
        startColor = material.color;

	}
	
	// Update is called once per frame
	void Update () {
	
        if (isTargeted)
        {
            Target();
            isTargeted = false;

        } else
        {
            UnTarget();
            
        }

	}


    public void Target()
    {
        material.color = Color.red;
    }

    public void UnTarget()
    {

        material.color = startColor;
    }
}
