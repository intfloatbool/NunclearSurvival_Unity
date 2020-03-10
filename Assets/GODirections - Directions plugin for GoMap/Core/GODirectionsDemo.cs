using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
using GoShared;


namespace GoDirections {
	#if UNITY_EDITOR

	[CustomEditor(typeof(GODirectionsDemo))]
	public class GODirectionsEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			GUIStyle style = new GUIStyle ();
			style.fontSize = 12;
			style.normal.textColor = Color.white;
			GUILayout.Space(10);

			GODirectionsDemo editor = (GODirectionsDemo)target;
			GUILayout.Label ("Use this component to make a demo path",style);
			GUILayout.Space(20);

			if(GUILayout.Button("Load demo directions"))
			{
				editor.LoadDemoRoute();
			}

			//			GUILayout.Space(20);
//			EditorGUILayout.HelpBox ("This destroys everything in the map hierarchy,\nuse this before loading another map inside the editor.",MessageType.Info);
//			if(GUILayout.Button("Destroy Map in Editor"))
//			{
//				editor.DestroyCurrentMap();
//			}

		}
	}
	#endif


	public class GODirectionsDemo : MonoBehaviour 
	{

		[Header("Demo Settings")]
		public Coordinates demo_start;
		public Coordinates demo_end;
		public List <Coordinates> demo_waypoints;
		public GODirections.GOTravelModes demo_travelMode = GODirections.GOTravelModes.driving;

		public bool startFromUserLocation = true;
		public bool useRandomDestination = true;
		public bool randomizeWaypoints = false;

		public void LoadDemoRoute () {

			GODirections directions = GetComponent<GODirections> ();
			if (directions == null) {
				Debug.LogError ("[GODirections Editor] GODirections script not found");
				return;
			}

			if (useRandomDestination) {
			
				Vector3 current = directions.goMap.locationManager.currentLocation.convertCoordinateToVector ();
				Vector3 destination =  current + UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(1000, 1000);
				demo_end = Coordinates.convertVectorToCoordinates (destination);
			}

			if (randomizeWaypoints) {
				
				Vector3 current = directions.goMap.locationManager.currentLocation.convertCoordinateToVector ();
				int number = UnityEngine.Random.Range (1, 1);
				for (int i = 0; i<number; i++) {

					Vector3 waypoint =  current + UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(1000, 1000);
					demo_waypoints.Add (Coordinates.convertVectorToCoordinates (waypoint));
				}
			}


			if (startFromUserLocation) {
				StartCoroutine (directions.RequestDirectionsFromUserLocation(demo_end,demo_waypoints.ToArray(),demo_travelMode));
			} else {
				StartCoroutine (directions.RequestDirectionsWithCoordinates(demo_start,demo_end,demo_waypoints.ToArray(),demo_travelMode));
			}

			demo_waypoints.RemoveRange(0,demo_waypoints.Count);
		}
	}
}

