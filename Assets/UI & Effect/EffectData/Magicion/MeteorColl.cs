using UnityEngine;
using System.Collections;

public class MeteorColl : MonoBehaviour {

	void OnParticleCollision(GameObject coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Map") || coll.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
			//Vector3 pos = coll.gameObject.GetComponent<Collider>().c

			//GameObject explosion = Instantiate(Resources.Load<GameObject>("Effect/Explosion"), , Quaternion.identity) as GameObject;
        }
    }
}
