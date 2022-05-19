using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class dictionaryroom : SerializedMonoBehaviour
{
	public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

}
