using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class RayCastShooter : MonoBehaviour {

	public GameObject[] colorsGO;
	public GameObject dotPrefab;
	public Bullet bullet;

    public bool mouseDown = false;
    public List<Vector2> dots;
    public List<GameObject> dotsPool;
    public int maxDots = 260;

    public float dotGap = 0.32f;
    public float bulletProgress = 0.0f;
    public float bulletIncrement = 0.0f;

    public int type = 0;
    public UnityEngine.UI.Button yourButton;
	//declare a boolean
	public bool buttonisclicked;

    // trial
	public GameObject Panel;


	// Use this for initialization
	public void Start () {

        /* for button */
        //Button btn = yourButton.GetComponent<Button>();
        //Button btn = Instantiate(UnityEngine.Object)yourButton).GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
        //set to false on start, may not be necessary, I just can't remember if C# will return 1 or 0 for undefined booleans 
        buttonisclicked = false;

		

		dots = new List<Vector2> ();
		dotsPool = new List<GameObject> ();

		var i = 0;
		var alpha = 1.0f / maxDots;
		var startAlpha = 1.0f;
		while (i < maxDots) {
			var dot = Instantiate (dotPrefab) as GameObject;
			var sp = dot.GetComponent<SpriteRenderer> ();
			var c = sp.color;

			c.a = startAlpha - alpha;
			startAlpha -= alpha;
			sp.color = c;

			dot.SetActive (false);
			dotsPool.Add (dot);
			i++;
		}

		//select initial type
		SetNextType();
	}

    public void SetNextType () {

		foreach (var go in colorsGO) {
			go.SetActive(false);
		}

		type = Random.Range (0, 1);
		colorsGO [type].SetActive (true);


	}

    public void HandleTouchDown (Vector2 touch) {
	}

    public void HandleTouchUp (Vector2 touch) {

		if (dots == null || dots.Count < 2)
			return;
		
		foreach (var d in dotsPool)
			d.SetActive (false);
		
		bulletProgress = 0.0f;
		bullet.SetType ((Ball.BALL_TYPE) type);
		bullet.gameObject.SetActive (true);
		bullet.transform.position = transform.position;
		InitPath ();


		SetNextType();
	}

    public void HandleTouchMove (Vector2 touch) {


		if (bullet.gameObject.activeSelf)
			return;

		if (dots == null) {
			return;
		}

		dots.Clear ();

		foreach (var d in dotsPool)
			d.SetActive (false);

		Vector2 point = Camera.main.ScreenToWorldPoint (touch);
		var direction = new Vector2 (point.x - transform.position.x, point.y - transform.position.y);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
		if (hit.collider != null) {
			
			dots.Add (transform.position);

			if (hit.collider.tag == "SideWall") {
				DoRayCast (hit, direction);
			} else {
				dots.Add (hit.point);
				DrawPaths ();
			}
		}
	}

    public void DoRayCast (RaycastHit2D previousHit, Vector2 directionIn) {

		dots.Add (previousHit.point);

		var normal = Mathf.Atan2 (previousHit.normal.y, previousHit.normal.x);
		var newDirection = normal + (  normal - Mathf.Atan2(directionIn.y, directionIn.x) );
		var reflection = new Vector2 (-Mathf.Cos (newDirection), -Mathf.Sin (newDirection));
		var newCastPoint = previousHit.point + (2 * reflection);

//		directionIn.Normalize ();
//		newCastPoint = new Vector2(previousHit.point.x + 2 * (-directionIn.x), previousHit.point.y + 2 * (directionIn.y));
//		reflection = new Vector2 (-directionIn.x, directionIn.y);

		var hit2 = Physics2D.Raycast(newCastPoint, reflection);
		if (hit2.collider != null) {
			if (hit2.collider.tag == "SideWall") {
				//shoot another cast
				DoRayCast (hit2, reflection);
			} else {
				dots.Add (hit2.point);
				DrawPaths ();
			}
		} else {
			DrawPaths ();
		}
	}


    // Update is called once per frame

    public void Update () {

        //bool isPanelActive = Panel;

        if (!Panel.activeSelf)

		{ 

			if (bullet.gameObject.activeSelf)
			{

				bulletProgress += bulletIncrement;

				if (bulletProgress > 1)
				{
					dots.RemoveAt(0);
					if (dots.Count < 2)
					{
						bullet.gameObject.SetActive(false);
						return;
					}
					else
					{
						InitPath();
					}
				}

				var px = dots[0].x + bulletProgress * (dots[1].x - dots[0].x);
				var py = dots[0].y + bulletProgress * (dots[1].y - dots[0].y);

				bullet.transform.position = new Vector2(px, py);

				return;
			}

			if (dots == null)
				return;

			if (Input.touches.Length > 0)
			{

				Touch touch = Input.touches[0];

				if (touch.phase == TouchPhase.Began)
				{
					HandleTouchDown(touch.position);
				}
				else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
				{
					HandleTouchUp(touch.position);
				}
				else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					HandleTouchMove(touch.position);
				}
				HandleTouchMove(touch.position);
				return;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				mouseDown = true;
				HandleTouchDown(Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				mouseDown = false;
				HandleTouchUp(Input.mousePosition);
			}
			else if (mouseDown)
			{
				HandleTouchMove(Input.mousePosition);
			}
		}
	}

    public void DrawPaths () {
		
		if (dots.Count > 1) {

			foreach (var d in dotsPool)
				d.SetActive (false);

			int index = 0;

			for (var i = 1; i < dots.Count; i++) {
				DrawSubPath (i - 1, i, ref index);
			}
		}
	}

    public void DrawSubPath (int start, int end, ref int index) {
		var pathLength = Vector2.Distance (dots [start], dots [end]);

		int numDots = Mathf.RoundToInt ( (float)pathLength / dotGap );
		float dotProgress = 1.0f / numDots;

		var p = 0.0f;

		while (p < 1) {
			var px = dots [start].x + p * (dots [end].x - dots [start].x);
			var py = dots [start].y + p * (dots [end].y - dots [start].y);

			if (index < maxDots) {
				var d = dotsPool [index];
				d.transform.position = new Vector2 (px, py);
				d.SetActive (true);
				index++;
			}

			p += dotProgress;
		}
	}

    public void InitPath () {
		var start = dots [0];
		var end = dots [1];
		var length = Vector2.Distance (start, end);
		var iterations = length / 0.15f;
		bulletProgress = 0.0f;
		bulletIncrement = 1.0f / iterations;
	}

}
