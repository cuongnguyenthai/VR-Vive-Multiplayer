using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VRSpawn : NetworkBehaviour {

    // Use this for initialization
    public Behaviour[] Behavior;

	void Start () {
		if (isLocalPlayer)
        {
            GetComponent<SteamVR_ControllerManager>().enabled = true;
        } else
        {
            foreach (Behaviour x in Behavior)
            {
                x.enabled = false;
            }
        }
	}

    [Command]
    public void CmdAddLocalAuthority(GameObject obj)
    {
        NetworkInstanceId nId = obj.GetComponent<NetworkIdentity>().netId;
        GameObject go = NetworkServer.FindLocalObject(nId);
        go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        RpcSendLog("Log: " + go.GetComponent<NetworkIdentity>().clientAuthorityOwner.ToString());
    }

    [ClientRpc]
    public void RpcSendLog(string conn)
    {
        Debug.Log(conn);
    }

    [Command]
    public void CmdRemoveLocalAuthority(GameObject obj)
    {
        NetworkInstanceId nId = obj.GetComponent<NetworkIdentity>().netId;
        GameObject go = NetworkServer.FindLocalObject(nId);
        NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
        ni.RemoveClientAuthority(ni.clientAuthorityOwner);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnDestroy()
    {
        if (GetComponentInChildren<SteamVR_ControllerManager>() != null)
        {
            GetComponentInChildren<SteamVR_ControllerManager>().enabled = false;
        }
    }
}
