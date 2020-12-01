using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : NodeManager<ObjectiveGraph, ObjectiveNode, ObjectiveOption, Flag> {
  
  public List<ObjectiveGraph> objectiveList;
  protected ObjectiveGraph currentObjective;

  virtual public void CompleteObjective() {
    currentNode = currentGraph.GetNodeById(currentNode.defaultOption.next);
    OnNodeGraphUIUpdate(currentNode);
  }

  virtual public void CompleteObjective(string id) {
    foreach(ObjectiveGraph graph in objectiveList) {
      currentNode = currentGraph.GetNodeById(currentNode.defaultOption.next);
    }
    OnNodeGraphUIUpdate(currentNode);
  }

  virtual public void CompleteObjective(ObjectiveOption option) {
    if (option != null) {
      currentNode = currentGraph.GetNodeById(option.next);
      OnNodeGraphUIUpdate(currentNode);
    }
  }

  override protected void OnNodeGraphUIUpdate(ObjectiveNode node) {
    OnObjectiveUIUpdate(node);
  }

  virtual protected void OnObjectiveUIUpdate(ObjectiveNode node) { }

}