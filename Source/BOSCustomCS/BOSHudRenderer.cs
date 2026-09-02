using System.Collections;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSHudRenderer : Renderer
{
    public static BOSHudRenderer Instance;
    private BOSDebug.DbgHudRenderer dbghud;
    private BOSHostScene HostScene => BOSHostScene.Instance;

    public bool Transitioning { get; private set; }

    private BOSUi.UiRoot PreviousUi;
    private BOSUi.UiRoot CurrentUi;
    private BOSUi.UiRoot NextUi;

    private Entity routineEntity;

    public BOSHudRenderer(Overworld.StartMode startMode)
    {
        Instance = this;
        HostScene.Add(routineEntity = new());
        HostScene.Add(dbghud = new BOSDebug.DbgHudRenderer());
        HostScene.Entities.UpdateLists();
        if (startMode == Overworld.StartMode.MainMenu)
            Goto("root");
        else if (startMode == Overworld.StartMode.AreaComplete)
            Goto("chapselect");
        else if (startMode == Overworld.StartMode.AreaQuit)
            Goto("chapselect");
        else
            Goto("root");
    }

    public override void Update(Scene scene)
    {
        if (CurrentUi == null || Transitioning)
        {
            CurrentUi?.Update();
            base.Update(scene);
            return;
        }
        if (Input.MenuUp.Pressed) CurrentUi.SelectUp();
        if (Input.MenuLeft.Pressed) CurrentUi.SelectLeft();
        if (Input.MenuDown.Pressed) CurrentUi.SelectDown();
        if (Input.MenuRight.Pressed) CurrentUi.SelectRight();
        if (Input.MenuConfirm.Pressed) CurrentUi.Press();
        if (Input.MenuCancel.Pressed) CurrentUi.Cancel();
        CurrentUi.Update();
        base.Update(scene);
    }

    public override void BeforeRender(Scene scene)
    {
        CurrentUi?.BeforeRender();
        base.BeforeRender(scene);
    }

    public override void Render(Scene scene)
    {
        CurrentUi?.Render();
        base.Render(scene);
    }

    private IEnumerator GotoRoutine(BOSUi.UiRoot newroot)
    {
        Transitioning = true;
        Logger.Info("BOS","goto called "+newroot.id);
        NextUi = newroot;
        if (CurrentUi!=null){
            yield return CurrentUi.Leave(NextUi);
            PreviousUi = CurrentUi;
            CurrentUi = null;
        }
        yield return NextUi.Enter(PreviousUi);
        CurrentUi = NextUi;
        NextUi = null;
        Transitioning = false;
    }
    public void Goto(string id)
    {
        BOSUi.UiRoot newroot = BOSUi.UiLoader.Load(id);
        routineEntity.Add(new Coroutine(GotoRoutine(newroot)));
    }

    public void End()
    {
        CurrentUi = null;
        Instance = null;
    }
}