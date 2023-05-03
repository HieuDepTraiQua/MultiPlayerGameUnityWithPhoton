using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class ConnectPlayers : MonoBehaviour
{
    public GameObject CurrentPlayer_Prefab;
    public GameObject CurrentPlayer_Grid;

    public void AddLocalPlayer(){
        GameObject obj = Instantiate(CurrentPlayer_Prefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(CurrentPlayer_Grid.transform, false);
        obj.GetComponentInChildren<TMP_Text>().text = "You: " + PhotonNetwork.NickName;
        obj.GetComponentInChildren<TMP_Text>().color = Color.green;
    }

    [PunRPC]
    public void UpdatePlayerList(string name){

        GameObject obj = Instantiate(CurrentPlayer_Prefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(CurrentPlayer_Grid.transform, false);
        obj.GetComponentInChildren<TMP_Text>().text = name;
    }
    
    public void RemovePlayerList(string name){
        foreach (TMP_Text playerName in CurrentPlayer_Grid.GetComponentsInChildren<TMP_Text>()){
            if (name == playerName.text){
                Destroy(playerName.transform.parent.gameObject);
            }
        }
    }
}
