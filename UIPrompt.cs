using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Update UIPrompt to provide override hooks.
[CreateAssetMenu(menuName = "UI/Prompt")]
public class UIPrompt : ScriptableObject {

  public GameObject canvas;
  GameObject currentCanvas;

  public void CreateCanvas() {
    DestroyCanvas();
    currentCanvas = Instantiate(canvas);
  }

  public void DestroyCanvas() {
    if (currentCanvas != null) {
      Destroy(currentCanvas);
      currentCanvas = null;
    }
  }

  public void UpdateCanvas() {
    // Do inherited stuff here.
  }

}
