using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentPoint : MonoBehaviour
{

    [SerializeField] private float radius = .32f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radius);
    }


}
