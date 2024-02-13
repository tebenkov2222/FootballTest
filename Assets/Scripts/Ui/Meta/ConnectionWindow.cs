using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Meta
{
    public class ConnectionWindow : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;

        private NetworkManager _networkManager;
        
        private void OnEnable()
        {
            _networkManager = FindFirstObjectByType<NetworkManager>();
            _hostButton.onClick.AddListener(OnHostButtonClicked);
            _clientButton.onClick.AddListener(OnClientButtonClicked);
        }

        private void OnDisable()
        {
            _hostButton.onClick.RemoveListener(OnHostButtonClicked);
            _clientButton.onClick.RemoveListener(OnClientButtonClicked);
        }

        private void OnHostButtonClicked()
        {
            _networkManager.StartHost();
        }

        private void OnClientButtonClicked()
        {
            _networkManager.networkAddress = "localhost";
            _networkManager.StartClient();
        }
    }
}