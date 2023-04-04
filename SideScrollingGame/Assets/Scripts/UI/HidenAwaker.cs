using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidenAwaker : MonoBehaviour
{
    [SerializeField] float waitingTime = 0.1f;
    [SerializeField] Behaviour[] needToBeEnabledComponents;

    private void Awake() {
        Invoke(nameof(Deactivate), waitingTime);
    }

    private void Deactivate() {
        foreach (Behaviour component in needToBeEnabledComponents) {
            component.enabled = true;
        }
        gameObject.SetActive(false);
    }
}
