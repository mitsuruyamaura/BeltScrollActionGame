using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AreaCameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] areaCameras;
    
    /// <summary>
    /// VirtualCameraの優先順位を切り替える
    /// </summary>
    /// <param name="cameraNo"></param>
    public void ChengeVirtualCamera(int cameraNo) {
        for (int i = 0; i < areaCameras.Length; i++) {
            if (cameraNo == i) {
                areaCameras[i].Priority = 11;
            } else {
                areaCameras[i].Priority = 10;
            }
        }
    }
}
