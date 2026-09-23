using UnityEngine;

/// Something the on-foot player can look at and use with the Interact key.
/// Put it on the object that owns the colliders, or on a parent of them.
public abstract class Interactable : MonoBehaviour
{
    /// The action shown after the key hint while the player looks at this, e.g. "Check hive".
    public abstract string Prompt { get; }

    /// False while this can't be used right now; the prompt hides and Interact is not sent.
    public virtual bool CanInteract => true;

    public abstract void Interact(PlayerInteractor player);

    /// Optional second action on the Secondary key (F), e.g. "Harvest honey"; null when there is none right now.
    public virtual string SecondaryPrompt => null;

    public virtual void SecondaryInteract(PlayerInteractor player) { }
}
