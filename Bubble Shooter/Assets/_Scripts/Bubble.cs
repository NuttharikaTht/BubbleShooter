using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public enum BubbleState {
    Loaded = 0,
    Launched,
    Stopped,
  }
  public class Bubble : MonoBehaviour {
    private int xIndex;
    private int yIndex;
    private Position position;
    private GameBoard gameBoard;
    private BubbleLauncher bubbleLauncher;
    private BubbleColor color;
    private BubbleState state = BubbleState.Loaded;

    public Bubble (int xIndex, int yIndex, Position position, BubbleColor color) {
      this.xIndex = xIndex;
      this.yIndex = yIndex;
      this.position = position;
      this.color = color;
    }

    public int GetXIndex () {
      return xIndex;
    }

    public int GetYIndex () {
      return yIndex;
    }

    public Position GetPosition () {
      return position;
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

      if (gameObject.tag == "UpperBorder" || gameObject.tag == "Bubble") {
        StopOnBoard ();
      }
    }

    public void Destroy () {
    }

    public void Fall () {
    }

    // Bounce the bubble.
    public void Bounce () {
      Rigidbody2D rb = GetComponent<Rigidbody2D> ();
      rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
    }

    // Stop on game board.
    public void StopOnBoard () {
      Rigidbody2D rb = GetComponent<Rigidbody2D> ();
      rb.velocity = Vector2.zero;
      rb.isKinematic = true;
      // Snap this bubble to game board.
      this.GameBoard.SnapBubble (this);
      // Set stopping flag.
      this.State = BubbleState.Stopped;
      // Load a new bubble.
      this.BubbleLauncher.LoadBubble ();
    }
  }
}