using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Playercontroller : MonoBehaviour
{
    // A simplest chracter controller ever for unity endless runner game similar to subway surfer or any kind of clone of this game simply apply to any kind of chracter for future 
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 10f; // Speed at which the player moves forward
    [SerializeField] private float laneDistance = 3f; // Distance between lanes
    [SerializeField] private float sideSpeed = 5f; // Speed of side movement
    private int targetLane = 1; // 0 = Left, 1 = Mid, 2 = Right
    private CharacterController controller;
     Animator p_Animator;
    void Start()
    {
        p_Animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        HandleForwardMovement();
        HandleLaneSwitching();
        MoveTowardsTargetLane();
    }
    /* calaculating the forwardmove vector3 and multiplay with the forward speed actully speed is multiplaying with vector 3 z of player change the direction
         to the world point as well the this calculated vector is applied to unity built in chracter controller to move forward because it only changing the value of so that why it 
         it will move forward */
    private void HandleForwardMovement()
    {
        
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;
        Vector3 worldForwardMove = transform.TransformDirection(forwardMove);
        controller.Move(worldForwardMove);
    }
    // A simple way to get the inputs and then in if left arrow is pressed vlaue will be -1 given to the down move lane function and vice versa 
    private void HandleLaneSwitching()
    {
      
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(-1);
            p_Animator.Play("dodgeLeft");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(1);
            p_Animator.Play("dodgeRight");
        }
    }
    /* a simple function to move the charcter into the dirserd lane and also main and important part is it is getting value from down function then check if player is in the dired lane 
     already or not actually for the accuracy and if its not then given below condition will be excuted ist it will find the difference ( formula used) then change it to the normalized 
     then check through the conditon if sqr mag is less then the differnce then it move in move dire if not then direct move to that differnce */
    private void MoveTowardsTargetLane()
    {

        Vector3 targetPosition = CalculateTargetPosition();

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * sideSpeed * Time.deltaTime;

            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }
    }
    // A simple function for to calculate the target position and direction of player in which lane and direction actully important one is it keep the values of z and y same mean no change in 
    // hight and speed just chnage the x vlaue and conditons are important to check if in lane 0 mean then move to left by distance and vice versa 

    private Vector3 CalculateTargetPosition()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (targetLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (targetLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        return targetPosition;
    }
    // a simple function to change the lance values based upon inputs and keep them under 0 and 2 
    private void MoveLane(int direction)
    {
        targetLane += direction;
        targetLane = Mathf.Clamp(targetLane, 0, 2); // Keep targetLane between 0 and 2
    }
}
