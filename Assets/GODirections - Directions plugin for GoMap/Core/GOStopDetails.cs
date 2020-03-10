using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GoShared;
using GoMap;

namespace GoDirections {

	[System.Serializable]
	public class GOStopDetails : MonoBehaviour {

		public enum StopType{
			Departure,
			Arrival
		};
		public StopType stopType;

		public string stopName;
		public Coordinates location;
		public Int64 timestamp;
		public string timeZone;

		public GOTransitDetails transitDetails;

		public static GOStopDetails Create (GODirectionsStep step, IDictionary stop, IDictionary time, StopType type, bool useElevation) {
		
			IDictionary l = (IDictionary)stop ["location"];
			double lat = (double)l ["lat"];
			double lon = (double)l ["lng"];
			Coordinates coords = new Coordinates (lat,lon,0);
			GameObject go = GODirections.spawnPrefab(step.route.directions.transitStopsPrefab,coords,step.transform, useElevation);


			GOStopDetails details = go.AddComponent<GOStopDetails> ();
			details.location = coords;
			details.stopType = type;
			details.stopName = (string)stop ["name"];
			details.transitDetails = step.transitDetails;

			details.timestamp =  (Int64)time ["value"];
			details.timeZone =  (string)time ["time_zone"];

			return details;
		}

	}
}