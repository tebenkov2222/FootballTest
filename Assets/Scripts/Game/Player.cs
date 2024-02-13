using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        [SerializeField] private GoalMove _goalMove;
        [SerializeField] private List<MeshRenderer> _meshRenderersToChangeColor;
        [SerializeField] private ColorConfig _colorConfig; //todo: вынести бы в отдельный статичный класс со всеми конфигами чтобы не прокидывать по всем инстанциями
        
        [SyncVar(hook = nameof(ScoreHookHandler))] private int _score;
        [SyncVar(hook = nameof(ColorTypeHookHandler))] private ColorType _colorType;

        public int Score => _score;

        public Goal Goal => _goal;

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
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

            if (!isServer)
            {
                Destroy(_goalMove);
                Destroy(_goal);
            }
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
            ViewColor();
        }
        
        [Server]
        public void SetColor(ColorType colorType)
        {
            _colorType = colorType;
            ViewColor();
        }

        private void ViewColor()
        {
            foreach (var meshRenderer in _meshRenderersToChangeColor)
            {
                meshRenderer.material = _colorConfig.Colors[_colorType].MaterialPlayer;
            }
        }
    }
}