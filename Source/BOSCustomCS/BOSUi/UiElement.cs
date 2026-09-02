using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmResolver.DotNet.Builder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiElement : Actor, IDisposable
{
    private BOSHostScene HostScene => BOSHostScene.Instance;
    private bool debugEnabled => HostScene.Debug;
    
    public event Action OnPress;

    private VirtualRenderTarget elemBuffer;
    
    public UiElement UpElement;
    public UiElement DownElement;
    public UiElement LeftElement;
    public UiElement RightElement;
    public bool Selected;
    private List<UiElement> Children;
    private int _zindex = 0;
    public int ZIndex
    {
        get => _zindex;
        set {
            _zindex = value;
            Parent?.ReorderChildren();
        }
    }
    public UiElement Parent;
    public string id;
    public string uuid { get; private set; }
    public string longid => id+","+GetType().Name+","+uuid;

    // Position
    public ScaleOffset Position = ScaleOffset.Zero;
    public ScaleOffset Size = ScaleOffset.FromOffset(100,100);
    public ScaleOffset TweenFrom = ScaleOffset.Zero;
    public ScaleOffset TweenTo = ScaleOffset.Zero;
    private UiEnum.RenderMode _renderMode = UiEnum.RenderMode.All; //TODO: improve clip mode

    public UiEnum.RenderMode RenderMode
    {
        get => _renderMode;
        set
        {
            if (value != _renderMode) {
                if (value != UiEnum.RenderMode.All) ResetBuffersIfNeeded();
                else DisposeBuffers();
            }
            _renderMode = value;
        }
    }

    public UiEnum.SortMode SortMode = UiEnum.SortMode.UseZindex;

    private Tween tween;
    
    // if we have no parent default to screen width
    private Vector2 gameSize => new(Engine.ViewWidth, Engine.ViewHeight);
    private Vector2 properSize => new(1920, 1080);
    public float ScaleFactor => Engine.ViewWidth/1920f;
    private Vector2 parentPos => Parent?.RealPosition ?? Vector2.Zero;
    private Vector2 parentRenderPos => Parent?.RenderPosition ?? Vector2.Zero;
    private Vector2 parentSize => Parent?.RealSize ?? gameSize;
    public Vector2 JustifiedPosition => Position.Offset * ScaleFactor + Position.Scale * parentSize;
    public Vector2 JustifiedSize => Size.Offset * ScaleFactor + Size.Scale * parentSize;

    public Vector2 RealPosition => JustifiedPosition + parentPos;
    public Vector2 RenderPosition => (RenderMode != UiEnum.RenderMode.All) ? Vector2.Zero : JustifiedPosition+parentRenderPos;
    
    public Vector2 RealSize => JustifiedSize;
    public Vector2 RenderSize => RealSize;

    public void BeforeRender()
    {
        if (RenderMode==UiEnum.RenderMode.ClipOverflow) ClipBeforeRender();
        Children.ForEach(e => { if (e.Visible) e.BeforeRender(); });
    }

    public override void Render()
    {
        if (RenderMode == UiEnum.RenderMode.All) renderAll();
        else renderElemBuffer();
        if (debugEnabled) dbg_renderAll();
    }

    private List<UiElement> doSort(List<UiElement> children)
    {
        switch (SortMode)
        {
            case UiEnum.SortMode.LastOnTop:
            {
                children.Sort((x,y) => 1);
                break;
            }
            case UiEnum.SortMode.FirstOnTop:
            {
                children.Sort((x,y) => -1);
                break;
            }
            case UiEnum.SortMode.UseZindex:
            {
                children.Sort((x,y) => x.ZIndex-y.ZIndex);
                break;
            }
        }
        return children;
    }

    public virtual void RenderElement()
    {
    }

    private void renderAll()
    {
        RenderElement();
        Children.ForEach(e => { if (e.Visible) e.Render(); });
    }
    public void dbg_renderAll()
    {
        Children.ForEach(e => { if (e.Visible) e.dbg_renderAll(); });
        dbg_DrawInfo();
    }
    
    private void renderElemBuffer()
    {
        switch (RenderMode) {
            case UiEnum.RenderMode.ClipOverflow:
            {
                Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend);
                Draw.SpriteBatch.Draw((RenderTarget2D)elemBuffer,JustifiedPosition,Color.White);
                Draw.SpriteBatch.End();
                break;
            }
        }
    }

    public void ClipBeforeRender()
    {
        RenderTargetBinding[] oldtargets = Engine.Graphics.GraphicsDevice.GetRenderTargets();
        ResetBuffersIfNeeded();
        Engine.Graphics.GraphicsDevice.SetRenderTargets((RenderTarget2D)elemBuffer);
        clearBuffer();
        renderAll();
        Engine.Graphics.GraphicsDevice.SetRenderTargets(oldtargets);
    }

    private void clearBuffer()
    {
        Draw.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Opaque);
        Draw.Rect(elemBuffer.Bounds,Color.Transparent);
        Draw.SpriteBatch.End();
    }

    private void ResetBuffersIfNeeded()
    {
        if (elemBuffer==null || elemBuffer.IsDisposed || elemBuffer.Width != (int)RealSize.X) {
            DisposeBuffers();
            elemBuffer = VirtualContent.CreateRenderTarget("draw-"+longid, (int)RealSize.X, (int)RealSize.Y);
        }
    }
    public void DisposeBuffers()
    {
        if (elemBuffer!=null&&!elemBuffer.IsDisposed)
            elemBuffer.Dispose();
    }
    
    public override void Update()
    {
        base.Update(); // update ourselves before children
        Children.ForEach(e => { if (e.Active) e.Update(); });
    }

    public void ReorderChildren()
    {
        doSort(Children);
    }

    public void AddChild(UiElement ch)
    {
        if (!Children.Exists(e => e==ch )) {
            ch.Parent = this;
            Children.Add(ch);
            ReorderChildren();
        }
    }

    public UiElement() : base(Vector2.Zero)
    {
        Children = new List<UiElement>();
        id = GetType().Name;
        uuid = Guid.NewGuid().ToString();
        TweenTo = Position;
        TweenFrom = Position;
        AddTag(Tags.HUD);
        if (RenderMode == UiEnum.RenderMode.ClipOverflow) ResetBuffersIfNeeded();
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

    public void Dispose()
    {
        DisposeBuffers();
    }

    public void InvokePressed() => OnPress?.Invoke();
}