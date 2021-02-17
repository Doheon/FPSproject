using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public static PlayerInput instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerInput>();
            }

            return m_instance;
        }
    }
    private static PlayerInput m_instance;


    public string move1 = "Vertical";
    public string move2 = "Horizontal"; 
    public string fireButtonName = "Fire1"; 
    public string reloadButtonName = "Reload";
    public string runButtonName = "Run";
    public string jumpButtonName = "Jump";
    public string mousex = "Mouse X";
    public string mousey = "Mouse Y";
    public string findsight = "Fire2";

    public float moveVertical { get; private set; } 
    public float moveHorizontal { get; private set; }
    public float mouseX {get; private set;}
    public float mouseY {get; private set;}
    public bool fire { get; private set; } 
    public bool reload { get; private set; }
    public bool run {get; private set;}
    public bool dash;
    public bool jump {get; private set;}
    public bool fly;
    
    public bool findSight {get; private set;}

    public bool weapon1 {get; private set;}
    public bool weapon2 {get; private set;}
    public bool weapon3 {get; private set;}

    public bool skill1 {get; private set;}
    public bool skill2 {get; private set;}
    public bool skill3 {get; private set;}
    public bool skill1Up {get; private set;}
    public bool skill2Up {get; private set;}
    public bool skill3Up {get; private set;}

    public bool rightMouseDown {get; private set;}
    public bool rightMouseUp {get; private set;}

    public bool pauseGame {get; private set;}
    public bool infoTab{get; private set;}
    

    private void Update() {
        //if (GameManager.instance != null && GameManager.instance.isGameover)
       // {
            // moveVertical = 0;
            // moveHorizontal = 0;
            // fire = false;
            // reload = false;
            // run = false;
            // jump = false;
            // mouseX = 0f;
            // mouseY = 0f;
            // return;
        //}

        moveVertical = Input.GetAxis(move1);
        moveHorizontal = Input.GetAxis(move2);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        run = Input.GetButton(runButtonName);
        dash = Input.GetButtonDown(runButtonName);
        jump = Input.GetButtonDown(jumpButtonName);
        fly = Input.GetButton(jumpButtonName);

        mouseX = Input.GetAxis(mousex);
        mouseY = Input.GetAxis(mousey);
        findSight = Input.GetButton(findsight);
        weapon1 = Input.GetKeyDown(KeyCode.Alpha1);
        weapon2 = Input.GetKeyDown(KeyCode.Alpha2);
        weapon3 = Input.GetKeyDown(KeyCode.Alpha3);
        rightMouseDown = Input.GetKeyDown(KeyCode.Mouse1);
        rightMouseUp = Input.GetKeyUp(KeyCode.Mouse1);
        pauseGame = Input.GetKeyDown(KeyCode.Escape);
        skill1 = Input.GetKeyDown(KeyCode.Q);
        skill2 = Input.GetKeyDown(KeyCode.E);
        skill3 = Input.GetKeyDown(KeyCode.Mouse1);
        skill1Up = Input.GetKeyDown(KeyCode.Z);
        skill2Up = Input.GetKeyDown(KeyCode.X);
        skill3Up = Input.GetKeyDown(KeyCode.C);
        infoTab = Input.GetKey(KeyCode.Tab);

    }
}