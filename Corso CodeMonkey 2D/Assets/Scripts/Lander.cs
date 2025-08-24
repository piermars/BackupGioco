using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
public class Lander : MonoBehaviour
{

    public static Lander Istance { get; private set; }

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnFuelEmpty;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandingEventArgs> OnLanding; // Lo utilizziamo per portaci come argomento extra lo score del landing. In questo modo, possiamo sommare lo score delle monete raccolte con lo score del landing.
    public class OnLandingEventArgs : EventArgs {
        public int score;
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;

    private void Awake() {
        Istance = this; // L'istanza viene chiamata durante il caricamento dell'istanza dello script
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>(); // La componente RigidBody2D è ora immagazzinata nella variabile landerRigidbody2D
    }

    // Se usiamo Fixed, non abbiamo bisogno di inserire Time.deltaTime per risolvere i problemi riguardo i frame rate
    private void FixedUpdate() 
    {
        if (fuelAmount <= 0f) { // Se non abbiamo fuel, tutto ciò che sta sotto non funzionerà perché non c'è carburante da consumare. Infatti questo if ci porta che se il fuel è 0 o sotto, non succede niente.
            // No fuel
            OnFuelEmpty?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (Keyboard.current.upArrowKey.isPressed || 
            Keyboard.current.leftArrowKey.isPressed || 
            Keyboard.current.rightArrowKey.isPressed) {
            // Consumo carburante
            FuelConsumption();
        }

        OnBeforeForce?.Invoke(this, EventArgs.Empty); // evento che si attiva ad ogni singolo frame (a meno che un altro evento sia già attivo, ad esempio OnUpForce)
        if (Keyboard.current.upArrowKey.isPressed) { // Immagazziniamo i numeri in variabili per scrivere un codice più comprensibile e chiaro. Scrivere solo i numeri senza variabili diventano "Magic Numbers", numeri che non sono subito chiari su che cosa servono.
            float force = 15f;
            landerRigidbody2D.AddForce(force * transform.up); // Utilizziamo transform(componente) perché, guardando la navicella dalla scena attraverso il move tool, quando la navicella ruota, la sua freccia y cambia in base alla direzione della navicella.
            OnUpForce?.Invoke(this, EventArgs.Empty); // Studiare meglio gli eventi. "this" si riferisce all'oggetto in questa condizione, che in questo caso è un evento che si attiva quando schiacciamo la freccetta in alto; stesso caso per sinistra e destra.
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = +5;
            landerRigidbody2D.AddTorque(turnSpeed);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -5;
            landerRigidbody2D.AddTorque(turnSpeed);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) { // TryGetComponent non sarà vera a meno che il gameObject ha come componente il landingPad
            Debug.Log("Sei finito sul terreno!");
            return;
        }

        float limiteAtterraggioMorbido = 4f;
        float velocitàRelativaMagnitude = collision2D.relativeVelocity.magnitude;
        if (velocitàRelativaMagnitude > limiteAtterraggioMorbido) {
            Debug.Log("Atterraggio troppo duro"); // Atterraggio troppo rapido
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minAngoloDot = .90f;
        if (dotVector < minAngoloDot) {
            Debug.Log("Atterraggio troppo ripido!"); // Atterraggio con angolo troppo ripido
            return;
        }

        Debug.Log("Atterraggio eseguito con successo!");

        float punteggioMassimoAngoloAtterraggio = 100f;
        float punteggioDotVettoreMoltiplicatore = 10f; // dptVector prende valori del tipo 0.9985849855, ecco perché il punteggio va bene
        float punteggioAngoloAtterraggio = punteggioMassimoAngoloAtterraggio - Mathf.Abs(dotVector - 1) * punteggioDotVettoreMoltiplicatore * punteggioMassimoAngoloAtterraggio;

        float punteggioMassimoVelocitàAtterraggio = 100f; // guardare la condizione dell'atterraggio troppo duro
        float punteggioVelocitàAtterraggio = (limiteAtterraggioMorbido - velocitàRelativaMagnitude) * punteggioMassimoVelocitàAtterraggio;

        Debug.Log("Punteggio angolo: " + punteggioAngoloAtterraggio);
        Debug.Log("Punteggio velocità " + punteggioVelocitàAtterraggio);

        int score = Mathf.RoundToInt(punteggioAngoloAtterraggio + punteggioVelocitàAtterraggio) * landingPad.ScoreMultiplier();

        Debug.Log("Punteggio: " + score);
        OnLanding?.Invoke(this, new OnLandingEventArgs { // Guardare la parte in cui creiamo l'evento, in alto
            score = score,
        });
    }

    private void FuelConsumption() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime; // Non so perché, quando non dichiaro il tipo della variabile mi fa scrivere -=, se lo dichiaro devo per forza scrivere =-
    }

    // Cosa succede quando prendo un Fuel
    private void OnTriggerEnter2D(Collider2D collider2d) {
        if (collider2d.gameObject.TryGetComponent(out FuelPickup fuelPickup)) {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax) {
                fuelAmount = fuelAmountMax; // in questo modo, il fuel massimo possibile sarà sempre 10 e non andrà mai oltre
            }
            fuelPickup.DestroyFuel();
        }
        if (collider2d.gameObject.TryGetComponent(out CoinPickup coinPickup)) {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroyCoin();
        }
    }

    public float GetSpeedX() {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY() {
        return landerRigidbody2D.linearVelocityY;
    }
    public float GetFuelAmount() {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized() {
        return fuelAmount / fuelAmountMax;
    }

}
