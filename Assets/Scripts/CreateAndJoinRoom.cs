using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks {
	[SerializeField] TMP_InputField _createInput = null;
	[SerializeField] TMP_InputField _joinInput = null;
	[SerializeField] string _loadedLevelName = "Gameplay";
	public dictionaryroom dictionaryroom;
	public GameObject buttonPrefab;
	public GameObject content;
	private RoomOptions options = new RoomOptions();

	private void Start() {
		Debug.Log(PhotonNetwork.CountOfRooms);
		options.MaxPlayers = 2;
	}

	public void CreateRoom() {
		PhotonNetwork.CreateRoom(_createInput.text, options); //And Join
	}

	public void JoinRoom() {
		PhotonNetwork.JoinRoom(_joinInput.text);
	}

	public override void OnJoinedRoom() {
		PhotonNetwork.LoadLevel(_loadedLevelName);
	}

	public override void OnJoinedLobby() {
		dictionaryroom.cachedRoomList.Clear();
	}

	public override void OnLeftLobby() {
		dictionaryroom.cachedRoomList.Clear();
	}

	public override void OnDisconnected(DisconnectCause cause) {
		dictionaryroom.cachedRoomList.Clear();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList) {
		content.transform.Clear();
		base.OnRoomListUpdate(roomList);
		dictionaryroom.cachedRoomList.Clear();
		foreach (var room in roomList) {
			if (!dictionaryroom.cachedRoomList.ContainsValue(room)) {
				dictionaryroom.cachedRoomList.Add(room.Name,room);
			}
			if (room.RemovedFromList) {
				dictionaryroom.cachedRoomList.Remove(room.Name);

			}
		}
		foreach (var VARIABLE in dictionaryroom.cachedRoomList.Values) {
			var but = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, content.transform);
			but.GetComponent<RectTransform>().position = Vector3.zero;
			but.GetComponentInChildren<TMPro.TMP_Text>().text = $"Name: {VARIABLE.Name} Players: {VARIABLE.PlayerCount} / {VARIABLE.MaxPlayers}";
			but.GetComponent<Button>().onClick.AddListener(delegate { JoinSpecificRoom(VARIABLE.Name); });
			if (VARIABLE.MaxPlayers == VARIABLE.PlayerCount) {
				but.GetComponent<Button>().interactable = false;
			}
		}
	}

	public void JoinSpecificRoom(string s) {
		PhotonNetwork.JoinRoom(s);
	}

}

public static class TransformEx {
	public static Transform Clear(this Transform transform) {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		return transform;
	}
}