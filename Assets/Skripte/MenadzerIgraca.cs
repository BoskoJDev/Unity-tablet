using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenadzerIgraca : NetworkBehaviour
{
    Text tekst;
    GameObject panel;
    GameObject karteIgraca;
    GameObject karteProtivnika;
    GameObject karteNaStolu;
    Button dodajKarte;

    [SyncVar(hook = nameof(PromenaReda))]
    bool red = true;

    readonly List<GameObject> igraceveKarte = new List<GameObject>();
    [SyncVar(hook = nameof(NaDodavanjePoena))]
    public int poeni = 0;
    public static int brojIgraca = 0;
    [SyncVar] public string nadimak;

    public static List<MenadzerIgraca> igraci = new List<MenadzerIgraca>(2);

    void NaDodavanjePoena(int f, int i)
    {
        this.tekst.text = this.nadimak + ": " + this.poeni;
    }

    void PromenaReda(bool b1, bool b2)
    {
        Transform karteIgraca = this.karteIgraca.transform;
        for (int i = 0; i < karteIgraca.childCount; i++)
        {
            GameObject t = karteIgraca.GetChild(i).gameObject;
            DragDrop dd = t.GetComponent<DragDrop>();
            dd.enabled = !dd.enabled;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkManagerHUD.broj++;

        this.panel = GameObject.Find("Panel");
        this.dodajKarte = GameObject.Find("Button").GetComponent<Button>();
        this.karteIgraca = GameObject.Find("KarteIgraca");
        this.karteProtivnika = GameObject.Find("KarteProtivnika");
        this.karteNaStolu = GameObject.Find("KarteNaStolu");

        brojIgraca++;
        if (brojIgraca != 2)
            return;

        int broj = 1;
        this.CmdDeliKarte(4, "igrana");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            MenadzerIgraca igrac = obj.GetComponent<MenadzerIgraca>();
            igrac.CmdPostaviNadimak(broj);
            igrac.CmdDeliKarte(6, "neigrana");
            igraci.Add(igrac);
            broj++;
        }
    }

    void Update()
    {
        if (brojIgraca != 2)
            return;

        bool jeZahtevanPrekidVeze = Input.GetKey(KeyCode.Escape) ||
            (MenadzerIgre.Instanca.SveKarte.Count == 0 && this.NemaKarata());
        if (jeZahtevanPrekidVeze)
        {
            string pobednik;
            if (igraci[0].poeni > igraci[1].poeni)
                pobednik = igraci[0].nadimak;
            else
                pobednik = igraci[1].nadimak;

            PlayerPrefs.SetString("pobednik", pobednik);
            PlayerPrefs.DeleteKey("karte");
            NetworkClient.Shutdown();
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        int karte = MenadzerIgre.Instanca.SveKarte.Count;
        if (SceneManager.GetActiveScene().name != "Odjava" && karte != 0)
            SceneManager.LoadScene("Odjava");
    }

    [Command]
    public void CmdPostaviNadimak(int broj) => this.RpcPostaviNadimak(broj);

    [ClientRpc]
    void RpcPostaviNadimak(int broj)
    {
        this.nadimak = PlayerPrefs.GetString("igrac" + broj);
        this.tekst = GetComponentInChildren<Text>();
        this.tekst.text = this.nadimak + ": " + this.poeni;
        this.tekst.gameObject.transform.position = new Vector3(5.0f, 0.0f);
        this.tekst.fontSize = 40;
        this.tekst.gameObject.transform.SetParent(this.panel.transform);
    }

    public bool NemaKarata()
    {
        return this.karteIgraca.transform.childCount == 0 &&
            this.karteProtivnika.transform.childCount == 0;
    }

    public bool NemaKarte() => this.karteIgraca.transform.childCount == 0;

    [Command(requiresAuthority = false)]
    public void CmdNisaniSvojuKartu(GameObject card) => TargetSvoje(card);

    [Command(requiresAuthority = false)]
    public void CmdNisaniTudjuKartu(GameObject card) => TargetTudje(card);

    [ClientRpc]
    void TargetSvoje(GameObject card) => this.DodajKartu(card);

    [ClientRpc]
    void TargetTudje(GameObject card) => this.DodajKartu(card);

    [Command]
    public void CmdDeliKarte(int karte, string tip)
    {
        for (int i = 0; i < karte; i++)
        {
            List<GameObject> sveKarte = MenadzerIgre.Instanca.SveKarte;
            GameObject karta = sveKarte[Random.Range(0, sveKarte.Count)];
            GameObject k = Instantiate(karta, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(k, connectionToClient);
            RpcPrikaziKartu(k, tip);
            sveKarte.Remove(karta);
            PlayerPrefs.SetInt("karte", sveKarte.Count);
        }
    }

    public void IzvuciKartu(GameObject card) => CmdIzvuciKartu(card);

    [Command(requiresAuthority = false)]
    public void CmdPokupi(GameObject karta)
    {
        int osvojeniPoeni = 0;
        this.DodajKartu(karta);
        this.igraceveKarte.ForEach(objekat =>
        {
            if (objekat == null)
                return;

            KartaIgraca ki = objekat.GetComponent<KartaIgraca>();
            Karta card = ki.karta;
            int broj = card.Broj;
            bool jeDvojkaTref = broj == 2 && card.Simbol == Karta.Znak.TREF;
            bool kartaDonelaPoen = broj == 1 || broj == 10 || broj == 12 ||
                broj == 13 || broj == 14;
            if (kartaDonelaPoen || jeDvojkaTref)
            {
                if (card.Broj == 10 && card.Simbol == Karta.Znak.KARO)
                    osvojeniPoeni += 2;
                else
                    osvojeniPoeni += 1;
            }
                
            RpcPrikaziKartu(objekat, "izvucena");
        });

        if (isServer)
            this.DodajPoene(osvojeniPoeni);
    }

    [Server]
    void DodajPoene(int poeni) => this.poeni += poeni;

    [Command]
    void CmdIzvuciKartu(GameObject card)
    {
        RpcPrikaziKartu(card, "igrana");
        if (isServer)
            red = !red;
    }

    [ClientRpc]
    void RpcPrikaziKartu(GameObject card, string tip)
    {
        if (card == null)
            return;

        switch (tip)
        {
            case "neigrana":
                {
                    if (hasAuthority)
                        card.transform.SetParent(this.karteIgraca.transform, false);
                    else
                        card.transform.SetParent(this.karteProtivnika.transform, false);

                    card.GetComponent<DragDrop>().enabled = isServer;
                    card.GetComponent<Animator>().enabled = true;
                }
                break;
            case "igrana":
                {
                    card.GetComponent<KartaIgraca>().jeNaStolu = true;
                    card.transform.SetParent(this.karteNaStolu.transform, false);
                    if (!hasAuthority)
                    {
                        card.GetComponent<OkretanjeKarata>().Okreni();
                        card.GetComponentInChildren<Button>().interactable = true;
                    }
                }
                break;
            case "izvucena":
                {
                    card.GetComponent<Animator>().enabled = true;
                    this.igraceveKarte.Remove(card);
                }
                break;
        }
    }

    public void DodajKartu(GameObject card)
    {
        if (card == null)
            return;

        this.igraceveKarte.Add(card);
    }

    public void UkloniKartu(GameObject card)
    {
        if (card == null)
            return;

        this.igraceveKarte.Remove(card);
    }

    public bool ImaKarte() => this.igraceveKarte.Count > 0;

    public List<GameObject> IgraceveKarte => this.igraceveKarte;
}