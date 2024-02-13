using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GoalMove : MonoBehaviour
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        [SerializeField] private Transform _goalRoot;
        [Space] 
        [SerializeField] private float _movementSpeedPercent;

        private float _curPercentPos = 0.5f;
        private int _direction = 1;

        private void Update()
        {
            _goalRoot.position = Vector3.Lerp(_start.position, _end.position, _curPercentPos);
            _curPercentPos += _movementSpeedPercent * _direction * Time.deltaTime;
            if (_curPercentPos > 1)
                _direction = -1;
            if (_curPercentPos < 0)
                _direction = 1;
            _curPercentPos = Mathf.Clamp01(_curPercentPos);
        }
    }
}