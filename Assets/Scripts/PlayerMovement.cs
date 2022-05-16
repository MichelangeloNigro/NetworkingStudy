using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float vel;
	public float jumpVel;
	public Rigidbody rb;
	public bool isGrounded;
	private RaycastHit hit;
	public float maxJumpCheck;

	// Start is called before the first frame update
	void Start() {
		if (!GetComponent<PhotonView>().IsMine) {
			Destroy(rb);
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update() {
		if (Physics.Raycast(transform.position, -transform.up, out hit, maxJumpCheck)) {
			if (hit.collider.gameObject.CompareTag("Platform")) {
				isGrounded = true;
			}
			else {
				isGrounded = false;
			}
		}
		else {
			isGrounded = false;
		}
		Debug.DrawRay(transform.position, -transform.up * maxJumpCheck, Color.red);
		if (Input.GetKey(KeyCode.D)) {
			rb.position += Vector3.right * Time.deltaTime * vel;
		}
		if (Input.GetKey(KeyCode.A)) {
			rb.position += Vector3.left * Time.deltaTime * vel;
		}
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			rb.AddForce(Vector3.up * Time.deltaTime * jumpVel, ForceMode.Impulse);
		}
		// if (Input.GetKey(KeyCode.D)) {
		//     transform.position += Vector3.right * Time.deltaTime * vel;
		// }
	}
}