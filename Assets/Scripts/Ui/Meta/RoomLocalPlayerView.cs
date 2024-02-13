using System;
using System.Collections.Generic;
using System.Linq;
using Color;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Meta
{
    public class RoomLocalPlayerView : MonoBehaviour
    {
        private const string _readyPlayerButtonText = "Ready";
        private const string _cancelReadyPlayerButtonText = "Cancel";
        
        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private TextMeshProUGUI _readyButtonText;
        [Space]
        [SerializeField] private TMP_Dropdown _dropdown;

        private List<Image> _dropdownSpawnedItems = new List<Image>();

        private NetworkRoomManagerFootball _networkManager;
        private NetworkRoomPlayerFootball _localPlayer;

        private bool _isReady = false;
        private ColorConfig _config;
        
        public void Init(NetworkRoomPlayerFootball localPlayer, ColorConfig config)
        {
            _config = config;
            _localPlayer = localPlayer;
            _localPlayer.ReadyChanged += LocalPlayerViewReady;

            LocalPlayerViewReady(_localPlayer.readyToBegin);

            InitDropdown();
        }

        private void InitDropdown()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_config.Colors.Select(v => v.Key.ToString()).ToList());
        }

        public void DeInit()
        {
            if (!_localPlayer)
                return;

            _localPlayer.ReadyChanged -= LocalPlayerViewReady;

            _localPlayer = null;

            DeInitDropdown();
        }
        

        private void DeInitDropdown()
        {
            foreach (var view in _dropdownSpawnedItems)
            {
                Destroy(view.gameObject);
            }
        }
        
        private void OnEnable()
        {
            _networkManager = NetworkRoomManagerFootball.Instance;

            _leaveButton.onClick.AddListener(OnLeaveButtonClicked);
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _readyButton.onClick.AddListener(OnReadyButtonClicked);
            
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void Update()
        {
            if(!_localPlayer)
                return;
            _startButton.gameObject.SetActive(_networkManager.allPlayersReady && _localPlayer.isServer);
            //мне не нравится но на ивенты переносить муторно
        }

        private void OnDisable()
        {
            _leaveButton.onClick.RemoveListener(OnLeaveButtonClicked);
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _readyButton.onClick.RemoveListener(OnReadyButtonClicked);
        }

        private void LocalPlayerViewReady(bool isReady)
        {
            _isReady = isReady;
            _readyButtonText.text = isReady ? _cancelReadyPlayerButtonText : _readyPlayerButtonText;
        }

        private void OnLeaveButtonClicked()
        {
            _networkManager.StopClient();

            if(NetworkServer.active)
            {
                _networkManager.StopServer();
            }
        }

        private void OnStartButtonClicked()
        {
            if(!_networkManager.allPlayersReady)
                return;
            if(!_localPlayer.isServer)
                return;
            _networkManager.ServerChangeScene(_networkManager.GameplayScene);
        }

        private void OnReadyButtonClicked()
        {
            _localPlayer.CmdChangeReadyState(!_isReady);
        }

        private void OnDropdownValueChanged(int index)
        {
            var viewByColorType = _config.Views[index];

            _localPlayer.CmdChangeColorType(viewByColorType.ColorType);
        }
    }
}