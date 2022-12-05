using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class AgentMovement : MonoBehaviour
    {
        private static readonly float Speed = 5f;
    
        public void RunCoroutine(Transform agentTransform, IList<Vector3> path)
        {
            foreach (var position in path)
            {
                StartCoroutine(MoveOverSeconds(agentTransform, position, 2f));
            }
        }

        private IEnumerator MoveAICoroutine(Transform agentTransform, IList<Vector3> path) {
            foreach(var item in path) {
                while (Vector3.Distance(agentTransform.position, item) > .0001) {
                    agentTransform.position = Vector3.MoveTowards(agentTransform.position, item, Speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        
        private IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
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
