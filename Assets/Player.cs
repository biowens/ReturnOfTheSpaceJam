using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public float directorSpeed;

	void Start () {
	
	}
	
	void Update ()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * directorSpeed * Time.deltaTime;
    }
}
