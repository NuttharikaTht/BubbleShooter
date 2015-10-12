using UnityEngine;
using System;
using System.Collections;

namespace BubbleShooter
{
	public class GameBoard
	{
		private float width;
		private float height;
		private int numBubblesEachRow;
		private float bubbleRadius;

		public GameBoard (float width, float height, int numBubblesEachRow)
		{
			this.width = width;
			this.height = height;
			this.numBubblesEachRow = numBubblesEachRow;
			this.bubbleRadius = 0.5f * width / numBubblesEachRow;
		}

		// Snap the bubble to the game board.
		public void snapBubble (Bubble bubble)
		{
		}

		public void destroyBubbles (IEnumerable bubbles)
		{
			foreach (Bubble bubble in bubbles) {
				bubble.Destroy ();
			}
		}

		public void makeBubblesFall (IEnumerable bubbles)
		{
			foreach (Bubble bubble in bubbles) {
				bubble.Fall ();
			}
		}

	}
}

