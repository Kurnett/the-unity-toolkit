using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Objectives/ObjectiveGraph")]
public class ObjectiveGraph : NodeGraph<ObjectiveNode, ObjectiveOption, Flag> {
  protected ObjectiveNode currentObjective;

  public void SetCurrentObjective (ObjectiveNode objective) {
    currentObjective = objective;
  }

  public ObjectiveNode GetCurrentObjective () {
    return currentObjective;
  }

  public void CompleteCurrentObjective () {
    ObjectiveNode next = GetNodeById(currentObjective.defaultOption.next);
    if (next != null) {
      currentObjective = next;
    }
  }

  public void CompleteCurrentObjective (ObjectiveOption option) {
    // Fill in later once branching objective paths are necessary.
  }
 }
