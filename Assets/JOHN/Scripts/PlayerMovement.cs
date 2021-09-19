using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : BasePlayer
{
    Rigidbody rb;

    Animator anim;

    [SerializeField]
    private GameObject mesh;

    private CameraMovement camMovement;
    protected override void OnEnable()
    {
        base.OnEnable();
        rb = this.GetComponent<Rigidbody>();
        anim = mesh.GetComponent<Animator>();
        camMovement = Camera.main.GetComponent<CameraMovement>();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (Player == player.player1)
            {
                float tempP1 = 0;
                if (camMovement.CanMove(camMovement.midPoint, rb.gameObject, out tempP1))
                {
                    rb.velocity = new Vector3(Input.GetAxis("HorizontalWASD") * movementSpeed * Time.fixedDeltaTime, 0, Input.GetAxis("VerticalWASD") * movementSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    if(tempP1 < 40)//at the bottom of the screen
                    {
                        var tempVertAxis = Mathf.Abs(Input.GetAxis("VerticalWASD"));
                        rb.velocity = new Vector3(Input.GetAxis("HorizontalWASD") * movementSpeed * Time.fixedDeltaTime, 0, tempVertAxis * movementSpeed * Time.fixedDeltaTime);
                    }
                    else//at the top of the screen
                    {
                        var tempVertAxis = -(Mathf.Abs(Input.GetAxis("VerticalWASD")));
                        rb.velocity = new Vector3(Input.GetAxis("HorizontalWASD") * movementSpeed * Time.fixedDeltaTime, 0, tempVertAxis * movementSpeed * Time.fixedDeltaTime);
                    }
                }
                if (rb.velocity != Vector3.zero)
                {
                    anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }
                if (Input.GetKey(KeyCode.W))//front
                {
                    RotateTowards(new Vector3(0, 0, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.A))//Left
                {
                    RotateTowards(new Vector3(0, 270, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.S))//Down
                {
                    RotateTowards(new Vector3(0, 180, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.D))//Right
                {
                    RotateTowards(new Vector3(0, 0, 0), Player, Time.fixedDeltaTime);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.rotation = transform.rotation;
                }
            }
            if (Player == player.player2)
            {
                float tempP2 = 0;
                if (camMovement.CanMove(camMovement.midPoint, rb.gameObject, out tempP2))
                {
                    rb.velocity = new Vector3(Input.GetAxis("HorizontalArrow") * movementSpeed * Time.fixedDeltaTime, 0, Input.GetAxis("VerticalArrow") * movementSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    if (tempP2 < 40)//at the bottom of the screen
                    {
                        var tempVertAxis = Mathf.Abs(Input.GetAxis("VerticalArrow"));
                        rb.velocity = new Vector3(Input.GetAxis("HorizontalArrow") * movementSpeed * Time.fixedDeltaTime, 0, tempVertAxis * movementSpeed * Time.fixedDeltaTime);
                    }
                    else//at the top of the screen
                    {
                        var tempVertAxis = -(Mathf.Abs(Input.GetAxis("VerticalArrow")));
                        rb.velocity = new Vector3(Input.GetAxis("HorizontalArrow") * movementSpeed * Time.fixedDeltaTime, 0, tempVertAxis * movementSpeed * Time.fixedDeltaTime);
                    }
                }
                if (rb.velocity != Vector3.zero)
                {
                    anim.SetBool("isWalk", true);
                }
                else
                {
                    anim.SetBool("isWalk", false);
                }
                if (Input.GetKey(KeyCode.UpArrow))//front
                {
                    RotateTowards(new Vector3(0, 0, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))//Left
                {
                    RotateTowards(new Vector3(0, 270, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.DownArrow))//Down
                {
                    RotateTowards(new Vector3(0, 180, 0), Player, Time.fixedDeltaTime);
                }
                else if (Input.GetKey(KeyCode.RightArrow))//Right
                {
                    RotateTowards(new Vector3(0, 0, 0), Player, Time.fixedDeltaTime);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.rotation = transform.rotation;
                }
            }
        }
    }
    private void RotateTowards(Vector3 dir, player player, float delta)
    {
        float horizontalInput = player == player.player1? Input.GetAxis("HorizontalWASD") : Input.GetAxis("HorizontalArrow");
        float verticalInput = player == player.player1 ? Input.GetAxis("VerticalWASD") : Input.GetAxis("VerticalArrow");

        Vector3 temp = new Vector3(horizontalInput, 0, verticalInput);
        temp.Normalize();

        if(temp != Vector3.zero)
        {
            Quaternion toRot = Quaternion.LookRotation(temp, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, 1000f * delta);
        }
    }

    private void PlayerAnimation()
    {

    }

    protected override void OnDeathPlayer(GameObject deadPlayer)
    {
        base.OnDeathPlayer(deadPlayer);
        deadPlayer.GetComponent<PlayerMovement>().anim.SetBool("isDead", true);
    }

    protected override void OnRevivedPlayer(GameObject ply)
    {
        base.OnRevivedPlayer(ply);
        ply.GetComponent<PlayerMovement>().anim.SetBool("isDead", false);
    }

    public void ChangePlayerSpeed(bool isSlow = false)
    {
        if(isSlow)
        {
            movementSpeed = 150;
        }
        else
        {
            movementSpeed = 300f;
        }
    }

}
