using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoMap;
using GoShared;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace GoDirections {
	
	public class GODirections : MonoBehaviour {

		[Header("Required settings")]
		public GOMap goMap;
        public GODirectionsAPI directionsApi = GODirectionsAPI.Google;
		public string googleAPIkey;
        public string mapboxAPIkey;
		private string googleBaseUrl = "https://maps.googleapis.com/maps/api/directions/json?";
        private string mapboxBaseUrl = "https://api.mapbox.com/directions/v5/mapbox/";
        public String language;
		public bool useSubsteps = false;
		public bool useColliders = false;
		public bool automaticallyRemoveRoutes = true;

        public enum GODirectionsAPI
        {
            Google,
            Mapbox
        };

        public enum GOTravelModes{
			driving, 
			walking,
			bicycling,
			transit,
            drivingTraffic_MapboxOnly
		};

		[Header("Customization")]
		public GameObject startPrefab;
		public GameObject destinationPrefab;
		public GameObject waypointPrefab;
		public GameObject transitStopsPrefab;

		public GODirectionsRendering [] renderingOptions;

		[HideInInspector]
		public bool IsReady = false;

		[Header("Events")]
		public GODirectionsRouteEvent OnRouteCreated;
		public GODirectionsErrorEvent OnRouteError;


		// Use this for initialization
		void Awake () {

			//register this class for location notifications
			goMap.locationManager.onOriginSet.AddListener((Coordinates) => {LoadData(Coordinates);});

			if (googleAPIkey.Length == 0 && directionsApi == GODirectionsAPI.Google) {
				Debug.LogError ("[GODirections] - GOOGLE API KEY IS REQUIRED, GET iT HERE: https://developers.google.com/places/web-service/intro");
				return;
			}

            if (mapboxAPIkey.Length == 0 && directionsApi == GODirectionsAPI.Mapbox)
            {
                Debug.LogError("[GODirections] - Mapbox access token is missing, GET IT HERE: https://www.mapbox.com/help/create-api-access-token/");
                return;
            }
        }

		void LoadData (Coordinates currentLocation) {//This is called when the location changes

			IsReady = true;
		}

		public void ClearRoute () {
		
			foreach (Transform t in transform) {
			
				Destroy (t.gameObject);
			}
		
		}

		public static GODirectionsRendering GetRenderingOptionsForTravelMode(GODirectionsRendering [] renderingOptions, GODirections.GOTravelModes travelMode) {

			GODirectionsRendering rendering = null;
			if (renderingOptions == null || renderingOptions.Count () == 0) {
				rendering = GODirectionsRendering.DefaultRendering ();
			} else {
				rendering = GODirectionsRendering.DefaultRendering ();
				foreach (GODirectionsRendering rend in renderingOptions) {
					if (rend.travelMode == travelMode) {
						rendering = rend;
					}
				}
			}
			return rendering;
		}

        #region PUBLIC METHODS

        public IEnumerator RequestDirectionsWithCoordinates(Coordinates start, Coordinates end, Coordinates[] waypoints, GOTravelModes travelmode)
        {
            if (directionsApi == GODirectionsAPI.Mapbox)
            {
                yield return requestMapboxDirectionsWithCoordinates(start, end, waypoints, travelmode);
            }
            else if (directionsApi == GODirectionsAPI.Google){

                yield return requestGoogleDirectionsWithCoordinates(start, end, waypoints, travelmode);
            }

            yield return null;
        }

        public IEnumerator RequestDirectionsWithAddresses(string start, string end, string[] waypoints, GOTravelModes travelmode)
        {
            if (directionsApi == GODirectionsAPI.Mapbox) {
                Debug.LogError("[GODirections] - Address directions not supported with Mapbox");
                yield break;
            }

            yield return requestGoogleDirectionsWithAddresses(start, end, waypoints, travelmode);

        }

        public IEnumerator RequestDirectionsFromUserLocationToAddress(Coordinates start, string end, GOTravelModes travelmode)
        {
            if (directionsApi == GODirectionsAPI.Mapbox)
            {
                Debug.LogError("[GODirections] - Address directions not supported with Mapbox");
                yield break;
            }

            yield return requestGoogleDirectionsFromUserLocationToAddress(start, end, travelmode);
        }

        public IEnumerator RequestDirectionsFromUserLocation(Coordinates end, Coordinates[] waypoints, GOTravelModes travelmode)
        {
            Coordinates userLocation = goMap.locationManager.currentLocation;
            yield return StartCoroutine(RequestDirectionsWithCoordinates(userLocation, end, waypoints, travelmode));
        }

        #endregion

        #region MAPBOX API

        private IEnumerator requestMapboxDirectionsWithCoordinates(Coordinates start, Coordinates end, Coordinates[] waypoints, GOTravelModes travelmode)
        {

            if (travelmode == GOTravelModes.transit) {
                Debug.LogWarning("[GODirections] - Travel Mode Transit not supported by Mapbox");
                travelmode = GOTravelModes.walking;
            }

            string modeString = travelmode.ToString();
            if (travelmode == GOTravelModes.drivingTraffic_MapboxOnly) {
                modeString = "driving-traffic"; 
            }

            if (automaticallyRemoveRoutes)
            {
                foreach (Transform t in transform)
                {
                    Destroy(t.gameObject);
                }
            }

            //Build Request Url
            string origin = start.toLongLatString();
            string waypointsString = null;
            string destination = end.toLongLatString();

            if (waypoints != null && waypoints.Count() > 0)
            {
                for (int i = 0; i < waypoints.Count(); i++)
                {
                    Coordinates via = waypoints[i];
                    //if (i == 0)
                    //    waypointsString += via.toLatLongString();
                    //else
                        waypointsString += ";" + via.toLongLatString();
                }
            }

            var finalUrl = mapboxBaseUrl+modeString+"/"+origin+waypointsString+";"+destination+"?"+"access_token="+mapboxAPIkey+ "&steps=true&voice_instructions=true";

            if (language != null && language.Length > 0)
            {
                finalUrl = finalUrl + "&language=" + language;
            }


            yield return requestMapboxDirections(finalUrl, travelmode, start,end,waypoints);
        }

        private IEnumerator requestMapboxDirections(string url, GOTravelModes travelmode, Coordinates start, Coordinates end, Coordinates[] waypoints)
        {

            //Build Request Url
            Debug.Log("[GODirections] Request url: " + url);

            var www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            ParseJob job = new ParseJob();
            job.InData = www.downloadHandler.text;
            job.Start();

            yield return StartCoroutine(job.WaitFor());

            IDictionary response = (IDictionary)job.OutData;

            string status = (string)response["code"];
            if (status != "Ok")
            {
                Debug.LogWarning("[GODirections] - DIRECTIONS DATA NOT FOUND: " + (string)response["error_message"] + " " + url);
                if (OnRouteError != null)
                    OnRouteError.Invoke((string)response["error_message"], response);
                yield break;
            }

            IList routes = (IList)response["routes"];
            foreach (IDictionary result in routes)
            {

                IList legs = (IList)result["legs"];
                string encodedPolyline = (string)result["geometry"];
                List<Coordinates> directionCoordinates = GOPolylineConverter.Decode(encodedPolyline).ToList();

                foreach (IDictionary leg in legs)
                {

                    GameObject r = new GameObject("Route");
                    GODirectionsRoute route = r.AddComponent<GODirectionsRoute>();
                    route.directions = this;
                    r.transform.parent = transform;

                    spawnMapboxPrefabs(start, end, waypoints, route);
                    route.InitRoute(directionCoordinates, leg, renderingOptions, travelmode, goMap.useElevation);

                    if (OnRouteCreated != null)
                        OnRouteCreated.Invoke(route);
                }
            }
        }

        private void spawnMapboxPrefabs(Coordinates start, Coordinates end, Coordinates[] waypoints, GODirectionsRoute route)
        {
            spawnPrefabs(start, end, waypoints, route);
        }

        #endregion

        #region GOOGLE API

        private IEnumerator requestGoogleDirectionsWithCoordinates(Coordinates start, Coordinates end, Coordinates [] waypoints, GOTravelModes travelmode){

			if (automaticallyRemoveRoutes) {
			
				foreach (Transform t in transform) {
					Destroy (t.gameObject);
				}
			}

			//Build Request Url
			var origin = "origin="+start.toLatLongString();
			var destination = "destination="+end.toLatLongString();

			if (waypoints != null && waypoints.Count () > 0) {
				var waypointsString = "&waypoints=";
				for (int i = 0; i < waypoints.Count (); i++) {
					Coordinates via = waypoints [i];
					if (i == 0)
						waypointsString += "via:" + via.toLatLongString ();
					else
						waypointsString += "|via:" + via.toLatLongString ();
				}
				destination += waypointsString;
			}

			var finalUrl = googleBaseUrl + origin + "&" + destination + "&mode="+travelmode.ToString()+ "&key=" + googleAPIkey;

			if (language != null && language.Length > 0) {
				finalUrl = finalUrl + "&language=" + language;
			}

			yield return requestGoogleDirections (finalUrl, travelmode);
		}

        private IEnumerator requestGoogleDirectionsWithAddresses(string start, string end, string [] waypoints, GOTravelModes travelmode){

			if (automaticallyRemoveRoutes) {

				foreach (Transform t in transform) {
					GameObject.Destroy (t.gameObject);
				}
			}

			//Build Request Url
			var origin = "origin="+start.Replace(" ","+");
			var destination = "destination="+end.Replace(" ","+");

			if (waypoints != null && waypoints.Count () > 0) {
				var waypointsString = "&waypoints=";
				for (int i = 0; i < waypoints.Count (); i++) {
					string via = waypoints [i];
					if (i == 0)
						waypointsString += "via:" + via.Replace(" ","+");
					else
						waypointsString += "|via:" + via.Replace(" ","+");
				}
				destination += waypointsString;
			}

			var finalUrl = googleBaseUrl + origin + "&" + destination + "&mode="+travelmode.ToString()+ "&key=" + googleAPIkey;

			if (language != null && language.Length > 0) {
				finalUrl = finalUrl + "&language=" + language;
			}
				
			yield return requestGoogleDirections(finalUrl, travelmode);

		}

        private IEnumerator requestGoogleDirectionsFromUserLocationToAddress(Coordinates start, string end, GOTravelModes travelmode){

			if (automaticallyRemoveRoutes) {

				foreach (Transform t in transform) {
					GameObject.Destroy (t.gameObject);
				}
			}

			//Build Request Url
			var origin = "origin="+start.toLatLongString();
			var destination = "destination="+end.Replace(" ","+");

			var finalUrl = googleBaseUrl + origin + "&" + destination + "&mode="+travelmode.ToString();
			if (googleAPIkey.Length > 0) {
				string key = "&key=" + googleAPIkey;
				finalUrl += key;
			}

			if (language != null && language.Length > 0) {
				finalUrl = finalUrl + "&language=" + language;
			}
				
			yield return requestGoogleDirections(finalUrl, travelmode);

		}

		private IEnumerator requestGoogleDirections(string url,  GOTravelModes travelmode){

			//Build Request Url
			Debug.Log ("[GODirections] Request url: " + url);

            var www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            ParseJob job = new ParseJob();
			job.InData = www.downloadHandler.text;
			job.Start();

			yield return StartCoroutine(job.WaitFor());

			IDictionary response = (IDictionary)job.OutData;

			string status = (string)response ["status"];
			if (status != "OK") {
				Debug.LogWarning ("[GODirections] - DIRECTIONS DATA NOT FOUND: "+(string)response["error_message"] + " " + url);
				if (OnRouteError != null)
					OnRouteError.Invoke ((string)response ["error_message"], response);
				yield break;
			}
				
			IList routes = (IList)response ["routes"];
			foreach (IDictionary result in routes) { 

				IList legs = (IList)result ["legs"]; 
				string encodedPolyline = (string)((IDictionary)result ["overview_polyline"])["points"];
				List<Coordinates> directionCoordinates = GOPolylineConverter.Decode (encodedPolyline).ToList();

				foreach (IDictionary leg in legs) {

					GameObject r = new GameObject ("Route");
					GODirectionsRoute route = r.AddComponent<GODirectionsRoute>();
					route.directions = this;
					r.transform.parent = transform;

					spawnGooglePrefabsWithLeg (leg,route);

					route.InitRoute(directionCoordinates,leg,renderingOptions,travelmode, goMap.useElevation);

					if (OnRouteCreated != null)
						OnRouteCreated.Invoke (route);
				}
			}
		}

        private void spawnGooglePrefabsWithLeg(IDictionary leg, GODirectionsRoute route)
        {

            //get start and end point
            IDictionary start_location = (IDictionary)leg["start_location"];
            IDictionary end_location = (IDictionary)leg["end_location"];

            Coordinates start = new Coordinates((double)start_location["lat"], (double)start_location["lng"], 0);
            Coordinates end = new Coordinates((double)end_location["lat"], (double)end_location["lng"], 0);

            IList via_waypoints = (IList)leg["via_waypoint"];
            List<Coordinates> vias = new List<Coordinates>();
            foreach (IDictionary via in via_waypoints)
            {
                IDictionary location = (IDictionary)via["location"];
                vias.Add(new Coordinates((double)location["lat"], (double)location["lng"], 0));
            }
            spawnPrefabs(start, end, vias.ToArray(), route);

        }

        #endregion

        #region Directions Prefabs

        private void spawnPrefabs (Coordinates start, Coordinates end, Coordinates [] waypoints, GODirectionsRoute route) {
		
			spawnPrefab (startPrefab, start,route.transform, goMap);
			spawnPrefab (destinationPrefab, end,route.transform, goMap);
			if (waypoints != null) {
				foreach (Coordinates coord in waypoints) {
					spawnPrefab (waypointPrefab, coord,route.transform, goMap);
				}
			}
		}

		public static GameObject spawnPrefab(GameObject prefab, Coordinates cord, Transform parent, bool useElevation) {
		
			if (prefab != null) {

				var randomRotation = Quaternion.Euler( 0 , UnityEngine.Random.Range(0, 360) , 0);
				float y = prefab.transform.position.y;
				Vector3 position = cord.convertCoordinateToVector (y);

                if (useElevation)
                {
                    position = GOMap.AltitudeToPoint(position);
                    position.y += y;
                }

				GameObject obj = (GameObject)Instantiate (prefab, position,randomRotation);
				obj.transform.parent = parent;
				return obj;
			}		
			return null;
		}

		#endregion

	}

	#region Rendering

	[System.Serializable]
	public class GODirectionsRendering
	{
		public GODirections.GOTravelModes travelMode;
		public Material material;
		public Material sidesMaterial;

		public float width;
		public float height;
		public float distanceFromFloor;
        public bool curved = false;

		public static GODirections.GOTravelModes StringToTravelMode (string travelMode) {

			try {
				GODirections.GOTravelModes parsed_enum = (GODirections.GOTravelModes)System.Enum.Parse( typeof( GODirections.GOTravelModes ), travelMode );
				return parsed_enum;
			} catch {
				Debug.LogWarning ("[GODirections] - Unable to convert "+travelMode+" to enum");
				return GODirections.GOTravelModes.driving;
			}
		}

		public static GODirectionsRendering DefaultRendering () {

			GODirectionsRendering rendering = new GODirectionsRendering ();
			rendering.width = 3;
			rendering.height = 2;
			rendering.distanceFromFloor = 2;

			return rendering;
		}

	}

	#endregion

	#region Events
	[Serializable]
	public class GODirectionsRouteEvent : UnityEvent <GODirectionsRoute> {


	}

	[Serializable]
	public class GODirectionsErrorEvent : UnityEvent <string, IDictionary> {


	}
	#endregion

}