using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TablicFunkcije : MonoBehaviour
{
    GameObject dijalog;
    GameObject roditelj;
    static int strana = 1;
    static bool jeZapoceto1;
    static bool jeZapoceto2;

    Button prev;
    Button next;
    AudioSource audio;

    Text[] tekstovi;

    void Start()
    {
        string imeScene = SceneManager.GetActiveScene().name;

        if (imeScene == "Instrukcije")
        {
            this.prev = GameObject.Find("Prev").GetComponent<Button>();
            this.next = GameObject.Find("Next").GetComponent<Button>();

            tekstovi = new Text[4]
            {
                GameObject.Find("Tekst1").GetComponent<Text>(),
                GameObject.Find("Tekst2").GetComponent<Text>(),
                GameObject.Find("Tekst3").GetComponent<Text>(),
                GameObject.Find("Stranica").GetComponent<Text>()
            };
        }
        
        this.dijalog = GameObject.Find("Podesavanje");

        if (imeScene == "Meni" || imeScene == "Postolje")
        {
            this.audio = GameObject.Find("Kamera").GetComponent<AudioSource>();
            this.audio.volume = PlayerPrefs.GetFloat("zvuk", 1.0f);
        }

        if (imeScene != "Meni")
            return;
        
        this.roditelj = GameObject.Find("Roditelj");

        jeZapoceto1 = false;
        jeZapoceto2 = false;
    }

    public static void OmogucivanjeTranzicije(string objekat)
    {
        GameObject trans = GameObject.Find(objekat);
        trans.GetComponent<Image>().enabled = true;
        trans.GetComponent<Animator>().enabled = true;
    }

    public void Zapocni() => OmogucivanjeTranzicije("PostoljeTranzicija");

    public void Izadji() => Application.Quit();

    public void Zavrsi()
    {
        PlayerPrefs.SetInt("igraPokrenuta", 0);
        OmogucivanjeTranzicije("Izlaz");
    }

    public void PrikaziSakrijPodesavanje(bool prikazati)
    {
        if (this.dijalog is not null)
        {
            if (SceneManager.GetActiveScene().name == "Meni")
                this.roditelj.GetComponent<Animator>().enabled = true;

            this.dijalog.GetComponent<Animator>().enabled = true;
            if (prikazati)
            {
                GameObject slajder = this.dijalog.transform.GetChild(0).gameObject;
                slajder.GetComponent<Slider>().value = PlayerPrefs.GetFloat("zvuk", 1.0f);
            }
        }
    }

    public void PromenaJacineZvuka(float vrednost)
    {
        this.audio.volume = vrednost;
        PlayerPrefs.SetFloat("zvuk", vrednost);
    }

    public void ScenaInstrukcija() => OmogucivanjeTranzicije("UputstvoTranzicija");

    public void VratiSeNaMeni() => OmogucivanjeTranzicije("Image");

    public void AzuriranjeTeksta(int vrednost)
    {
        strana += vrednost;
        this.prev.interactable = strana != 1;
        this.next.interactable = strana != 3;
    }

    public void CuvanjePodesavanja()
    {
        Slider slajder = GameObject.Find("Slider").GetComponent<Slider>();
        float vrednostSlajdera = slajder.value;
        PlayerPrefs.SetFloat("zvuk", vrednostSlajdera);
        GameObject.Find("Main Camera").GetComponent<AudioSource>().volume = vrednostSlajdera;
    }

    void Update()
    {
        string nazivScene = SceneManager.GetActiveScene().name;
        if (nazivScene == "Meni")
        {
            Animator animator1 = this.dijalog.GetComponent<Animator>();
            Animator animator2 = this.roditelj.GetComponent<Animator>();
            float vreme1 = animator1.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float vreme2 = animator2.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (vreme1 > 0.5f && !jeZapoceto1)
            {
                animator1.enabled = false;
                jeZapoceto1 = true;
            }

            if (vreme1 > 1.0f)
            {
                jeZapoceto1 = false;
                animator1.Rebind();
                animator1.enabled = false;
            }

            if (vreme2 > 0.5f && !jeZapoceto2)
            {
                animator2.enabled = false;
                jeZapoceto2 = true;
            }

            if (vreme2 > 1.0f)
            {
                jeZapoceto2 = false;
                animator2.Rebind();
                animator2.enabled = false;
            }
        }
        else if (nazivScene == "Postolje")
        {
            Animator animator = this.dijalog.GetComponent<Animator>();
            float vreme = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (vreme > 0.5f && !jeZapoceto1)
            {
                animator.enabled = false;
                jeZapoceto1 = true;
            }

            if (vreme > 1.0f)
            {
                jeZapoceto1 = false;
                animator.Rebind();
                animator.enabled = false;
            }
        }

        if (nazivScene != "Instrukcije")
            return;

        this.tekstovi[0].enabled = strana == 1;
        this.tekstovi[1].enabled = strana == 2;
        this.tekstovi[2].enabled = strana == 3;
        this.tekstovi[3].text = "Strana " + strana;
    }
}