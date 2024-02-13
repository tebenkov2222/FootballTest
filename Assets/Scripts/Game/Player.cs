using System;
using Color;
using Mirror;
using Ui.Game;
using UnityEngine;

namespace Game
{
    public delegate void IntValueChangedHandler(int oldValue, int newValue);
    
    public class Player : NetworkBehaviour
    {
        public event IntValueChangedHandler ScoreChanged;
        
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Gun _gun;
        [SerializeField] private Goal _goal;
        [SerializeField] private MeshRenderer _bodyMeshRenderer;
        [SerializeField] private ColorConfig _colorConfig; //todo: вынести бы в отдельный статичный класс со всеми конфигами чтобы не прокидывать по всем инстанциями
        
        [SyncVar(hook = nameof(ScoreHookHandler))] private int _score;
        [SyncVar(hook = nameof(ColorTypeHookHandler))] private ColorType _colorType;

        public int Score => _score;

        public Goal Goal => _goal;

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("Player OnStartServer");

        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("OnStartClient");
            if (!isOwned)
            {
                Destroy(_gun);
            }
            else
            {
                RelocateCamera();
                _gun.Init(this);
                Hud.Instance.Init(this, _gun);
            }
            
            if(!isServer)
                Destroy(_goal);

        }
        
        private void RelocateCamera()
        {
            var main = Camera.main;
            if(main == null)
                return;
            main.transform.SetParent(_cameraRoot);
            main.transform.localPosition = Vector3.zero;
            main.transform.localRotation = Quaternion.identity;

        }
        public void AddScore()
        {
            _score++;
        }

        private void ScoreHookHandler(int oldScore, int newScore)
        {
            ScoreChanged?.Invoke(oldScore, newScore);
        }

        private void ColorTypeHookHandler(ColorType oldColor, ColorType newColor)
        {
            SetColor(newColor);
        }
        
        public void SetColor(ColorType colorType)
        {
            _colorType = colorType;
            _bodyMeshRenderer.material = _colorConfig.Colors[colorType].MaterialPlayer;
        }
    }
}