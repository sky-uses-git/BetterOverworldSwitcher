using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

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
        yield return 0f;
        Visible = true;
        Active = true;
    }

    public virtual IEnumerator Leave(UiRoot next)
    {
        yield return 0f;
        Visible = false;
        Active = false;
    }
}