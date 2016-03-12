using UnityEngine;
using System.Collections;

public class DirectorCamScript : MonoBehaviour {

	public float defaultCamSize;
	public float zoomInMult;
	float zoomInCamSize;

	float prevMouseX, prevMouseY;
	float timeTilZoomOut;
	float lastMouseMoveTime;

	Camera directorCam;

	void Start () {
		directorCam = GetComponent<Camera> ();

		directorCam.orthographicSize = defaultCamSize;
		zoomInCamSize = defaultCamSize / zoomInMult;

		prevMouseX = Input.GetAxis ("Mouse X");
		prevMouseY = Input.GetAxis ("Mouse Y");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Mouse X") != prevMouseX || Input.GetAxis ("Mouse Y") != prevMouseY) {
			lastMouseMoveTime = Time.timeSinceLevelLoad;
			prevMouseX = Input.GetAxis ("Mouse X");
			prevMouseY = Input.GetAxis ("Mouse Y");
		}


			
		float timeSinceLastMove = Time.timeSinceLevelLoad - lastMouseMoveTime;

		if (timeSinceLastMove < 1f) {
			lerpCamSize (defaultCamSize, zoomInCamSize, 0.01f);
		} else {
			lerpCamSize (zoomInCamSize, defaultCamSize, 0.01f);
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
