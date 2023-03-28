using UnityEngine;
using UnityEngine.UI;

public class OkretanjeKarata : MonoBehaviour
{
    [SerializeField] Sprite pozadiKarte;

    public void Okreni()
    {
        Sprite liceKarte = GetComponent<KartaIgraca>().karta.Sprajt;
        Image slika = GetComponentInChildren<Button>().image;
        if (slika.sprite == liceKarte)
            slika.sprite = this.pozadiKarte;
        else
            slika.sprite = liceKarte;
    }
}