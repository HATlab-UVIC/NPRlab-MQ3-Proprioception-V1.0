using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _menu;
    // [SerializeField] UnityEngine.Object _menuSurface;
    // [SerializeField] PointableCanvas _menuPointableCanvas; 
    [SerializeField] GameObject _openMenu;
    // [SerializeField] UnityEngine.Object _openMenuSurface;
    // [SerializeField] PointableCanvas _openMenuPointableCanvas; 
    private PokeInteractable _pokeInteractable;
    private RayInteractable _rayInteractable;
    private PointableCanvasUnityEventWrapper _pointableCanvasUnityEventWrapper;

    private void Start() {
        // _pointableCanvasUnityEventWrapper = transform.GetChild(0).GetComponent<PointableCanvasUnityEventWrapper>();
    }

    public void CloseMenu() {
        _menu.SetActive(false);
        _openMenu.SetActive(true);
        // _pokeInteractable.InjectSurfacePatch((ISurfacePatch)_openMenuSurface);
        // _rayInteractable.InjectSurface((ISurface)_openMenuSurface);
    }

    public void OpenMenu() {
        _menu.SetActive(true);
        _openMenu.SetActive(false);
        // _pokeInteractable.InjectSurfacePatch((ISurfacePatch) _menuSurface);
        // _rayInteractable.InjectSurface((ISurface)_menuSurface);
    }
}
