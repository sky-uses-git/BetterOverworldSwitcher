using System;
using System.Collections;
using AsmResolver.PE.Debug.Builder;
using Celeste.Mod.Core;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSHudRenderer : Renderer
{
    public static BOSHudRenderer Instance;
    private BOSDebug.DbgHudRenderer dbghud;
    private BOSHostScene HostScene => BOSHostScene.Instance;
    private BOSUi.UiRoot uiroot;
    
    private string PreviousUi = "root";
    private string CurrentUi = "root";
    private string NextUi = "root";

    public BOSHudRenderer(Overworld.StartMode startMode)
    {
        Instance = this;
        HostScene.Add(dbghud = new BOSDebug.DbgHudRenderer());
        if (startMode == Overworld.StartMode.MainMenu)
            uiroot = BOSUi.UiLoader.Load("root");
        else if (startMode == Overworld.StartMode.AreaComplete)
            uiroot = BOSUi.UiLoader.Load("chapselect");
        else if (startMode == Overworld.StartMode.AreaQuit)
            uiroot = BOSUi.UiLoader.Load("chapselect");
        else
            uiroot = BOSUi.UiLoader.Load("root");
        uiroot.Enter();
    }

    public override void Update(Scene scene)
    {
        if (uiroot == null)
        {
            base.Update(scene);
            return;
        }
        if (Input.MenuUp.Pressed) uiroot.SelectUp();
        if (Input.MenuLeft.Pressed) uiroot.SelectLeft();
        if (Input.MenuDown.Pressed) uiroot.SelectDown();
        if (Input.MenuRight.Pressed) uiroot.SelectRight();
        if (Input.MenuConfirm.Pressed) uiroot.Press();
        if (Input.MenuCancel.Pressed) uiroot.Cancel();
        uiroot.Update();
        base.Update(scene);
    }

    public override void Render(Scene scene)
    {
        uiroot?.Render();
        base.Render(scene);
    }
/*
    public IEnumerable Goto(string id)
    {
        Logger.Info("BOS","goto called "+id);
        NextUi = id;
        yield return 0f;
        BOSUi.UiRoot newroot = BOSUi.UiLoader.Load(id);
        root.Leave();
        yield return 0f;
        root = newroot;
        PreviousUi = CurrentUi;
        CurrentUi = NextUi;
        NextUi = null;
        newroot.Enter();
    }*/
    public void Goto(string id)
    {
        Logger.Info("BOS","goto "+id+" called");
        NextUi = id;
        BOSUi.UiRoot newroot = BOSUi.UiLoader.Load(id);
        uiroot.Leave();
        PreviousUi = CurrentUi;
        CurrentUi = NextUi;
        NextUi = null;
        uiroot = newroot;
        uiroot.Enter();
    }

    public void End()
    {
        uiroot.Leave();
        uiroot = null;
        Instance = null;
    }
}