using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // if (other.TryGetComponent<Ball)
        Debug.Log("Goal");
    }
}
