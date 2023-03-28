using Mirror;

public class DeliKarte : NetworkBehaviour
{
    public void Razdeli()
    {
        NetworkIdentity identitet = NetworkClient.connection.identity;
        MenadzerIgraca igrac = identitet.GetComponent<MenadzerIgraca>();
        if (MenadzerIgraca.brojIgraca == 2 && igrac.NemaKarte())
            igrac.CmdDeliKarte(6, "neigrana");
    }
}