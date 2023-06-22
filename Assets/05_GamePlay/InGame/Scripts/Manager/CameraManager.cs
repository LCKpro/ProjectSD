using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Camera;

public class CameraManager : MonoBehaviour
{
    public CameraController controller;
    public CameraMotor cameraMotor_Player;
    public CameraMotor cameraMotor_Target;

    public void ChangeCam_PlayerToTarget()
    {
        controller.currentCameraMotor = cameraMotor_Target;
    }

    public void ChangeCam_TargetToPlayer()
    {
        controller.currentCameraMotor = cameraMotor_Player;
    }
}
