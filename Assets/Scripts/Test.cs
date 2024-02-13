using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        
        private void OnEnable()
        {
            foreach (var item in items)
            {
                item.Pressed += OnItemPressed; // подписываемся на все объекты
            }
        }

        private void OnDisable()
        {
            foreach (var item in items)
            {
                item.Pressed -= OnItemPressed;
            }
        }

        private void OnItemPressed(Item item)
        {
            //тут мы знаем что мы нажали вот на этот объект по этому можно уже дальше че то думать
        }
    }

    public class Item : MonoBehaviour
    {
        public event Action<Item> Pressed; //в треугольных скобочках указываются аргументы. В данном случае мы указали что аргументом будет этот же самый класс

        public void PressBuy() //предположим что этот метод вызывается при нажатии кнопки в юнити
        {
            Pressed?.Invoke(this); // в Invoke передаем все указанные нами аргументы. А конкретнее передаем наш объект, что мы нажали
        }
    }
}