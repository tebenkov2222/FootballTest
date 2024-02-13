using System;
using Color;

namespace Mirror
{
    public class NetworkRoomPlayerFootball : NetworkRoomPlayer
    {
        [SyncVar(hook = "ChangeColorTypeHookHandler")] private ColorType _colorType;
        public event Action<bool> ReadyChanged;
        public event Action<ColorType> ColorChanged;

        public ColorType ColorType => _colorType;

        public override void Start()
        {
            base.Start();
            
            if (NetworkManager.singleton is NetworkRoomManagerFootball room)
            {
                room.Add(this);
            }
        }

        public override void OnDisable()
        {
            if (NetworkClient.active && NetworkManager.singleton is NetworkRoomManagerFootball room)
            {
                room.Remove(this);
            }

            base.OnDisable();
        }

        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
            ReadyChanged?.Invoke(newReadyState);
        }
        
        public override void OnGUI()
        {
            base.OnGUI();
        }

        [Command]
        public void CmdChangeColorType(ColorType colorType)
        {
            _colorType = colorType;
            ColorChanged?.Invoke(colorType);
        }

        private void ChangeColorTypeHookHandler(ColorType oldColorType, ColorType newColorType)
        {
            ColorChanged?.Invoke(newColorType);
        }
    }
}
