using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private Animator animator;
    public static Transition instance {get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    public void PlayTransition()
    {
        animator.SetTrigger("Play");
    }

    public void PlayEntireTransition()
    {
        StartCoroutine(PlayEntireTransitionIE());
    }

    IEnumerator PlayEntireTransitionIE()
    {
        animator.SetTrigger("Play");

        yield return new WaitForSeconds(1);

        animator.SetTrigger("End");
    }
}
