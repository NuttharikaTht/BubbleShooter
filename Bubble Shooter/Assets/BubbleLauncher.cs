using UnityEngine;
using System.Collections;

public class BubbleLauncher : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		LoadBubble ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Prepare a bubble at the starting point.
	public void LoadBubble ()
	{
		Debug.Log ("A new bubble is loaded.");
	}

	// Launch the bubble.
	public void LaunchBubble (Vector3 direction)
	{
		Debug.LogFormat ("A new bubble is launched towards direction: {0}.", direction.normalized);
	}
}
