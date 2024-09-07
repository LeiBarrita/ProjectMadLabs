using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

// Deprecated, using Player instead
public class BasePlayer : NetworkBehaviour
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;

    // private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // public override void OnNetworkSpawn()
    // {
    //     randomNumber.OnValueChanged += (int prevVal, int newVal) =>
    //     {
    //         Debug.Log(OwnerClientId + ": " + randomNumber.Value);
    //     };
    // }

    void Start()
    {
        // Camera.main.transform.parent = transform;
        // Camera.main.transform.position = Vector3.zero;
        // UIDocument playerUI = GameObject.Find("PlayerUI").GetComponent<UIDocument>();
        // playerUI.enabled = true;

        onPlayerSpawns.Raise(this, null);
    }

    void Update()
    {
        if (!IsOwner) return;

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     randomNumber.Value = Random.Range(0, 100);
        // }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
