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
		private float percentageOfGameHeight = 0.8f;

		private float bubbleRadius;
		// Maximal number of bubbles vertically.
		private int numColumns;

		// The two-dementional array to store the info of bubble map.
		private Bubble[,] bubbleMap;

		public GameBoard (float width, float height, int numBubblesEachRow, float percentageOfGameHeight) {
			this.percentageOfGameHeight = percentageOfGameHeight;
			GameBoard (width, height, numBubblesEachRow);
		}

		public GameBoard (float width, float height, int numBubblesEachRow)
		{
			this.width = width;
			this.height = height;
			this.numBubblesEachRow = numBubblesEachRow;
			this.bubbleRadius = 0.5f * (width / numBubblesEachRow);

			float gameHeight = percentageOfGameHeight * height;
			if ((gameHeight - 2 * this.bubbleRadius) < 0) {
				this.numColumns = 0;
			} else {
				this.numColumns = 1 + Convert.ToInt32((gameHeight - 2 * this.bubbleRadius) / (this.bubbleRadius * Math.Sqrt(3)));
			}
			// Initialize the bubble map.
			bubbleMap = new Bubble[this.numBubblesEachRow, this.numColumns];
		}

		// Checks whether the indexes exists in game's map.
		private bool IndexCheck (int xIndex, int yIndex) {
			if (yIndex > this.numColumns)
				return false;
			
			Position position = new Position();
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
			if (!IndexCheck(xIndex,yIndex))
				return null;

			Position position = new Position();
			if (yIndex % 2 == 1) {
				position.SetX ((-0.5 * this.width) + this.bubbleRadius * (yIndex * 2 - 1));

			} else {
				position.SetX ((-0.5 * this.width) + this.bubbleRadius * (yIndex * 2));
			}
			position.SetY ((0.5 * this.height) - (this.bubbleRadius * (1 + Math.Sqrt(3) * (xIndex - 1))));
			return position;
		}

		private bool StoreBubbleToMap (Bubble bubble) {
			bubbleMap [bubble.GetXIndex(), bubble.GetYIndex()] = bubble;
		}

//		// Snap the bubble to the game board.
//		public void SnapBubble (Bubble bubble)
//		{
//		}
//
//		// Destroy the bubbles.
//		public void DestroyBubbles (IEnumerable bubbles)
//		{
//			foreach (Bubble bubble in bubbles) {
//				bubble.Destroy ();
//			}
//		}
//
//		public void MakeBubblesFall (IEnumerable bubbles)
//		{
//			foreach (Bubble bubble in bubbles) {
//				bubble.Fall ();
//			}
//		}

	}
}

