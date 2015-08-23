using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //TODO DECIDE IF WE WANT CHARACTER CONTROLLER OR RIGIDBODY

    private CharacterController playerController;
    public float rotateSpeed = 3.0f;
    private Transform playerTransform;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {

        Move();
    }

    void Move()
    {


        if (Input.GetKey(KeyCode.W))
        {
            Vector3 forward = playerTransform.TransformDirection(Vector3.forward);
            playerController.SimpleMove(forward * 2.0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 backward = playerTransform.TransformDirection(Vector3.back);
            playerController.SimpleMove(backward * 2.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 left = playerTransform.TransformDirection(Vector3.left);
            playerController.SimpleMove(left * 2.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 right = playerTransform.TransformDirection(Vector3.right);
            playerController.SimpleMove(right * 2.0f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            playerTransform.Rotate(Vector3.up * 50f * Time.deltaTime * -1);
        }
        if (Input.GetKey(KeyCode.E))
        {

            playerTransform.Rotate(Vector3.up * 50f * Time.deltaTime);
        }
    }
}
