using UnityEngine;

namespace BubbleShooter {
  public class Position {
    private float x;
    private float y;

    public Position () {
    }
    
    public Position (float x, float y) {
      this.x = x;
      this.y = y;
    }
    
    public void SetX (float x) {
      this.x = x;
    }
    
    public void SetY (float y) {
      this.y = y;
    }
    
    public float GetX () {
      return this.x;
    }
    
    public float GetY () {
      return this.y;
    }
  }
}

