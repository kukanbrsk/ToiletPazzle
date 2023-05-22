using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject backlight;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private PlayersType exitType;

    public PlayersType typePlayers => exitType;

    public void OnLight() => backlight.gameObject.SetActive(true);

    public void OffLight() => backlight.gameObject.SetActive(false);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && player.typePlayers == exitType)
        {
            uIManager.WindowWin();
            player.HappyAnimation();
        }
    }
}

public enum PlayersType
{
    Green,
    Pink,
    Universal
}