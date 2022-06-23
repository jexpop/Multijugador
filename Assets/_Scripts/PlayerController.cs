using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    // Variables en relación a la cámara del jugador
    private Transform mainCamera; // Referencia a la cámara
    private float cameraDistance = 15f; // Distancia del jugador a la cámara
    private float cameraHeight = 15f; // Altura de la cámara por encima del jugador
    private Vector3 cameraOffset; // Delay entre la cámara y el jugador 


    [SyncVar]private Color randomColor;

    private float speedH = 150.0f;
    private float speedV = 3.0f;

    public GameObject turrentObject, bulletPrefab;

    public Transform bulletSpawn;


    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            turrentObject.GetComponent<MeshRenderer>().material.color = randomColor;
        }

        cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        mainCamera = Camera.main.transform;

        MoveCamera();

    }

    [Command]
    void Cmd_ProvideColorToServer(Color c)
    {

        randomColor = c;
    }

    [ClientCallback]
    void TransmitColor()
    {
        if (isLocalPlayer)
        {
            Cmd_ProvideColorToServer(randomColor);
        }
    }

    public override void OnStartClient()
    {
        StartCoroutine(UpdateColor(1.5f));

    }

    IEnumerator UpdateColor(float time)
    {

        float timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            TransmitColor();
            if (!isLocalPlayer)
                turrentObject.GetComponent<MeshRenderer>().material.color = randomColor;


            yield return null;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speedH;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speedV;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        MoveCamera();

    }

    /// <summary>
    /// Función que dispara la bala
    /// </summary>Command
    [Command]
    void CmdFire()
    {
        // Instanciar bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Mover bala
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

        // Destruir bala
        Destroy(bullet, 1.0f);

        // Mostrar balas en red
        NetworkServer.Spawn(bullet);

    }

    void MoveCamera()
    {
        //Ponemos la cámara en la posición y la rotación del tanque
        mainCamera.position = transform.position;
        mainCamera.rotation = transform.rotation;
        //La trasladamos con el vector offset para atrás
        mainCamera.Translate(cameraOffset);
        //Hacemos que mire al tanque
        mainCamera.LookAt(transform);
    }

}
