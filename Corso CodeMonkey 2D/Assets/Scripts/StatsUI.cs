using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh; // Inseriamo l'oggetto riguardante i punteggi per poi modificarlo a nostro piaciamento
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private Image fuelImage;

    private void Update() {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh() { // Utilizziamo i Singleton per facilitarci la vita e ottenere ciò che ci serve
        speedRightArrowGameObject.SetActive(Lander.Istance.GetSpeedX() >= 0); // Non funziona con gli if, così però è molto più pulito, semplice e comodo
        speedLeftArrowGameObject.SetActive(Lander.Istance.GetSpeedX() < 0);
        speedUpArrowGameObject.SetActive(Lander.Istance.GetSpeedY() >= 0);
        speedDownArrowGameObject.SetActive(Lander.Istance.GetSpeedY() < 0);

        fuelImage.fillAmount = Lander.Istance.GetFuelAmountNormalized(); // Abbiamo creato questa funzione perché il fillAmount, guardando dall'inspector dell'immagine su Unity, va da 0 a 1. Con questa funzione, troviamo costantemente dei valori da 0 e 1 in formato float.
        statsTextMesh.text =
            GameManager.Istance.GetScore() + "\n" +
            MathF.Round(GameManager.Istance.GetTime()) + "\n" +  // Ho fatto in modo leggermente diverso dal video e funziona lo stesso
            Math.Abs(MathF.Round(Lander.Istance.GetSpeedX() * 10f)) + "\n" +
            Math.Abs(MathF.Round(Lander.Istance.GetSpeedY() * 10f));
    }
}
