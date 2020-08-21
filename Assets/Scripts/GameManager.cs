using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    //--------------------------------------------------------
    void Start()
    {
        
    }

    //--------------------------------------------------------
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnBazuca());
        }
    }

    //--------------------------------------------------------
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartCoroutine(SpawnBazuca());
        }
    }

    //--------------------------------------------------------
    public IEnumerator SpawnBazuca()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

            Vector3 position = Random.insideUnitSphere * 25;
            position.y = 1.5f;
            PhotonNetwork.InstantiateSceneObject("BazucaObject", position, Quaternion.Euler(270, Random.Range(0, 360), 180));
        }
    }
}













