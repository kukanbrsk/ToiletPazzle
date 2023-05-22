using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask exitMask;

    private const float Threshold = 0.001f;

    private int _counter;
    private Camera _mainCamera;
    private Vector2 _lastMousePosition;
    private RaycastHit _hit;
    private Player _currentPlayer;
    private Action _playerMovements;

    private Ray Ray => _mainCamera.ScreenPointToRay(Input.mousePosition);

    public event Action playerSelected;
    public Action playerCollided;

    void Start()
    {
        _mainCamera = Camera.main;
        _counter = FindObjectsOfType<Player>().Length;
    }

    void Update()
    {
        CheckPress();

        if (_currentPlayer == null)
            return;

        CheckDrag();
       
        CheckRelease();
    }

    private void CheckPress()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Physics.Raycast(Ray, out _hit, 20, playerMask))
            {
                playerSelected?.Invoke();
                if (_currentPlayer == null)
                {
                    _currentPlayer = _hit.collider.GetComponent<Player>();
                    _currentPlayer.ClearLine();
                    _playerMovements -= _currentPlayer.RoadToPoint;
                    _currentPlayer.StartDrawing();
                }
                else
                {
                    return;
                }
            }
        }
    }

    private void CheckDrag()
    {
        if (Input.GetMouseButton(0))
        {
            var worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var distance = Vector3.Distance(_lastMousePosition, worldMousePos);
            if (distance <= Threshold)
                return;

            _lastMousePosition = worldMousePos;
            _currentPlayer.DrawingLine(_lastMousePosition);
        }
    }

    private void CheckRelease()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _currentPlayer.StopDrawing();

            if (Physics.Raycast(Ray, out _hit, 20, exitMask))
            {
                var currentExit = _hit.collider.GetComponent<Exit>();
                if (_currentPlayer.typePlayers == currentExit.typePlayers || currentExit.typePlayers == PlayersType.Universal)
                {
                    _playerMovements += _currentPlayer.RoadToPoint;

                    _currentPlayer = null;
                    if (_playerMovements.GetInvocationList().Length == _counter)
                        _playerMovements.Invoke();

                    return;
                }
            }
            _currentPlayer.ClearLine();
            _currentPlayer = null;
        }
    }


}