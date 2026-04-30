using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [HideInInspector] public int index;
    [SerializeField] private float _disappearingDuration = 2f;
    private float _disappearTime = 0;
    private bool _startDisappearing = false;
    private Material _material;
    private Grabbable _grabbable;
    private HandGrabInteractable _handGrabInteractable;
    private GrabInteractable _grabInteractable;

    void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _grabbable = GetComponent<Grabbable>();
        _handGrabInteractable = transform.GetChild(0).GetComponent<HandGrabInteractable>();
        _grabInteractable = transform.GetChild(0).GetComponent<GrabInteractable>();
        _disappearTime = 0;
    }

    void Update()
    {
        if (_startDisappearing)
        {
            _disappearTime += Time.deltaTime;
            if ( _disappearTime > _disappearingDuration)
            {
                _startDisappearing = false;
                ControlManager.Singleton.DestroyThisTarget(transform);
                GameObject.Destroy(gameObject);
            }
            else
            {
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, (_disappearingDuration - _disappearTime) / _disappearingDuration);
                gameObject.GetComponent<MeshRenderer>().material = _material;
            }
        }
    }

    public void Disappear() {
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _startDisappearing = true;
        _grabbable.enabled = false;
        _handGrabInteractable.enabled = false;
        _grabInteractable.enabled = false;
    }

    public void DisappearNow() {
        ControlManager.Singleton.DestroyThisTarget(transform);
        GameObject.Destroy(gameObject);
    }

    public void Capture() {
        ControlManager.Singleton.SendCaptureToServer(index + 1, transform.position);
    }
}
