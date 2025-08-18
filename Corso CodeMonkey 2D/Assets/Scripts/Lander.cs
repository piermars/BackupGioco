using UnityEngine;
using UnityEngine.InputSystem;

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
}
