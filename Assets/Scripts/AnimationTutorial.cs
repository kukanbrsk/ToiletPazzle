using UnityEngine;
using DG.Tweening;

public class AnimationTutorial : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject handTouch;
    [SerializeField] private Transform start;
    [SerializeField] private Transform target;

    private const float Duration = 1.5f;

    private Sequence _circleSequence;
    private Sequence _handSequence;

    void Start()
    {
        FindObjectOfType<Game>().playerSelected += KillAnimation;
        MoveHand();
        CircleScale();
    }

    private void CircleScale()
    {
        _circleSequence = DOTween.Sequence();
        _circleSequence.Append(circle.transform.DOScale(0, Duration))
            .Append(circle.transform.DOScale(1, Duration))
            .SetLoops(-1, LoopType.Restart);
    }

    private void MoveHand()
    {
       _handSequence = DOTween.Sequence();
        var targetPos = target.transform.position;
        var startPos = target.transform.position;

        _handSequence.Append(handTouch.transform.DOMove(new Vector3(targetPos.x + 1, start.transform.position.y), Duration))
            .Append(handTouch.transform.DOMove(new Vector3(startPos.x + 1, startPos.y), Duration))
                        .SetLoops(-1, LoopType.Yoyo);
    }

    private void KillAnimation()
    {
        FindObjectOfType<Game>().playerSelected -= KillAnimation;
        _circleSequence.Kill();
        _handSequence.Kill();
        circle.SetActive(false);
        handTouch.SetActive(false);
    }
}
