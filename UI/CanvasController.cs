using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {
  public string id;

  virtual public IEnumerator Open() {
    yield return new WaitForSeconds(0f);
  }

  virtual public IEnumerator Close() {
    gameObject.SetActive(false);
    yield return new WaitForSeconds(0f);
  }
}
