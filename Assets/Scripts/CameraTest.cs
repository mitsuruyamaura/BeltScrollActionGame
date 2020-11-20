using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTester : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private GameObject sub;
    public GameObject cube;

    float pos;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            return;
        }

        transform.position = player.transform.position + offset;

        //if (transform.position.x != player.transform.position.x) {
        //    sub = player;
        //    player = null;
        //} else {
        //    if(sub != null)
        //    player = sub;
        //    cube.transform.localPosition = Vector3.zero;
        //    transform.position = player.transform.position + offset;
        //}
    }
}
