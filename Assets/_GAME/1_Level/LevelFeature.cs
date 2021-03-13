using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Level
{
    public class LevelFeature : MonoBehaviour
    {
        public Action InitLevel;
        public Action LevelInitialized;

        private void Start()
        {
            //init level on start
            InitLevel?.Invoke();
        }

        private void OnEnable()
        {
            InitLevel += InitializeLevel;
        }

        private void OnDisable()
        {
            InitLevel -= InitializeLevel;
        }

        
        //init level
        private void InitializeLevel()
        {
            
            LevelInitialized?.Invoke();
        }
    }
}
