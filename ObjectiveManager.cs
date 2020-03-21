// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// TODO: Convert ObjectiveManager to NodeGraph inheritance model.
// TODO: Implement ObjectiveEditor derived from NodeEditor.
// TODO: Split UI elements into local inherited class.

// public class ObjectiveManager : MonoBehaviour {

//     [System.Serializable]
//     public class Objective {
//         public string id;
//         public string label;
//         public string[] nextObjective;
//         public bool cutscene;

//         public Transform playerSpawn;

//         public Transform NPCDestination;
//         public string targetNPC;
//     }

//     public string startObjective;
//     public List<Objective> objectiveList = new List<Objective> ();
//     public Objective currentObjective;
//     public GameObject objectiveCanvas;
//     public GameObject currentCanvas;

//     public GameObject playerPrefab;
//     GameObject currentPlayer;

//     void Start () {
//         SetObjective (startObjective);
//         GenerateUI ();
//     }

//     void Update () {
//         if (Input.GetKeyDown (KeyCode.T)) {
//             ResetToObjective ();
//         }
//     }

//     public void GenerateUI () {
//         if (currentCanvas) {
//             Destroy (currentCanvas);
//         }
//         currentCanvas = Instantiate (objectiveCanvas);
//         SetLabel (currentObjective.label);
//     }

//     public void DestroyUI () {
//         if (currentCanvas) {
//             Destroy (currentCanvas);
//         }
//     }

//     Objective GetObjectiveByID (string id) {
//         foreach (Objective objective in objectiveList) {
//             if (objective.id == id) {
//                 return objective;
//             }
//         }
//         return null;
//     }

//     void SetLabel (string label) {
//         if (currentCanvas) {
//             currentCanvas.transform.GetChild (0).GetComponent<Text> ().text = label;
//         }
//     }

//     public void CompleteObjective (string id) {
//         foreach (string option in currentObjective.nextObjective) {
//             if (option == id) {
//                 Objective newObjective = GetObjectiveByID (id);

//                 SetLabel (newObjective.label);
//                 currentObjective = newObjective;
//                 ClearNPCDestinations ();
//                 if (currentObjective.NPCDestination != null) {
//                     GetNPCByName (currentObjective.targetNPC).SetGoToTarget (currentObjective.NPCDestination);
//                 }
//             }
//         }
//     }

//     void ResetToObjective () {
//         Destroy (currentPlayer);
//         currentPlayer = Instantiate (playerPrefab, currentObjective.playerSpawn.position, currentObjective.playerSpawn.rotation);
//         ClearNPCDestinations ();
//         if (currentObjective.NPCDestination != null) {
//             GetNPCByName (currentObjective.targetNPC).SetGoToTarget (currentObjective.NPCDestination);
//         }
//     }

//     public void SetObjective (string newObjective) {
//         currentObjective = GetObjectiveByID (newObjective);
//         ResetToObjective ();
//     }

//     void ClearNPCDestinations () {
//         CharacterAI[] npcList = FindObjectsOfType<CharacterAI> ();
//         foreach (CharacterAI npc in npcList) {
//             npc.ClearGoToTarget ();
//         }
//     }

//     CharacterAI GetNPCByName (string name) {
//         CharacterAI[] npcList = FindObjectsOfType<CharacterAI> ();
//         foreach (CharacterAI npc in npcList) {
//             if (npc.characterName == name) {
//                 return npc;
//             }
//         }
//         return null;
//     }

// }