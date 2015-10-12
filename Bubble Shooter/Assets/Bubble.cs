using UnityEngine;
using System.Collections;

namespace BubbleShooter
{
	public class Bubble : MonoBehaviour
	{
		private int xIndex;
		private int yIndex;
		private Position position;

		public enum Color { Red, Blue, Yellow, Green, Purple };
		private Color color;

		public Bubble (int xIndex, int yIndex, Position position, Color color) {
			this.xIndex = xIndex;
			this.yIndex = yIndex;
			this.position = position;
			this.color = color;
		}

		public int GetXIndex() {
			return xIndex;
		}

		public int GetYIndex() {
			return yIndex;
		}

		public Position GetPosition() {
			return position;
		}

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		public void Destroy ()
		{
		}

		public void Fall ()
		{
		}
	}
}