using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : NodeManager<ObjectiveGraph, ObjectiveNode, ObjectiveOption> {

  virtual public void CompleteObjective() {
    currentNode = currentGraph.GetNodeById(currentNode.defaultOption.next);
    OnNodeGraphUIUpdate((ObjectiveNode)currentNode);
  }

  virtual public void CompleteObjective(ObjectiveOption option) {
    if (option != null) {
      currentNode = currentGraph.GetNodeById(option.next);
      OnNodeGraphUIUpdate((ObjectiveNode)currentNode);
    }
  }

  override protected void OnNodeGraphUIUpdate(ObjectiveNode node) {
    OnObjectiveUIUpdate((ObjectiveNode)node);
  }

  virtual protected void OnObjectiveUIUpdate(ObjectiveNode node) { }

}