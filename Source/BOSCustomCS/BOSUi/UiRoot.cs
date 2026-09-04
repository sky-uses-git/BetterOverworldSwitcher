using System.Collections;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// root / group object meant to contain other uielements and handle transitions between ui screens
public class UiRoot : UiElement
{
    public UiElement Selected;
    public UiElement SelectFirst;
    public Entity routEnt;
    public UiRoot(string rootid) : base(Vector2.Zero,Vector2.Zero)
    {
        routEnt = new();
        id = rootid;
        Size = ScaleOffset.FromScale(1, 1);
        Visible = false;
        Active = false;
    }

    public UiRoot() : this("unnamed")
    {
    }

    public override void Added(Scene scene)
    {
        scene.Add(routEnt);
        base.Added(scene);
    }

    public override void Removed(Scene scene)
    {
        routEnt.RemoveSelf();
        base.Removed(scene);
    }

    public void SelectUp()
    {
        if (Selected.UpElement != null)
            SelectElem(Selected.UpElement);
    }
    public void SelectLeft()
    {
        if (Selected.LeftElement != null)
            SelectElem(Selected.LeftElement);
    }
    public void SelectDown()
    {
        if (Selected.DownElement != null)
            SelectElem(Selected.DownElement);
    }
    public void SelectRight()
    {
        if (Selected.RightElement != null)
            SelectElem(Selected.RightElement);
    }
    
    public void SelectElem(UiElement elem)
    {
        UiElement lastElem = Selected;
        if (lastElem != null) {
            lastElem.Selected = false;
            routEnt.Add(new Coroutine(lastElem.Deselect(elem)));
        }
        if (elem != null) {
            Selected = elem;
            Selected.Selected = true;
            routEnt.Add(new Coroutine(Selected.Select(lastElem)));
        }
    }

    public void DeselectElem()
    {
        if (Selected != null)
            routEnt.Add(new Coroutine(Selected.Deselect(null)));
        Selected = null;
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
        PositionTween.Duration = .4f;
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
        routEnt.RemoveSelf();
    }
}