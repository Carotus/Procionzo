using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerSeenEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerSeenEvent", message: "[Agent] has [spotted] [Player]", category: "Events", id: "aeb5f54737a6f6eb0fb38ebaac5e7b6b")]
public partial class PlayerSeenEvent : EventChannelBase
{
    public delegate void PlayerSeenEventEventHandler(GameObject Agent, bool spotted, GameObject Player);
    public event PlayerSeenEventEventHandler Event; 

    public void SendEventMessage(GameObject Agent, bool spotted, GameObject Player)
    {
        Event?.Invoke(Agent, spotted, Player);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<bool> spottedBlackboardVariable = messageData[1] as BlackboardVariable<bool>;
        var spotted = spottedBlackboardVariable != null ? spottedBlackboardVariable.Value : default(bool);

        BlackboardVariable<GameObject> PlayerBlackboardVariable = messageData[2] as BlackboardVariable<GameObject>;
        var Player = PlayerBlackboardVariable != null ? PlayerBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Agent, spotted, Player);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        PlayerSeenEventEventHandler del = (Agent, spotted, Player) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            BlackboardVariable<bool> var1 = vars[1] as BlackboardVariable<bool>;
            if(var1 != null)
                var1.Value = spotted;

            BlackboardVariable<GameObject> var2 = vars[2] as BlackboardVariable<GameObject>;
            if(var2 != null)
                var2.Value = Player;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as PlayerSeenEventEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as PlayerSeenEventEventHandler;
    }
}

