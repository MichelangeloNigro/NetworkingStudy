using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FightInput : MonoBehaviourPunCallbacks {
	public Action OnDamageTaken;
	public int life;
	public int MAXlife;
	public int damage;
	public float jumpForce;
	public Collider2D fist;
	public Collider2D shield;
	public Collider2D own;
	public List<Collider2D> hit;
	public Animator animator;
	public KeyCode punchHI;
	public KeyCode punchLow;
	public KeyCode DefenseHi;
	public KeyCode DefenseLow;
	public KeyCode Jump;
	public GameObject model;
	public GameObject enemy;
	public float vel;
	public PhotonView photonView;
	public bool canJump;

	private void Start() {
		if (this.gameObject==AssignPosition.player1Obj) {
			enemy = AssignPosition.player2Obj;
		}
		else {
			enemy = AssignPosition.player1Obj;
		}
	}

	private void Update() {
		if (photonView.IsMine) {
			if (Input.GetKeyDown(punchHI)) {
				animator.SetTrigger("AttackHi");
			}
			if (Input.GetKeyDown(punchLow)) {
				animator.SetTrigger("AttackLow");
			}
			if (Input.GetKeyDown(DefenseHi)) {
				animator.SetTrigger("ShieldHi");
			}
			if (Input.GetKeyDown(DefenseLow)) {
				animator.SetTrigger("ShieldLow");
			}
			if (Input.GetKeyDown(Jump)&& Physics2D.OverlapCircle(new Vector2(model.transform.position.x,model.transform.position.y-own.bounds.extents.y),0.5f,LayerMask.GetMask("Water")) ) {
				animator.SetTrigger("Jump");
				GetComponentInChildren<Rigidbody2D>().AddForce(Vector2.up*Time.deltaTime*jumpForce,ForceMode2D.Impulse);
			}
			Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
			model.transform.position += horizontal * Time.deltaTime * vel;
			animator.SetFloat("Blend", Mathf.Abs(Input.GetAxis("Horizontal")));
		}
		if (enemy!=null) {
			if (model.transform.position.x < enemy.transform.position.x && transform.localRotation.eulerAngles.y != 180) {
				Debug.Log("rotate");
				transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
			}
			if (model.transform.position.x > enemy.transform.position.x && transform.localRotation.eulerAngles.y != 0) {
				Debug.Log("rotate");
				transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
			}
		}
		
	}

	

	public void CheckHit() {
		hit.Clear();
		hit = Physics2D.OverlapCircleAll(fist.gameObject.transform.position, 0.6f).ToList();
		foreach (var vaCollider2D in hit) {
			if (vaCollider2D.CompareTag("Shield")) {
				Debug.Log("defense");
				return;
			}
		}
		foreach (var vaCollider2D in hit) {
			if (vaCollider2D.CompareTag("Player")) {
				if (vaCollider2D != own) {
					vaCollider2D.GetComponentInParent<FightInput>().takeDamege(damage);
				}
			}
		}
	}

	public void takeDamege(int dmg) {
		life -= dmg;
		Hashtable setHpValue = new Hashtable();
		setHpValue.Add($"{photonView.ViewID}Player", life);
		PhotonNetwork.LocalPlayer.SetCustomProperties(setHpValue);
		if (OnDamageTaken != null) {
			OnDamageTaken();
		}
	}

	public override void OnJoinedRoom() {
		base.OnJoinedRoom();
		foreach (var VARIABLE in FindObjectsOfType<FightInput>()) {
			if (VARIABLE!=this) {
				enemy = VARIABLE.gameObject;
			}
		}
	}

	

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(fist.gameObject.transform.position, 0.6f);
	}
}