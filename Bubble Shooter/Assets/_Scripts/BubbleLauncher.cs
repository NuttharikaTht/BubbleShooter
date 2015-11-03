using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public class BubbleLauncher : MonoBehaviour {
    public GameObject bubblePrefab;
    public GameBoard gameBoard;
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

      Bubble bubble = loadedBubble.GetComponent<Bubble> ();
      bubble.Color = pallette.GetRandomBubbleColor ();
      bubble.State = BubbleState.Loaded;
      bubble.GameBoard = gameBoard;
      bubble.BubbleLauncher = this;

      SpriteRenderer renderer = loadedBubble.GetComponent<SpriteRenderer> ();
      renderer.color = pallette.GetColor (bubble.Color);
    }

    // Launch the bubble.
    public void LaunchBubble (Vector2 direction) {
      if (loadedBubble == null)
        return;

      // Set the velocity of the bubble.
      Rigidbody2D rb = loadedBubble.GetComponent<Rigidbody2D> ();
      rb.velocity = launchSpeed * direction.normalized;

      Bubble bubble = loadedBubble.GetComponent<Bubble> ();
      bubble.State = BubbleState.Launched;

      loadedBubble = null;

      Debug.LogFormat ("A new bubble is launched towards direction: {0}.", direction.normalized);
    }
  }
}