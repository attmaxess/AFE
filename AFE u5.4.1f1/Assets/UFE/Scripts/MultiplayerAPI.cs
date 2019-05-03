using UnityEngine;
using System;
using System.Collections.ObjectModel;

public abstract class MultiplayerAPI : MonoBehaviour
{
#if !UNITY_WEBGL
    #region public delegate definitions: Common Delegates
    public delegate void OnMessageReceivedDelegate(byte[] bytes);
    #endregion

    #region public delegate definitions: Client Delegates
    public delegate void OnDisconnectionDelegate();
    public delegate void OnLanGamesDiscoveredDelegate(ReadOnlyCollection<string> addresses);
    public delegate void OnLanGamesDiscoveryErrorDelegate();
    public delegate void OnServerConnectionErrorDelegate();
    public delegate void OnServerConnectionSuccessDelegate();
    #endregion

    #region public delegate definitions: Server Delegates
    public delegate void OnPlayerConnectedToServerDelegate();
    public delegate void OnPlayerDisconnectedFromServerDelegate();
    public delegate void OnServerInitializationErrorDelegate();
    public delegate void OnServerStartedDelegate();
    public delegate void OnServerStoppedDelegate();
    #endregion

    #region public event definitions: Common Events
    public event OnMessageReceivedDelegate OnMessageReceived;
    #endregion

    #region public class event definitions: Client Events
    public event OnDisconnectionDelegate OnDisconnection;
    public event OnLanGamesDiscoveredDelegate OnLanGamesDiscovered;
    public event OnLanGamesDiscoveryErrorDelegate OnLanGamesDiscoveryError;
    public event OnServerConnectionErrorDelegate OnServerConnectionError;
    public event OnServerConnectionSuccessDelegate OnServerConnectionSuccess;
    #endregion

    #region public event definitions: Server Events
    public event OnPlayerConnectedToServerDelegate OnPlayerConnectedToServer;
    public event OnPlayerDisconnectedFromServerDelegate OnPlayerDisconnectedFromServer;
    public event OnServerInitializationErrorDelegate OnServerInitializationError;
    public event OnServerStartedDelegate OnServerStarted;
    public event OnServerStoppedDelegate OnServerStopped;
    #endregion

    #region public abstract properties
    public abstract int Connections { get; }
    public abstract float SendRate { get; set; }
    #endregion

    #region private instance fields
    protected string _uuid = null;
    #endregion

    #region public instance methods
    public virtual bool Initialize(string uuid)
    {
        if (uuid == null)
        {
            throw new ArgumentNullException("uuid");
        }

        this._uuid = uuid;
        return true;
    }
    #endregion

    #region public abstract methods
    // Client
    public abstract void Connect(string ip, int port);
    public abstract bool Disconnect();
    public abstract void FindLanGames();

    // Common
    public abstract NetworkState GetConnectionState();

    // Server
    public abstract void StartServer(int connections, int listenPort, bool useNat);
    public abstract bool StopServer();
    #endregion

    #region public instance methods
    public virtual int GetLastPing()
    {
        return -1;
    }

    public bool IsClient()
    {
        return this.GetConnectionState() == NetworkState.Client;
    }

    public bool IsConnected()
    {
        return this.GetConnectionState() != NetworkState.Disconnected;
    }

    public bool IsServer()
    {
        return this.GetConnectionState() == NetworkState.Server;
    }

    public bool SendNetworkMessage<T>(NetworkMessage<T> message)
    {
        return this.SendNetworkMessage(message.Serialize());
    }
    #endregion

    #region protected abstract methods
    protected abstract bool SendNetworkMessage(byte[] bytes);
    #endregion

    #region protected instance methods: Common Events
    protected virtual void RaiseOnMessageReceived(byte[] bytes)
    {

    }
    #endregion

    #region protected instance methods: Client Events
    protected virtual void RaiseOnDisconnection()
    {
        
    }

    protected virtual void RaiseOnOnLanGamesDiscovered(ReadOnlyCollection<string> addresses)
    {
        if (this.OnLanGamesDiscovered != null)
        {
            this.OnLanGamesDiscovered(addresses);
        }
    }

    protected virtual void RaiseOnLanGamesDiscoveryError()
    {
        
    }

    protected virtual void RaiseOnServerConnectionError()
    {
        
    }

    protected virtual void RaiseOnServerConnectionSuccess()
    {
        if (this.OnServerConnectionSuccess != null)
        {
            this.OnServerConnectionSuccess();
        }
    }
    #endregion

    #region protected instance methods: Server Events
    protected virtual void RaiseOnPlayerConnectedToServer()
    {
        
    }

    protected virtual void RaiseOnPlayerDisconnectedFromServer()
    {
        
    }

    protected virtual void RaiseOnServerInitializationError()
    {
        
    }

    protected virtual void RaiseOnServerStarted()
    {
        if (this.OnServerStarted != null)
        {
            this.OnServerStarted();
        }
    }

    protected virtual void RaiseOnServerStopped()
    {
        if (this.OnServerStopped != null)
        {
            this.OnServerStopped();
        }
    }
    #endregion
#endif
}
