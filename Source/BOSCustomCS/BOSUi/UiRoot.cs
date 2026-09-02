using System.Collections;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// root / group object meant to contain other uielements and handle transitions between ui screens
public class UiRoot : UiElement
{
    public UiElement Selected;
    public UiRoot(string rootid) : base(Vector2.Zero,Vector2.Zero)
    {
        id = rootid;
        Size = ScaleOffset.FromScale(1, 1);
        Visible = false;
        Active = false;
    }

    public UiRoot() : this("unnamed")
    {
    }

    public void SelectUp()
    {
        if (Selected.UpElement != null) {
            Selected.Deselect();
            Selected = Selected.UpElement;
            Selected.Select();
        }
    }
    public void SelectLeft()
    {
        if (Selected.LeftElement != null) {
            Selected.Deselect();
            Selected = Selected.LeftElement;
            Selected.Select();
        }
    }
    public void SelectDown()
    {
        if (Selected.DownElement != null) {
            Selected.Deselect();
            Selected = Selected.DownElement;
            Selected.Select();
        }
    }
    public void SelectRight()
    {
        if (Selected.RightElement != null) {
            Selected.Deselect();
            Selected = Selected.RightElement;
            Selected.Select();
        }
    }
    
    public void Select(UiElement elem)
    {
        if (Selected != null)
            Selected.Deselect();
        Selected = elem;
        Selected.Select();
    }
    
    public void Press()
    {
        if (Selected != null) {
            Logger.Info("BOS","press called");
            Selected.InvokePressed();
        }
    }
    public virtual IEnumerable Cancel()
    {
        return null;
    }

    public virtual IEnumerator Enter(UiRoot last)
    {
        Position = new ScaleOffset(0, 0, 0, 1);
        PositionTween.Duration = .2f;
        Position = new ScaleOffset(0, 0, 0, 0);
        yield return .2f;
        Visible = true;
        Active = true;
    }

    public virtual IEnumerator Leave(UiRoot next)
    {
        Position = new ScaleOffset(0, 0, 0, 1);
        yield return .2f;
        Visible = false;
        Active = false;
    }
}