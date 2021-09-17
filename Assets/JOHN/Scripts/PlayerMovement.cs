using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : BasePlayer
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(this.tag == "Player1")
        {
            if (Input.GetKey(KeyCode.W))//front
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.forward * movementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))//Left
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.right * -1 * movementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))//Down
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.forward * -1 * movementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))//Right
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.right * movementSpeed * Time.deltaTime;
            }
            else
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        if(this.tag == "Player2")
        {
            if (Input.GetKey(KeyCode.UpArrow))//front
            {

            }
            if (Input.GetKey(KeyCode.LeftArrow))//Left
            {

            }
            if (Input.GetKey(KeyCode.DownArrow))//Down
            {

            }
            if (Input.GetKey(KeyCode.RightArrow))//Right
            {

            }
        }
    }

    public override void Death()
    {
        base.Death();
    }

}
