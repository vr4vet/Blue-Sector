﻿using UnityEngine;
using UnityEngine.Events;

namespace InteractiveCalculator
{
    public class CalculatorButton : MonoBehaviour
    {
        public AudioSource player;
        public UnityEvent OnClick;
        [SerializeField]
        private float _travelDistance = 0.05f;
        private Vector3 _homePosition;
        private bool _isPressed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Finger"))
            {
                if (player != null)
                {
                    player.Play();
                }
                OnClick?.Invoke();
                //Press();
            }
        }
        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.gameObject.tag == "Finger")
        //     {
        //         Release();
        //     }
        // }
        
        private void Press()
        {
            if (_isPressed) return;
            transform.localPosition = _homePosition + Vector3.down * _travelDistance;
            _isPressed = true;
        }

        private void Release()
        {
            if (!_isPressed) return;
            transform.localPosition = _homePosition;
            _isPressed = false;
        }

        private void Awake()
        {
            _homePosition = transform.localPosition;
        }

        public void OnMouseDown()
        {
            OnClick?.Invoke();
            Press();
        }

        public void OnMouseUp()
        {
            Release();
        }

        public void OnMouseExit()
        {
            Release();
        }
    }
}