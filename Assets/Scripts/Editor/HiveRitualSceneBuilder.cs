using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// Builds the first-playable hive-ritual yard (plans/hive-ritual.md) out of
/// placeholder primitives, so the scene can be regenerated as the slice grows.
public static class HiveRitualSceneBuilder
{
    [MenuItem("BeeKeeper/Build Hive Ritual Scene")]
    static void Build()
    {
        if (File.Exists(kScenePath) && !EditorUtility.DisplayDialog(
                "Rebuild the hive ritual scene?",
                kScenePath + " already exists. Rebuilding replaces it, including anything you changed in it by hand.",
                "Rebuild", "Cancel"))
            return;

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        // DefaultGameObjects gives a Main Camera and a Directional Light set up
        // for the active render pipeline; the camera becomes the player's head.
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        Camera camera = FindSceneCamera(scene);

        GameClock clock = BuildSimulation(out EconomyManager economy);
        BuildGround();
        BuildHive(new Vector3(0f, 0f, 4f), clock);
        BuildShed(new Vector3(-6f, 0f, 7f), clock);
        BuildHoneyStand(new Vector3(5f, 0f, 5f), economy);
        PlayerHud hud = BuildHud(clock, economy);
        BuildPlayer(camera, hud, new Vector3(0f, 0.1f, 0f));

        EditorSceneManager.SaveScene(scene, kScenePath);
        AddSceneToBuildSettings();
        Debug.Log("Built " + kScenePath + " — press Play to walk the yard.");
    }

    static GameClock BuildSimulation(out EconomyManager economy)
    {
        var simulation = new GameObject("Simulation");
        var clock = simulation.AddComponent<GameClock>();
        var ticker = simulation.AddComponent<HiveTicker>();
        ticker.clock = clock;

        // Its own object: EconomyManager's duplicate check destroys its whole
        // GameObject, which mustn't take the clock and ticker with it.
        economy = new GameObject("Economy").AddComponent<EconomyManager>();
        return clock;
    }

    // A roadside table: jars at the front (local -Z), the sign behind them
    // (local +Z). Turned so the front faces the player's start point at the
    // origin, i.e. the player walks up on the customer's side.
    static void BuildHoneyStand(Vector3 position, EconomyManager economy)
    {
        var stand = new GameObject("Honey Stand");
        stand.transform.position = position;
        stand.transform.rotation = Quaternion.LookRotation(new Vector3(position.x, 0f, position.z));

        var tableWood = new Color(0.50f, 0.36f, 0.22f);
        AddBox(stand.transform, "Table Top", new Vector3(0f, 0.8f, 0f), new Vector3(1.2f, 0.05f, 0.6f), "StandWood", tableWood);
        for (int i = 0; i < 4; i++)
        {
            float x = i % 2 == 0 ? -0.55f : 0.55f;
            float z = i < 2 ? -0.25f : 0.25f;
            AddBox(stand.transform, "Leg " + (i + 1), new Vector3(x, 0.4f, z), new Vector3(0.05f, 0.8f, 0.05f), "StandWood", tableWood);
        }
        AddBox(stand.transform, "Sign Post", new Vector3(0f, 0.8f, 0.28f), new Vector3(0.05f, 1.6f, 0.05f), "StandWood", tableWood);
        AddBox(stand.transform, "Sign", new Vector3(0f, 1.4f, 0.26f), new Vector3(0.8f, 0.3f, 0.03f), "StandSign", new Color(0.92f, 0.85f, 0.55f));

        for (int i = 0; i < 3; i++)
        {
            // The cylinder primitive is 2 m tall at scale 1: these jars are 0.12 m tall.
            GameObject jar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            jar.name = "Jar " + (i + 1);
            jar.transform.SetParent(stand.transform, false);
            jar.transform.localPosition = new Vector3((i - 1) * 0.3f, 0.885f, -0.05f);
            jar.transform.localScale = new Vector3(0.08f, 0.06f, 0.08f);
            SetMaterial(jar, "CappedHoney", new Color(0.96f, 0.80f, 0.38f));
            RemoveCollider(jar);
        }

        stand.AddComponent<HoneyStand>().economy = economy;
    }

