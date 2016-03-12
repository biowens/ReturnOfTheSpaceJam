﻿using UnityEngine;
using System.Collections;

public class DirectorController : MonoBehaviour {

	public enum layerMaskName {ground = 0, wall, size};

	public GameObject[] placeables;

	public LayerMask[] raycastMask;
	LayerMask finalMask;

	bool hasGhostObj;
	bool isPlaceable;

	// For demoing where to place object
	GameObject ghostObj;

	// Use this for initialization
	void Start () {
		hasGhostObj = false;

		setFinalMask (true, false);
	}
	
	// Update is called once per frame
	void Update () {
		// If player clicks
		RaycastHit mouseRayHit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0) 
			&& Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
			ghostObj = Instantiate<GameObject> (placeables [0]);
			ghostObj.transform.position = mouseRayHit.point + new Vector3 (0, ghostObj.transform.localScale.y / 2f, 0);
			hasGhostObj = true;
		}
		if (Input.GetMouseButton (0) && hasGhostObj) {
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out mouseRayHit, Mathf.Infinity, finalMask)) {
				ghostObj.transform.position = new Vector3(mouseRayHit.point.x, ghostObj.transform.position.y, mouseRayHit.point.z);
			}
		}
		if (Input.GetMouseButtonUp (0) && hasGhostObj) {
			hasGhostObj = false;
		}
	}

	void setFinalMask (bool ground, bool wall) {
		finalMask = 0;
		if (ground) {
			finalMask = finalMask | raycastMask [(int)layerMaskName.ground];
		}
		if (wall) {
			finalMask = finalMask | raycastMask [(int)layerMaskName.wall];
		}
	}
}
