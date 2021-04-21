using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private RaycastHit _hit;
    private int _layer = 1 << 9;
    private bool canShoot = true;
    [SerializeField] private GameObject _bloodsplat;
   
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if(canShoot)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 centre = new Vector3(0.5f, 0.5f, 0);
                Ray _ray = Camera.main.ViewportPointToRay(centre);
                RaycastHit _hit;
                if (Physics.Raycast(_ray, out _hit))
                {
                    Debug.Log("Hit" + _hit.transform.name);
                    IDamagable IHit = _hit.collider.GetComponent<IDamagable>();
                    if (IHit != null)
                    {
                        IHit.Damage(1);
                        GameObject blood =  Instantiate(_bloodsplat, _hit.point, Quaternion.LookRotation(_hit.normal));
                        blood.transform.parent = _hit.transform;
                        Destroy(blood, 0.5f);
                    }
                    canShoot = false;
                    StartCoroutine("ShootCooldown");
                }
            } 

        }
    }

    IEnumerator ShootCooldown()
    {
        while (!canShoot)
        {
            yield return new WaitForSeconds(0.2f);
            canShoot = true;
        }
    }
}
