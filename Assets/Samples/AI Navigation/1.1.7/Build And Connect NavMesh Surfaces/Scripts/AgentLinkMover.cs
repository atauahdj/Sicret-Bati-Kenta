using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Unity.AI.Navigation.Samples
{
    public enum OffMeshLinkMoveMethod
    {
        Teleport,
        NormalSpeed,
        Parabola,
        Curve
    }

    /// <summary>
    /// Move an agent when traversing a OffMeshLink given specific animated methods
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
        public AnimationCurve m_Curve = new AnimationCurve();

        [Header("Parabola Settings")]
        public float jumpHeight = 0.5f;
        public float jumpDuration = 0.5f;
        private float currentTime;
        public Animator anim;

        IEnumerator Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.autoTraverseOffMeshLink = false;

            while (true)
            {
                if (agent.isOnOffMeshLink)
                {
                    if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return StartCoroutine(NormalSpeed(agent));
                    else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(agent, jumpHeight, jumpDuration));
                    else if (m_Method == OffMeshLinkMoveMethod.Curve)
                        yield return StartCoroutine(Curve(agent, 0.5f));

                    agent.CompleteOffMeshLink();
                }

                yield return null;
            }
        }

        IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }
        private void Update()
        {
           if(anim.GetBool("Jumping"))
            {
                currentTime -= Time.deltaTime;
            }
            if (currentTime <= 0f)
            {
                anim.SetBool("Jumping", false);
                currentTime = 1f;
            }
        }
        IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            anim.SetBool("Jumping", true);
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            Vector3 moveDirection = (endPos - startPos).normalized;
            moveDirection.y = 0;
            agent.transform.rotation = Quaternion.LookRotation(moveDirection);
            

            while (normalizedTime < 1.0f)
            {
                float parabola = 4.0f * normalizedTime * (1.0f - normalizedTime);
                float yOffset = height * parabola;

                Vector3 newPos = Vector3.Lerp(startPos, endPos, normalizedTime);
                newPos.y = startPos.y + yOffset;
                agent.transform.position = newPos;

                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;

            while (normalizedTime < 1.0f)
            {
                float yOffset = m_Curve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}