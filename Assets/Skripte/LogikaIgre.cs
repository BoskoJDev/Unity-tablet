using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LogikaIgre : NetworkBehaviour
{
    public bool kartaSelektovana = false;

    public void IzvuciKartu()
    {
        if (!GetComponent<KartaIgraca>().jeNaStolu)
            return;

        this.kartaSelektovana = !this.kartaSelektovana;
        
        NetworkIdentity id = NetworkClient.connection.identity;
        MenadzerIgraca igrac = id.GetComponent<MenadzerIgraca>();
        Button dugme = GetComponentInChildren<Button>();
        if (this.kartaSelektovana)
        {
            dugme.image.color = Color.yellow;
            Vector3 skaliranje = transform.localScale;
            skaliranje.x += 0.24f;
            skaliranje.y += 0.24f;
            transform.localScale = skaliranje;

            Debug.Log(transform.localScale);

            igrac.DodajKartu(gameObject);
        }
        else
        {
            dugme.image.color = Color.white;
            transform.localScale = new Vector3(0.87f, 0.87f);
            igrac.UkloniKartu(gameObject);
        }
    }
}