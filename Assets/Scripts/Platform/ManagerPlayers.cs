using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ManagerPlayers : MonoBehaviour {
	public GameObject player;
	public GameObject platforms;
	public Transform transform1;
	public Transform transform2;

	private void Start() {
		if (PhotonNetwork.PlayerList.Length == 1) {
			PhotonNetwork.Instantiate(player.name, transform1.position, transform1.rotation);
		}
		if (PhotonNetwork.PlayerList.Length == 2) {
			PhotonNetwork.Instantiate(platforms.name, transform2.position, transform2.rotation);
		}
	}
}