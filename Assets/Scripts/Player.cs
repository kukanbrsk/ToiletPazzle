using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private Animator animator;
    [SerializeField] private Exit exit;
    [SerializeField] private LineRenderer line;
    [SerializeField] private PlayersType playersType;

    private Game _game;
    private List<Vector2> _wayToTheExit = new List<Vector2>();
    private List<Vector3> TheExit = new List<Vector3>();
    private Vector2 targetPosition;
    private TweenerCore<Vector3, Path, PathOptions> path;

    public PlayersType typePlayers => playersType;

    private void Start()
    {
        _game = FindObjectOfType<Game>();
        _game.playerCollided += OnPlayerCollided;

    }
    public void RoadToPoint()
    {
        foreach (Vector2 v2 in _wayToTheExit)
        {
            Vector3 v3 = new Vector3(v2.x, v2.y, 0);
            TheExit.Add(v3);
        }
        path = transform.DOPath(TheExit.ToArray(), speed * Time.deltaTime, PathType.Linear, PathMode.Ignore);
        animator.SetBool("isWalk", true);
    }

    public void HappyAnimation() => animator.SetBool("isHappy", true);

    public void DeathAnimation() => animator.SetTrigger("death");

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.TryGetComponent(out Exit exit))
        {
            _game.playerCollided?.Invoke();
        }
    }

    private void OnPlayerCollided()
    {
        DeathAnimation();
        path.Kill();
        uIManager.WindowLoos();
        _game.playerCollided -= OnPlayerCollided;
    }

    public void StartDrawing()
    {
        exit.OnLight();
    }

    public void StopDrawing()
    {
        exit.OffLight();
    }

    public void DrawingLine(Vector3 lastMousePosition)
    {
        _wayToTheExit.Add(lastMousePosition);
        UpdateLine();
    }

    private void UpdateLine()
    {
        line.positionCount = _wayToTheExit.Count;
        line.SetPosition(_wayToTheExit.Count - 1, _wayToTheExit[^1]);
    }

    public void ClearLine()
    {
        _wayToTheExit.Clear();
        line.positionCount = 0;
    }
}
