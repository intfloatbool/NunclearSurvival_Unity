using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoDirections;
using System;
using GoShared;

public class GOTransitPrefabDemo : MonoBehaviour {

	public GameObject banner;
	public Text bannerText;
	public GameObject transitSign;
	public Shader fontShader;

	// Use this for initialization
	void Start () {

		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
		GetComponentInChildren<Canvas> ().planeDistance = 10.2f;
//		banner = GetComponentInChildren

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


		GOStopDetails stopDetails = gameObject.GetComponent<GOStopDetails> ();
		if (stopDetails!= null) {
				
			SpriteRenderer iconRenderer = transform.GetComponentInChildren<SpriteRenderer> ();
			Texture2D texture = stopDetails.transitDetails.icon;
			iconRenderer.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));

			TextMesh textMesh = transitSign.GetComponentInChildren<TextMesh> ();
			textMesh.text =  stopDetails.transitDetails.name;

			MeshRenderer textMeshRenderer = transitSign.GetComponentInChildren<MeshRenderer> ();
			Material m = textMeshRenderer.material;
			m.shader = fontShader;
			m.color = Color.black;

			bannerText.text = "<b>"+stopDetails.stopName+"</b>" + "\n" + "Take the " + "<b>"+stopDetails.transitDetails.name +"</b>"+ " to " + 
				stopDetails.transitDetails.headsign + " for "+stopDetails.transitDetails.numberOfStops+" stops."+
				"\nDeparture time: "+String.Format("{0:HH:mm}", GOUtils.UnixTimeStampToDateTime(stopDetails.timestamp));

		}
	}

	void OnMouseDown() {
		StartCoroutine (HandlePanel (true));

	}

	public void Dismiss () {
		StartCoroutine (HandlePanel (false));	
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
			touch = Input.GetTouch (0).tapCount > 0;
			pos = Input.GetTouch (0).position;
		} else {
			touch = Input.GetMouseButtonDown (0);
			if (touch)
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
