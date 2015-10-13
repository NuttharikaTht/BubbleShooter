using UnityEngine;
using System;
using System.Collections;

namespace BubbleShooter {
  public class GameBoard {
    private float width;
    private float height;
    private int numBubblesEachRow;
    private float percentageOfGameHeight = 0.8f;
    // Keep this constant for efficiency.
    private const float sqrt3 = 1.73205f;
    private float bubbleRadius;
    // Maximal number of bubbles vertically.
    private int numColumns;

    // The two-dementional array to store the info of bubble map.
    private Bubble[,] bubbleMap;

    public GameBoard (float width, float height, int numBubblesEachRow, float percentageOfGameHeight) {
      this.percentageOfGameHeight = percentageOfGameHeight;
      SetUpGameBoard (width, height, numBubblesEachRow);
    }

    public GameBoard (float width, float height, int numBubblesEachRow) {
      SetUpGameBoard (width, height, numBubblesEachRow);
    }

    private void SetUpGameBoard (float width, float height, int numBubblesEachRow) {
      this.width = width;
      this.height = height;
      this.numBubblesEachRow = numBubblesEachRow;
      this.bubbleRadius = 0.5f * (width / numBubblesEachRow);
      
      float gameHeight = percentageOfGameHeight * height;
      if ((gameHeight - 2 * this.bubbleRadius) < 0) {
        this.numColumns = 0;
      } else {
        this.numColumns = 1 + Convert.ToInt32 ((gameHeight - 2 * this.bubbleRadius) / (this.bubbleRadius * sqrt3));
      }
      // Initialize the bubble map.
      bubbleMap = new Bubble[this.numBubblesEachRow, this.numColumns];
    }

    // Checks whether the indexes exists in game's map.
    private bool IndexCheck (int xIndex, int yIndex) {
      if (yIndex > this.numColumns)
        return false;
      
      Position position = new Position ();
      if (yIndex % 2 == 1) {
        if (xIndex > this.numBubblesEachRow)
          return false;
      } else {
        if (xIndex > (this.numBubblesEachRow - 1))
          return false;
      }
      return true;
    }

    // Attention: The index starts from 1.
    public Position CalculateBubblePosition (int xIndex, int yIndex) {
      if (!IndexCheck (xIndex, yIndex))
        return null;

      Position position = new Position ();
      if (yIndex % 2 == 1) {
        position.SetX ((-0.5f * this.width) + this.bubbleRadius * (yIndex * 2 - 1));

      } else {
        position.SetX ((-0.5f * this.width) + this.bubbleRadius * (yIndex * 2));
      }
      position.SetY ((0.5f * this.height) - (this.bubbleRadius * (1 + sqrt3 * (xIndex - 1))));
      return position;
    }

    private void StoreBubbleToMap (Bubble bubble) {
      bubbleMap [bubble.GetXIndex (), bubble.GetYIndex ()] = bubble;
    }

    // Snap the bubble to the game board.
    public void SnapBubble (Bubble bubble) {
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

