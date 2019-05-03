using UnityEngine;

public class NullMultiplayerAPI : MultiplayerAPI{
#if !UNITY_WEBGL
	#region public override properties
	public override int Connections{
		get{
			return 0;
		}
	}	
	
	public override float SendRate{get; set;}
	#endregion

	#region public override methods
	public override void Connect(string ip, int port){
		this.RaiseOnServerConnectionError();
	}

	public override bool Disconnect(){
		return true;
	}

	public override void FindLanGames(){
		this.RaiseOnLanGamesDiscoveryError();
	}

	public override NetworkState GetConnectionState(){
		return NetworkState.Disconnected;
	}

	public override void StartServer(int maxConnections, int listenPort, bool useNat){
		this.RaiseOnServerInitializationError();
	}

	public override bool StopServer(){
		return true;
	}
	#endregion

	#region protected override methods
	protected override bool SendNetworkMessage(byte[] bytes){
		return false;
	}
	#endregion
#endif
}
