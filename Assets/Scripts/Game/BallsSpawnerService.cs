using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Game
{
    public class BallsSpawnerService : NetworkBehaviour
    {
        [SerializeField] private Ball _prefabBall;
        [SerializeField] private Transform _root;
        [SerializeField] private int _startPull;

        private Pool<Ball> _balls;
        
        private static BallsSpawnerService _instance;

        public static BallsSpawnerService Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            _balls = new Pool<Ball>(InstantiateBall,0);
            SpawnStartPull();
        }

        private void SpawnStartPull()
        {
            for (int i = 0; i < _startPull; i++)
            {
                ReturnToPull(InstantiateBall());
            }
        }
        
        [Server]
        private Ball InstantiateBall()
        {
            var ball = Instantiate(_prefabBall, _root);
            NetworkServer.Spawn(ball.gameObject);
            return ball;
        }

        public void SpawnBall(Player player, Vector3 startPos, Vector3 direction, float impulse)
        {
            SpawnBallCmd(player, startPos, direction, impulse);
        }
        
        [Command(requiresAuthority = false)]
        private void SpawnBallCmd(Player player, Vector3 startPos, Vector3 direction, float impulse)
        {
            var ball = GetFromPull();
            ball.Init(player);
            NetworkServer.Spawn(ball.gameObject);
            ball.transform.position = startPos;
            ball.Rigidbody.AddForce(direction * impulse, ForceMode.Impulse);        
        }
        
        [Server]
        private Ball GetFromPull()
        {
            var ball =  _balls.Get();
            
            ball.Destroyed += OnBallDestroyed;
            ball.SetActive(true);
            
            return ball;
        }
        
        [Server]
        private void ReturnToPull(Ball ball)
        {
            ball.Rigidbody.velocity = Vector3.zero;
            ball.Rigidbody.angularVelocity = Vector3.zero;
            
            NetworkServer.UnSpawn(ball.gameObject); 

            ball.SetActive(false);

            _balls.Return(ball);
        }
        
        [Server]
        private void OnBallDestroyed(Ball ball)
        {
            ball.Destroyed -= OnBallDestroyed;

            ReturnToPull(ball);
        }
    }
}