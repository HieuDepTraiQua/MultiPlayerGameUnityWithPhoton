using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class RoomNameButton : MonoBehaviour
{
    public TMP_Text RoomName;

    private void Start() {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => JoinRoomByName(RoomName.text));
    }

    public void JoinRoomByName(string RoomName){
        PhotonNetwork.JoinRoom(RoomName);
    }
}
