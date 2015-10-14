using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public class BubbleLauncher : MonoBehaviour {
    public GameObject bubblePrefab;
    public float launchSpeed;

    private GameObject loadedBubble;

    // Use this for initialization
    void Start () {
      LoadBubble ();
    }
	
    // Update is called once per frame
    void Update () {
	
    }

    // Prepare a bubble at the starting point.
    public void LoadBubble () {
      loadedBubble = Instantiate (bubblePrefab, transform.position, Quaternion.identity) as GameObject;

      Debug.Log ("A new bubble is loaded.");
    }

    // Launch the bubble.
    public void LaunchBubble (Vector2 direction) {
      if (loadedBubble == null)
        return;

      // Set the velocity of the bubble.
      Rigidbody2D rb = loadedBubble.GetComponent<Rigidbody2D> ();
      rb.velocity = launchSpeed * direction.normalized;

      LoadBubble ();

      Debug.LogFormat ("A new bubble is launched towards direction: {0}.", direction.normalized);
    }
  }
}