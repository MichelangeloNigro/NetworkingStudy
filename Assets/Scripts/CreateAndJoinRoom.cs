using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_InputField _createInput = null;
	[SerializeField] TMP_InputField _joinInput = null;
	[SerializeField] string _loadedLevelName = "Gameplay";

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(_createInput.text); //And Join
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(_joinInput.text);
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.LoadLevel(_loadedLevelName);
	}
}
