using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiElement : Entity
{
    private BOSHostScene HostScene => BOSHostScene.Instance;
    private bool debugEnabled => HostScene.Debug;
    
    public event Action OnPress;
    
    public UiElement UpElement;
    public UiElement DownElement;
    public UiElement LeftElement;
    public UiElement RightElement;
    public bool Selected;
    private List<UiElement> Children;
    public int ZIndex = 0;
    public UiElement Parent;
    public string id;
    // Position
    public Vector2 Offset = Vector2.Zero;
    public Vector2 Size = Vector2.One*100;
    public Vector2 SizeOffset = Vector2.Zero;
    
    // if we have no parent default to screen width
    private Vector2 gameSize => new(Engine.Viewport.Width, Engine.Viewport.Height);
    private Vector2 parentSize => Parent?.RealSize ?? gameSize;
    private Vector2 parentPos => Parent?.RealPosition ?? Vector2.Zero;
    public Vector2 JustifiedPosition => Position + new Vector2(Offset.X * parentSize.X, Offset.Y * parentSize.Y);
    public Vector2 JustifiedSize => Size + new Vector2(SizeOffset.X * parentSize.X, SizeOffset.Y * parentSize.Y);
    public Vector2 RealPosition => JustifiedPosition+parentPos;
    public Vector2 RealSize => JustifiedSize;
    public override void Render()
    {
        base.Render(); // draw behind children
        if (debugEnabled) dbg_DrawInfo();
        Children.ForEach(e => { if (e.Visible) e.Render(); });
    }
    
    public override void Update()
    {
        base.Update(); // update ourselves before children
        Children.ForEach(e => { if (e.Active) e.Update(); });
    }

    public void AddChild(UiElement ch)
    {
        if (!Children.Exists(e => e==ch )) {
            ch.Parent = this;
            Children.Add(ch);
        }
    }

    public UiElement(Vector2 position) : base(position)
    {
        Children = new List<UiElement>();
        id = GetType().Name;
        AddTag(Tags.HUD);
    }
    
    public UiElement(Vector2 position, Vector2 offs) : base(position)
    {
        Children = new List<UiElement>();
        id = GetType().Name;
        AddTag(Tags.HUD);
        Offset = offs;
    }

    private void dbg_DrawInfo()
    {
        Vector2 idsize = Draw.DefaultFont.MeasureString(id);
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.Additive);
        Draw.Rect(RealPosition,idsize.X,idsize.Y,Color.Red);
        Draw.HollowRect(RealPosition,RealSize.X,RealSize.Y,Color.Red);
        Draw.Text(Draw.DefaultFont,id,RealPosition,Color.LawnGreen);
        Draw.SpriteBatch.End();
    }
    public virtual IEnumerable Select()
    {
        Selected = true;
        return null;
    }
    public virtual IEnumerable Deselect()
    {
        Selected = false;
        return null;
    }

    public void InvokePressed()
    {
        Logger.Info("BOS", OnPress?.GetType().Name);
        OnPress?.Invoke();
    }
}