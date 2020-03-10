using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using GoMap;
using System.Linq;
using GoShared;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GoDirections
{

    public class GOToAddressDemo : MonoBehaviour
    {

        public InputField inputField;
        public Button button;
        public GODirections goDirections;
        public GOMap goMap;
        public GameObject addressMenu;
        public GODirections.GOTravelModes travelMode = GODirections.GOTravelModes.transit;
        public bool UseGoogleAPI = false;

        GameObject addressTemplate;

        public void Start()
        {

            addressTemplate = addressMenu.transform.Find("Address template").gameObject;

            inputField.onEndEdit.AddListener(delegate (string text)
            {
                GoToAddress();
            });
        }


        public void GoToAddress()
        {

            if (inputField.text.Any(char.IsLetter))
            { //Text contains letters
                SearchAddress();
            }
            else if (inputField.text.Contains(","))
            {

                string s = inputField.text;
                Coordinates coords = new Coordinates(inputField.text);
                DirectionsToCoordinates(coords);
            }
        }

        public void SearchAddress()
        {

            addressMenu.SetActive(false);
            string completeUrl;

            if (UseGoogleAPI)
            {
                //https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=YOUR_API_KEY
                string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=";
                string apiKey = goDirections.googleAPIkey;
                string text = inputField.text;
                completeUrl = baseUrl + UnityWebRequest.EscapeURL(text) + "&key=" + apiKey;
            }
            else if (goMap.mapType == GOMap.GOMapType.Nextzen)
            {
                string baseUrl = "https://tile.nextzen.org/tilezen/vector/v1/search?";
                string apiKey = goMap.nextzen_api_key;
                string text = inputField.text;
                completeUrl = baseUrl + "&text=" + UnityWebRequest.EscapeURL(text) + "&api_key=" + apiKey;
            }
            else
            {
                string baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
                string apiKey = goMap.mapbox_accessToken;
                string text = inputField.text;
                completeUrl = baseUrl + UnityWebRequest.EscapeURL(text) + ".json" + "?access_token=" + apiKey;

                if (goMap.locationManager.currentLocation != null)
                {
                    completeUrl += "&proximity=" + goMap.locationManager.currentLocation.longitude + "%2C" + goMap.locationManager.currentLocation.latitude;
                }
                else if (goMap.locationManager.worldOrigin != null)
                {
                    completeUrl += "&proximity=" + goMap.locationManager.worldOrigin.longitude + "%2C" + goMap.locationManager.worldOrigin.latitude;
                }
            }
            Debug.Log(completeUrl);

            IEnumerator request = GOUrlRequest.jsonRequest(this, completeUrl, false, null, (Dictionary<string, object> response, string error) =>
            {

                if (UseGoogleAPI && (string)response["status"] == "OK")
                {

                    IList features = (IList)response["results"];
                    LoadGoogleChoices(features);

                }
                else if (string.IsNullOrEmpty(error))
                {
                    IList features = (IList)response["features"];
                    LoadChoices(features);
                }

            });

            StartCoroutine(request);
        }

        public void LoadGoogleChoices(IList features)
        {

            while (addressMenu.transform.childCount > 1)
            {
                foreach (Transform child in addressMenu.transform)
                {
                    if (!child.gameObject.Equals(addressTemplate))
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }

            for (int i = 0; i < Math.Min(features.Count, 5); i++)
            {

                IDictionary feature = (IDictionary)features[i];

                IDictionary geometry = (IDictionary)feature["geometry"];
                IDictionary coordinates = (IDictionary)geometry["location"];
                GOLocation location = new GOLocation();
                Coordinates coords = new Coordinates(Convert.ToDouble(coordinates["lat"]), Convert.ToDouble(coordinates["lng"]), 0);

                location.addressString = (string)feature["formatted_address"];

                location.coordinates = coords;

                if (features.Count == 1)
                {
                    DirectionsToAddress(location);
                    return;
                }

                GameObject cell = Instantiate(addressTemplate);
                cell.transform.SetParent(addressMenu.transform);
                cell.transform.GetComponentInChildren<Text>().text = location.addressString;
                cell.name = location.addressString;
                cell.SetActive(true);

                Button btn = cell.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    DirectionsToAddress(location);
                });

            }

            addressMenu.SetActive(true);

        }

        public void LoadChoices(IList features)
        {

            while (addressMenu.transform.childCount > 1)
            {
                foreach (Transform child in addressMenu.transform)
                {
                    if (!child.gameObject.Equals(addressTemplate))
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }

            for (int i = 0; i < Math.Min(features.Count, 5); i++)
            {

                IDictionary feature = (IDictionary)features[i];

                IDictionary geometry = (IDictionary)feature["geometry"];
                IList coordinates = (IList)geometry["coordinates"];
                GOLocation location = new GOLocation();
                IDictionary properties = (IDictionary)feature["properties"];
                Coordinates coords = new Coordinates(Convert.ToDouble(coordinates[1]), Convert.ToDouble(coordinates[0]), 0);

                if (goMap.mapType == GOMap.GOMapType.Nextzen)
                {
                    location.addressString = (string)properties["label"];
                }
                else
                {
                    location.addressString = (string)feature["matching_place_name"] ?? (string)feature["place_name"];
                }
                location.coordinates = coords;
                location.properties = properties;

                if (features.Count == 1)
                {
                    DirectionsToAddress(location);
                    return;
                }

                GameObject cell = Instantiate(addressTemplate);
                cell.transform.SetParent(addressMenu.transform);
                cell.transform.GetComponentInChildren<Text>().text = location.addressString;
                cell.name = location.addressString;
                cell.SetActive(true);

                Button btn = cell.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    DirectionsToAddress(location);
                });

            }

            addressMenu.SetActive(true);

        }


        public void DirectionsToAddress(GOLocation location)
        {
            inputField.text = location.addressString;
            addressMenu.SetActive(false);
            if (inputField.text.Length > 0 && gameObject.activeSelf)
                StartCoroutine(goDirections.RequestDirectionsFromUserLocationToAddress(goDirections.goMap.locationManager.currentLocation, location.addressString, travelMode));

        }

        public void DirectionsToCoordinates(Coordinates coordinates)
        {
            inputField.text = coordinates.toLatLongString();
            addressMenu.SetActive(false);
            if (inputField.text.Length > 0 && gameObject.activeSelf)
                StartCoroutine(goDirections.RequestDirectionsWithCoordinates(goDirections.goMap.locationManager.currentLocation, coordinates, null, travelMode));

        }

    }
}
