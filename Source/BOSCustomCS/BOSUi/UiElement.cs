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
    public ScaleOffset Position = ScaleOffset.Zero;
    public ScaleOffset Size = ScaleOffset.FromOffset(100,100);
    public ScaleOffset TweenFrom = ScaleOffset.Zero;
    public ScaleOffset TweenTo = ScaleOffset.Zero;
    private Tween tween;
    
    // if we have no parent default to screen width
    private Vector2 gameSize => new(Engine.ViewWidth, Engine.ViewHeight);
    private Vector2 properSize => new(1920, 1080);
    public Vector2 ScaleFactor => gameSize/properSize;
    private Vector2 parentSize => Parent?.RealSize ?? gameSize;
    private Vector2 parentPos => Parent?.RealPosition ?? Vector2.Zero;
    public Vector2 JustifiedPosition => Position.Offset * ScaleFactor + Position.Scale * parentSize;
    public Vector2 JustifiedSize => Size.Offset * ScaleFactor + Size.Scale * parentSize;
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

    public UiElement() : base()
    {
        Children = new List<UiElement>();
        id = GetType().Name;
        TweenTo = Position;
        TweenFrom = Position;
        AddTag(Tags.HUD);
    }
    
    public UiElement(Vector2 offset, Vector2 scale) : this()
    {
        Position = new ScaleOffset(offset, scale);
        TweenTo = Position;
        TweenFrom = Position;
    }

    private void dbg_DrawInfo()
    {
        string typename = GetType().Name;
        float idfontsize = 20f / ActiveFont.FontSize.Size;
        float typefontsize = 12f / ActiveFont.FontSize.Size;
        Vector2 idsize = ActiveFont.Measure(id) * idfontsize;
        Vector2 typesize = ActiveFont.Measure(typename) * typefontsize;
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        Draw.Rect(RealPosition,Math.Max(idsize.X,typesize.X),idsize.Y+typesize.Y,Color.Blue);
        Draw.HollowRect(RealPosition,RealSize.X,RealSize.Y,Color.Red);
        ActiveFont.Draw(id,RealPosition,Vector2.Zero, Vector2.One * idfontsize, Color.White);
        ActiveFont.Draw(typename,RealPosition+new Vector2(0,idsize.Y),Vector2.Zero, Vector2.One * typefontsize, Color.White);
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

    public void InvokePressed() => OnPress?.Invoke();
}