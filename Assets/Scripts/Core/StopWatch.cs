using System;
using UnityEngine;

namespace Core
{
    public class StopWatch
    {
        public event Action Elapsed;
        
        private float _elapsedSeconds;

        private float _currentSeconds;
        private bool _isElapsed;
        private bool _isEnabled;
        
        public StopWatch(float elapsedSeconds)
        {
            _elapsedSeconds = elapsedSeconds;
        }

        public float CurrentSeconds => _currentSeconds;

        public void Start()
        {
            _isEnabled = true;
        }

        public void Stop()
        {
            _isEnabled = false;
        }
        
        public void FixedTick()
        {
            if(!_isEnabled)
                return;
            
            if(_isElapsed)
                return;
            
            _currentSeconds = CurrentSeconds + Time.fixedDeltaTime;
            if (_currentSeconds < _elapsedSeconds)
                return;

            _currentSeconds = _elapsedSeconds;
            Elapsed?.Invoke();
            _isElapsed = true;
        }

        public void SetElapsedSeconds(float value)
        {
            _elapsedSeconds = value;
        }
        
        public void Reset()
        {
            _currentSeconds = 0;
            _isElapsed = false;
        }
        
        public void Restart()
        {
            Reset();
            Start();
        }
    }
}