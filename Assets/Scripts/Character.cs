using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Character : MonoBehaviourPun
{
    public GameObject playerCam; 
    public float MoveSpeed = 5;
    public SpriteRenderer sprite;
    public PhotonView photonview;
    public Animator anim;
    private bool AllowMoving = true;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPointRight;
    public Transform BulletSpawnPointLeft;
    public TMP_Text PlayerName;
    public bool DisableInputs = false;
    public bool IsGrounded = false;
    private Rigidbody2D rb;
    public float jumpForce;
    public string MyName;
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine){
            GameManager.instance.LocalPlayer = this.gameObject;
            playerCam.SetActive(true);
            PlayerName.text = "You: " + PhotonNetwork.NickName;
            PlayerName.color = Color.green;
            MyName = PhotonNetwork.NickName;
        } else {
            PlayerName.text = photonview.Owner.NickName;
            PlayerName.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !DisableInputs){
            checkInputs();
        }
    }

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void checkInputs(){
        if (AllowMoving){
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove") == false){
            shoot();
        } else if (Input.GetKeyUp(KeyCode.RightControl)){
            anim.SetBool("IsShoot", false);
            AllowMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded){
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)){
            anim.SetBool("IsMove", true);
        }
        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShoot") == false){
            // FlipSprite_Right();
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(-0.53f, 1.03f, 0);
            photonview.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        } else if (Input.GetKeyUp(KeyCode.D)){
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShoot") == false){
            // FlipSprite_Left();
            playerCam.GetComponent<CameraFollow2D>().offset = new Vector3(-5.3f, 1.03f, 0);
            photonview.RPC("FlipSprite_Left", RpcTarget.AllBuffered);

        } else if (Input.GetKeyUp(KeyCode.A)){
            anim.SetBool("IsMove", false);
        }
    }

    private void shoot(){
        if (sprite.flipX == false){
            GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(BulletSpawnPointRight.position.x, BulletSpawnPointRight.position.y), Quaternion.identity, 0);
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
        }
        if(sprite.flipX == true){
            GameObject bullet = PhotonNetwork.Instantiate(BulletPrefab.name, new Vector2(BulletSpawnPointLeft.position.x, BulletSpawnPointLeft.position.y), Quaternion.identity, 0);
            bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
        anim.SetBool("IsShoot", true);
        AllowMoving = false;
    }
    [PunRPC]
    void FlipSprite_Right(){
        sprite.flipX = false;

    }

    [PunRPC]
    void FlipSprite_Left(){
        sprite.flipX = true;

    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            IsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            IsGrounded = false;
        }
    }

    void Jump(){
        rb.AddForce(new Vector2(0, jumpForce * Time.deltaTime));
    }
}
