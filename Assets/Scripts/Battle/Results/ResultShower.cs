using System;
using NunclearGame.Metro;
using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class ResultShower : MonoBehaviour
    {
        [SerializeField] private BattleResultController _resultController;
        [SerializeField] private GameObject[] _activatedObjectsByAnyResult;
        private void Awake()
        {
            Assert.IsNotNull(_resultController, "_resultController != null");
            if (_resultController != null)
            {
                _resultController.OnBattleFail += OnAnyResult;
                _resultController.OnBattleWin += OnAnyResult;
                
                _resultController.OnBattleFail += OnPlayerFail;
                _resultController.OnBattleWin += OnPlayerWin;
            }
        }

        private void OnDestroy()
        {
            if (_resultController != null)
            {
                _resultController.OnBattleFail -= OnAnyResult;
                _resultController.OnBattleWin -= OnAnyResult;
                
                _resultController.OnBattleFail -= OnPlayerFail;
                _resultController.OnBattleWin -= OnPlayerWin;
            }
        }

        private void OnAnyResult()
        {
            for (int i = 0; i < _activatedObjectsByAnyResult.Length; i++)
            {
                _activatedObjectsByAnyResult[i]?.SetActive(true);
            }
        }

        private void OnPlayerWin()
        {
            MetroHolder metroHolder = GameHelper.MetroHolder;
            if (metroHolder == null)
            {
                Debug.LogError("MetroHolder is missing!");
                return;
            }

            StationProperties currentStationInside = metroHolder.PotentialStationToGo;
            if (currentStationInside == null)
            {
                Debug.LogError("PotentialStation IS MISSING!");
                return;
            }

            metroHolder.SetLastPlayerStation(currentStationInside.Name);
        }

        private void OnPlayerFail()
        {
            
        }
    }
}
