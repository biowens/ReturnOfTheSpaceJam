using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;




public class Player : NetworkBehaviour {
    public float directorSpeed;
    public List<NetworkBehaviour> FPSBehaviors;
    public List<NetworkBehaviour> DirectorBehaviors;


    void Start () {
        if (isServer)
        {
            for (int index = 0; index < FPSBehaviors.Count; index++)
                DirectorBehaviors[index].enabled = true;
            this.GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            for (int index = 0; index < FPSBehaviors.Count; index++)
                FPSBehaviors[index].enabled = true;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
        
	}
	
	void Update ()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * directorSpeed * Time.deltaTime;
    }
}
