using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoDirections;
using System;
using GoShared;

public class GOStartStopPrefabDemo : MonoBehaviour {

	public GameObject banner;
	public Text bannerText;

	// Use this for initialization
	void Start () {

		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
		GetComponentInChildren<Canvas> ().planeDistance = 10.2f;

		BuildSign ();
	}
	
	// Update is called once per frame
	void Update () {

		//Rotation
		Vector3 targetDir = transform.position-Camera.main.transform.position;
		Quaternion finalRotation = Quaternion.LookRotation (targetDir);
		Vector3 euler = transform.eulerAngles;
		euler.y = finalRotation.eulerAngles.y;
		transform.eulerAngles = euler;

//		if (OutisdeClick (banner)) {
//			StartCoroutine (HandlePanel (false));			
//		}
	}

	void BuildSign () {

		GODirectionsRoute route = transform.parent.GetComponent<GODirectionsRoute> ();
		if (route!= null) {

			bannerText.text = "<b>Directions from:</b> " + route.startAddress + "\n<b>To: </b>" + route.endAddress+"\n";
			foreach (string s in route.htmlDirections) {
				bannerText.text += (s+"\n");
			}
		
			bannerText.text = GOUtils.RemoveDivs(bannerText.text);

		}
	}

	public void Dismiss () {
		StartCoroutine (HandlePanel (false));	
	}
		
	void OnMouseDown() {
		StartCoroutine (HandlePanel (true));
	}
		
	bool isPanelOn () {

		RectTransform bannerRect = banner.GetComponent<RectTransform> ();
		return bannerRect.anchoredPosition.y > bannerRect.sizeDelta.y -2;

	}

	IEnumerator HandlePanel (bool show) {
	
		if (show) {
			banner.SetActive (true);
			yield return StartCoroutine (Slide (true, 1));
		} else {			
			yield return StartCoroutine (Slide (false, 1));
			banner.SetActive (false);
		}
	}

	private bool OutisdeClick(GameObject panel) {
		
		bool touch;
		Vector3 pos = Vector3.zero;
		if (Application.isMobilePlatform) {
			touch = Input.touchCount > 0;
			if (touch)
				pos = Input.GetTouch (0).position;
		} else {
			touch = Input.GetMouseButtonDown (0);
			pos = Input.mousePosition;
		}


		if (isPanelOn() && touch && panel.activeSelf && 
			!RectTransformUtility.RectangleContainsScreenPoint(
				panel.GetComponent<RectTransform>(), 
				pos, 
				Camera.main)) {
			Debug.Log ("true");
			return true;
		}
		return false;
	}

	private IEnumerator Slide(bool show, float time) {

		Vector2 newPosition;
		RectTransform bannerRect = banner.GetComponent<RectTransform> ();

		if (!show) {//Open
			newPosition = new Vector2 (bannerRect.anchoredPosition.x, 0);
		} else { //Close
			newPosition = new Vector2 (bannerRect.anchoredPosition.x, bannerRect.sizeDelta.y);
		} 

		float elapsedTime = 0;
		while (elapsedTime <= time)
		{
			bannerRect.anchoredPosition = Vector2.Lerp(bannerRect.anchoredPosition, newPosition, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

	}
}
