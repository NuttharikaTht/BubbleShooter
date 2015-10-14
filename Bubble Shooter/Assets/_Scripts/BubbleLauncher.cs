using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public class BubbleLauncher : MonoBehaviour {
    public GameObject bubblePrefab;
    public float launchSpeed;

    private GameObject loadedBubble;
    private Pallette pallette = new Pallette ();

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
      SpriteRenderer renderer = loadedBubble.GetComponent<SpriteRenderer> ();
      renderer.color = pallette.GetRandomColor ();
    }

    // Launch the bubble.
    public void LaunchBubble (Vector2 direction) {
      if (loadedBubble == null)
        return;

      // Set the velocity of the bubble.
      Rigidbody2D rb = loadedBubble.GetComponent<Rigidbody2D> ();
      rb.velocity = launchSpeed * direction.normalized;

      // Currently load a new bubble immediately after launching.
      // This will help in testing.
      LoadBubble ();

      Debug.LogFormat ("A new bubble is launched towards direction: {0}.", direction.normalized);
    }
  }
}