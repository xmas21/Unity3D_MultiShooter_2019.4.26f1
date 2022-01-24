using UnityEngine;

public class scr_SingleShotGun : scr_Gun
{
    [SerializeField]
    private Camera cam;

    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<scr_IDamagable>()?.TakeDamage(((scr_GunInfo)itemInfo).damage);
        }
    }
}
