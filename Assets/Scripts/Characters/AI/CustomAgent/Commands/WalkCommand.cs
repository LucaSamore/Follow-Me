using UnityEngine;

namespace Characters.AI.CustomAgent.Commands
{
    public sealed class WalkCommand<T> : IAgentCommand where T : struct
    {
        public PathBuilder<T> PathBuilder { get; }
        
        public WalkCommand(PathBuilder<T> pathBuilder) => PathBuilder = pathBuilder;
        
        public void Execute()
        {
            Debug.Log("WALK");
        }
    }
}