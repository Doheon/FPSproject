using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    //상태변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;



    //민감도
    [SerializeField]
    private float lookSensitivity = 0f;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    private CapsuleCollider capsuleCollider; //땅착지 여부


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>(); //드래그보다 빠름
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;

    }
    // Update is called once per frame
    void Update(){
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryCrouch(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Crouch();
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            CrouchCancel();
        }
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine(){
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY){
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15) break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0,applyCrouchPosY, 0);
        //yield return new WaitForSeconds(1f);
    }

    private void Crouch(){
        isCrouch = true;
        applySpeed = crouchSpeed;
        applyCrouchPosY = crouchPosY;
    }

    private void CrouchCancel(){
        isCrouch = false;
        applySpeed = walkSpeed;
        applyCrouchPosY = originPosY;
    }


    private void IsGround(){
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump(){
        if(Input.GetKeyDown(KeyCode.Space) && isGround){
            Jump();
        }
    }

    private void Jump(){
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun(){
        if(isCrouch) return; //앉아있을땐 안뜀

        if(Input.GetKey(KeyCode.LeftShift)){
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            RunningCancel();
        }
    }

    private void Running(){
        isRun = true;
        applySpeed = runSpeed;

    }

    private void RunningCancel(){
        isRun = false;
        applySpeed = walkSpeed;
    }
    
    private void Move(){
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //속도를 일정하게

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        //transform.position += _velocity * Time.deltaTime; //이런식으로하면 안됨(느림)
    }
    private void CharacterRotation(){
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
    private void CameraRotation(){
        //상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //1 or -1
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
