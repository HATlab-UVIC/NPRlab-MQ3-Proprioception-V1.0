using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty _pinchAnimationAction;
    [SerializeField] private Animator _handAnimator;

    private float _prevPinchAnimationActionValue;

    private OVRPlugin.Hand _leftHand = OVRPlugin.Hand.HandLeft;
    private OVRPlugin.Hand _rightHand = OVRPlugin.Hand.HandRight;
    private OVRPlugin.Posef _previousPoseHandGripLeft;
    private event Action<OVRPlugin.Posef> _onGetActionPoseChanged;
    private string _actionGripPose = "pose";
    private string _actionInputTriggerValue = "trigger";

    private void Awake() {
        
    }

    void Start()
    {
        _previousPoseHandGripLeft = new OVRPlugin.Posef();
        _prevPinchAnimationActionValue = _pinchAnimationAction.action.ReadValue<float>();
        NetworkDebugConsole.Singleton.SetDebugString("First left input trigger value: " + _prevPinchAnimationActionValue);
    }

    void Update()
    {
        /*OVRPlugin.Posef _currentPoseHandGripLeft = new OVRPlugin.Posef();
        if (OVRPlugin.GetActionStatePose(_actionGripPose, _leftHand, out _currentPoseHandGripLeft))
        {
            if (!_currentPoseHandGripLeft.Equals(_previousPoseHandGripLeft))
            {
                _onGetActionPoseChanged?.Invoke(_currentPoseHandGripLeft);
                _previousPoseHandGripLeft = _currentPoseHandGripLeft;
            }
        }
        
        float _leftInputTriggerValue;
        if (OVRPlugin.GetActionStateFloat(_actionInputTriggerValue, out _leftInputTriggerValue))
        {
            NetworkDebugConsole.Singleton.Debug("Left input trigger value by action set: " + _leftInputTriggerValue);
            _handAnimator.SetFloat("Trigger", _leftInputTriggerValue);
        }
        else if (_prevPinchAnimationActionValue != _pinchAnimationAction.action.ReadValue<float>())*/
        if (_prevPinchAnimationActionValue != _pinchAnimationAction.action.ReadValue<float>())
        {
            float triggerValue = _pinchAnimationAction.action.ReadValue<float>();
            NetworkDebugConsole.Singleton.SetDebugString("Left input trigger value: " + triggerValue);
            _handAnimator.SetFloat("Trigger", triggerValue);

        }
    }
}
