using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UpdateUI : MonoBehaviourPunCallbacks {
	public Slider player1Life;
	public Slider player2Life;
	public FightInput player1;
	public FightInput player2;
	public FadeManager fade;
	// Start is called before the first frame update

	void SetHP1Online() {
		ExitGames.Client.Photon.Hashtable setHpValue = new ExitGames.Client.Photon.Hashtable();
		setHpValue.Add("player1Hp", player1.life);
		PhotonNetwork.CurrentRoom.SetCustomProperties(setHpValue);
		player1.GetComponentInChildren<Rigidbody2D>().AddForce(-player1.model.transform.right.normalized * 3f,ForceMode2D.Impulse);
		StartCoroutine(stopKnockBack1());
	}

	void sethp2OnCloud() {
		ExitGames.Client.Photon.Hashtable setHpValue = new ExitGames.Client.Photon.Hashtable();
		setHpValue.Add("player2Hp", player2.life);
		PhotonNetwork.CurrentRoom.SetCustomProperties(setHpValue);
		player2.GetComponentInChildren<Rigidbody2D>().AddForce(-player2.model.transform.right.normalized * 3f,ForceMode2D.Impulse);
		StartCoroutine(stopKnockBack2());
	}

	private void Update() {
		if (player2!=null&& player1!=null) {
			player2Life.value = player2.life;
			player1Life.value = player1.life;
		}
	}

	public IEnumerator stopKnockBack1() {
		yield return new WaitForSeconds(0.4f);
		player1.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero ;
	}
	public IEnumerator stopKnockBack2() {
		yield return new WaitForSeconds(0.4f);
		player2.GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero ;
	}
	public override void OnPlayerEnteredRoom(Player newPlayer) {
		base.OnPlayerEnteredRoom(newPlayer);
		photonView.RPC("setUIFunc", RpcTarget.All);
	}

	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
		base.OnRoomPropertiesUpdate(propertiesThatChanged);
		if (propertiesThatChanged.ContainsKey("player2Hp")) {
			object temp;
			propertiesThatChanged.TryGetValue("player2Hp",out temp);
			player2.life = (int)temp;
		}if (propertiesThatChanged.ContainsKey("player1Hp")) {
			object temp;
			propertiesThatChanged.TryGetValue("player1Hp",out temp);
			player1.life = (int)temp;
		}
	}

	public IEnumerator setUI() {
		fade.gameObject.SetActive(true);
		yield return new WaitForSecondsRealtime(1f);
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(10);
		player1.OnDamageTaken += SetHP1Online;
		player1Life.maxValue = player1.life;
		player1Life.value = player1.life;
		player2.OnDamageTaken += sethp2OnCloud;
		player2Life.maxValue = player2.life;
		player2Life.value = player2.life;
		fade.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	[PunRPC]
	public void setUIFunc() {
		StartCoroutine(setUI());
	}
}