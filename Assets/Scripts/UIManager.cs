using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject windowWin;
    [SerializeField] private GameObject windowLoos;

    public void WindowWin() => windowWin.gameObject.SetActive(true);

    public void WindowLoos() => windowLoos.SetActive(true);
}
