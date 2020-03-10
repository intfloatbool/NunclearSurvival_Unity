using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;

namespace GoDirections
{
    public class GODirectionsRaycast : MonoBehaviour
    {

        [Layer] public int hitLayers = 0;
        public List<string> tags;
        public GODirections goDirections;
        public GODirections.GOTravelModes travelMode = GODirections.GOTravelModes.transit;

        public bool debugLog = false;
        private bool alert = false;

        //In the update we detect taps of mouse or touch to trigger a raycast on the ground
        void Update()
        {
            if (alert == true) return;

            bool drag = false;
            if (Application.isMobilePlatform)
            {
                drag = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
            }
            else
                drag = Input.GetMouseButton(0);

            if (drag)
            {

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && (tags.Count == 0 || tags.Contains(hit.transform.tag)))
                {

                    //From the raycast data it's easy to get the vector3 of the hit point 
                    Vector3 worldVector = hit.point;
                    //And it's just as easy to get the gps coordinate of the hit point.
                    Coordinates gpsCoordinates = Coordinates.convertVectorToCoordinates(hit.transform.position);

                    if (debugLog)
                    {
                        //There's a little debug string
                        Debug.Log(string.Format("[GOMap] Tap world vector: {0}, GPS Coordinates: {1}", worldVector, gpsCoordinates.toLatLongString()));
                    }

                    alert = true;
                    GOAlert.LoadAlert("Directions", "Do you want directions to this location?", new string[] { "Take me there!", "Cancel" }, delegate (int buttonIndex, string buttontext) {
                        alert = false;
                        switch (buttonIndex)
                        {
                            case 1:
                                break;
                            case 0:
                                goDirections.StartCoroutine(goDirections.RequestDirectionsFromUserLocation(gpsCoordinates, null, travelMode));
                                break;
                        }
                    });
                }
            }
        }
    }
}
