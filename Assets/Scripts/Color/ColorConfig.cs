using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Color
{
    [CreateAssetMenu(fileName = "ColorConfig", menuName = "ColorConfig", order = 0)]
    public class ColorConfig : ScriptableObject
    {
        [SerializeField] private List<ViewByColorType> _views;

        private Dictionary<ColorType, ViewByColorType> _colors;

        public Dictionary<ColorType, ViewByColorType> Colors => 
            _colors ??= Views.ToDictionary(c => c.ColorType, c => c);

        public List<ViewByColorType> Views => _views;
    }
}