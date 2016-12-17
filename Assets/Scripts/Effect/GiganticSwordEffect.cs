using UnityEngine;
using System.Collections;

public class GiganticSwordEffect : MonoBehaviour
{

    ParticleSystem myParticle;

    void Start()
    {
        myParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        StartCoroutine(EffectMove());
    }

    IEnumerator EffectMove()
    {
        while (myParticle.isPlaying)
        {
            myParticle.gameObject.transform.Translate(0, 0, 4 * Time.deltaTime, Space.Self);

            yield return null;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Instantiate(Resources.Load<GameObject>("Effect/Explosion"), transform.position, Quaternion.identity);
        }
    }

}