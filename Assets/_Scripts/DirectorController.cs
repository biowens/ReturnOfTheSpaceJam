using UnityEngine;
using System.Collections;

public class DirectorController : MonoBehaviour {

	public static DirectorController S;

	public enum layerMaskName {ground = 0, wall, directorObj, size};
	public LayerMask[] raycastMask;

	public GameObject[] placeables;
	LayerMask finalMask;

	public Material ghostPlace;
	public Material ghostError;

	bool hasGhostObj;
	bool isPlaceable;

	// For demoing where to place object
	GameObject ghostObj;
	GameObject delObj;

	// Use this for initialization
	void Start () {
		hasGhostObj = false;

		setFinalMask (true, false, false);

		S = this;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit mouseRayHit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		setFinalMask (true, false, false);

		// If player left clicks, place objects
		if (Input.GetMouseButtonDown(0) 
			&& Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
			ghostObj = Instantiate<GameObject> (placeables [0]);
			ghostObj.GetComponent<Renderer> ().material = ghostPlace;
			ghostObj.transform.position = mouseRayHit.point + new Vector3 (0,0.2f + ghostObj.transform.localScale.y / 2f, 0);
			hasGhostObj = true;
		}
		if (Input.GetMouseButton (0) && hasGhostObj) {
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
				if (isPlaceable) {
					ghostObj.GetComponent<Renderer> ().material = ghostPlace;
				} else {
					ghostObj.GetComponent<Renderer> ().material = ghostError;
				}
				ghostObj.transform.position = new Vector3 (mouseRayHit.point.x, ghostObj.transform.position.y, mouseRayHit.point.z);
			}
		}
		if (Input.GetMouseButtonUp (0) && hasGhostObj) {
			if (!isPlaceable) {
				DestroyObject (ghostObj);
			} else {
				ghostObj.GetComponent<CollisionAlert> ().resetMat ();
			}
			hasGhostObj = false;
		}

		// If player right clicks, remove objects
		setFinalMask(false, false, true);

		if (Input.GetMouseButton (1)) {
			if (Physics.Raycast (mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
				delObj = mouseRayHit.collider.gameObject;
				delObj.GetComponent<Renderer> ().material = ghostError;
			} else {
				// I dont think this is how try and catch are suppose to be used lol
				try { delObj.GetComponent<CollisionAlert> ().resetMat (); }
				catch {}
			}
		}
		if (Input.GetMouseButtonUp (1)) {
			if (Physics.Raycast (mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
				if (delObj == mouseRayHit.collider.gameObject) {
					DestroyObject (delObj);
				}
			}
		}
	}
		
	void setFinalMask (bool ground, bool wall, bool directorObj) {
		finalMask = 0;
		if (ground) {
			finalMask = finalMask | raycastMask [(int)layerMaskName.ground];
		}
		if (wall) {
			finalMask = finalMask | raycastMask [(int)layerMaskName.wall];
		}
		if (directorObj) {
			finalMask = finalMask | raycastMask [(int)layerMaskName.directorObj];
		}
	}

	public void setIsPlaceable (bool newIsPlaceable) {
		isPlaceable = newIsPlaceable;
	}
}
