using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {
  List<CanvasController> controllers;
  CanvasController current;
  public string defaultCanvasId;

  void Awake() {
    controllers = GetComponentsInChildren<CanvasController>().ToList();
    controllers.ForEach(c => c.gameObject.SetActive(false));
    if (defaultCanvasId != "") {
      SetCurrentCanvas(defaultCanvasId);
    }
  }

  protected void SetCurrentCanvas(string id) {
    if (current) {
      current.gameObject.SetActive(false);
    }
    current = controllers.Find(c => c.id == id);
    if (current != null) {
      current.gameObject.SetActive(true);
    }
  }
}
