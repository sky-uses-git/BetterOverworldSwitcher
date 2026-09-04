using System.Xml;
using Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// TEMPORARY!!!!!!!
// TODO: load from xml files
public static class UiLoader
{
    private static XmlLoader xmlLoader = new();
    public static UiRoot LoadFromXML(string filename)
    {
        XmlElement ui = xmlLoader.Load(filename);
        //Logger.Info("BOS UiLoader",ui.LocalName);
        return null;
    }

    public static UiRoot Load(string id)
    {
        LoadFromXML("Graphics/Atlases/Mountain/SkyIsYou/BetterOverworldSwitcher/Ui/" + id);
        Logger.Info("BOS","load ui "+id+" called");
        if (id.Equals("root")) return loadroot();
        if (id.Equals("fileselect")) return loadfilesel();
        if (id.Equals("chapselect")) return loadcs();
        return loadnotfound(id);
    }

    private static UiRoot loadnotfound(string id)
    {
        UiRoot root = new("notfound");
        root.Size = ScaleOffset.FromScale(1,1);
        UiTextLabel notfoundtx = new("UI of ID "+id+" not found or failed to load", 72,new Vector2(0, -50), new Vector2(0, 0.333f));
        notfoundtx.TextColor = Color.Red;
        notfoundtx.BackgroundColor = Color.Black*.75f;
        notfoundtx.Size = new ScaleOffset(0,100,1,0);
        UiFancyButton backButton = new("Back to root", new Vector2(-200, -40), new Vector2(0.5f, 0.667f));
        backButton.TextColor = Color.White;
        backButton.BackgroundColor = Color.Black;
        backButton.Size = ScaleOffset.FromOffset(400, 80);
        backButton.OnPress += () => BOSHudRenderer.Instance.Goto("root");
        root.AddChild(notfoundtx);
        root.AddChild(backButton);
        root.SelectFirst = backButton;
        return root;
    }

