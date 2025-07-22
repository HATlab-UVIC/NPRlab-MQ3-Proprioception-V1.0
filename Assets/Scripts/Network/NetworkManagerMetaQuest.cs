using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEngine;

public class NetworkManagerMetaQuest : MonoBehaviour
{
    [SerializeField] TMP_Text _joinCode;
    private ISession _activeSession;
    private string _playerNamePropertyKey = "playerName";


    ISession _ActiveSession
    {
        get => _activeSession;
        set
        {
            _activeSession = value;
            Debug.Log($"Active session: {_activeSession}");
        }
    }


    private async void Start() {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously successed! Player ID: {AuthenticationService.Instance.PlayerId}");

            // JoinSessionByCode();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // This is for testing if joining and quiting work
    /*private float _timer = 0;
    private void Update() {
        _timer += Time.deltaTime;
        if (_timer > 6.0f)
        {
            _timer = 0;

            if ( _activeSession == null || _activeSession.State == SessionState.Disconnected)
            {
                JoinSessionByCode();
            }
            else
            {
                QuitSession();
            }
        }
    }*/

    private async Task<Dictionary<string, PlayerProperty>> GetPlayerProperties() {
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        var playerNameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        return new Dictionary<string, PlayerProperty> { { _playerNamePropertyKey, playerNameProperty } };
    }

    public async void JoinSessionByCode() {
        string sessionJoinCode = _joinCode.text.Replace("\u200B", "");
        try
        {
            _ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionJoinCode);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            NetworkDebugConsole.Singleton.SetDebugString($"{e.Message}");
            return;
        }
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkDebugConsole.Singleton.SetDebugString($"Connected to to host with code {sessionJoinCode}");
        }
    }

    public async void QuitSession() {
        _joinCode.text = string.Empty;
        QuerySessionsOptions _queryOption = new QuerySessionsOptions();

        if (_activeSession == null)
        {
            NetworkDebugConsole.Singleton.SetDebugString("Session does not exist");
            return;
        }
        else if (_activeSession.State == SessionState.Disconnected)
        {
            NetworkDebugConsole.Singleton.SetDebugString("Cannot quit session. Session is already disconnected");
        }
        try
        {
            string _playerId = AuthenticationService.Instance.PlayerId;
            await _activeSession.LeaveAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            NetworkDebugConsole.Singleton.SetDebugString($"{e.Message}");
            return;
        }
        NetworkDebugConsole.Singleton.SetDebugString($"Exited session with code {_ActiveSession.Code}");
    }


    private async void StartSessionAsHost() {
        var playerProperties = await GetPlayerProperties();

        var options = new SessionOptions
        {
            MaxPlayers = 2,
            IsLocked = false,
            IsPrivate = false,
            PlayerProperties = playerProperties,
        }.WithRelayNetwork();

        _ActiveSession = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session {_ActiveSession.Id} created! Join code: {_ActiveSession.Code}");
        NetworkDebugConsole.Singleton.SetDebugString($"Session {NetworkManager.Singleton.LocalClientId} created! Join code: {_ActiveSession.Code}");
    }

    private async Task LeaveSession() {
        if (_ActiveSession != null)
        {
            try
            {
                await _ActiveSession.LeaveAsync();
            }
            catch
            {
                // Ignored as we are exiting the game
            }
            finally
            {
                _ActiveSession = null;
            }
        }
    }

    private void OnDestroy() {
    }
}
