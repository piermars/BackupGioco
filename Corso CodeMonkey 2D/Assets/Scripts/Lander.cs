using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake() {
        landerRigidbody2D = GetComponent<Rigidbody2D>(); // La componente RigidBody2D è ora immagazzinata nella variabile landerRigidbody2D
    }

    // Se usiamo Fixed, non abbiamo bisogno di inserire Time.deltaTime per risolvere i problemi riguardo i frame rate
    private void FixedUpdate() 
    {
        if (Keyboard.current.upArrowKey.isPressed) { // Immagazziniamo i numeri in variabili per scrivere un codice più comprensibile e chiaro. Scrivere solo i numeri senza variabili diventano "Magic Numbers", numeri che non sono subito chiari su che cosa servono.
            float force = 15f;
            landerRigidbody2D.AddForce(force * transform.up); // Utilizziamo transform(componente) perché, guardando la navicella dalla scena attraverso il move tool, quando la navicella ruota, la sua freccia y cambia in base alla direzione della navicella.
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = +5;
            landerRigidbody2D.AddTorque(turnSpeed);
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -5;
            landerRigidbody2D.AddTorque(turnSpeed);
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
    }
}
