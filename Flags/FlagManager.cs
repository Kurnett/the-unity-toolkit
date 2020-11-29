using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour {
  [SerializeField]
  private List<Flag> activeFlags = new List<Flag>();

  public void SetFlag(Flag flag) {
    if (!activeFlags.Contains(flag)) {
      activeFlags.Add(flag);
    }
  }

  public void RemoveFlag(Flag flag) {
    if (activeFlags.Contains(flag)) {
      activeFlags.Remove(flag);
    }
  }

  public bool HasFlag(Flag flag) {
    return activeFlags.Contains(flag);
  }
}
