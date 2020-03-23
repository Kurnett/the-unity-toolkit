using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

  public Questline currentQuestline;
  public QuestNode currentNode;

  virtual protected void ObjectiveDidUpdate(QuestNode node) { }

  virtual protected void CompleteObjective() {
    currentNode = (QuestNode)currentQuestline.GetNodeById(currentNode.defaultOption.next);
    ObjectiveDidUpdate(currentNode);
  }

}