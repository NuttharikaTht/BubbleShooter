using UnityEngine;
using System.Collections;

namespace BubbleShooter
{
	public class BubbleLauncher : MonoBehaviour
	{
		public GameObject bubblePrefab;

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
			GameObject newBubble = Instantiate (bubblePrefab, transform.position, Quaternion.identity) as GameObject;

			Debug.Log ("A new bubble is loaded.");
		}

		// Launch the bubble.
		public void LaunchBubble (Vector2 direction)
		{
			Debug.LogFormat ("A new bubble is launched towards direction: {0}.", direction.normalized);
		}
	}
}