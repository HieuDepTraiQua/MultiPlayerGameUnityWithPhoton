using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{  
    // Start is called before the first frame update
    [SerializeField] private GameObject userNameScreen, connectScreen, roomListUI;
    [SerializeField] private GameObject createUserNameButton;
    [SerializeField] private TMP_InputField usernameInput, createRoomInput, joinRoomInput;

    void Awake() {
        PhotonNetwork.ConnectUsingSettings();  
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Connect to master!!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby (){
        Debug.Log("Connect to Lobby!!");
        userNameScreen.SetActive(true);
    }

    public override void OnJoinedRoom(){
        //Play game screen
        PhotonNetwork.LoadLevel(1);
    }

    #region UIMethods 

    public void Onclick_CreateNameBtn(){
        if (usernameInput.text.Length >= 2)
        PhotonNetwork.NickName = usernameInput.text;
        userNameScreen.SetActive(false);
        connectScreen.SetActive(true);
        roomListUI.SetActive(true);
    }

    public void OnNameField_Chnaged(){
        if (usernameInput.text.Length >= 2){
            createUserNameButton.SetActive(true); 
        } else {
            createUserNameButton.SetActive(false);
        }
    } 

    public void OnClick_JoinRoom(){
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 4;
        PhotonNetwork. JoinOrCreateRoom(joinRoomInput.text, room, TypedLobby.Default);
    }

    public void OnClick_CreateRoom(){
        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions{MaxPlayers = 4}, null);
    }

    #endregion

}
