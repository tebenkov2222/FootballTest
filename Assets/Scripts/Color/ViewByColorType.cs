using System;
using UnityEngine;

namespace Color
{
    [Serializable]
    public class ViewByColorType
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private UnityEngine.Color _colorView;
        [SerializeField] private Material _materialPlayer;
        
        public ColorType ColorType => _colorType;
        public UnityEngine.Color ColorView => _colorView;
        public Material MaterialPlayer => _materialPlayer;
    }
}