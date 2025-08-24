using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Istance { get; private set; }

    // Qui ci inseriamo scopi generici del gioco per organizzare meglio gli script e le linee di codice
    private int score;
    private float time;

    private void Awake() {
        Istance = this;
    }

    private void Start() { // Non posso invocare EVENTI se mi trovo all'esterno di una funzione. Solo all'interno di funzioni, come in questo caso Start(), posso invocarli.
        Lander.Istance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Istance.OnLanding += Lander_OnLanding;
    }

    private void Lander_OnLanding(object sender, Lander.OnLandingEventArgs e) {
        AddScore(e.score); // quella "e" la utilizziamo per accedere al "custom event" che abbiamo creato.
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e) {
        AddScore(500);
    }

    private void AddScore (int addScoreAmount) {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public float GetScore() {
        return score;
    }

    public float GetTime() {
        return time += Time.deltaTime;
    }
}
