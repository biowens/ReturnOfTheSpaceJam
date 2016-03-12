using UnityEngine;
using System.Collections;

public class DirectorCamScript : MonoBehaviour {

	public float defaultCamSize;
	public float zoomInMult;
	float zoomInCamSize;

	//float prevMouseX, prevMouseY;
	public float timeTilZoomOut;
	float lastMouseMoveTime;

	public float mouseXConstraint, mouseYConstraint;
	public float camPosXConstraint, camPosYConstraint;

	public float zoomSpeed;

	Camera directorCam;

	void Start () {
		directorCam = GetComponent<Camera> ();

		directorCam.orthographicSize = defaultCamSize;
		zoomInCamSize = defaultCamSize / zoomInMult;

		/*
		prevMouseX = Input.mousePosition.x;
		prevMouseY = Input.mousePosition.y;
		*/

		lastMouseMoveTime = -10f;
	}
	
	// Update is called once per frame
	void Update () {

		/*
		// Camera zoom control
		if (Mathf.Abs(Input.mousePosition.x - prevMouseX) > 10f || Mathf.Abs(Input.mousePosition.y - prevMouseY) > 10f) {
			lastMouseMoveTime = Time.timeSinceLevelLoad;
			prevMouseX = Input.mousePosition.x;
			prevMouseY = Input.mousePosition.y;
		}
		*/

		if (Input.GetAxis ("Mouse X") > 0.1f || Input.GetAxis ("Mouse Y") > 0.1f) {
			lastMouseMoveTime = Time.timeSinceLevelLoad;
		}

		float timeSinceLastMove = Time.timeSinceLevelLoad - lastMouseMoveTime;
		print (timeSinceLastMove);

		// Zoom in if the mouse has been moved within a number of seconds
		if (timeSinceLastMove < timeTilZoomOut) {
			lerpCamSize (defaultCamSize, zoomInCamSize, zoomSpeed);
		} else {
			lerpCamSize (zoomInCamSize, defaultCamSize, zoomSpeed);
		}

		// Camera move controls
		Vector3 screenPos = directorCam.WorldToScreenPoint(directorCam.ScreenToWorldPoint(Input.mousePosition));
		float screenPosFromCenterX = (screenPos.x / directorCam.pixelWidth - 0.5f);
		float screenPosFromCenterZ = (screenPos.y / directorCam.pixelHeight - 0.5f);

		// move cam left/right
		if (screenPosFromCenterX < -mouseXConstraint || screenPosFromCenterX > mouseXConstraint) {
			// Treat camera move as mouse move
			if (transform.position.x + (1f * screenPosFromCenterX) < camPosXConstraint
			    && transform.position.x + (1f * screenPosFromCenterX) > -camPosXConstraint)
				lastMouseMoveTime = Time.timeSinceLevelLoad;

			float newXPos = Mathf.Clamp (transform.position.x + (1f * screenPosFromCenterX), -camPosXConstraint, camPosXConstraint);
			transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
		}
		// move cam up/down
		if (screenPosFromCenterZ < -mouseYConstraint || screenPosFromCenterZ > mouseYConstraint) {
			// Treat camera move as mouse move
			if (transform.position.z + (1f * screenPosFromCenterZ) < camPosYConstraint
				&& transform.position.z + (1f * screenPosFromCenterZ) > -camPosYConstraint)
				lastMouseMoveTime = Time.timeSinceLevelLoad;
			
			float newZPos = Mathf.Clamp (transform.position.z + (1f * screenPosFromCenterZ), -camPosYConstraint, camPosYConstraint);
			transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);
		}
	}

	void lerpCamSize(float origSize, float finalSize, float speed) {
		float u = Mathf.Abs(directorCam.orthographicSize - origSize) / Mathf.Abs (finalSize - origSize);
		if (u < 1) {
			u += speed;
			directorCam.orthographicSize = origSize * (1f - u) + finalSize * u;
		}
	}
}
