using Characters.AI.CustomAgent.Commands;

namespace Characters.AI.CustomAgent
{
    public sealed class Agent : IAgent
    {
        public IAgentCommand WalkCommand { get; set; } = new WalkCommand();
        public IAgentCommand JumpCommand { get; set; } = new JumpCommand();
        
        public Agent() {}

        public Agent(Agent agent)
        {
            WalkCommand = agent.WalkCommand;
            JumpCommand = agent.JumpCommand;
        }

        public IAgent Clone() => new Agent(this);
    }
}