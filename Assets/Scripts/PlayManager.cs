using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;
    bool isBallOutside;
    bool isBallTeleporting;
    bool isGoal;
    Vector3 lastBallPosition;
    private void Update() 
    {   
        if (ballController.ShootingMode)
        {
            lastBallPosition = ballController.transform.position;
        }
        var inputActive = Input.GetMouseButton(0) 
            && ballController.IsMove() == false 
            && ballController.ShootingMode == false
            && isBallOutside == false;


            
        camController.SetInputActive(inputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled =false;
        //todo pop up window

    }

    public void OnBallOutside()
    {
        if (isGoal)
            return;
        if (isBallTeleporting == false)
        {
            Invoke("TeleportBallLastPosition",2);
        }
        ballController.enabled = false;
        isBallOutside = true;
        isBallTeleporting = true;
    }

    public void TeleportBallLastPosition()
    {
        TeleportBall(lastBallPosition); 
    }

    public void TeleportBall(Vector3 targetPosition)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = targetPosition;
        rb.isKinematic = false;
        ballController.enabled = true;
        isBallOutside = false;
        isBallTeleporting = false;
    }
}
