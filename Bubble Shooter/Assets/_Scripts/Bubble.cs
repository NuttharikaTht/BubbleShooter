using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public enum BubbleState {
    Loaded = 0,
    Launched,
    Stopped,
  }

  public class Bubble : MonoBehaviour {
    private IndexPair index;
    private GameBoard gameBoard;
    private BubbleLauncher bubbleLauncher;
    private BubbleColor color;
    private BubbleState state = BubbleState.Loaded;

    public Bubble (int xIndex, int yIndex, BubbleColor color) {
      this.index = new IndexPair (xIndex, yIndex);
      this.color = color;
    }

    public IndexPair Index {
      get {
        return index;
      }
      set {
        index = value;
      }
    }

    public BubbleColor Color {
      get {
        return color;
      }
      set {
        color = value;
      }
    }

    public BubbleState State {
      get {
        return state;
      }
      set {
        state = value;
      }
    }

    public GameBoard GameBoard {
      get {
        return gameBoard;
      }
      set {
        gameBoard = value;
      }
    }

    public BubbleLauncher BubbleLauncher {
      get {
        return bubbleLauncher;
      }
      set {
        bubbleLauncher = value;
      }
    }

    // Use this for initialization
    void Start () {
    }
  
    // Update is called once per frame
    void Update () {
    }

    void OnCollisionEnter2D (Collision2D collision) {
      // If current bubble is not launched, do nothing.
      if (this.State != BubbleState.Launched) {
        return;
      }

      GameObject gameObject = collision.gameObject;
      if (gameObject.tag == "UpperBorder") {
        StopOnBoard (null);
      } else if (gameObject.tag == "Bubble") {
        StopOnBoard (collision.gameObject.GetComponent<Bubble> ());
      }
    }

    public void Blast () {
      // Play animation.
      Animation animation = this.GetComponent<Animation> ();
      animation.Play ("Bubble Blast");

      // Destroy bubble game object.
      Destroy (this);
    }

    public void Fall () {
      Debug.Log ("Bubble is falling!");
      Destroy (this);
    }

    // Bounce the bubble.
    public void Bounce () {
      Rigidbody2D rb = GetComponent<Rigidbody2D> ();
      rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
    }

    // Stop on game board.
    private void StopOnBoard (Bubble collidedBubble) {
      Rigidbody2D rb = GetComponent<Rigidbody2D> ();
      rb.velocity = Vector2.zero;
      rb.isKinematic = true;
      // Snap this bubble to game board.
      this.GameBoard.SnapBubble (this, collidedBubble);
      // Erase and fall bubbles if possible.
      this.GameBoard.EraseAndFall (this);
      // Set stopping flag.
      this.State = BubbleState.Stopped;
      // Load a new bubble.
      this.BubbleLauncher.LoadBubble ();
    }
  }
}