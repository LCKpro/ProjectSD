using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Camera;

public class CameraManager : MonoBehaviour
{
    public CameraController controller;
    public CameraMotor cameraMotor_Player;
    public CameraMotor cameraMotor_Target;

    public CameraMotor cameraMotor_Sky;
    public CameraMotor cameraMotor_NPC;

    public void ChangeCam_PlayerToTarget()
    {
        controller.ChangeCameraMotor(cameraMotor_Target, 0.2f);
        //controller.currentCameraMotor = cameraMotor_Target;
    }

    public void ChangeCam_TargetToPlayer()
    {
        controller.ChangeCameraMotor(cameraMotor_Player, 0.2f);
        //controller.currentCameraMotor = cameraMotor_Player;
    }

    public void ChangeCam_Sky()
    {
        controller.ChangeCameraMotor(cameraMotor_Sky, 0.7f);
        //controller.currentCameraMotor = cameraMotor_Sky;
    }

    public void ChangeCam_NPC()
    {
        controller.ChangeCameraMotor(cameraMotor_NPC, 0.7f);
        //controller.currentCameraMotor = cameraMotor_NPC;
    }
}
