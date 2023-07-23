using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    Camera Cam = null;

    [SerializeField]    Transform nestTowers = null;

    [SerializeField]    Transform CamPoint = null;
    [SerializeField]    Transform CamPosition = null;

    [SerializeField]    Transform greenBuild = null;
    [SerializeField]    GameObject greenCrys = null;
    [SerializeField]    GameObject greenProj = null;

    [SerializeField]    LayerMask layerGround = 0;
    [SerializeField]    LayerMask layerTower = 0;
    [SerializeField]    LayerMask layerGrass = 0;

    private GameplayControls inputControls = null;
    int BuildingFromInventory = -1;

    bool isSprinting = true;

    float MoveSpeed = 12.0f, SprintSpeed = 2.5f, ZoomSpeed = 0.0f;

    public CommandType typeOne = CommandType.Select;
    public CommandType typeTwo = CommandType.CommandUnit;

    private void Awake()
    {

        inputControls = new GameplayControls();
    }

    private void OnEnable()
    {
        inputControls.Player.Enable();
    }
    private void OnDisable()
    {
        inputControls.Player.Disable();
    }

    void Start()
    {
        SetButtonCalls();

    }

    // Update is called once per frame
    void Update()
    {
        ActionsUpdate();

    }

    private void SetButtonCalls()
    {
        // Main Inputs
        if (inputControls != null)
        {
            inputControls.Player.Sprint.performed += _ => Sprint();
            inputControls.Player.ActionOne.performed += _ => ActionsType(typeOne);
            inputControls.Player.ActionTwo.performed += _ => ActionsType(typeTwo);

            inputControls.Player.MenuOne.performed += _ => Pause();
            inputControls.Player.MenuTwo.performed += _ => OpenMenu();


            //shortcuts

        }
    }



    #region Actions

    public void SetActionType(bool _one, CommandType _type)
    {
        if (_one)
        {
            typeOne = _type;
        }
        else
        {
            typeTwo = _type;
        }
    }

    public void ActionsType(CommandType _type)
    {
        switch (_type)
        {
            case CommandType.Nothing:       break;
            case CommandType.MoveCamera:    break;
            case CommandType.RotateCamera:  break;
            case CommandType.BuildMode:
                Build();
                break;
            case CommandType.StopBuildMode:
                StopBuildMode();
                break;
            case CommandType.CommandUnit:
                Command();
                break;
            case CommandType.Select:
                Select();
                break;
            default:
                break;
        }
    }
    public void ActionsUpdate()
    {
        bool isMoveActive = false, isTurnActive = false;

        switch (typeOne)
        {
            case CommandType.MoveCamera:
                isMoveActive = inputControls.Player.ActionOne.ReadValue<float>() > 0.1f;
                break;
            case CommandType.RotateCamera:
                isTurnActive = inputControls.Player.ActionOne.ReadValue<float>() > 0.1f;
                break;
            case CommandType.BuildMode:
                //TODO: Set Green Tower where mouse is pointing
                {
                    Vector2 mousePoint = inputControls.Player.MousePoint.ReadValue<Vector2>();

                    //RaycastHit is the collider that gets touched by the ray/laser
                    RaycastHit hit;
                    //Ray is the class that contains what we need for the laser
                    Ray ray = Camera.main.ScreenPointToRay(mousePoint);

                    if (Physics.Raycast(ray, out hit, 500.0f, layerGround))
                    {
                        Vector3 pinPoint = new Vector3();
                        pinPoint.x = Mathf.RoundToInt(hit.point.x);
                        pinPoint.y = 0.2f;
                        pinPoint.z = Mathf.RoundToInt(hit.point.z);
                        greenBuild.position = pinPoint;
                    }
                }
                break;
            default:
                break;
        }
        switch (typeTwo)
        {
            case CommandType.MoveCamera:
                isMoveActive = inputControls.Player.ActionTwo.ReadValue<float>() > 0.1f;
                break;
            case CommandType.RotateCamera:
                isTurnActive = inputControls.Player.ActionTwo.ReadValue<float>() > 0.1f;
                break;
            default:
                break;
        }

        //CameraMove(isMoveActive, isTurnActive);
        //CameraTurns();

        CameraFollow();
    }

    #endregion


    #region Movement and Camera

    //Variables for the Dragging and Moving Camera
    bool isDragMove = false;
    bool isDragTurn = false;
    Vector2 mouseSTpos = Vector2.zero;

    Vector3 camST = Vector3.zero;
    Vector3 camED = Vector3.zero;

    Vector3 towards = Vector3.zero;
    Vector3 right = Vector3.zero;
    
    private void SetCameraTR()
    {
        towards = CamPoint.position - CamPosition.position;
        towards.y = 0.0f;
        towards.Normalize();

        right = Vector3.Cross(Vector3.up, towards);
        right.y = 0.0f;
        right.Normalize();
    }

    private void CameraMove(bool _movement, bool _turning)
    {
        Vector2 mousePos = inputControls.Player.MousePoint.ReadValue<Vector2>();

        if (_turning && !_movement)
        {
            if (!isDragTurn) StartDragTurn(mousePos);
            //if (isDragMove) EndDragMove();

            DragTurn(mousePos);
        }
        else EndDragTurn();

        if (_movement && !_turning)
        {
            if (!isDragMove) StartDragMove(mousePos);
            //if (isDragTurn) EndDragTurn();

            DragMove(mousePos);
        }
        else EndDragMove();

        if (!_movement && !_turning)
        {
            Vector2 movement = inputControls.Player.Move.ReadValue<Vector2>();

            Vector3 moveTo = Vector3.zero;
            if (movement.sqrMagnitude > 0.0f)
            {
                if (movement.magnitude > 1.0f || isSprinting)
                    moveTo.Normalize();

                moveTo = towards * movement.y + right * movement.x;
            }

            UpdateMovement(moveTo);
        }
    }
    private void CameraTurns()
    {

        float zoom = inputControls.Player.CameraZoom.ReadValue<float>();
        if (Mathf.Abs(zoom) > 0.01f)
        {
            if (Cam)
            {
                if (Cam.orthographic)
                {
                    float size = Cam.orthographicSize;
                    size = Mathf.Clamp(size + -Mathf.Sign(zoom) * ZoomSpeed, 5.0f, 40.0f);
                    Cam.orthographicSize = size;
                }
                else
                {
                    Vector3 length = CamPosition.localPosition;
                    length.y = Mathf.Clamp(length.y + -Mathf.Sign(zoom) * ZoomSpeed, 5.0f, 60.0f);
                    CamPosition.localPosition = length;
                }
            }
        }

    }

    private void UpdateMovement(Vector3 _moveTo)
    {
        if (CamPoint && _moveTo.magnitude > 0.0f)
        {
            if (isSprinting)
                _moveTo *= SprintSpeed;

            //CamPoint.position += _moveTo * MoveSpeed * Time.unscaledDeltaTime;
        }
    }
    private void CameraFollow()
    {
        if (CamPoint && PlayerManager.Instance)
        {
            CamPoint.position = PlayerManager.Instance.GetPlayerPosition();
        }
    }

    private void StartDragMove(Vector2 _mousePoint)
    {
        isDragMove = true;
        camST = CamPoint.position;
        mouseSTpos = _mousePoint;
    }
    private void EndDragMove()
    {
        isDragMove = false;
    }

    private void StartDragTurn(Vector2 _mousePoint)
    {
        isDragTurn = true;
        camST = CamPoint.localEulerAngles;
        mouseSTpos = _mousePoint;
    }
    private void EndDragTurn()
    {
        isDragTurn = false;
    }

    private void DragMove(Vector2 _mousePoint)
    {
        SetCameraTR();

        if (Screen.width > 0 && Screen.height > 0)
        {
            camED = towards * (mouseSTpos.y - _mousePoint.y) * (40.0f / Screen.height) + right * (mouseSTpos.x - _mousePoint.x) * (40.0f / Screen.width);
        }
        else
        {
            camED = towards * (mouseSTpos.y - _mousePoint.y) + right * (mouseSTpos.x - _mousePoint.x);
        }

        if (CamPosition)
        {
            camED *= CamPosition.localPosition.y / 45;
        }

        //CamPoint.position = camST + camED * 3;
    }
    private void DragTurn(Vector2 _mousePoint)
    {
        SetCameraTR();

        if (Screen.width > 0 && Screen.height > 0)
        {
            camED.x = (mouseSTpos.y - _mousePoint.y) * (80.0f / Screen.height);
            camED.y = (mouseSTpos.x - _mousePoint.x) * (150.0f / Screen.width);
            camED.z = 0;
        }

        Vector3 turnResult = camST + camED;
        turnResult.x = Mathf.Clamp(turnResult.x, 10.0f, 75.0f);
        turnResult.y = (turnResult.y + 360) % 360;

        //CamPoint.localEulerAngles = turnResult;
    }

    #endregion
    #region Input Events

    private void Sprint()
    {
        isSprinting = !isSprinting;
    }

    //Action Related
    private void Build()
    {
        //If using Mouse
        if (true)
        {
            Vector2 mousePoint = inputControls.Player.MousePoint.ReadValue<Vector2>();

            //RaycastHit is the collider that gets touched by the ray/laser
            RaycastHit hit;
            RaycastHit towcheck;
            //Ray is the class that contains what we need for the laser
            Ray ray = Camera.main.ScreenPointToRay(mousePoint);

            if (Physics.Raycast(ray, out hit, 500.0f, layerGrass))
            {
                if (Physics.Raycast(ray, out towcheck, 500.0f, layerTower))
                {
                    //nothing!
                }
                else if (BuildingFromInventory >= 0)
                {
                    Vector3 buildPOS = greenBuild.position;
                    GameObject BuildingTower = PlayerManager.Instance.RemoveFromInventory(BuildingFromInventory);
                    //TODO: Instantiate new Tower at this position (also check if this doesn't block anything)
                    GameObject tow = Instantiate(BuildingTower, nestTowers);
                    tow.transform.position = buildPOS;

                    greenBuild.position = new Vector3(50, -10, 50);

                    StopBuildMode();

                    AudioManager.Instance.Play("PlayerPlaceTowerCommand");
                }
            }
        }
    }
    private void Select()
    {
        //If using Mouse
        if (true)
        {
            Vector2 mousePoint = inputControls.Player.MousePoint.ReadValue<Vector2>();

            //RaycastHit is the collider that gets touched by the ray/laser
            RaycastHit hit;
            //Ray is the class that contains what we need for the laser
            Ray ray = Camera.main.ScreenPointToRay(mousePoint);

            if (Physics.Raycast(ray, out hit, 500.0f, layerTower))
            {
                GameObject towObj = hit.collider.gameObject;

                if (PlayerManager.Instance != null)
                {
                    if (PlayerManager.Instance.PickUpTower(towObj))
                    {
                        //success inserted to Inventory
                        Destroy(towObj);
                    }
                }

            }
        }
    }
    private void Command()
    {
        //If using Mouse
        if (true)
        {
            Vector2 mousePoint = inputControls.Player.MousePoint.ReadValue<Vector2>();

            //RaycastHit is the collider that gets touched by the ray/laser
            RaycastHit hit;
            //Ray is the class that contains what we need for the laser
            Ray ray = Camera.main.ScreenPointToRay(mousePoint);

            if (Physics.Raycast(ray, out hit, 100.0f, layerGround))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (PlayerManager.Instance)
                    {
                        PlayerManager.Instance.CommandUnit(hit.collider.transform.position);
                    }
                }
                else
                {
                    if (PlayerManager.Instance)
                    {
                        PlayerManager.Instance.CommandUnit(hit.point);
                    }
                }
            }
        }
    }

    //UI Related
    private void Pause()
    {

    }
    private void OpenMenu()
    {

    }

    public void GoBuildMode(int towID, int num)
    {
        SetActionType(true, CommandType.BuildMode);
        SetActionType(false, CommandType.StopBuildMode);

        greenCrys.SetActive(towID >= 3);
        greenProj.SetActive(towID < 3);
        BuildingFromInventory = num;

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBuildMode(true);
        }
    }
    public void StopBuildMode()
    {
        SetActionType(true, CommandType.Select);
        SetActionType(false, CommandType.CommandUnit);


        greenBuild.position = new Vector3(50, -10, 50);
        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.UndoCall();
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBuildMode(false);
        }
    }

    #endregion

}
