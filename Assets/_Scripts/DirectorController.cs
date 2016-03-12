using UnityEngine;
using System.Collections;

public class DirectorController : MonoBehaviour {

	public GameObject[] placeables;

	bool hasGhostObj;

	// For demoing where to place object
	GameObject ghostObj;

	// Use this for initialization
	void Start () {
		hasGhostObj = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		// If player clicks
		RaycastHit mouseRayHit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0) && Physics.Raycast(mouseRay, out mouseRayHit)) {
			ghostObj = Instantiate<GameObject> (placeables [0]);
			ghostObj.transform.position = mouseRayHit.point + new Vector3 (0, ghostObj.transform.localScale.y / 2f, 0);
			hasGhostObj = true;
		}
		if (Input.GetMouseButton (0) && hasGhostObj) {

		}
		if (Input.GetMouseButtonUp (0) && hasGhostObj) {

			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.position = mouseRayHit.point;

			hasGhostObj = false;
		}
	}
}
