using UnityEngine;
using System.Collections;

public class CollisionAlert : MonoBehaviour {
	void Awake() {
		DirectorController.S.setIsPlaceable (true);
	}

	void OnCollisionStay(Collision coll) {
		DirectorController.S.setIsPlaceable (false);
	}

	void OnCollisionExit(Collision coll) {
		DirectorController.S.setIsPlaceable (true);
	}
}