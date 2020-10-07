using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;
    [Header("回転速度")]
    public float rotateSpeed;

    [Header("ジャンプ力")]
    public float jumpPower;
    [Header("地面判定用レイヤー")]
    public LayerMask groundLayer;

    public int attackPower;
    public float bulletSpeed;

    //public Joystick joystick;
    float x;
    float z;


    private Rigidbody rb;
    private Animator anim;

    public int maxHp;
    public int hp;


    public bool isGround;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string JUMP = "Jump";
    private const string ATTACK = "Action";


    private enum AnimatorState {
        Attack,
        Jump,
        Speed,
        AttackSpeed,

    }

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
    }

    /// <summary>
	/// Player情報の初期設定
	/// </summary>
	private void InitPlayer() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // 地面の判定
        CheckGround();


        // ジャンプの判定
        JudgeJump();

        // TODO 攻撃
        
    }

    void FixedUpdate() {
        if(transform.position.x < gameManager.leftLimitPos) {
            transform.position = new Vector3(gameManager.leftLimitPos, transform.position.y, transform.position.z);
        }

        if (transform.position.x > gameManager.rightLimitPos) {
            transform.position = new Vector3(gameManager.rightLimitPos, transform.position.y, transform.position.z);
        }


        //if (anim.GetCurrentAnimatorStateInfo(0).IsName(AnimatorState.Attack.ToString())) {
        //	return;
        //}

        // キー入力
        x = Input.GetAxis(HORIZONTAL);
        z = Input.GetAxis(VERTICAL);

        // JoyStick入力
        //x = joystick.Horizontal;
        //z = joystick.Vertical;


        // 移動
        Move(x, z);
    }

    /// <summary>
	/// 地面の判定
	/// </summary>
	private void CheckGround() {
        //  Linecastでキャラの足元に地面があるか判定  地面があるときはTrueを返す
        isGround = Physics.Linecast(
                        transform.position + transform.up * 1,
                        transform.position - transform.up * 0.3f,
                        groundLayer);
    }

    /// <summary>
	/// 移動する
	/// </summary>
	/// <param name="x">X軸の移動値</param>
	/// <param name="z">Z軸の移動値</param>
	private void Move(float x, float z) {
        //isAttack = true;

        //float horizontal = joystick.Horizontal;
        //float vertical = joystick.Vertical;

        // 入力値を正規化
        //Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;
        //transform.position += new Vector3(moveDir.x, 0, moveDir.z);

        // 入力値を正規化
        Vector3 moveDir = new Vector3(x, 0, z).normalized;


        // RigidbodyのAddforceメソッドを利用して移動
        //rb.AddForce(moveDir * moveSpeed);

        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);

        if (moveDir != Vector3.zero) {
            //anim.SetFloat(AnimatorState.Speed.ToString(), 0.8f);
        } else {
            //Debug.Log(rb.velocity.magnitude);
            //anim.SetFloat("Speed", rb.velocity.magnitude);
            //anim.SetFloat(AnimatorState.Speed.ToString(), 0);
        }

        // 移動に合わせて向きを変える
        LookDirection(moveDir);
    }


    /// <summary>
	/// 向きを変える
	/// </summary>
	/// <param name="dir"></param>
	private void LookDirection(Vector3 dir) {
        // ベクトル(向きと大きさ)の2乗の長さをfloatで戻す = 動いているかどうかの確認
        if (dir.sqrMagnitude <= 0f) {
            return;
        }

        //Debug.Log(dir);

        if (dir.x == 0) {
            return;
        }

        float pos = 0;
        if (dir.x > 0) {
            pos = 0;
        } else {
            pos = 180;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, pos, 0));

        //isAttack = false;
    }

    /// <summary>
	/// ジャンプできるか判定
	/// </summary>
	private void JudgeJump() {
        //  キー入力のJumpで反応（GetButton）スペースキー(GetKey)
        if (Input.GetButtonDown(JUMP) && isGround) {
            //  着地していたとき
            Jump();
        }
    }

    /// <summary>
    /// ジャンプボタンでジャンプ
    /// </summary>
    private void OnClickJump() {
        if (isGround) {
            Jump();
        }
    }

    /// <summary>
	/// ジャンプ
	/// </summary>
	private void Jump() {
        //  着地判定をfalse
        isGround = false;

        //  Jumpステートへ遷移してジャンプアニメを再生
        anim.Play(AnimatorState.Jump.ToString());

        //  AddForceにて上方向へ力を加える
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
