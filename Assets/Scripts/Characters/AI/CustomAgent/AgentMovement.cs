using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    /// <summary>
    /// Moves a custom agent along a generated path.
    /// </summary>
    public sealed class AgentMovement : MonoBehaviour
    {
        private static readonly float Seconds = .5f;

        /// <summary>
        /// Starts a coroutine that moves the agent from source to destination, following a given path.
        /// For each step, a new coroutine is created.
        /// This coroutine moves the agent between two points in half a second.
        /// </summary>
        /// <param name="agentTransform">The agent <c>Transform</c> object.</param>
        /// <param name="path">The path to follow.</param>
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
