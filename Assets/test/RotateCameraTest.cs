using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraTest : MonoBehaviour
{
    public GameObject player;   // ヒエラルキーでターゲットとなるゲームオブジェクトをアサインし、それを中心に公転させる
    private Vector3 offset;

    void Start() {
        offset = transform.position - player.transform.position;
        Debug.Log(player.transform.position);
    }

    void LateUpdate() {
        // まずはカメラ位置をプレイヤーに追従させて...
        transform.position = player.transform.position + offset;

        // プレイヤーを中心にカメラを回すと、プレイヤーとカメラの相対位置が
        // 変化するはずなので、RotateAroundの後でoffsetを更新する
        if (Input.GetKey("right")) {
            transform.RotateAround(player.transform.position, Vector3.up, -2.0f);
            // transform.RotateAround(Vector3.zero,Vector3.up,-2.0f);
            offset = transform.position - player.transform.position;
        }
        if (Input.GetKey("left")) {
            transform.RotateAround(player.transform.position, Vector3.up, 2.0f);
            // transform.RotateAround(Vector3.zero,Vector3.up,-2.0f);
            offset = transform.position - player.transform.position;
        }
        if (Input.GetKey("up")) {
            transform.RotateAround(player.transform.position, Vector3.right, -2.0f);
            offset = transform.position - player.transform.position;
        }
        if (Input.GetKey("down")) {
            transform.RotateAround(player.transform.position, Vector3.right, 2.0f);
            offset = transform.position - player.transform.position;
        }
    }
}
