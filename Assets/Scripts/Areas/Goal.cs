using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Victory());
        }
    }

    IEnumerator Victory()
    {
        Transition.instance.PlayEntireTransition();

        yield return new WaitForSeconds(1);

        Transition.instance.PlayTransition();

        GameUI.instance.ActivateVictoryScreen();
    }
}
