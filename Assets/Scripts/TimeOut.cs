using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class TimeOut : MonoBehaviourPun
{
    private float idleTime = 120f;
    private float timer = 10;
    public GameObject TimeOutUI;
    public TMP_Text TimeOutUIText;
    private bool TimeOver = false;
    void Update()
    {
        if (!TimeOver){
            if (Input.anyKey){
                idleTime = 120;
            }
            idleTime -= Time.deltaTime;
            if (idleTime <= 0 ){
                PlayerNotMoving();
            }
            if (TimeOutUI.activeSelf){
                timer -= Time.deltaTime;
                TimeOutUIText.text = "Disconnecting in: " + timer.ToString("F0");
                if (timer <= 0){
                    TimeOver = true;
                } 
                else if (timer > 0 && Input.anyKey){
                    idleTime = 120;
                    timer = 10; 
                    TimeOutUI.SetActive(false);
                } 
            }
        } else {
            leaveGame();
        }
    }

    public void PlayerNotMoving(){
        TimeOutUI.SetActive(true);
    }

    void leaveGame(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
