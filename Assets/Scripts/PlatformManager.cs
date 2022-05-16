using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlatformManager : MonoBehaviour {
	public float vel;
	public List<GameObject> platform = new List<GameObject>();

	// Start is called before the first frame update
	

	// Update is called once per frame
	void Update() {
		if (GetComponentInChildren<PhotonView>().IsMine) {
			if (Input.GetKey(KeyCode.LeftArrow)) {
				foreach (var VARIABLE in platform) {
					VARIABLE.transform.position += Vector3.left * Time.deltaTime * vel;
				}
			}
			if (Input.GetKey(KeyCode.RightArrow)) {
				foreach (var VARIABLE in platform) {
					VARIABLE.transform.position += Vector3.right * Time.deltaTime * vel;
				}
			}
			if (Input.GetKey(KeyCode.UpArrow)) {
				foreach (var VARIABLE in platform) {
					VARIABLE.transform.position += Vector3.up * Time.deltaTime * vel;
				}
			}
			if (Input.GetKey(KeyCode.DownArrow)) {
				foreach (var VARIABLE in platform) {
					VARIABLE.transform.position += Vector3.down * Time.deltaTime * vel;
				}
			}
		}
	}

}