    private static UiRoot loadroot()
    {
        UiRoot root = new("mainmenu");
        root.Size = ScaleOffset.FromScale(1,1);
        UiTextLabel labelTest = new("BetterOverworldSwitcher Host",72,new Vector2(-500,-40),new Vector2(0.5f,0.15f));
        labelTest.Size = ScaleOffset.FromOffset(1000,80);
        root.AddChild(labelTest);
        UiFrame buttonsGrp = new(new Vector2(-400,-250), new Vector2(.5f, .575f));
        buttonsGrp.Size = ScaleOffset.FromOffset(800, 500);
        UiFancyButton csbutton = new("file select", new Vector2(-100,0), new Vector2(.5f,0f));
        UiFancyButton dbgbutton = new("debug", new Vector2(-100,-20), new Vector2(.5f,.25f));
        UiFancyButton button3 = new("test notfound", new Vector2(-100,-40), new Vector2(.5f,.5f));
        UiFancyButton button4 = new("settings (not work)", new Vector2(-100,-60), new Vector2(.5f,.75f));
        UiFancyButton exitbtn = new("exit game", new Vector2(-100,-80), new Vector2(.5f,1f));
        UiFancyButton vanillabtn = new("back to vanilla", new Vector2(-200,0), new Vector2(1f,.0f));
        buttonsGrp.RenderMode = UiEnum.RenderMode.ClipOverflow;
        csbutton.Size = ScaleOffset.FromOffset(200, 80);
        csbutton.UpElement = exitbtn;
        csbutton.DownElement = dbgbutton;
        csbutton.LeftElement = vanillabtn;
        csbutton.RightElement = vanillabtn;
        csbutton.OnPress += rootGoFS;
        dbgbutton.Size = ScaleOffset.FromOffset(200, 80);
        dbgbutton.UpElement = csbutton;
        dbgbutton.DownElement = button3;
        dbgbutton.OnPress += rootGoCS;
        button3.Size = ScaleOffset.FromOffset(200, 80);
        button3.UpElement = dbgbutton;
        button3.DownElement = button4;
        button3.OnPress += () => BOSHudRenderer.Instance.Goto("asdfhjasfhdjasdhjfjksadfhjjkafhjk");
        button4.Size = ScaleOffset.FromOffset(200, 80);
        button4.UpElement = button3;
        button4.DownElement = exitbtn;
        exitbtn.Size = ScaleOffset.FromOffset(200, 80);
        exitbtn.UpElement = button4;
        exitbtn.DownElement = csbutton;
        exitbtn.OnPress += () => Engine.Instance.Exit();
        vanillabtn.Size = ScaleOffset.FromOffset(200, 80);
        vanillabtn.LeftElement = csbutton;
        vanillabtn.RightElement = csbutton;
        vanillabtn.DownElement = dbgbutton;
        vanillabtn.UpElement = exitbtn;
        vanillabtn.OnPress += () => BOSHostScene.Instance.LoadVanilla();
        csbutton.BackgroundColor = Color.DarkRed * .4f;
        dbgbutton.BackgroundColor = Color.OrangeRed * .4f;
        button3.BackgroundColor = Color.Gold * .4f;
        button4.BackgroundColor = Color.Green * .4f;
        exitbtn.BackgroundColor = Color.DarkBlue * .4f;
        vanillabtn.BackgroundColor = Color.SkyBlue * .4f;
        vanillabtn.TextColor = Color.Black;
        buttonsGrp.AddChild(csbutton);
        buttonsGrp.AddChild(dbgbutton);
        buttonsGrp.AddChild(button3);
        buttonsGrp.AddChild(button4);
        buttonsGrp.AddChild(exitbtn);
        buttonsGrp.AddChild(vanillabtn);
        root.AddChild(buttonsGrp);
        root.SelectFirst = csbutton;
        return root;
    }
    private static UiRoot loadfilesel()
    {
        UiRoot root = new("filesel");
        root.Size = ScaleOffset.FromScale(1,1);
        UiTextLabel labelTest = new("File Select",72,new Vector2(-125,-40),new Vector2(0.5f,0.15f));
        labelTest.Size = ScaleOffset.FromOffset(250,80);
        root.AddChild(labelTest);
        UiFrame buttonsGrp = new(new Vector2(-400,-250), new Vector2(.5f, .575f));
        buttonsGrp.Size = ScaleOffset.FromOffset(800, 500);
        UiFancyButton backbtn = new("go back", new Vector2(0,0), new Vector2(0f,0f));
        UiFancyButton chp1btn = new("slot1", new Vector2(0,-50), new Vector2(0f,.5f));
        UiFancyButton chp2btn = new("slot2", new Vector2(-17,-50), new Vector2(.16f,.5f));
        UiFancyButton chp3btn = new("slot3", new Vector2(-33,-50), new Vector2(.33f,.5f));
        backbtn.Size = ScaleOffset.FromOffset(200, 80);
        backbtn.DownElement = chp1btn;
        backbtn.OnPress += csGoBack;
        chp1btn.Size = ScaleOffset.FromOffset(100, 100);
        chp1btn.UpElement = backbtn;
        chp1btn.RightElement = chp2btn;
        chp1btn.LeftElement = chp3btn;
        chp1btn.OnPress += () => { goFs(1); };
        chp2btn.Size = ScaleOffset.FromOffset(100, 100);
        chp2btn.UpElement = backbtn;
        chp2btn.RightElement = chp3btn;
        chp2btn.LeftElement = chp1btn;
        chp2btn.OnPress += () => { goFs(2); };
        chp3btn.Size = ScaleOffset.FromOffset(100, 100);
        chp3btn.UpElement = backbtn;
        chp3btn.RightElement = chp1btn;
        chp3btn.LeftElement = chp2btn;
        chp3btn.OnPress += () => { goFs(3); };
        backbtn.BackgroundColor = Color.DarkRed * .5f;
        chp1btn.BackgroundColor = Color.Black * .5f;
        chp2btn.BackgroundColor = Color.Black * .5f;
        chp3btn.BackgroundColor = Color.Black * .5f;
        buttonsGrp.AddChild(backbtn);
        buttonsGrp.AddChild(chp1btn);
        buttonsGrp.AddChild(chp2btn);
        buttonsGrp.AddChild(chp3btn);
        root.AddChild(buttonsGrp);
        root.SelectFirst = backbtn;
        return root;
    }
    private static UiRoot loadcs()
    {
        UiRoot root = new("chaptersel");
        root.Size = ScaleOffset.FromScale(1,1);
        UiTextLabel labelTest = new("Chapter select",72,new Vector2(-125,-40),new Vector2(0.5f,0.15f));
        labelTest.Size = ScaleOffset.FromOffset(250,80);
        root.AddChild(labelTest);
        UiFrame buttonsGrp = new(new Vector2(-400,-250), new Vector2(.5f, .575f));
        buttonsGrp.Size = ScaleOffset.FromOffset(800, 500);
        UiFancyButton backbtn = new("go back", new Vector2(0,-40), new Vector2(0f,.25f));
        UiFancyButton chp1btn = new("ch1", new Vector2(0,-50), new Vector2(0f,.5f));
        UiFancyButton chp2btn = new("ch2", new Vector2(-17,-50), new Vector2(.16f,.5f));
        UiFancyButton chp3btn = new("ch3", new Vector2(-33,-50), new Vector2(.33f,.5f));
        UiFancyButton chp4btn = new("ch4", new Vector2(-50,-50), new Vector2(.5f,.5f));
        UiFancyButton chp5btn = new("ch5", new Vector2(-67,-50), new Vector2(.67f,.5f));
        UiFancyButton chp6btn = new("ch6", new Vector2(-83,-50), new Vector2(.83f,.5f));
        UiFancyButton chp7btn = new("ch7", new Vector2(-100,-50), new Vector2(1f,.5f));
        UiFancyButton chp8btn = new("ch8", new Vector2(0,-50), new Vector2(0f,.75f));
        UiFancyButton chp9btn = new("ch9", new Vector2(-17,-50), new Vector2(.16f,.75f));
        backbtn.Size = ScaleOffset.FromOffset(200, 80);
        backbtn.DownElement = chp1btn;
        backbtn.OnPress += csGoBack;
        chp1btn.Size = ScaleOffset.FromOffset(100, 100);
        chp1btn.UpElement = backbtn;
        chp1btn.RightElement = chp2btn;
        chp1btn.LeftElement = chp9btn;
        chp1btn.DownElement = chp8btn;
        chp1btn.OnPress += () => { goCh(1); };
        chp2btn.Size = ScaleOffset.FromOffset(100, 100);
        chp2btn.UpElement = backbtn;
        chp2btn.RightElement = chp3btn;
        chp2btn.LeftElement = chp1btn;
        chp2btn.DownElement = chp9btn;
        chp2btn.OnPress += () => { goCh(2); };
        chp3btn.Size = ScaleOffset.FromOffset(100, 100);
        chp3btn.UpElement = backbtn;
        chp3btn.RightElement = chp4btn;
        chp3btn.LeftElement = chp2btn;
        chp3btn.DownElement = chp9btn;
        chp3btn.OnPress += () => { goCh(3); };
        chp4btn.Size = ScaleOffset.FromOffset(100, 100);
        chp4btn.UpElement = backbtn;
        chp4btn.RightElement = chp5btn;
        chp4btn.LeftElement = chp3btn;
        chp4btn.DownElement = chp9btn;
        chp4btn.OnPress += () => { goCh(4); };
        chp5btn.Size = ScaleOffset.FromOffset(100, 100);
        chp5btn.UpElement = backbtn;
        chp5btn.RightElement = chp6btn;
        chp5btn.LeftElement = chp4btn;
        chp5btn.DownElement = chp9btn;
        chp5btn.OnPress += () => { goCh(5); };
        chp6btn.Size = ScaleOffset.FromOffset(100, 100);
        chp6btn.UpElement = backbtn;
        chp6btn.RightElement = chp7btn;
        chp6btn.LeftElement = chp5btn;
        chp6btn.DownElement = chp9btn;
        chp6btn.OnPress += () => { goCh(6); };
        chp7btn.Size = ScaleOffset.FromOffset(100, 100);
        chp7btn.UpElement = backbtn;
        chp7btn.RightElement = chp8btn;
        chp7btn.LeftElement = chp6btn;
        chp7btn.DownElement = chp9btn;
        chp7btn.OnPress += () => { goCh(7); };
        chp8btn.UpElement = chp1btn;
        chp8btn.DownElement = backbtn;
        chp8btn.LeftElement = chp7btn;
        chp8btn.RightElement = chp9btn;
        chp8btn.OnPress += () => { goCh(8); };
        chp9btn.UpElement = chp2btn;
        chp9btn.DownElement = backbtn;
        chp9btn.LeftElement = chp8btn;
        chp9btn.RightElement = chp1btn;
        chp9btn.OnPress += () => { goCh(9); };
        backbtn.BackgroundColor = Color.DarkRed * .5f;
        chp1btn.BackgroundColor = Color.Black * .5f;
        chp2btn.BackgroundColor = Color.Black * .5f;
        chp3btn.BackgroundColor = Color.Black * .5f;
        chp4btn.BackgroundColor = Color.Black * .5f;
        chp5btn.BackgroundColor = Color.Black * .5f;
        chp6btn.BackgroundColor = Color.Black * .5f;
        chp7btn.BackgroundColor = Color.Black * .5f;
        chp8btn.BackgroundColor = Color.Black * .5f;
        chp9btn.BackgroundColor = Color.Black * .5f;
        buttonsGrp.AddChild(backbtn);
        buttonsGrp.AddChild(chp1btn);
        buttonsGrp.AddChild(chp2btn);
        buttonsGrp.AddChild(chp3btn);
        buttonsGrp.AddChild(chp4btn);
        buttonsGrp.AddChild(chp5btn);
        buttonsGrp.AddChild(chp6btn);
        buttonsGrp.AddChild(chp7btn);
        buttonsGrp.AddChild(chp8btn);
        buttonsGrp.AddChild(chp9btn);
        root.AddChild(buttonsGrp);
        root.SelectFirst = backbtn;
        return root;
    }

    private static void csGoBack()
    {
        Logger.Info("BOS","goto root");
        BOSHudRenderer.Instance.Goto("root");
    }
    private static void rootGoCS()
    {
        Logger.Info("BOS","goto chapselect");
        BOSHudRenderer.Instance.Goto("chapselect");
    }
    private static void rootGoFS()
    {
        Logger.Info("BOS","goto fileselect");
        BOSHudRenderer.Instance.Goto("fileselect");
    }

    private static void goCh(int ch)
    {
        SaveData.InitializeDebugMode();
        LevelEnter.Go(new Session(AreaData.Get(ch).ToKey()), fromSaveData: false);
    }
    private static void goFs(int fs)
    {
//        SaveData.Instance.CurrentSession.A = ch;
        rootGoCS();
//        LevelEnter.Go(new Session(ch), fromSaveData: true);
    }

}