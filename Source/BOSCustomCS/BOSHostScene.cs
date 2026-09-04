using Celeste.Mod.Celeste3DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

// what we wanna do:
// - copy vanilla overworld
// - except add system to cleanly hand full control over to mods' overworlds TODO: should we give full control?
// - '- other option: use config files + a separate editor app (ewie)
// - config files for like better detail on how to switch over might be needed
// - first hand over control to any other specified default or just go with our self
// - !!! mod overworlds will have to cooperate for us to get control back cleanly
// - fallback to us first; we can get diagnostics. fallback to vanilla if we fail !

public class BOSHostScene : Scene
{
    public static BOSHostScene Instance { get; private set; }

    public Scene3D Viewer { get; private set; }
    public BOSCamController CamController { get; private set; }
    public BOSHudRenderer Hud { get; private set; }
    public HiresSnow Snow { get; private set; }
    public bool Debug { get; private set; } = false;
    
    public override void Update()
    {
        if (MInput.Keyboard.Pressed(Keys.Space)) Instance.Debug = !Instance.Debug;
        base.Update();
    }

    public BOSHostScene(OverworldLoader loader)
    {
        Instance = this;
        EntityData e = new EntityData();
        e.Values = new();
        e.Values["modelsPath"] = "Graphics/3DEngine/DefaultModels";
        e.Values["texturesPath"] = "Graphics/3DEngine/DefaultTextures";
        e.Values["exportsPath"] = "Graphics/3DEngine/DefaultExports";
        e.Values["fontsPath"] = "Graphics/3DEngine/DefaultFonts";
        e.Values["audioPath"] = "Graphics/3DEngine/DefaultAudio";
        e.Values["persistent"] = true;
        Add(new EngineEntity(e, Vector2.Zero));
        EngineEntity.OnEngineLoad += LoadMtnScene3D;
        Add(Hud = new(loader.StartMode));
        Add(Snow = loader.Snow ?? new());
        Add(CamController = new());
        RendererList.UpdateLists();
        Entities.UpdateLists();
    }

    private void LoadMtnScene3D(EngineEntity engine, Scene scene)
    {
        Viewer = new Scene3D();
        Camera3D mainCam = new();
        Viewer.AddGameObject(mainCam);
        Viewer.SetMainCamera(mainCam);
        GameObject mtnplane = GameObject.DefaultPlane;
        mtnplane.transform.SetScale(new Vector3(25,1,25));
        mtnplane.GetComponent<MeshRenderer>().ChangeLayer(ModelLayer.Foreground);
        Viewer.AddGameObject(mtnplane);
        GameObject mtn = GameObject.DefaultCube;
        mtn.transform.SetScale(new Vector3(.5f,.5f,.5f));
        mtn.transform.SetPosition(new Vector3(0,.25f,0));
        mtn.GetComponent<MeshRenderer>().ChangeLayer(ModelLayer.Foreground);
        Viewer.AddGameObject(mtn);
        Viewer.ChangeSkyBox("sky");
        Viewer.GetRenderer().SetHDMode(true);
        EngineEntity.LoadScene(Viewer);
    }

    public override void End()
    {
        Hud.End();
        Instance = null;
        base.End();
    }

    public void LoadVanilla()
    {
        BOSHooks.forceVanilla = true;
        Engine.Scene = new OverworldLoader(Overworld.StartMode.Titlescreen);
    }
}