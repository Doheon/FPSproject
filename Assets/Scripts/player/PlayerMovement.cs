using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //move
    public float walkSpeed;
    public float applySpeed;
    public float gravity;
    public bool canRun = true;

    public float yVelocity;
    public Vector3 direction;
    public Vector3 addDirection;

    private Vector3 lastPos;

    //jump
    public float jumpForce;
    public int jumpNumber;
    private int curJump;

    //rotate
    public float lookSensitivity;
    public float cameraRotationLimit;
    private float lastMouseY;

    //component
    public Camera theCamera;
    private CapsuleCollider capsuleCollider;
    private CharacterController controller;

    private Animator playerAnimator;


    protected PlayerStatus status;

    private void Start() {
        capsuleCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
        lastPos = transform.position;
    }

    private void Update(){
        Move();
        if(GameManager.instance.canPlayerMove){
            
            Rotation();
            CameraRotation();
            CheckMove();
        }
    }

    public virtual void Run(){

    }


    //------------about move---------

    private void CheckMove(){
        if(GameManager.instance.isStart == false){
            if((transform.position - lastPos).magnitude > 1f) GameManager.instance.isStart = true;
        }
    }

    private void Move(){
        if(GameManager.instance.canPlayerMove){
            float _moveVertical = PlayerInput.instance.moveVertical; 
            float _moveHorizontal = PlayerInput.instance.moveHorizontal;

            Vector3 _verticalVelocity = transform.forward * _moveVertical;
            Vector3 _horizontalVelocity = transform.right * _moveHorizontal;
            direction = (_verticalVelocity + _horizontalVelocity).normalized;
            
            //direction += addDirection;
            Jump();
            Run();

            Vector3 velocity = direction * applySpeed;
            if(!controller.isGrounded) yVelocity -= gravity * Time.deltaTime;
            direction.y = yVelocity;
        }
        else direction = Vector3.zero;
        controller.Move(direction * applySpeed * Time.deltaTime);
    }

    public virtual void Jump(){
        if(controller.isGrounded) curJump = jumpNumber;
        if(PlayerInput.instance.jump && curJump > 0){
            curJump--;
            yVelocity = jumpForce;
        }
    }



    public void AdditionalMove(Vector3 _direction, float _time){
        StartCoroutine(AdditionalMoveCoroutine(_direction, _time));
    }
    IEnumerator AdditionalMoveCoroutine(Vector3 _direction, float _time){
        float _curtime = Time.time;
        while(Time.time < _time + _curtime){
            addDirection = _direction;
            yield return null;
        }
        addDirection = Vector3.zero;
    }
    
    private void Rotation(){
        float _rotation = PlayerInput.instance.mouseX;
        Vector3 _rotationY = Vector3.up * _rotation * lookSensitivity * Time.deltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(_rotationY);
    }

    private void CameraRotation(){
        float _rotation = PlayerInput.instance.mouseY;

        float _rotationX = _rotation * lookSensitivity * Time.deltaTime;

        float cur = theCamera.transform.localEulerAngles.x;
        if(cur > 180f) cur -= 360f;

        if(cur > cameraRotationLimit && _rotationX < 0f) return;
        if(cur < -cameraRotationLimit && _rotationX > 0f) return;
        theCamera.transform.localEulerAngles += Vector3.right * (-_rotationX);
    }
    //----------------------------------------

}