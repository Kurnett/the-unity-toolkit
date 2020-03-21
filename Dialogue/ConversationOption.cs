using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationOption : NodeOption {
  public string response;

  // Needs initialization
  public Conversation conversation;
  private Action<Conversation> SaveConversation;

  // public ConversationOption(
  //   NodeGraph graph,
  //   Action<NodeGraph> SaveGraph
  // ) : base(graph, SaveGraph) { }
}