using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public GameObject bazuca;
    public Transform bulletSpawn;

    Rigidbody rbody;
    PhotonView photonView;

    float inputRotation;
    float inputSpeed;

    //--------------------------------------------------------
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        bazuca.SetActive(false);
    }

    //--------------------------------------------------------
    public void Died()
    {
        bazuca.SetActive(false);
        rbody.MovePosition(Vector3.zero);
    }

    //--------------------------------------------------------
    void Update()
    {
        if (photonView.IsMine)
        {
            inputRotation = Input.GetAxis("Horizontal");
            inputSpeed = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.C))
            {
                photonView.RPC("ChangeColor", RpcTarget.All, Random.Range(0.0f, 1.0f));
            }

            if (Input.GetKeyDown(KeyCode.Space) && bazuca.activeSelf)
            {
                bazuca.SetActive(false);
                photonView.RPC("Fire", RpcTarget.All);
            }
        }
    }

    //--------------------------------------------------------
    [PunRPC]
    public void Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject prefabBullet = Resources.Load("Bullet") as GameObject;
        GameObject bulletObject = Instantiate(prefabBullet, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bulletObject.GetComponent<BulletController>().Shoot(lag);
    }

    //--------------------------------------------------------
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Quaternion rot = rbody.rotation * Quaternion.Euler(0, inputRotation * Time.deltaTime * 60, 0);
            rbody.MoveRotation(rot);

            Vector3 force = rot * Vector3.forward * inputSpeed * 1000 * Time.deltaTime;
            rbody.AddForce(force);

            if (rbody.velocity.magnitude > 2)
            {
                rbody.velocity = rbody.velocity.normalized * 2;
            }
        }
    }

    //--------------------------------------------------------
    [PunRPC]
    public void ChangeColor(float hue, PhotonMessageInfo info)
    {
        Color newColor = Color.HSVToRGB(hue, 1, 1);
        GetComponent<MeshRenderer>().material.color = newColor;
    }

    //--------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (bazuca.activeSelf == false && other.name.Contains("BazucaObject"))
        {
            bazuca.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}











