// using System.Collections;
// using NUnit.Framework;
// using Unity.Netcode;
// using Unity.Netcode.TestHelpers.Runtime;   // NetcodeIntegrationTest + helpers
// using UnityEngine;
// using UnityEngine.TestTools;

// // (1) Le composant réseau qu'on veut tester : un simple score répliqué.
// public class Score : NetworkBehaviour
// {
//     // écrit par le serveur, lu par tout le monde
//     public NetworkVariable<int> Value = new NetworkVariable<int>(
//         0,
//         NetworkVariableReadPermission.Everyone,
//         NetworkVariableWritePermission.Server);
// }

// // (2) Le test : 1 serveur + 2 clients dans le MÊME process, en loopback.
// public class ScoreReplicationTests : NetcodeIntegrationTest
// {
//     // La classe de base démarre serveur + N clients pour toi
//     // (c'est ça, ton "StartServerAndClients")
//     protected override int NumberOfClients => 2;

//     private GameObject m_Prefab;

//     // Appelé AVANT le start : on enregistre le prefab réseau des deux côtés
//     protected override void OnServerAndClientsCreated()
//     {
//         m_Prefab = CreateNetworkObjectPrefab("ScoreObject");
//         m_Prefab.AddComponent<Score>();
//         base.OnServerAndClientsCreated();
//     }

//     [UnityTest]
//     public IEnumerator Server_SetsScore_ClientsSeeIt()
//     {
//         // --- spawn ce qu'il faut (côté serveur = autorité) ---
//         var serverObj = SpawnObject(m_Prefab, m_ServerNetworkManager)
//                         .GetComponent<NetworkObject>();
//         ulong netId = serverObj.NetworkObjectId;

//         // --- attendre que l'objet soit répliqué chez les 2 clients (avec timeout) ---
//         yield return WaitForConditionOrTimeOut(() =>
//             m_ClientNetworkManagers[0].SpawnManager.SpawnedObjects.ContainsKey(netId) &&
//             m_ClientNetworkManagers[1].SpawnManager.SpawnedObjects.ContainsKey(netId));
//         AssertOnTimeout("L'objet n'a pas été répliqué chez les clients");

//         // --- le serveur modifie l'état ---
//         serverObj.GetComponent<Score>().Value.Value = 42;

//         // --- avancer les ticks + attendre que la valeur arrive (avec timeout) ---
//         yield return WaitForConditionOrTimeOut(() =>
//         {
//             var c0 = m_ClientNetworkManagers[0].SpawnManager.SpawnedObjects[netId].GetComponent<Score>();
//             var c1 = m_ClientNetworkManagers[1].SpawnManager.SpawnedObjects[netId].GetComponent<Score>();
//             return c0.Value.Value == 42 && c1.Value.Value == 42;
//         });
//         AssertOnTimeout("Le score répliqué n'a pas atteint 42 chez les clients");
//     }
// }