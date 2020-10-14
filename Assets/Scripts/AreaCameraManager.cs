using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AreaCameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtualCameras;
    
    /// <summary>
    /// VirtualCameraの優先順位を切り替える
    /// </summary>
    /// <param name="cameraNo"></param>
    public void ChengeVirtualCamera(int cameraNo) {
        for (int i = 0; i < virtualCameras.Length; i++) {
            if (cameraNo == i) {
                virtualCameras[i].Priority = 11;
            } else {
                virtualCameras[i].Priority = 10;
            }
        }
    }
}
