using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
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

            // StartSessionAsHost();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async Task<Dictionary<string, PlayerProperty>> GetPlayerProperties() {
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        var playerNameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        return new Dictionary<string, PlayerProperty> { { _playerNamePropertyKey, playerNameProperty } };
    }

    public async void JoinSessionByCode() {
        string sessionJoinCode = _joinCode.text.Replace("\u200B", "");
        _ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionJoinCode);
        Debug.Log($"Connected to {NetworkManager.Singleton.ConnectedClientsIds} with code {sessionJoinCode}");
        NetworkDebugConsole.Singleton.SetDebugString($"Connected to {NetworkManager.Singleton.ConnectedClientsIds[0]} with code {sessionJoinCode}");
    }

    public async void Quit() {
        _joinCode.text = string.Empty;
        QuerySessionsOptions _queryOption = new QuerySessionsOptions();
        await MultiplayerService.Instance.QuerySessionsAsync(_queryOption);
        Debug.Log($"Exited session with code {_ActiveSession.Code}");
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
