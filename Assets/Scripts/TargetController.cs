using UnityEngine;

public class TargetController : MonoBehaviour
{
    [HideInInspector] public int index;
    [SerializeField] private float _disappearingDuration = 2f;
    private float _disappearTime = 0;
    private bool _startDisappearing = false;
    private Material _material;
    void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().material;
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
        _startDisappearing = true;
    }

    public void Capture() {
        ControlManager.Singleton.SendCaptureToServer(index + 1);
    }
}
