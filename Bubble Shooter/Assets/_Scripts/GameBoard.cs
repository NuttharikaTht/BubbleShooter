using UnityEngine;
using System;
using System.Collections;

namespace BubbleShooter {
  public class GameBoard : MonoBehaviour {
    public float width;
    public float height;
    public int numBubblesEachRow;
    public float percentageOfGameHeight = 0.8f;

    // Keep this constant for efficiency.
    private const float sqrt3 = 1.73205f;
    private float bubbleRadius;
    // Maximal number of bubbles vertically.
    private int numRows;

    // The two-dementional array to store the info of bubble map.
    private Bubble[,] bubbleMap;
    private Vector2[,] bubbleMapPosition;

    void Start () {
      SetUpGameBoard (width, height, numBubblesEachRow, percentageOfGameHeight);
    }

    private void SetUpGameBoard (float width, float height, int numBubblesEachRow, float percentageOfGameHeight) {
      this.width = width;
      this.height = height;
      this.numBubblesEachRow = numBubblesEachRow;
      this.bubbleRadius = 0.5f * (width / numBubblesEachRow);
      
      float gameHeight = percentageOfGameHeight * height;
      if ((gameHeight - 2 * this.bubbleRadius) < 0) {
        this.numRows = 0;
      } else {
        this.numRows = 1 + Convert.ToInt32 ((gameHeight - 2 * this.bubbleRadius) / (this.bubbleRadius * sqrt3));
      }
      // Initialize the bubble map.
      bubbleMap = new Bubble[this.numRows + 1, this.numBubblesEachRow + 1];
      bubbleMapPosition = new Vector2[this.numRows + 1, this.numBubblesEachRow + 1];
      InitializeBubbleMapPosition ();
    }

    // Attention: The index starts from 1.
    private Vector2 CalculateBubblePosition (int xIndex, int yIndex) {
//      if (!IndexCheck (xIndex, yIndex))
//        return null;

      Vector2 position = new Vector2 ();
      if (xIndex % 2 == 1) {
        position.x = (-0.5f * width) + bubbleRadius * (yIndex * 2 - 1);

      } else {
        position.x = (-0.5f * width) + bubbleRadius * (yIndex * 2);
      }
      position.y = (0.5f * height) - (bubbleRadius * (1 + sqrt3 * (xIndex - 1)));
      return position;
    }

    private void InitializeBubbleMapPosition () {
      for (int i = 1; i <= numRows; i++) {
        for (int j = 1; j <= numBubblesEachRow; j++) {
          if (i % 2 == 0 && j == numBubblesEachRow) {
            continue;
          }
          bubbleMapPosition [i, j] = CalculateBubblePosition (i, j);
        }
      }
    }

    // Checks whether the indexes exists in game's map.
    private bool IndexCheck (int xIndex, int yIndex) {
      if (xIndex < 1 || yIndex < 1 || xIndex > numRows)
        return false;
      
      if (xIndex % 2 == 1) {
        if (yIndex > numBubblesEachRow)
          return false;
      } else {
        if (yIndex > (numBubblesEachRow - 1))
          return false;
      }
      return true;
    }

    private void StoreBubbleToMap (Bubble bubble, int xIndex, int yIndex) {
      bubble.XIndex = xIndex;
      bubble.YIndex = yIndex;
      bubbleMap [xIndex, yIndex] = bubble;
    }

    private IndexPair[] getAllNearbyIndex (int indexX, int indexY) {
      IndexPair[] nearbyIndex = new IndexPair[6]; // left, right, leftup, leftbottom, rightup, rightbottom
      nearbyIndex [0] = new IndexPair (indexX, indexY - 1); // left
      nearbyIndex [1] = new IndexPair (indexX, indexY + 1); // right
      if (indexX % 2 == 0) {
        nearbyIndex [2] = new IndexPair (indexX - 1, indexY); // leftup
        nearbyIndex [3] = new IndexPair (indexX + 1, indexY); // leftbottom
        nearbyIndex [4] = new IndexPair (indexX - 1, indexY + 1); // rightup
        nearbyIndex [5] = new IndexPair (indexX + 1, indexY + 1); // rightbottom
      } else {
        nearbyIndex [2] = new IndexPair (indexX - 1, indexY - 1); // leftup
        nearbyIndex [3] = new IndexPair (indexX + 1, indexY - 1); // leftbottom
        nearbyIndex [4] = new IndexPair (indexX - 1, indexY); // rightup
        nearbyIndex [5] = new IndexPair (indexX + 1, indexY); // rightbottom
      }
      return nearbyIndex;
    }

    // Snap the bubble to the game board.
    public void SnapBubble (Bubble newBubble, Bubble collidedBubble) {
      int snappedXIndex = 1;
      int snappedYIndex = 1;
      if (collidedBubble == null) {
        // Collision object is UpperBorder.
        float minDistance = 2 * bubbleRadius;
        for (int i = 1; i <= numBubblesEachRow; i++) {
          float distance = Vector2.Distance (newBubble.transform.position, bubbleMapPosition [1, i]);
          if (distance < minDistance) {
            minDistance = distance;
            snappedYIndex = i;
          }
        }
      } else {
        // Collision object is an existing bubble.
        int collidedX = collidedBubble.XIndex;
        int collidedY = collidedBubble.YIndex;
        IndexPair[] nearbyIndex = getAllNearbyIndex (collidedX, collidedY);
        float minDistance = 2 * bubbleRadius;
        snappedXIndex = collidedX;
        snappedYIndex = collidedY;
        for (int i = 0; i <= 5; i++) {
          if (!IndexCheck (nearbyIndex [i].X, nearbyIndex [i].Y))
            continue;
          if (bubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y] != null)
            continue;
          float distance = Vector2.Distance (newBubble.transform.position, bubbleMapPosition [nearbyIndex [i].X, nearbyIndex [i].Y]);
          if (distance < minDistance) {
            minDistance = distance;
            snappedXIndex = nearbyIndex [i].X;
            snappedYIndex = nearbyIndex [i].Y;
          }
        }
        // This should never happen.
        if (snappedXIndex == collidedX && snappedYIndex == collidedY)
          return;
      }
      newBubble.transform.position = new Vector2 (bubbleMapPosition [snappedXIndex, snappedYIndex].x, bubbleMapPosition [snappedXIndex, snappedYIndex].y);
      StoreBubbleToMap (newBubble, snappedXIndex, snappedYIndex);
    }

    // Destroy the bubbles.
    public void DestroyBubbles (IEnumerable bubbles) {
      foreach (Bubble bubble in bubbles) {
        bubble.Destroy ();
      }
    }

    public void MakeBubblesFall (IEnumerable bubbles) {
      foreach (Bubble bubble in bubbles) {
        bubble.Fall ();
      }
    }

  }
}

