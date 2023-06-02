using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;
    public void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, maxDistance))
        {
            IHittable hittable = hit.transform.GetComponent<IHittable>();
            ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            effect.transform.parent = hit.transform;
            Destroy(effect.gameObject, 3f);

            //TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            StartCoroutine(TrailRoutine( muzzleEffect.transform.position, hit.point));
            //Destroy(trail.gameObject, 3f);
            hittable?.Hit(hit,damage);
        }
        else
        {
            //TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            StartCoroutine(TrailRoutine( muzzleEffect.transform.position, Camera.main.transform.forward*maxDistance));
            //Destroy(trail.gameObject, 3f);
        }
    }

    IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
    {
        TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

        float rate = 0;
        while (rate < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
            rate += Time.deltaTime / totalTime;

            yield return null;
        }
        Destroy(trail);
    }
    

}
