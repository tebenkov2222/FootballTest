using System;
using Game;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Game
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private Gun _gun;

        [Space] 
        [SerializeField] private Image _filledAttack;
        [SerializeField] private Image _isReadyToAttack;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private static Hud _instance;
        private Player _player;

        public static Hud Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public void Init(Player player,Gun gun)
        {
            _player = player;
            _player.ScoreChanged += PlayerOnScoreChanged;
            _gun = gun;
        }

        private void OnDestroy()
        {
            if(_player == null) 
                return;
            
            _player.ScoreChanged -= PlayerOnScoreChanged;
        }

        private void Update()
        {
            if(_gun == null)
                return;
            _isReadyToAttack.gameObject.SetActive(_gun.IsReadyToAttack);
            _filledAttack.fillAmount = _gun.CurImpulseMultiplier;
        }

        private void PlayerOnScoreChanged(int oldvalue, int newvalue)
        {
            _scoreText.text = newvalue.ToString();
        }
    }
}