using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : GeneralNetworkTask
{
    public override IEnumerator Implementation_Server()
    {
        yield return null;
    }

    public override void SendToClients(Server server)
    {
    }

    public override IEnumerator Implementation_Client()
    {
        yield return null;
    }

    public override void RequestServer()
    {
    }
}
