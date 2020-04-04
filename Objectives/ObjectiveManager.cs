using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : NodeManager {

  virtual public void CompleteObjective() {
    currentNode = currentGraph.GetNodeById(currentNode.defaultOption.next);
    OnNodeGraphUIUpdate((QuestNode)currentNode);
  }

  override protected void OnNodeGraphUIUpdate(Node node) {
    OnObjectiveUIUpdate((QuestNode)node);
  }

  virtual protected void OnObjectiveUIUpdate(QuestNode node) { }

}