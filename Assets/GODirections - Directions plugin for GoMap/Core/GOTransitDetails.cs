using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GoMap;
using UnityEngine.Networking;

namespace GoDirections {

	[System.Serializable]
	public class GOTransitDetails {

		public IDictionary transitDetails;

		public Texture2D icon;
		public string headsign;
		public Int64 numberOfStops;
		public string name;


		public InspectorDictionary[] line;
		public InspectorDictionary[] vehicle;


		public void InitDetails (IDictionary stepDict, GODirectionsStep step, bool useElevation) {

			transitDetails = (IDictionary)stepDict["transit_details"];

			if (transitDetails.Contains ("num_stops")) {
				numberOfStops = (Int64)transitDetails ["num_stops"];
			}

			if (transitDetails.Contains ("headsign")) {
				headsign = (string)transitDetails ["headsign"];
			}

			if (transitDetails.Contains ("line")) {

				Dictionary<string,object> lineDict = (Dictionary<string,object>)transitDetails ["line"];
				line = InspectorDictionary.MakeDict ((Dictionary<string,object>)lineDict);

				if (lineDict.ContainsKey ("short_name")) {
					name = (string)lineDict ["short_name"];
				}

				if (lineDict.ContainsKey ("vehicle")) {
					Dictionary<string,object> vehicleDict = (Dictionary<string,object>)lineDict ["vehicle"];
					vehicle = InspectorDictionary.MakeDict (vehicleDict);
					step.StartCoroutine (Coroutines (lineDict,step, useElevation));
				}
			}

		}

		private IEnumerator Coroutines(Dictionary<string,object> lineDict, GODirectionsStep step,bool useElevation ) {
		
			if (lineDict.ContainsKey ("vehicle")) {
				Dictionary<string,object> vehicleDict = (Dictionary<string,object>)lineDict ["vehicle"];
				vehicle = InspectorDictionary.MakeDict (vehicleDict);

				yield return step.StartCoroutine (DownloadIcon ((string)vehicleDict["icon"]));
			}

			yield return CreateStopsPrefabs (step, useElevation);

		}


		private IEnumerator DownloadIcon (string url) {

            var www = UnityWebRequestTexture.GetTexture("https:" + url);
            yield return www.SendWebRequest();
			icon = ((DownloadHandlerTexture)www.downloadHandler).texture;
		}

		private IEnumerator CreateStopsPrefabs (GODirectionsStep step, bool useElevation) {


			if (transitDetails.Contains ("departure_stop") && transitDetails.Contains("departure_time")) {

				IDictionary depart = (IDictionary)transitDetails ["departure_stop"];
				IDictionary dt = (IDictionary)transitDetails ["departure_time"];
				GOStopDetails.Create (step, depart, dt, GOStopDetails.StopType.Departure, useElevation);
			}

			if (transitDetails.Contains ("arrival_stop") && transitDetails.Contains("arrival_time")) {
				
				IDictionary depart = (IDictionary)transitDetails ["arrival_stop"];
				IDictionary dt = (IDictionary)transitDetails ["arrival_time"];
				GOStopDetails.Create (step, depart, dt, GOStopDetails.StopType.Arrival, useElevation);
			}

			yield return null;

		}

	}
}