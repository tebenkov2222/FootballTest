using System;
using Color;
using Game;
using Mirror;
using Mirror.Examples.NetworkRoom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Meta
{
    public class RoomPlayerView : MonoBehaviour
    {
        private const string _readyText = "Ready";
        private const string _notReadyText = "Not Ready";
        
        [SerializeField] private TextMeshProUGUI _isReady;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private Image _colorPlayer;

        private NetworkRoomPlayerFootball _playerFootball;
        private ColorConfig _config;

        public void Init(NetworkRoomPlayerFootball playerFootball, ColorConfig config)
        {
            _config = config;
            _playerFootball = playerFootball;
            if(!_playerFootball)
                return;
            
            _playerFootball.ReadyChanged += ShowCurrentIsReady;
            _playerFootball.ColorChanged += ShowCurrentColor;
            ShowCurrentIsReady(_playerFootball.readyToBegin);
            ShowCurrentColor(_playerFootball.ColorType);
            
            _playerName.text = "Player " + playerFootball.index.ToString();
        }
        
        private void OnDestroy()
        {
            if(!_playerFootball)
                return;
            
            _playerFootball.ReadyChanged -= ShowCurrentIsReady;
            _playerFootball.ColorChanged -= ShowCurrentColor;
        }
        
        private void ShowCurrentIsReady(bool isReady)
        {
            _isReady.text = isReady ? _readyText : _notReadyText;
        }

        private void ShowCurrentColor(ColorType colorType)
        {
            _colorPlayer.color = _config.Colors[colorType].ColorView;
        }
    }
}