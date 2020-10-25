using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : NodeGraph<ConversationNode, ConversationOption> { }
