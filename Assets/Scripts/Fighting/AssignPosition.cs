using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AssignPosition : MonoBehaviourPunCallbacks {
	public GameObject player;
	public Transform position1;
	public Transform position2;
	public static GameObject player2Obj;
	public static GameObject player1Obj;
	public UpdateUI ui;
	public FightInput[] temp;

	private void Start() {
		if (PhotonNetwork.PlayerList.Length == 1) {
			player1Obj = PhotonNetwork.Instantiate(player.name, position1.position, position1.rotation);
			ui.player1 = player1Obj.GetComponent<FightInput>();
		}
		if (PhotonNetwork.PlayerList.Length == 2) {
			player2Obj = PhotonNetwork.Instantiate(player.name, position2.position, position2.rotation);
			player2Obj.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
			ui.player2 = player2Obj.GetComponent<FightInput>();
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer) {
		base.OnPlayerEnteredRoom(newPlayer);
		photonView.RPC("setPlayersForAll", RpcTarget.All);
	}

	public IEnumerator searchAllPlayer() {
		yield return new WaitForSecondsRealtime(4);
		temp = FindObjectsOfType<FightInput>();
		assignPlayer1();
		assignPlayer2();
		player2Obj.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
		player1Obj.GetComponent<FightInput>().enemy = player2Obj.GetComponent<FightInput>().model;
		player2Obj.GetComponent<FightInput>().enemy = player1Obj.GetComponent<FightInput>().model;
	}
	[PunRPC]
	public void setPlayersForAll() {
		StartCoroutine(searchAllPlayer());
	}

	public void assignPlayer1() {
		foreach (var VARIABLE in temp) {
			VARIABLE.life = VARIABLE.MAXlife;
			if (VARIABLE.gameObject != player2Obj) {
				player1Obj = VARIABLE.gameObject;
				ui.player1 = player1Obj.GetComponent<FightInput>();
			}
		}
	}

	public void assignPlayer2() {
		foreach (var VARIABLE in temp) {
			if (VARIABLE.gameObject != player1Obj) {
				Debug.Log("settign");
				player2Obj = VARIABLE.gameObject;
				ui.player2 = player2Obj.GetComponent<FightInput>();
			}
		}
	}


}