using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : BasePlayer
{
    Rigidbody rb;
    protected override void OnEnable()
    {
        base.OnEnable();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (Player == player.player1)
        {
            rb.velocity = new Vector3(Input.GetAxis("HorizontalWASD") * movementSpeed * Time.fixedDeltaTime, 0, Input.GetAxis("VerticalWASD") * movementSpeed * Time.fixedDeltaTime);
            //PlayerAnimation();
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
            rb.velocity = new Vector3(Input.GetAxis("HorizontalArrow") * movementSpeed * Time.fixedDeltaTime, 0, Input.GetAxis("VerticalArrow") * movementSpeed * Time.fixedDeltaTime);
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
    public override void DeathEvent(GameObject player)
    {
        base.DeathEvent(player);
    }

    private void PlayerAnimation()
    {
        bool isRight = false;
        float currentRot = isRight?-20: 20;
        float currentLerp;
        currentLerp = Mathf.Lerp(transform.eulerAngles.z, currentRot, 0.01f);
        //transform.eulerAngles = transform.rotation,Mathf.Lerp(transform.eulerAngles, Quaternion.Euler(0, 20, 0), 0.2f);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentLerp));
        if(!isRight)
        {
            if(transform.eulerAngles.z == 20)
            {
                isRight = true;
            }
        }
        else
        {
            isRight = false;
        }
    }
}
