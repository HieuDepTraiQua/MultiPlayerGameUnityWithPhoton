using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject canvas;
    public GameObject sceneCam;
    public TMP_Text pingrate;
    public TMP_Text spawnTime;
    public GameObject respawnUI;
    public GameObject settingUI;
    public GameObject waitingUI;
    private float TimeAmount = 5;
    private bool startRespawn;
    public GameObject LeaveScreen;
    public GameObject feedBox;
    public GameObject feedTextPrefab;
    public GameObject KilledFeedBox;
    public ConnectPlayers cp;
    public GameObject cpCanvas;
    private Character playerScript;
    private int playerCount = 0;
    private bool isSpawn = false;
    private bool isDisplayWaiting = true;

    [HideInInspector]
    public GameObject LocalPlayer;
    public static GameManager instance = null;

    private void Awake()
    {
        instance = this;
        canvas.SetActive(true);
    }

    private void Start()
    {
        cp.AddLocalPlayer();
        cp.GetComponent<PhotonView>().RPC("UpdatePlayerList", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-2, 2);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y), Quaternion.identity, 0);
        canvas.SetActive(false);
        sceneCam.SetActive(false);
        isSpawn = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }
        if (startRespawn)
        {
            StartRespawn();
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            cpCanvas.SetActive(true);
        }
        else
        {
            cpCanvas.SetActive(false);
        }
        pingrate.text = "NetworkPing: " + PhotonNetwork.GetPing();


        if (PhotonNetwork.InRoom)
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log("Số lượng người chơi trong phòng: " + playerCount);
        }
        else
        {
            Debug.Log("Không có phòng được kết nối.");
        }

        if (isSpawn)
        {
            if (playerCount < 2 && isDisplayWaiting)
            {
                waitingUI.SetActive(true);
            }
            else
            {
                waitingUI.SetActive(false);
            }
        }

    }

    void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnTime.text = "Respawn Time: " + TimeAmount.ToString("F0");
        if (TimeAmount <= 0)
        {
            respawnUI.SetActive(false);
            startRespawn = false;
            PlayerRelocation();
            LocalPlayer.GetComponent<Health>().EnableInputs();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }

    public void EnableRespawn()
    {
        TimeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }

    public void PlayerRelocation()
    {
        float randomPosition = Random.Range(-3, 3);
        LocalPlayer.transform.localPosition = new Vector2(randomPosition, 2);
    }

    public void ToggleLeaveScreen()
    {
        if (LeaveScreen.activeSelf)
        {
            LeaveScreen.SetActive(false);
            isDisplayWaiting = true;
        }
        else
        {
            LeaveScreen.SetActive(true);
            isDisplayWaiting = false;
            waitingUI.SetActive(false);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void ReturnToGame(){
        settingUI.SetActive(false);
    }
    public void ShowSetting(){
        settingUI.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedBox.transform);
        go.GetComponent<TMP_Text>().text = player.NickName + " has joined the room";
        Destroy(go, 3);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        cp.RemovePlayerList(player.NickName);
        GameObject go = Instantiate(feedTextPrefab, new Vector2(0f, 0f), Quaternion.identity);
        go.transform.SetParent(feedBox.transform);
        go.GetComponent<TMP_Text>().text = player.NickName + " has left the room";
        Destroy(go, 3);
    }

}
