using System.Collections;
using Characters.AI.CustomAgent.Commands;

namespace Characters.AI.CustomAgent
{
    public interface IAgent
    { 
        IAgent Clone();
        
        void Walk();
        
        void Jump();
    }
}