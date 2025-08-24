using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;

    private Lander lander;

    private void Awake() {
       lander = GetComponent<Lander>();

        lander.OnUpForce += Lander_OnUpForce; // questo serve per attaccare un "listener"(ascoltatore), quindi quando quell'evento viene attivato, viene invocata la funzione riguardo quell'evento
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce; // Questo evento serve per far si che quando non stiamo schiacciando nessun tasto, la navicella non spara nessuna particella. Senza di questo, se schiacciassimo il tasto Su rimarebbe fisso quell'evento in cui tutte le particelle sono attive e non si spegnerebbero mai.
        lander.OnFuelEmpty += Lander_OnFuelEmpty;

        // Ovviamente vogliamo far si che all'inizio le particelle non siano attive, bensì che si attivino solo se certi eventi sono soddisfatti o meno.
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Lander_OnFuelEmpty(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e) { // In right e left non poniamo tutte le condizioni per far si che, quando schiacciamo contemporaneamente sx e su o dx e su, la navicella spara tutte le particelle e non solo quella sinistra o quella destra. Così ha una visual più carina., 
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true); // Poichè non abbiamo inserito tutte le condizioni, non entrano in conflitto false e true.
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled) { // Questa funzione serve per stabilire se quelle particelle, in base ad un certo evento, vogliamo che siano attive o meno.
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
