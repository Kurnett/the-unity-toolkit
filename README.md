# The Unity Toolkit
A collection of common video game features, implemented in C# for Unity. The goal is to make a collection of tools that are easy-to-use, customizable, and lightweight.

## Not-even-an-alpha disclaimer
The Unity Toolkit is currently still in development. If you find parts of it that are already useful (such as the dialogue editor), feel free to use it or make PRs for improvements! However, be aware that everything is still subject to change without warning until the project is much further along in the development process.

## Components
Versioning is approximate. We'll switch over to semantic versioning as soon as APIs are moderately finalized - they just change too rapidly to realistically track right now.

### Versions

- v0: Not started.
- v1: Something functional. Not using inheritance and next to no API support.
- v2: Functional and partially extensible. Uses inheritance for input, UI, etc.
- v3: Functional and mostly extensible. More-or-less completely implemented inheritance and a solid start with APIs.
- v4: Entirely extensible. APIs might change somewhat, but are essentially complete.
- v5: Ready for release?

### Component List

- Movement: v1
- Camera Controls: v1
- Inventory: v2
- Dialogue Management: v3
- NPC AI: v1
- Player Interactions: v2
- Objective Management: v2
- Level Management: v0
- Health: v0
- Menu Management: v0

## Dialogue
### Creating a Conversation
Conversation objects can be created by right-clicking in the Project directory and selecting Create > Dialogue > Conversation.
### Creating a Speaker.
Speaker objects are used to structure data related to a given character in the dialogue editor. You can create a speaker by right-clicking in the Project directory and selecting Create > Dialogue > Speaker.
### Accessing the Editor
You can open the editor by going to Window > Dialogue Editor. Once you've created a conversation, you can select it to begin creating a dialogue tree.
### Editing a Conversation
When you have a conversation selected in the dialogue editor, you can right-click in the editor window to create a dialogue node. Similarly, you can right-click on a dialogue node to remove it. You can also click and drag nodes to move them in the editor window.

Nodes define the flow of the conversation.
- Speaker: The character saying the line.
- Start Conv: Indicates the default starting node of the conversation. Only one node can be marked as the start of the conversation.
- End Conv: Indicates an ending point of the conversation.
- Auto-Proceed: Indicates that a node does not require a response from the player (for instance, to handle multiple lines of back-and-forth dialogue without any response options).
- Auto-Length: Indicates how long the conversation wait before auto-proceeding.
- Dialogue: Indicates the text of the dialogue node.
- Response Options
  - Up: Move the option up in the list of responses.
  - Down: Move the option down in the list of responses.
  - Text: An input field for the text of the response.
  - R: Removes the response from the list of options.
  - +: Allows the user to create a connection between a response and the next node in the dialogue.
- Add Response: Adds an option to the list of possible responses.
### Extending the Dialogue System
#### OnConversationUIUpdate
By default, the dialogue system does not include a pre-packaged UI. To build your own, create a class that inherits `DialogueManager` and override the `OnConversationUIUpdate` method. This method is called each time the conversation progresses to a new node, and passes in a `ConversationNode` object representing the new state of the conversation.

```csharp
public class DialogueManagerWithUI : DialogueManager {
  override public void OnConversationUIUpdate(ConversationNode node) {
    // Do UI stuff here.
  }
}
```
#### OnConversationResponse
Placeholder. Currently allows you to call the `Respond` method on the base class and pass in an integer representing the index of the option in the response list. Will likely change in the near future.
#### StartConversation
Placeholder. This will likely change to `ConversationWillStart` and `ConversationDidStart` in the near future.
#### EndConversation
Placeholder. This will likely change to `ConversationWillEnd` and `ConversationDidEnd` in the near future.

## Follow the Development Process
Think this looks useful? The Toolkit is regularly improved upon in my game development streams. Follow along at [twitch.tv/john_of_all_trades_](https://www.twitch.tv/john_of_all_trades_)!
