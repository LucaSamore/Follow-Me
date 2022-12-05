using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class AgentMovement : MonoBehaviour
    {
        private static readonly float Seconds = .5f;
        
        public void RunCoroutine(Transform agentTransform, IList<Vector3> path) =>
            StartCoroutine(SequenceStart(agentTransform, path));

        private IEnumerator SequenceStart(Transform agentTransform, IList<Vector3> path)
        {
            for (var i = 1; i < path.Count; i++)
            {
                yield return StartCoroutine(MoveOverSeconds(agentTransform, path[i], Seconds));
            }
        }

        private static IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
        {
            float elapsedTime = 0;
            var startingPos = objectToMove.position;
            while (elapsedTime < seconds)
            {
                objectToMove.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.position = end;
        }
    }
}
