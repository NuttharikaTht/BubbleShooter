using UnityEngine;
using System;
using System.Collections.Generic;

namespace BubbleShooter {
  // Enum for bubble color.
  public enum BubbleColor {
    Red = 0,
    Green,
    Blue,
    Yellow,
    Purple,
  }

  public class Pallette {
    private int numBubbleColors;
    private Color[] colorTable;
    private System.Random rand;

    public Pallette () {
      // Initialize random number generator with a random seed.
      rand = new System.Random (Guid.NewGuid ().GetHashCode ());
      InitializeColorTable ();
    }

    // Initialize color table in this function.
    private void InitializeColorTable () {
      numBubbleColors = Enum.GetNames (typeof(BubbleColor)).Length;
      colorTable = new Color[numBubbleColors];

      // Initialize colors here.
      colorTable [(int)BubbleColor.Red] = new Color (255, 107, 107);
      colorTable [(int)BubbleColor.Green] = new Color (113, 248, 142);
      colorTable [(int)BubbleColor.Blue] = new Color (167, 150, 255);
      colorTable [(int)BubbleColor.Yellow] = new Color (255, 251, 111);
      colorTable [(int)BubbleColor.Purple] = new Color (248, 120, 255);
    }

    // Get color from BubbleColor enum type.
    public Color GetColor (BubbleColor bubbleColor) {
      return colorTable [(int)bubbleColor];
    }

    // Get a random BubbleColor.
    public BubbleColor GetRandomBubbleColor () {
      return (BubbleColor)rand.Next (numBubbleColors);
    }

    // Get a random color.
    public Color GetRandomColor () {
      BubbleColor bubbleColor = GetRandomBubbleColor ();
      return GetColor (bubbleColor);
    }
  }
}