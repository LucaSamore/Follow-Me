using System.Linq;
using UnityEngine;

namespace Characters.AI.CustomAgent.Commands
{
    public sealed class WalkCommand<T> : IAgentCommand where T : struct
    {
        private static readonly float AgentSpeed = 2f;
        
        private readonly CharacterController _characterController;
        private readonly T _startingPosition;
        public PathBuilder<T> PathBuilder { get; }

        public WalkCommand(CharacterController characterController, PathBuilder<T> pathBuilder, T startingPosition)
        {
            _characterController = characterController;
            PathBuilder = pathBuilder;
            _startingPosition = startingPosition;
        }
        
        public void Execute()
        {
            var path = PathBuilder.BuildPath(_startingPosition, 3).Select(t => t.Item1).ToList();

            for (var i = 0; i < path.Count; i++)
            {
                Debug.Log(path[i]);
                var offset = path[i] - _characterController.transform.position;

                if (offset.magnitude > .1f)
                {
                    offset = offset.normalized * AgentSpeed;
                    _characterController.Move(offset * Time.deltaTime);
                }
            }
            
        }
    }
}