using System.Collections.Generic;
using System.Linq;
using Color;
using Mirror;
using UnityEngine;

namespace Ui.Meta
{
    public class RoomWindow : MonoBehaviour
    {
        [SerializeField] private RoomLocalPlayerView _localPlayerView;
        [SerializeField] private RoomPlayerView _roomPlayerViewPrefab;
        [SerializeField] private Transform _rootPlayers;
        [Space]
        [SerializeField] private ColorConfig _colorConfig;

        private Dictionary<NetworkRoomPlayerFootball, RoomPlayerView> _views = new Dictionary<NetworkRoomPlayerFootball, RoomPlayerView>();

        private NetworkRoomManagerFootball _networkManager;

        private void OnEnable()
        {
            _networkManager = NetworkRoomManagerFootball.Instance;

            _networkManager.PlayerConnected += AddPlayer;
            _networkManager.PlayerDisconnected += RemovePlayer;
            
            foreach (var playerFootball in _networkManager.roomSlots.Select(networkManagerRoomSlot => networkManagerRoomSlot as NetworkRoomPlayerFootball))
            {
                AddPlayer(playerFootball);
            }
        }

        private void OnDisable()
        {
            _networkManager.PlayerConnected -= AddPlayer;
            _networkManager.PlayerDisconnected -= RemovePlayer;
        }

        private RoomPlayerView InstantiateView(NetworkRoomPlayerFootball playerFootball)
        {
            var roomPlayerView = Instantiate(_roomPlayerViewPrefab, _rootPlayers);
            roomPlayerView.Init(playerFootball, _colorConfig);
            return roomPlayerView;
        }
        
        private void AddPlayer(NetworkRoomPlayerFootball playerFootball)
        {
            if (playerFootball.isLocalPlayer)
            {
                _localPlayerView.Init(playerFootball, _colorConfig);
            }
            
            _views.Add(playerFootball, InstantiateView(playerFootball));
        }
        
        private void RemovePlayer(NetworkRoomPlayerFootball playerFootball)
        {
            if (playerFootball.isLocalPlayer)
            {
                _localPlayerView.DeInit();
            }
            
            if (_views.Remove(playerFootball, out var view))
            {
                Destroy(view.gameObject);
            }
        }
    }
}