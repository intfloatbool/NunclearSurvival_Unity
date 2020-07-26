using System;
using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroController : MonoBehaviour
    {
        private MetroHolder _metroHolder;
        private MetroMapView _lastMetroMapView;

        private void Awake()
        {
            _metroHolder = GameHelper.MetroHolder;
            Assert.IsNotNull(_metroHolder, "_metroHolder != null");
        }

        public void OnPlayerClickOnMetro(MetroMapView metroMapView)
        {
            if (metroMapView == _lastMetroMapView)
                return;

            TryTransitionOnMetroStation(metroMapView);
            _lastMetroMapView = metroMapView;
        }

        private void TryTransitionOnMetroStation(MetroMapView metroMapView)
        {
            //TODO: Complete transition logic, checking for clearing and so on..
            //test
            _metroHolder.SetLastPlayerStation(metroMapView.MetroNameKey);
        }
    }
}
