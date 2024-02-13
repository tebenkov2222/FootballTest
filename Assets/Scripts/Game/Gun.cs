using System;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [Space] 
        [Header("Rotation")]
        [SerializeField] private float _horizontalAngleMin;
        [SerializeField] private float _horizontalAngleMax;
        [SerializeField] private float _verticalAngleMin;
        [SerializeField] private float _verticalAngleMax;
        [SerializeField] private float _horizontalRotationSpeed;
        [SerializeField] private float _verticalRotationSpeed;
        [Space] 
        [Header("Shot")]
        [SerializeField] private float _maxImpulseValue;
        [SerializeField] private float _secondsToMaxImpulse;
        [SerializeField] private AnimationCurve _curveImpulseValueByTime;
        [SerializeField] private float _secondsReload;
        
        private Vector2 _targetAngle;
        private Vector2 _currentAngle;

        private StopWatch _impulseTimeStopWatch;
        private float _curImpulseMultiplier;
        private float _curImpulseValue;

        private StopWatch _reloadStopWatch;
        private bool _isReadyToAttack = true;
        private bool _isAttacked;
        private Player _player;

        public bool IsReadyToAttack => _isReadyToAttack;
        public float CurImpulseMultiplier => _curImpulseMultiplier;

        public void Init(Player player)
        {
            _player = player;
        }
        
        private void Awake()
        {
            _impulseTimeStopWatch = new StopWatch(_secondsToMaxImpulse);
            _reloadStopWatch = new StopWatch(_secondsReload);
            _reloadStopWatch.Elapsed += OnReloaded;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Rotate();
            Attack();
        }

        private void FixedUpdate()
        {
            _impulseTimeStopWatch.FixedTick();
            _reloadStopWatch.FixedTick();
        }

        private void OnDisable()
        {
            _reloadStopWatch.Elapsed -= OnReloaded;
        }

        private void Rotate()
        {
            var horInput = Input.GetAxis("Mouse X");
            var verInput = Input.GetAxis("Mouse Y");
            var horDelta = _horizontalRotationSpeed * horInput * Time.deltaTime;
            var verDelta = _verticalRotationSpeed * verInput * Time.deltaTime;
            _targetAngle += new Vector2(verDelta, horDelta);
            _targetAngle = new Vector2(Mathf.Clamp(_targetAngle.x, _verticalAngleMin, _verticalAngleMax), Mathf.Clamp(_targetAngle.y, _horizontalAngleMin, _horizontalAngleMax));
            _currentAngle = _targetAngle;
            
            transform.localRotation = Quaternion.Euler(_currentAngle);
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_isReadyToAttack)
                {
                    _isReadyToAttack = false;
                    _isAttacked = true;
                    _impulseTimeStopWatch.Restart();
                }
            }

            if(!_isAttacked)
                return;
            
            if (Input.GetMouseButton(0))
            {
                var timePercent = _impulseTimeStopWatch.CurrentSeconds / _secondsToMaxImpulse;
                _curImpulseMultiplier = _curveImpulseValueByTime.Evaluate(timePercent);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _impulseTimeStopWatch.Stop();
                _reloadStopWatch.Restart();
                
                _curImpulseValue = CurImpulseMultiplier * _maxImpulseValue;

                BallsSpawnerService.Instance.SpawnBall(_player, _spawnPoint.position, _spawnPoint.forward,
                    _curImpulseValue);
    
                _impulseTimeStopWatch.Stop();
                _curImpulseMultiplier = 0;
                _curImpulseValue = 0;
                _isAttacked = false;
            }
        }

        private void OnReloaded()
        {
            _isReadyToAttack = true;
        }
    }
}
