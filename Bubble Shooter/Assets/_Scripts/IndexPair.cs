using UnityEngine;

namespace BubbleShooter {
  public class IndexPair {
    private int x;
    private int y;
    
    public IndexPair (int x, int y) {
      this.x = x;
      this.y = y;
    }

    public int X {
      get {
        return x;
      }
      set {
        x = value;
      }
    }
    
    public int Y {
      get {
        return y;
      }
      set {
        y = value;
      }
    }

    public override bool Equals (System.Object other) {
      IndexPair otherPair = other as IndexPair;

      if (otherPair == null)
        return false;

      return this.X == otherPair.X && this.Y == otherPair.Y;
    }

    public override int GetHashCode () {
      return this.X << 15 + this.Y;
    }
  }
}

