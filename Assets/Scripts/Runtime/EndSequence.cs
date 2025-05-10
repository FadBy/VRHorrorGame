using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EndSequence : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger("Begin");
            AudioListener.volume = 0f;
        }
    }
}