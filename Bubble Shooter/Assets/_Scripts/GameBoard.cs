using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
      bubble.Index = new IndexPair (xIndex, yIndex);
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
        int collidedX = collidedBubble.Index.X;
        int collidedY = collidedBubble.Index.Y;
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

    // Get a list of indeies of bubbles that connect with new bubble and have the same color.
    private ArrayList GetAdjacentSameColorBubbles (IndexPair newBubbleIndex, BubbleColor color) {
      ArrayList bubbleIndexList = new ArrayList ();
      bubbleIndexList.Add (newBubbleIndex);
      Stack bubbleIndexStack = new Stack ();
      bubbleIndexStack.Push (newBubbleIndex); 
      while (bubbleIndexStack.Count != 0) {
        IndexPair current = (IndexPair)bubbleIndexStack.Pop ();
        IndexPair[] nearbyIndex = getAllNearbyIndex (current.X, current.Y);
        for (int i = 0; i <= 5; i++) {
          if (!IndexCheck (nearbyIndex [i].X, nearbyIndex [i].Y))
            continue;

          Bubble bubble = bubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y];
          if (bubble == null || bubble.Color != color || bubbleIndexList.Contains (nearbyIndex [i]))
            continue;

          bubbleIndexStack.Push (nearbyIndex [i]);
          bubbleIndexList.Add (nearbyIndex [i]);
        }
      }
      return bubbleIndexList;
    }

    // Destroy the bubbles.
    public void DestroyBubbles (ArrayList bubbleIndexList) {
      foreach (IndexPair index in bubbleIndexList) {
        StartCoroutine (bubbleMap [index.X, index.Y].Blast ());
        bubbleMap [index.X, index.Y] = null;
      }
    }

    // Get a list of indeies of bubbles that connect with new bubble and have the same color.
    private ArrayList GetFallBubbles (ArrayList bubbleIndexList) {
      bool[,] keptBubbleMap = new bool[this.numRows + 1, this.numBubblesEachRow + 1];
      Stack bubbleStack = new Stack ();

      // Keep all bubbles in destroyed bubble list.
      foreach (IndexPair index in bubbleIndexList) {
        keptBubbleMap [index.X, index.Y] = true;
      }

      // Keep all top bubbles.
      for (int i = 1; i <= numBubblesEachRow; i++) {
        if (bubbleMap [1, i] != null && !keptBubbleMap [1, i]) {
          // Note that do not expand any bubbles in destroyed bubble list.
          keptBubbleMap [1, i] = true;
          bubbleStack.Push (bubbleMap [1, i]);
        }
      }

      while (bubbleStack.Count != 0) {
        Bubble current = (Bubble)bubbleStack.Pop ();
        IndexPair[] nearbyIndex = getAllNearbyIndex (current.Index.X, current.Index.Y);
        for (int i = 0; i <= 5; i++) {
          if (!IndexCheck (nearbyIndex [i].X, nearbyIndex [i].Y))
            continue;
          if (bubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y] != null
            && !keptBubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y]) {
            keptBubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y] = true;
            bubbleStack.Push (bubbleMap [nearbyIndex [i].X, nearbyIndex [i].Y]);
          }
        }
      }

      ArrayList fallBubbleList = new ArrayList ();
      for (int i = 1; i <= numRows; i++) {
        for (int j = 1; j <= numBubblesEachRow; j++) {
          if (bubbleMap [i, j] != null && !keptBubbleMap [i, j]) {
            fallBubbleList.Add (bubbleMap [i, j]);
          }
        }
      }
      return fallBubbleList;
    }
    
    // The animation to fall bubbles.
    public void MakeBubblesFall (ArrayList fallBubbleList) {
      foreach (Bubble bubble in fallBubbleList) {
        IndexPair index = bubble.Index;
        StartCoroutine (bubble.Fall ());
        bubbleMap [index.X, index.Y] = null;
      }
    }

    public void EraseAndFall (Bubble alignedBubble) {
      ArrayList bubbleIndexList = GetAdjacentSameColorBubbles (alignedBubble.Index, alignedBubble.Color);
      // Do nothing if the number of same color connecting bubbles is less than three.
      if (bubbleIndexList.Count < 3)
        return;

      // Get the list of all bubbles that should fall.
      ArrayList fallBubbleList = GetFallBubbles (bubbleIndexList);

      // Remove direct nodes from bubbleMap.
      DestroyBubbles (bubbleIndexList);
      // Make these bubbles to fall.
      MakeBubblesFall (fallBubbleList);
    }
  }
}

