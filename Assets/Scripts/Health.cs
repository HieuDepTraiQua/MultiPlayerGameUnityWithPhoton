using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class Health : MonoBehaviourPun
{
    public Image fillImage; 
    public float health = 1; 
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D collider;
    public GameObject playerCanvas;
    public Character playerScript;
    public GameObject KilledByText;
    public void CheckHealth(){
        if (photonView.IsMine && health <= 0){
            GameManager.instance.EnableRespawn();
            playerScript.DisableInputs = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death(){
        rb.gravityScale = 0;
        collider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void Revive(){
        rb.gravityScale = 1;
        fillImage.fillAmount = 1;
        collider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        health = 1;
    }

    [PunRPC]
    public void HealthUpdate(float damage){

        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        CheckHealth();
    }

    public void EnableInputs(){
        playerScript.DisableInputs = false;
    }

    [PunRPC]
    void YouKilledBy(string name){
        GameObject go = Instantiate(KilledByText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.KilledFeedBox.transform, false);
        go.GetComponent<TMP_Text>().text = "You killed by: " + name;
        go.GetComponent<TMP_Text>().color = Color.red;
        Destroy(go, 3);
    }

    [PunRPC]
    void YouKilled(string name){
        GameObject go = Instantiate(KilledByText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.KilledFeedBox.transform, false);
        go.GetComponent<TMP_Text>().text = "You killed: " + name;
        go.GetComponent<TMP_Text>().color = Color.green;
        Destroy(go, 3);
    }


}
