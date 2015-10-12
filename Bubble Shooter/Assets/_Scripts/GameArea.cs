using UnityEngine;
using System.Collections;

public class GameArea : MonoBehaviour
{
	public BubbleLauncher bubbleLauncher;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Hello, world!");
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void OnMouseDown ()
	{
	}

	// When mouse button is released in game area, launch a new bubble.
	void OnMouseUp ()
	{
		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		bubbleLauncher.LaunchBubble (currentMousePosition - bubbleLauncher.transform.position);
	}
}
