using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : NodeManager<Questline, QuestNode, QuestOption> {

  virtual public void CompleteObjective() {
    currentNode = currentGraph.GetNodeById(currentNode.defaultOption.next);
    OnNodeGraphUIUpdate((QuestNode)currentNode);
  }

  virtual public void CompleteObjective(QuestOption option) {
    if (option != null) {
      currentNode = currentGraph.GetNodeById(option.next);
      OnNodeGraphUIUpdate((QuestNode)currentNode);
    }
  }

  override protected void OnNodeGraphUIUpdate(QuestNode node) {
    OnObjectiveUIUpdate((QuestNode)node);
  }

  virtual protected void OnObjectiveUIUpdate(QuestNode node) { }

}