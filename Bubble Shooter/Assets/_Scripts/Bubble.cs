using UnityEngine;
using System.Collections;

namespace BubbleShooter {
  public class Bubble : MonoBehaviour {
    private int xIndex;
    private int yIndex;
    private Position position;
    private GameBoard gameBoard;
    private BubbleLauncher bubbleLauncher;

    private BubbleColor color;

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

    void OnTriggerEnter2D (Collider2D collider) {
      GameObject gameObject = collider.gameObject;

      if (gameObject.tag == "LeftBorder" || gameObject.tag == "RightBorder") {
        Bounce ();
      } else if (gameObject.tag == "UpperBorder" || gameObject.tag == "Bubble") {
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
      // Snap this bubble to game board.
      this.GameBoard.SnapBubble (this);
      // Load a new bubble.
      this.BubbleLauncher.LoadBubble ();
    }
  }
}