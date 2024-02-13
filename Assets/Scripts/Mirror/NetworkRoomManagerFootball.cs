using Color;
using Game;
using Mirror.Examples.NetworkRoom;
using UnityEngine;

namespace Mirror
{
    public delegate void ChangePlayerConnectionHandler(NetworkRoomPlayerFootball playerFootball);
    
    public class NetworkRoomManagerFootball : NetworkRoomManager
    {
        public event ChangePlayerConnectionHandler PlayerConnected;
        public event ChangePlayerConnectionHandler PlayerDisconnected;
        
        private static NetworkRoomManagerFootball _instance;
        public static NetworkRoomManagerFootball Instance => _instance;

        public override void Awake()
        {
            base.Awake();
            _instance = this;
        }
        
        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
            if (Utils.IsHeadless())
            {
                base.OnRoomServerPlayersReady();
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer,
            GameObject gamePlayer)
        {
            var gamePlayerFootball = gamePlayer.GetComponent<Player>();
            var roomPlayerFootball = roomPlayer.GetComponent<NetworkRoomPlayerFootball>();

            gamePlayerFootball.SetColor(roomPlayerFootball.ColorType);

            return true;
        }
        
        public void Add(NetworkRoomPlayerFootball networkRoomPlayerFootball)
        {
            roomSlots.Add(networkRoomPlayerFootball);
            PlayerConnected?.Invoke(networkRoomPlayerFootball);
        }
        
        public void Remove(NetworkRoomPlayerFootball networkRoomPlayerFootball)
        {
            roomSlots.Remove(networkRoomPlayerFootball);
            PlayerDisconnected?.Invoke(networkRoomPlayerFootball);
        }
    }
}
