using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _openMenu;

    public void CloseMenu() {
        _menu.SetActive(false);
        _openMenu.SetActive(true);
    }

    public void OpenMenu() {
        _menu.SetActive(true);
        _openMenu.SetActive(false);
    }
}
