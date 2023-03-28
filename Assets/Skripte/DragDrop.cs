using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    private bool povlaciSe = false;
    public bool jeMogucePovlaciti = false;
    private GameObject pocetniRoditelj;
    private Vector2 pocetnaPozicija;
    private bool jeIznadStola;

    void Start() => this.jeMogucePovlaciti = hasAuthority;

    void Update()
    {
        if (this.povlaciSe)
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void OnCollisionEnter2D(Collision2D c) => this.jeIznadStola = true;

    void OnCollisionExit2D(Collision2D c) => this.jeIznadStola = false;

    public void Povlaci()
    {
        if (!this.jeMogucePovlaciti)
            return;

        this.povlaciSe = true;
        this.pocetniRoditelj = transform.parent.gameObject;
        this.pocetnaPozicija = transform.position;
    }
    
    public void NePovlaci()
    {
        if (!this.jeMogucePovlaciti)
            return;

        this.povlaciSe = false;
        if (this.jeIznadStola)
        {
            this.jeMogucePovlaciti = false;

            NetworkIdentity identitet = NetworkClient.connection.identity;
            MenadzerIgraca igrac = identitet.GetComponent<MenadzerIgraca>();
            igrac.IzvuciKartu(gameObject);
            if (igrac.ImaKarte())
            {
                KartaIgraca karta = GetComponent<KartaIgraca>();
                int zbirKarata = 0;
                bool imaAs = false;
                bool asOdabran = karta.karta.Broj == 1;
                int brojKarata = 0;
                igrac.IgraceveKarte.ForEach(card =>
                {
                    if (card == null)
                        return;

                    Karta karta = card.GetComponent<KartaIgraca>().karta;
                    imaAs = karta.Broj == 1;
                    zbirKarata += karta.Broj;
                    brojKarata++;
                });

                brojKarata /= 2;
                zbirKarata /= 2;

                if (zbirKarata != 0 && brojKarata != 0)
                {
                    int brojKarte = karta.karta.Broj;
                    Debug.Log(zbirKarata + " : " + brojKarte);

                    Debug.Log((zbirKarata % 11 == 0 && imaAs).ToString() +
                        " : " + (zbirKarata % brojKarte == 0).ToString()
                        + " : " +(zbirKarata % brojKarte == 10 && imaAs));

                    bool mozeDaPokupiKarte = (zbirKarata % 11 == 0 && imaAs) ||
                        zbirKarata % brojKarte == 0 ||
                        (zbirKarata % brojKarte == 10 && imaAs);
                    if (mozeDaPokupiKarte)
                        igrac.CmdPokupi(gameObject);
                }
            }
        }
        else
        {
            transform.position = this.pocetnaPozicija;
            transform.SetParent(this.pocetniRoditelj.transform, false);
        }
    }

    public void Stopiraj() => GetComponent<Animator>().enabled = false;

    public void Unisti() => Destroy(gameObject);
}