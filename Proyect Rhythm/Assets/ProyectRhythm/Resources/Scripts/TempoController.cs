using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RP.core
{
    public class TempoController : MonoBehaviour
    {
        [FoldoutGroup("Settings"), SerializeField] private float BPM = 60;
        private float beatInterval;
        private float currentTimer = 0;

        public event Action Tick;
        void Start()
        {
            beatInterval = 60 / BPM;
        }

        void FixedUpdate()
        {
            OnBeatMechanism();
        }
        private void OnBeatMechanism()
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= beatInterval)
            { 
                Tick?.Invoke();
                currentTimer = 0;
            }
        }
        public int GetCurrentTempo()
        {
            int accuary = (int)((currentTimer * 100) / beatInterval);
            print(accuary +"%");

            return accuary;
        }
        public void SetBPM(int bpm)
        {
            BPM = bpm;
            beatInterval = 60 / bpm;
        }
        public float GetIterval()
        {
            return beatInterval;
        }

    }
}
