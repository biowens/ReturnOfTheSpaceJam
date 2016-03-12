using UnityEngine;
using System.Collections;

public class CollisionAlert : MonoBehaviour {
	Material origMaterial;

	void Awake() {
		origMaterial = GetComponent<Renderer> ().material;
		DirectorController.S.setIsPlaceable (true);

	}

	void OnCollisionStay(Collision coll) {
		DirectorController.S.setIsPlaceable (false);
	}

	void OnCollisionExit(Collision coll) {
		DirectorController.S.setIsPlaceable (true);
	}

	public void resetMat() {
		GetComponent<Renderer> ().material = origMaterial;
	}
}