using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Bullet : MonoBehaviourPun
{
    public float MoveSpeed = 8;
    public float DestroyTime = 2f;
    public bool MovingDerection;
    public float bulletDamage = 0.3f;
    public string killerName;
    public GameObject localPlayerObj;

    IEnumerable destroyBullet(){
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
    }

    void Start(){
        if (photonView.IsMine)
        killerName = localPlayerObj.GetComponent<Character>().MyName;
    }

    void Update(){
        if (!MovingDerection){
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);

        }
    }

    [PunRPC]
    public void ChangeDirection(){
        MovingDerection = true; 
    }

    [PunRPC]
    void Destroy(){
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!photonView.IsMine){
            return; 
        }
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if (target != null && (!target.IsMine || target.IsRoomView)){
            if (target.tag == "Player"){
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);
                target.GetComponent<HurtEffect>().GotHit();
                if (target.GetComponent<Health>().health <= 0){
                    Player GotKilled = target.Owner;
                    target.RPC("YouKilledBy", GotKilled, killerName);
                    target.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }
            }else {
            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }
        
    }
}
