using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{

    public static int maxHealth = 100;
    [SyncVar(hook = "HealthSync")]
    public int currentHealth = maxHealth;
    public Slider playerHealthSlider;

    private void Start()
    {
        playerHealthSlider.value = maxHealth;
    }


    public void GetDamage(int damage)
    {

        /*if (!isServer)
        {
            return;
        }*/

        currentHealth -= damage;        

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }

    }

    void HealthSync(int health)
    {
        // Actualziar barra de salud del jugador
        playerHealthSlider.value = currentHealth;
    }

}
