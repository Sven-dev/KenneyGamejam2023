using UnityEngine;

public enum CommandType
{
    Nothing,
    MoveCamera,
    RotateCamera,
    Information,
    CommandUnit
}

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    Camera Cam = null;

    [SerializeField]    Transform CamPoint = null;
    [SerializeField]    Transform CamLength = null;

    private GameplayControls inputControls = null;

    bool isSprinting = true;

    float MoveSpeed = 8.0f, SprintSpeed = 2.5f, ZoomSpeed = 5.0f;

    public CommandType typeOne = CommandType.MoveCamera;
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
            case CommandType.Information:
                Select();
                break;
            case CommandType.CommandUnit:
                Command();
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

        CameraMove(isMoveActive, isTurnActive);
        CameraTurns();
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
        towards = CamPoint.position - CamLength.position;
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
                    size = Mathf.Clamp(size + -Mathf.Sign(zoom) * ZoomSpeed, 4.0f, 40.0f);
                    Cam.orthographicSize = size;
                }
                else
                {
                    Vector3 length = CamLength.localPosition;
                    length.y = Mathf.Clamp(length.y + -Mathf.Sign(zoom) * ZoomSpeed, 4.0f, 60.0f);
                    CamLength.localPosition = length;
                }
            }
        }

    }

    private void UpdateMovement(Vector3 _moveTo)
    {
        if (CamPoint)
        {
            if (isSprinting)
                _moveTo *= SprintSpeed;

            CamPoint.position += _moveTo * MoveSpeed * Time.unscaledDeltaTime;
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

        if (CamLength)
        {
            camED *= CamLength.localPosition.y / 45;
        }

        CamPoint.position = camST + camED;
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

        CamPoint.localEulerAngles = turnResult;
    }

    #endregion
    #region Input Events

    private void Sprint()
    {
        isSprinting = !isSprinting;
    }

    //Action Related
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

            if (Physics.Raycast(ray, out hit, 500.0f))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {


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

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (PlayerManager.Instance)
                    {
                        PlayerManager.Instance.CommandUnit(hit.collider.transform.position, hit.collider.gameObject);
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
            else
            {


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

    #endregion

}
