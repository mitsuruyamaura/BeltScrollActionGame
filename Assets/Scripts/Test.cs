using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject obj;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("car");

        // 上記の２つの命令を1行にする場合には、下記のように書きます
        spriteRenderer = GameObject.Find("car").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            obj.name = "SuperCar";
        }

        // キーボードの D を押した際に
        if (Input.GetKeyDown(KeyCode.D)) {

            // "car" ゲームオブジェクトのSprite Rendererコンポーネントをオン/オフする
            //   =>　"car" ゲームオブジェクトのSprite Rendererコンポーネントの状態を確認することで制御できる
            //if (spriteRenderer.enabled == true) {    // 表示中なら
            //    spriteRenderer.enabled = false;  　// 非表示
            //} else {
            //    spriteRenderer.enabled = true;     // 非表示なら表示
            //}

            // 上記のif文4行分を1行で書く場合
            spriteRenderer.enabled = !spriteRenderer.enabled; 
        }

        // キーボードの B キーを押した際
        if (Input.GetKeyDown(KeyCode.B)) {

            // carゲームオブジェクトにRigidbody2dがアタッチされているか確認
            //rb = obj.GetComponent<Rigidbody2D>();

            //// アタッチされていなければ　=　すでにアタッチされていれば処理も行わない
            //if (rb == null) {

            //    // car ゲームオブジェクトに Rigidbody2Dを追加する
            //    obj.AddComponent<Rigidbody2D>();
            //    rb = obj.GetComponent<Rigidbody2D>();
            //    Debug.Log("コンポーネント追加");
            //}


            if (!obj.TryGetComponent(out rb)) {
                rb = obj.AddComponent<Rigidbody2D>().GetComponent<Rigidbody2D>();
                Debug.Log("コンポーネント追加");
                //obj.AddComponent<Rigidbody2D>();
                //rb = obj.GetComponent<Rigidbody2D>();
            }

        }

        // キーボードの V キーを押した際
        if (Input.GetKeyDown(KeyCode.V)) {

            // コンポーネントがアタッチされていなければ処理をしない
            if (rb == null) {
                Debug.Log("削除するコンポーネントなし");
                return;
            }

            // car ゲームオブジェクトのRigidbody2Dを削除する
            Destroy(rb);
            //rb = null;

            Debug.Log("コンポーネント削除");
        }
    }
}
