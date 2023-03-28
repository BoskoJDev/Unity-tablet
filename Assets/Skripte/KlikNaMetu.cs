using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class KlikNaMetu : NetworkBehaviour
{
    [SyncVar] public bool kartaKliknuta = false;

    public void NaKlikMete()
    {
        this.kartaKliknuta = !this.kartaKliknuta;

        NetworkIdentity id = NetworkClient.connection.identity;
        MenadzerIgraca igrac = id.GetComponent<MenadzerIgraca>();
        Button dugme = GetComponentInChildren<Button>();
        if (this.kartaKliknuta)
        {
            dugme.image.color = Color.yellow;
            Vector3 skaliranje = transform.localScale;
            skaliranje.x += 0.24f;
            skaliranje.y += 0.24f;
            transform.localScale = skaliranje;
            igrac.DodajKartu(gameObject);
        }
        else
        {
            dugme.image.color = Color.white;
            transform.localScale = new Vector3(0.87f, 0.87f);
            igrac.UkloniKartu(gameObject);
        }

        if (hasAuthority)
            igrac.CmdNisaniSvojuKartu(gameObject);
        else
            igrac.CmdNisaniTudjuKartu(gameObject);
    }
}