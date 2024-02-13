using System;
using Core;
using Mirror;
using UnityEngine;

namespace Game
{
    public class Ball : NetworkBehaviour
    {
        public event Action<Ball> Destroyed;

        [SerializeField] private float _lifeSeconds;
        
        [SerializeField] private Rigidbody _rigidbody;

        private StopWatch _lifeTimeSeconds;
        [SyncVar] private Player _player;

        public Rigidbody Rigidbody => _rigidbody;
        
        private void Awake()
        {
            _lifeTimeSeconds = new StopWatch(_lifeSeconds);
        }

        public void Init(Player player)
        {
            _player = player;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            _lifeTimeSeconds.Elapsed += OnLifeTimeElapsed;
            _lifeTimeSeconds.Restart();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            
            _lifeTimeSeconds.Stop();
            _lifeTimeSeconds.Elapsed -= OnLifeTimeElapsed;
        }
        
        private void OnLifeTimeElapsed()
        {
            Debug.Log($"OnLifeTimeElapsed");
            OnLifeTimeElapsedRpc();
            Destroyed?.Invoke(this);
        }
        
        [ClientRpc]
        private void OnLifeTimeElapsedRpc()
        {
            Destroyed?.Invoke(this);
        }
        
        private void FixedUpdate()
        {
            if(!isServer)
                return;
            
            _lifeTimeSeconds.FixedTick();
        }

        [Server]
        public void CollidedWithGoal(Goal goal)
        {
            Destroyed?.Invoke(this);
            if(_player.Goal != goal)
                _player.AddScore();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}