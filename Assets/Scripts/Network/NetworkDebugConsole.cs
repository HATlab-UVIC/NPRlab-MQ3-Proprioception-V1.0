using System;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkDebugConsole : MonoBehaviour
{
    public static NetworkDebugConsole Singleton { get; private set; }
    public enum ConnectionStatus {
        Connected,
        Disconnected,
    }
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private int _lineCountMax = 19;
    private int _lineCount = 0;

    private void Awake() {
        if (Singleton != null)
        {
            throw new Exception($"Detected more than one instance of {nameof(NetworkDebugConsole)}! " +
                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }

    private void Start() {
        if (Singleton != this)
        {
            return;
        }
        if (NetworkManager.Singleton == null)
        {
            throw new Exception($"There is no {nameof(NetworkManager)} for the {nameof(NetworkDebugConsole)} to do stuff with! " +
                $"Please add a {nameof(NetworkManager)} to the scene.");
        }
    }

    public void SetDebugString(string str) {
        string[] lines = _tmpText.text.Split(new[] { '\n' }, StringSplitOptions.None);
        if (_lineCount > 0)
        {
            if (_lineCount >= _lineCountMax)
            {
                int index = _tmpText.text.IndexOf(System.Environment.NewLine);
                _tmpText.text = string.Join("\n", lines.Skip(1)); ;
                _lineCount--;
            }
        }
        _tmpText.text += DateTime.Now.ToString("HH:mm:ss") + " " + str + "\n";
        _lineCount++;
        Debug.Log(_tmpText.text);
    }

}