    static void BuildGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        // The plane primitive is 10 x 10 m at scale 1.
        ground.transform.localScale = new Vector3(kGroundSizeMeters / 10f, 1f, kGroundSizeMeters / 10f);
        SetMaterial(ground, "Grass", new Color(0.33f, 0.47f, 0.22f));
    }

    static void BuildHive(Vector3 position, GameClock clock)
    {
        var hiveObject = new GameObject("Hive");
        hiveObject.transform.position = position;

        var hive = hiveObject.AddComponent<HiveController>();
        hive.breed = AssetDatabase.LoadAssetAtPath<BeeBreedData>(kStarterBreedPath);
        if (hive.breed == null)
            Debug.LogWarning("No bee breed at " + kStarterBreedPath + "; the hive won't make honey until one is assigned.");
        // The colony starts with its reserve already stored, so everything it
        // makes from day 1 is harvestable.
        hive.honeyStoredKg = hive.honeyCapacityKg * hive.harvestReserveFraction;

        var interaction = hiveObject.AddComponent<HiveInteraction>();
        interaction.clock = clock;

        // A plain box hive, about 0.5 m square and 0.9 m tall, entrance facing
        // the player's start point. Replaced by the Blender model later.
        AddBox(hiveObject.transform, "Stand", new Vector3(0f, 0.15f, 0f), new Vector3(0.56f, 0.3f, 0.56f), "HiveStand", new Color(0.30f, 0.22f, 0.15f));
        AddBox(hiveObject.transform, "Body", new Vector3(0f, 0.55f, 0f), new Vector3(0.5f, 0.5f, 0.5f), "HiveBody", new Color(0.86f, 0.74f, 0.42f));
        AddBox(hiveObject.transform, "Entrance", new Vector3(0f, 0.33f, -0.251f), new Vector3(0.3f, 0.03f, 0.01f), "HiveEntrance", new Color(0.08f, 0.06f, 0.04f));

        // Frames hang from top bars that sit just proud of the body's top face
        // (y = 0.80), under the lid; the comb hangs down inside the box.
        var frames = new GameObject("Frames");
        frames.transform.SetParent(hiveObject.transform, false);
        frames.transform.localPosition = new Vector3(0f, 0.805f, 0f);
        float middle = (kFrameCount - 1) * 0.5f;
        for (int i = 0; i < kFrameCount; i++)
        {
            // Outer frames carry more honey, the middle ones more brood;
            // the shares average out to about 1 across the box.
            float honeyShare = 0.5f + Mathf.Abs(i - middle) / middle;
            BuildFrame(frames.transform, interaction, i + 1, new Vector3((i - middle) * kFrameSpacingMeters, 0f, 0f), honeyShare);
        }

        GameObject lid = AddBox(hiveObject.transform, "Lid", new Vector3(0f, 0.86f, 0f), new Vector3(0.58f, 0.08f, 0.58f), "HiveLid", new Color(0.36f, 0.42f, 0.36f));
        interaction.lid = lid.transform;
    }

    // A frame: a top bar with the comb below it. One box collider on the root
    // covers bar and comb, so it's easy to aim at from above and while held.
    static void BuildFrame(Transform parent, HiveInteraction hive, int frameNumber, Vector3 localPosition, float honeyShare)
    {
        var frameObject = new GameObject("Frame " + frameNumber);
        frameObject.transform.SetParent(parent, false);
        frameObject.transform.localPosition = localPosition;

        var hitBox = frameObject.AddComponent<BoxCollider>();
        hitBox.center = new Vector3(0f, -kCombHeightMeters * 0.5f, 0f);
        hitBox.size = new Vector3(0.04f, kCombHeightMeters + 0.04f, 0.46f);

        RemoveCollider(AddBox(frameObject.transform, "Top Bar", Vector3.zero, new Vector3(0.025f, 0.02f, 0.46f), "FrameWood", new Color(0.78f, 0.62f, 0.38f)));
        GameObject honey = AddBox(frameObject.transform, "Honey", new Vector3(0f, -0.015f, 0f), new Vector3(0.012f, 0.01f, 0.42f), "CappedHoney", new Color(0.96f, 0.80f, 0.38f));
        GameObject brood = AddBox(frameObject.transform, "Brood", new Vector3(0f, -0.01f - kCombHeightMeters * 0.5f, 0f), new Vector3(0.012f, kCombHeightMeters, 0.42f), "Brood", new Color(0.55f, 0.38f, 0.20f));
        RemoveCollider(honey);
        RemoveCollider(brood);
        honey.SetActive(false);

        var frame = frameObject.AddComponent<HiveFrame>();
        frame.hive = hive;
        frame.honeyBand = honey.transform;
        frame.broodBand = brood.transform;
        frame.frameNumber = frameNumber;
        frame.honeyShare = honeyShare;
        frame.combHeightMeters = kCombHeightMeters;
    }

    // Stands in for the apiary support building. Its door is where the player sleeps.
    static void BuildShed(Vector3 position, GameClock clock)
    {
        var shed = new GameObject("Shed");
        shed.transform.position = position;
        AddBox(shed.transform, "Walls", new Vector3(0f, 1.2f, 0f), new Vector3(3f, 2.4f, 2.5f), "ShedWalls", new Color(0.55f, 0.45f, 0.35f));
        AddBox(shed.transform, "Roof", new Vector3(0f, 2.5f, 0f), new Vector3(3.3f, 0.2f, 2.8f), "ShedRoof", new Color(0.35f, 0.20f, 0.18f));

        GameObject door = AddBox(shed.transform, "Door", new Vector3(0f, 1f, -1.28f), new Vector3(0.9f, 2f, 0.06f), "ShedDoor", new Color(0.25f, 0.16f, 0.10f));
        door.AddComponent<SleepSpot>().clock = clock;
    }

    static PlayerHud BuildHud(GameClock clock, EconomyManager economy)
    {
        var hudObject = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler));
        hudObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = hudObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        var crosshair = new GameObject("Crosshair", typeof(RectTransform), typeof(Image));
        crosshair.transform.SetParent(hudObject.transform, false);
        var crosshairRect = (RectTransform)crosshair.transform;
        crosshairRect.anchorMin = crosshairRect.anchorMax = crosshairRect.pivot = new Vector2(0.5f, 0.5f);
        crosshairRect.anchoredPosition = Vector2.zero;
        crosshairRect.sizeDelta = new Vector2(6f, 6f);
        crosshair.GetComponent<Image>().raycastTarget = false;

        Font font = Resources.GetBuiltinResource<Font>(kBuiltinFontName);
        Text prompt = AddText(hudObject.transform, "Prompt", font, 30, new Vector2(0.5f, 0.5f), new Vector2(0f, -60f), new Vector2(900f, 50f), TextAnchor.MiddleCenter);
        Text message = AddText(hudObject.transform, "Message", font, 28, new Vector2(0.5f, 0f), new Vector2(0f, 150f), new Vector2(1500f, 160f), TextAnchor.MiddleCenter);
        Text clockText = AddText(hudObject.transform, "Clock", font, 30, new Vector2(0f, 1f), new Vector2(190f, -45f), new Vector2(340f, 50f), TextAnchor.MiddleLeft);

        var hud = hudObject.AddComponent<PlayerHud>();
        hud.promptText = prompt;
        hud.messageText = message;
        hud.clockText = clockText;
        hud.clock = clock;
        hud.walletText = AddText(hudObject.transform, "Wallet", font, 30, new Vector2(1f, 1f), new Vector2(-190f, -60f), new Vector2(340f, 90f), TextAnchor.MiddleRight);
        hud.economy = economy;
        return hud;
    }

    static void BuildPlayer(Camera camera, PlayerHud hud, Vector3 position)
    {
        var player = new GameObject("Player");
        player.transform.position = position;

        var body = player.AddComponent<CharacterController>();
        body.height = kPlayerHeightMeters;
        body.radius = 0.3f;
        body.center = new Vector3(0f, kPlayerHeightMeters * 0.5f, 0f);

        if (camera == null)
        {
            var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            camera = cameraObject.GetComponent<Camera>();
        }
        camera.transform.SetParent(player.transform, false);
        camera.transform.localPosition = new Vector3(0f, kEyeHeightMeters, 0f);
        camera.transform.localRotation = Quaternion.identity;
        camera.nearClipPlane = 0.05f;
        camera.fieldOfView = 70f;

        var controller = player.AddComponent<FirstPersonController>();
        controller.cameraPivot = camera.transform;

        var interactor = player.AddComponent<PlayerInteractor>();
        interactor.viewCamera = camera;
        interactor.hud = hud;

        var inventory = player.AddComponent<PlayerInventory>();
        interactor.inventory = inventory;
        hud.inventory = inventory;

        BuildSmoker(camera.transform, interactor);
    }

    // The smoker is held at the bottom-right of the view: a canister with a
    // nozzle, and the smoke puff emitted from the nozzle tip.
    static void BuildSmoker(Transform head, PlayerInteractor interactor)
    {
        var smokerObject = new GameObject("Smoker");
        smokerObject.transform.SetParent(head, false);
        smokerObject.transform.localPosition = new Vector3(0.28f, -0.28f, 0.5f);

        // The cylinder primitive is 2 m tall at scale 1: this makes it 0.2 m tall, 0.1 m wide.
        GameObject canister = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        canister.name = "Canister";
        canister.transform.SetParent(smokerObject.transform, false);
        canister.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        SetMaterial(canister, "SmokerMetal", new Color(0.62f, 0.64f, 0.66f));
        MakeHeldProp(canister);

        GameObject nozzle = AddBox(smokerObject.transform, "Nozzle", new Vector3(0f, 0.12f, 0.04f), new Vector3(0.04f, 0.04f, 0.1f), "SmokerMetal", new Color(0.62f, 0.64f, 0.66f));
        MakeHeldProp(nozzle);

        var smoker = smokerObject.AddComponent<Smoker>();
        smoker.interactor = interactor;
        smoker.puffParticles = BuildSmokePuff(smokerObject.transform, new Vector3(0f, 0.12f, 0.1f));
    }

    // Held props must not block the look ray or bump into the world, and
    // shouldn't cast odd shadows from in front of the camera.
    static void MakeHeldProp(GameObject prop)
    {
        RemoveCollider(prop);
        prop.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    static void RemoveCollider(GameObject primitive)
    {
        Object.DestroyImmediate(primitive.GetComponent<Collider>());
    }

    // A particle system that emits nothing on its own; Smoker calls Emit per puff.
    // Particles live in world space, so a puff stays where it was blown.
    static ParticleSystem BuildSmokePuff(Transform parent, Vector3 localPosition)
    {
        var puffObject = new GameObject("SmokePuff");
        puffObject.transform.SetParent(parent, false);
        puffObject.transform.localPosition = localPosition;

        var particles = puffObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.playOnAwake = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.2f, 2f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 2.5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.06f, 0.14f);
        main.startColor = new Color(0.9f, 0.9f, 0.9f, 0.7f);
        main.gravityModifier = -0.05f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 200;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;

        // The cone emits along the puff object's forward, i.e. where the player looks.
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 12f;
        shape.radius = 0.02f;

        ParticleSystem.LimitVelocityOverLifetimeModule drag = particles.limitVelocityOverLifetime;
        drag.enabled = true;
        drag.limit = 0.3f;
        drag.dampen = 0.15f;

        ParticleSystem.SizeOverLifetimeModule growth = particles.sizeOverLifetime;
        growth.enabled = true;
        growth.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0.6f, 1f, 3f));

        var fade = new Gradient();
        fade.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(0.8f, 0f), new GradientAlphaKey(0f, 1f) });
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = fade;

        var particleMaterial = AssetDatabase.LoadAssetAtPath<Material>(kParticleMaterialPath);
        if (particleMaterial == null)
            Debug.LogWarning("URP particle material not found at " + kParticleMaterialPath + "; smoke will render with the default material.");
        puffObject.GetComponent<ParticleSystemRenderer>().sharedMaterial = particleMaterial;
        return particles;
    }

    static Camera FindSceneCamera(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.TryGetComponent(out Camera camera))
                return camera;
        }
        return null;
    }

    static GameObject AddBox(Transform parent, string name, Vector3 localPosition, Vector3 sizeMeters, string materialName, Color color)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent, false);
        box.transform.localPosition = localPosition;
        box.transform.localScale = sizeMeters;
        SetMaterial(box, materialName, color);
        return box;
    }

    static Text AddText(Transform parent, string name, Font font, int fontSize, Vector2 anchor, Vector2 anchoredPosition, Vector2 size, TextAnchor alignment)
    {
        var textObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(Shadow));
        textObject.transform.SetParent(parent, false);
        var rect = (RectTransform)textObject.transform;
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;

        var text = textObject.GetComponent<Text>();
        text.font = font;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        textObject.GetComponent<Shadow>().effectDistance = new Vector2(2f, -2f);
        return text;
    }

    static void SetMaterial(GameObject target, string materialName, Color color)
    {
        target.GetComponent<Renderer>().sharedMaterial = GetPlaceholderMaterial(materialName, color);
    }

    // Reuses an existing material asset if there is one, so colours tweaked in
    // the editor survive a rebuild.
    static Material GetPlaceholderMaterial(string materialName, Color color)
    {
        string path = kMaterialFolder + "/" + materialName + ".mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material != null)
            return material;

        EnsureFolder(kMaterialFolder);
        material = new Material(Shader.Find(kLitShaderName));
        material.SetColor("_BaseColor", color);
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
            return;

        string parent = Path.GetDirectoryName(folderPath).Replace('\\', '/');
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(folderPath));
    }

    static void AddSceneToBuildSettings()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (scenes.Exists(s => s.path == kScenePath))
            return;

        scenes.Insert(0, new EditorBuildSettingsScene(kScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    const string kScenePath = "Assets/Scenes/HiveRitual.unity";
    const string kMaterialFolder = "Assets/Art/Materials/Placeholder";
    const string kStarterBreedPath = "Assets/ItalianBeeBreed.asset";
    const string kLitShaderName = "Universal Render Pipeline/Lit";
    const string kBuiltinFontName = "LegacyRuntime.ttf";
    const string kParticleMaterialPath = "Packages/com.unity.render-pipelines.universal/Runtime/Materials/ParticlesUnlit.mat";
    const int kFrameCount = 10;
    const float kFrameSpacingMeters = 0.045f;
    const float kCombHeightMeters = 0.4f;
    const float kGroundSizeMeters = 40f;
    const float kPlayerHeightMeters = 1.8f;
    const float kEyeHeightMeters = 1.65f;
}
