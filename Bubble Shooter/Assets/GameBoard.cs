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
				this.numColumns = 1 + (gameHeight - 2 * this.bubbleRadius) / (this.bubbleRadius * Math.Sqrt(3));
			}
		}

		// Attention: The index starts from 1.
		public Position getPosition (int xIndex, int yIndex) {
			if (yIndex > this.numColumns)
				return null;

			Position position = new Position();
			if (yIndex % 2 == 1) {
				if (xIndex > this.numBubblesEachRow)
					return null;
				position.setX ((-0.5 * this.width) + this.bubbleRadius * (yIndex * 2 - 1));

			} else {
				if (xIndex > (this.numBubblesEachRow - 1))
					return null;
				position.setX ((-0.5 * this.width) + this.bubbleRadius * (yIndex * 2));
			}
			position.setY ((0.5 * this.height) - (this.bubbleRadius * (1 + Math.Sqrt(3) * (xIndex - 1))));
			return position;
		}



//		// Snap the bubble to the game board.
//		public void snapBubble (Bubble bubble)
//		{
//		}
//
//		// Destroy the bubbles.
//		public void destroyBubbles (IEnumerable bubbles)
//		{
//			foreach (Bubble bubble in bubbles) {
//				bubble.Destroy ();
//			}
//		}
//
//		public void makeBubblesFall (IEnumerable bubbles)
//		{
//			foreach (Bubble bubble in bubbles) {
//				bubble.Fall ();
//			}
//		}

	}

	public class Position {
		private float x;
		private float y;

		public Position (float x, float y) {
			this.x = x;
			this.y = y;
		}

		public void setX (float x) {
			this.x = x;
		}

		public void setY (float y) {
			this.y = y;
		}

		public float getX () {
			return this.x;
		}

		public float getY () {
			return this.y;
		}
	}
}

