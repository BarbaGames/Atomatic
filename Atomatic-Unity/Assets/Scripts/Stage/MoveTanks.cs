using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stage
{
    public class MoveTanks : MonoBehaviour
    {
        [SerializeField] private Animator[] animators;

        private const float MaxTime = 11f;
        private const int ActivationChance = 30;
        private float timer = MaxTime;

        private const string BoolKey = "Start";
        private static readonly int StartKey = Animator.StringToHash(BoolKey);

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = MaxTime;
                StartCoroutine(StartAnimations());
            }
        }

        private IEnumerator StartAnimations()
        {
            foreach (Animator animator in animators)
            {
                int rand = Random.Range(0, 100);
                if (rand <= ActivationChance)
                {
                    animator.SetBool(StartKey, true);
                }
            }

            yield return new WaitForSeconds(1.0f);

            foreach (Animator animator in animators)
            {
                animator.SetBool(StartKey, false);
            }
        }
    }
}