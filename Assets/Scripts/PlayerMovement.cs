using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //move
    public float walkSpeed;
    public float runSpeed;
    private float applySpeed;
    private bool isRun = false;

    private Vector3 lastPos;
    private bool isMove = false;

    //jump
    public float jumpForce;
    public int jumpNumber;
    private int curJump;

    //rotate
    public float lookSensitivity;
    public float cameraRotationLimit;
    private float currentCameraRotationX = 0f;
    private float lastMouseY;

    //component
    public Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private Crosshair theCrosshair;

    private PlayerStatus1 status;

    private void Start() {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        theCrosshair = FindObjectOfType<Crosshair>();
        status = GetComponent<PlayerStatus1>();
    }

    private void Update() {
        TryJump();
        checkSpeed();
        Move();
        MoveCheck();
        Rotation();
        CameraRotation();
        lastMouse();
    }

    // --------about jump-------
    private void OnCollisionEnter(Collision collision){
        if(collision.contacts[0].normal.y > 0.9f){
            curJump = jumpNumber;
        }
    }
    private void TryJump(){
        if(playerInput.jump && curJump >0){
            curJump--;
            Jump();
        }
    }
    private void Jump(){
        myRigid.velocity = Vector3.up * jumpForce;
        playerAnimator.SetTrigger("Jump");
    }
    //--------------------------

    //------------about move---------
    private void checkSpeed(){
        if(playerInput.run && status.SP > 0){
            isRun = true;
            applySpeed = runSpeed;
            status.DecreaseSP();
        }
        else{
            isRun = false;
            applySpeed = walkSpeed;
        }
    }

    private void MoveCheck(){
        if(Vector3.Distance(lastPos, transform.position) >= 0.01f) isMove = true;
        else isMove = false;
        //theCrosshair.walkAnimation(isMove);
        lastPos = transform.position;
    }

    private void Move(){
        float _moveVertical = playerInput.moveVertical; 
        float _moveHorizontal = playerInput.moveHorizontal;

        Vector3 _verticalVelocity = transform.forward * _moveVertical;
        Vector3 _horizontalVelocity = transform.right * _moveHorizontal;
        Vector3 _Velocity = (_verticalVelocity + _horizontalVelocity).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _Velocity * Time.deltaTime);
    }

    private void Rotation(){
        float _rotation = playerInput.mouseX;
        Vector3 _rotationY = Vector3.up * _rotation * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_rotationY));
    }

    private void lastMouse(){
        lastMouseY = playerInput.mouseY;
    }
    private void CameraRotation(){
        float _rotation = -lastMouseY + playerInput.mouseY;
        float _rotationX = _rotation * lookSensitivity;
        currentCameraRotationX -= _rotationX;

        float cur = theCamera.transform.localEulerAngles.x;
        if(cur > 180f) cur -= 360f;

        if(cur > cameraRotationLimit && currentCameraRotationX > 0f) return;
        if(cur < -cameraRotationLimit && currentCameraRotationX < 0f) return;
        theCamera.transform.localEulerAngles += Vector3.right * currentCameraRotationX;
    }
    //----------------------------------------

}