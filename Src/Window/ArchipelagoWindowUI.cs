using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using Parkitect.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoMod.Src.Window
{
    public class ArchipelagoWindowUI : UIWindow
    {
        public enum PageNames
        {
            Challenges,
            EnergyLink,
        }
        public enum AssetNames
        {
            ButtonFF,
            ButtonMenu,
            ButtonSkip,
            ButtonStandard,
            History,
            Page,
            Archipelago,
            Challenges,
            ChallengesList,
            EnergyLink,
        }

        private RectTransform Window = null;
        private ParkitectController ParkitectController = null;
        private GameObject Menu = null;
        private Dictionary<PageNames, Button> MenuButtons = new Dictionary<PageNames, Button>();
        private Dictionary<PageNames, GameObject> Pages = new Dictionary<PageNames, GameObject>();
        private Dictionary<AssetNames, Sprite> Sprites = new Dictionary<AssetNames, Sprite>();

        public event Action<PageNames> OnSwitch;

        private const float PAGE_WIDTH = 320f;
        private const float PAGE_HEIGHT = 320f;
        private const int PAGE_PADDING_SIDE = 8;

        public void Spawn()
        {
            Helper.Debug($"[ArchiWindow::Spawn]");
            if (UIWindowsController.Instance == null)
            {
                Helper.Debug($"[ArchiWindow::Spawn] No instance of UIWindowsController found");
                return;
            }

            WindowHelper.Prepare();
            Helper.Debug($"[ArchiWindow::Spawn] WindowHelper.Prepare");
           
            GameObject gameObject = new GameObject("Archipelago", new Type[] { typeof(RectTransform) });
            
            this.Window = (RectTransform)gameObject.transform;
            this.Window.sizeDelta = new Vector2(420f, 400f);

            UIWindowSettings UIWindowSettings = gameObject.AddComponent<UIWindowSettings>();
            UIWindowSettings.title = "Archipelago";
            UIWindowSettings.uniqueTagString = "ArchipelagoMod";
            UIWindowSettings.closable = true;
            UIWindowSettings.pinnable = true;
            UIWindowSettings.destroyWhenClosed = false;
            UIWindowSettings.contentDeterminesWindowSize = true;
            UIWindowSettings.spawnLocation = UIWindowSettings.SpawnLocation.Free;

            VerticalLayoutGroup VerticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            //VerticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
            VerticalLayoutGroup.spacing = 0f;
            VerticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            VerticalLayoutGroup.childControlWidth = true;
            VerticalLayoutGroup.childControlHeight = true;
            VerticalLayoutGroup.childForceExpandWidth = false;
            VerticalLayoutGroup.childForceExpandHeight = true;

            ArchipelagoWindowUI ArchiWindow = gameObject.AddComponent<ArchipelagoWindowUI>();
            ArchiWindow.Prepare(this.Window, GetComponent<ParkitectController>());
            ArchiWindow.BuildMenu();
            ArchiWindow.BuildPages();

            RectTransform menuRect = ArchiWindow.Menu.GetComponent<RectTransform>();
            RectTransform menuButton1 = ArchiWindow.Menu.transform.Find("Challenges").GetComponent<RectTransform>();
            RectTransform menuButton1Sprite = menuButton1.transform.Find("ChallengesSprite").GetComponent<RectTransform>();
            Helper.Debug($"[Menu Debug] Size: {menuButton1.sizeDelta}, ChildCount: {menuButton1.childCount}");
            Helper.Debug($"[Menu Debug] Size: {menuButton1Sprite.sizeDelta}, ChildCount: {menuButton1Sprite.childCount}");

            UIWindowsController.Instance.spawnWindow(ArchiWindow);
        }

        private void Prepare(RectTransform window, ParkitectController parkitectController)
        {
            Helper.Debug($"[ArchiWindow::Prepare]");
            this.Window = window;
            this.ParkitectController = parkitectController;

            this.Sprites[AssetNames.ButtonFF] = WindowHelper.CreateSpriteFrom("assets/btn-ff");
            this.Sprites[AssetNames.ButtonMenu] = WindowHelper.CreateSpriteFrom("assets/btn-menu");
            this.Sprites[AssetNames.ButtonSkip] = WindowHelper.CreateSpriteFrom("assets/btn-skip");
            this.Sprites[AssetNames.ButtonStandard] = WindowHelper.CreateSpriteFrom("assets/btn-standard");
            this.Sprites[AssetNames.History] = WindowHelper.CreateSpriteFrom("assets/history");
            this.Sprites[AssetNames.Page] = WindowHelper.CreateSpriteFrom("assets/page");
            this.Sprites[AssetNames.ChallengesList] = WindowHelper.CreateSpriteFrom("assets/challenges-list");
            this.Sprites[AssetNames.Challenges] = WindowHelper.CreateSpriteFrom("archipelago-challenges-icon-fit");
            this.Sprites[AssetNames.EnergyLink] = WindowHelper.CreateSpriteFrom("archipelago-energyLink-icon-fit");
            this.Sprites[AssetNames.Archipelago] = WindowHelper.CreateSpriteFrom("archipelago-icon");

            this.MenuButtons[PageNames.Challenges] = null;
            this.MenuButtons[PageNames.EnergyLink] = null;
            
            this.Pages[PageNames.EnergyLink] = null;
            this.Pages[PageNames.Challenges] = null;
        }

        private void SwitchTab(PageNames spriteName)
        {
            foreach (KeyValuePair<PageNames, Button> entry in this.MenuButtons)
            {
                PageNames _spriteName = entry.Key;
                // disabled state by default
                Color color = Colors.ConvertFromHex(Colors.Deactivated);
                bool isActive = false;

                // Set active
                if (spriteName == _spriteName)
                {
                    color = Colors.ConvertFromHex(Colors.LightGrey);
                    isActive = true;
                }

                // disable
                this.Pages[_spriteName].SetActive(isActive);
                this.MenuButtons[_spriteName].GetComponent<Image>().color = color;
            }

            this.OnSwitch?.Invoke(spriteName);
        }

        private void BuildMenu()
        {
            Helper.Debug($"[ArchiWindow::BuildMenu]");
            // Fake Menu
            //Transform TabMenu = WindowHelper.BuildColumn(this.Window, "TabMenu", 400f);

            // Menu
            this.Menu = WindowHelper.CreateUIObject("Menu", this.Window.transform, new Type[]
            {
                typeof(RectTransform),
                typeof(LayoutElement),
                typeof(HorizontalLayoutGroup),
            });

            LayoutElement LayoutElement = this.Menu.GetComponent<LayoutElement>();
            LayoutElement.flexibleHeight = 1f;
            LayoutElement.flexibleWidth = 0f;
            LayoutElement.minHeight = 42f;

            HorizontalLayoutGroup horizontalLayoutGroup = this.Menu.GetComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.padding = new RectOffset(24, 24, 0, 0);
            horizontalLayoutGroup.spacing = 4f;
            horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            Helper.Debug($"[ArchiWindow::BuildMenu] Menu");

            //Transform parent = WindowHelper.BuildColumn(this.Menu.transform, "Menu", 400f);
            //this.BuildMenuButtons(parent);

            //GameObject gameObject = WindowHelper.CreateHorizontalRow("Tabs", Menu.transform, 28f, false, false);
            //Button ChallengesButton = WindowHelper.CreateMenuButton(gameObject.transform, this.Sprites[SpriteName.Challenges], delegate
            //{
            //    this.SwitchTab(SpriteName.Challenges);
            //});
            //Button EnergyLinkButton = WindowHelper.CreateMenuButton(gameObject.transform, this.Sprites[SpriteName.EnergyLink], delegate
            //{
            //    this.SwitchTab(SpriteName.EnergyLink);
            //});
            this.MenuButtons[PageNames.Challenges] = WindowHelper.CreateMenuButton(
                this.Menu.transform,
                "Challenges",
                this.Sprites[AssetNames.ButtonMenu],
                this.Sprites[AssetNames.Challenges],
                () => { this.SwitchTab(PageNames.Challenges); }
            );
            this.MenuButtons[PageNames.Challenges] = WindowHelper.CreateMenuButton(
                this.Menu.transform,
                "EnergyLink",
                this.Sprites[AssetNames.ButtonMenu],
                this.Sprites[AssetNames.EnergyLink],
                () => { this.SwitchTab(PageNames.EnergyLink); }
            );
            Helper.Debug($"[ArchiWindow::BuildMenu] Buttons");
        }

        private void BuildPages()
        {
            Helper.Debug($"[ArchiWindow::BuildPages]");
            // Pages
            GameObject Pages = WindowHelper.CreateContentBox(
                this.Window,
                "Pages",
                this.Sprites[AssetNames.Page],
                ArchipelagoWindowUI.PAGE_WIDTH,
                ArchipelagoWindowUI.PAGE_HEIGHT
            );

            VerticalLayoutGroup VerticalLayoutGroup = Pages.AddComponent<VerticalLayoutGroup>();
            VerticalLayoutGroup.padding = new RectOffset(ArchipelagoWindowUI.PAGE_PADDING_SIDE, ArchipelagoWindowUI.PAGE_PADDING_SIDE, ArchipelagoWindowUI.PAGE_PADDING_SIDE, ArchipelagoWindowUI.PAGE_PADDING_SIDE);
            VerticalLayoutGroup.spacing = 10f;
            VerticalLayoutGroup.childControlWidth = true;
            VerticalLayoutGroup.childScaleWidth = true;
            VerticalLayoutGroup.childForceExpandWidth = true;
            VerticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            Helper.Debug($"[ArchiWindow::BuildPages] Pages");

            this.Pages[PageNames.Challenges] = this.BuildChallengesContent(WindowHelper.CreateBlankPage(Pages.transform, "Challenges"));
            this.Pages[PageNames.EnergyLink] = this.BuildEnergyLinkContent(WindowHelper.CreateBlankPage(Pages.transform, "EnergyLink"));
        }

        private GameObject BuildChallengesContent(GameObject rootGameObject)
        {
            Helper.Debug($"[ArchiWindow::BuildChallengesContent]");
            RectTransform root = rootGameObject.GetComponent<RectTransform>();
            WindowHelper.AddLabel(root, "Title", "Complete Challenges to unlock items:");
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] AddLabel - Title");

            int padding = ArchipelagoWindowUI.PAGE_PADDING_SIDE * 2;

            // List
            GameObject List = WindowHelper.CreateContentBox(
                root,
                "List",
                this.Sprites[AssetNames.ChallengesList],
                ArchipelagoWindowUI.PAGE_WIDTH - padding,
                (ArchipelagoWindowUI.PAGE_HEIGHT - padding) / 2
            );
            // update color to be dark
            Image backgroundImage = List.GetComponent<Image>();
            backgroundImage.color = Colors.ConvertFromHex(Colors.DarkGrey);
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] List");

            VerticalLayoutGroup VerticalLayoutGroup = List.AddComponent<VerticalLayoutGroup>();
            VerticalLayoutGroup.padding = new RectOffset(8, 8, 8, 8);
            VerticalLayoutGroup.spacing = 10f;
            VerticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            VerticalLayoutGroup.childControlWidth = true;
            VerticalLayoutGroup.childControlHeight = true;
            VerticalLayoutGroup.childScaleWidth = true;
            VerticalLayoutGroup.childScaleHeight = false;
            VerticalLayoutGroup.childForceExpandWidth = true;
            VerticalLayoutGroup.childForceExpandHeight = true;

            // Create all 3 Challenges
            RectTransform ListRectTransform = List.GetComponent<RectTransform>();
            this.CreateChallengeContent(ListRectTransform, "Challenge 1");
            this.CreateChallengeContent(ListRectTransform, "Challenge 2");
            this.CreateChallengeContent(ListRectTransform, "Challenge 3");

            return rootGameObject;
        }

        private GameObject CreateChallengeContent(RectTransform parent, string name)
        {
            // Testing
            Challenge ch = new Challenge(this.ParkitectController);
            ch.SetAttraction(Prefabs.WoodenCoaster.ToString(), 1);
            ch.AddGuestsRating(1000);
            ch.AddRevenueRating(100f);
            ch.AddProfitRating(1000f);
            ch.AddExcitement(50f);
            ch.AddIntensity(50f);
            ch.AddNausea(50f);
            ch.AddVoucher(100);
            ch.AddSatisfaction(50f);
            ch.AddDecoRating(Constants.Attraction.DecoRatings[1]);
            ch.AddPhoto(100);
            ch.SetLocationId(-1);

            Helper.Debug($"[ArchiWindow::CreateChallengeContent] {name}");
            // config
            float textHeight = 14f;
            float spacing = 10f; // for our horizontal
            float height = (3 * textHeight) + spacing;
            int padding = ArchipelagoWindowUI.PAGE_PADDING_SIDE * 2;

            // Challenge Box
            GameObject Challenge = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(LayoutElement),
                typeof(HorizontalLayoutGroup)
            });

            LayoutElement ChallengeLayoutElement = Challenge.GetComponent<LayoutElement>();
            ChallengeLayoutElement.minHeight = 42f;
            ChallengeLayoutElement.preferredHeight = 42f;

            HorizontalLayoutGroup ChallengeHorizontalLayoutGroup = Challenge.GetComponent<HorizontalLayoutGroup>();
            ChallengeHorizontalLayoutGroup.spacing = 12f;
            ChallengeHorizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            ChallengeHorizontalLayoutGroup.childForceExpandWidth = true;
            ChallengeHorizontalLayoutGroup.childForceExpandHeight = true;
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] Challenge");

            // Challenge button (with text and subtext)
            GameObject Button = WindowHelper.CreateUIObject("Button", Challenge.transform, new Type[]
            {
                typeof(RectTransform),
                typeof(Button),
                typeof(LayoutElement),
                typeof(VerticalLayoutGroup)
            });

            Button ButtonBtn = Button.GetComponent<Button>();
            ButtonBtn.enabled = true;
            ButtonBtn.interactable = true;

            float widthSkip = 20;
            float width = ArchipelagoWindowUI.PAGE_WIDTH - padding - widthSkip - spacing;

            LayoutElement ButtonLayoutElement = Button.GetComponent<LayoutElement>();
            ButtonLayoutElement.preferredHeight = height;
            ButtonLayoutElement.minWidth = width;

            VerticalLayoutGroup ButtonVerticalLayoutGroup = Button.GetComponent<VerticalLayoutGroup>();
            ButtonVerticalLayoutGroup.spacing = spacing;
            ButtonVerticalLayoutGroup.childControlWidth = false;
            ButtonVerticalLayoutGroup.childControlHeight = false;
            ButtonVerticalLayoutGroup.childForceExpandWidth = false;
            ButtonVerticalLayoutGroup.childForceExpandHeight = false;
            ButtonVerticalLayoutGroup.childScaleWidth = true;
            ButtonVerticalLayoutGroup.childScaleHeight = false;
            
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] ButtonBtn");
            // Challenge button text
            GameObject ButtonText = WindowHelper.AddLabel(Button.transform, "Text", ch.Text(), 11f, TextAlignmentOptions.TopLeft);
            //GameObject ButtonText = WindowHelper.AddLabel(Button.transform, "Text", "", 11f, TextAlignmentOptions.TopLeft);
            TextMeshProUGUI ButtonTextTextMeshProUGUI = ButtonText.GetComponent<TextMeshProUGUI>();
            ButtonTextTextMeshProUGUI.enableWordWrapping = true;
           
            LayoutElement ButtonTextLayoutElement = ButtonText.AddComponent<LayoutElement>();
            ButtonTextLayoutElement.flexibleHeight = 1f;

            Helper.Debug($"[ArchiWindow::BuildChallengesContent] ButtonText");
            // Challenge button subtext
            GameObject ButtonSubText = WindowHelper.AddLabel(Button.transform, "SubText", ch.SubText(), 10f, TextAlignmentOptions.TopLeft);
            //GameObject ButtonSubText = WindowHelper.AddLabel(Button.transform, "SubText", ""), 10f, TextAlignmentOptions.TopLeft);
            TextMeshProUGUI TextMeshProUGUI = ButtonSubText.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI.enableWordWrapping = true;

            LayoutElement ButtonSubTextLayoutElement = ButtonSubText.AddComponent<LayoutElement>();
            ButtonSubTextLayoutElement.flexibleHeight = 2f;

            Helper.Debug($"[ArchiWindow::BuildChallengesContent] ButtonSubText");
            // Challenge Skip
            GameObject Skip = WindowHelper.CreateContentBox(
                Challenge.transform,
                "Skip",
                this.Sprites[AssetNames.ButtonSkip],
                widthSkip,
                widthSkip,
                0f,
                0f
            );

            Button SkipButton = Skip.AddComponent<Button>();
            SkipButton.enabled = true;
            SkipButton.interactable = true;
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] Skip");

            // Challenge Skip Text
            WindowHelper.AddLabel(SkipButton.transform, "Text", ">>", 13f, TextAlignmentOptions.Midline);
            Helper.Debug($"[ArchiWindow::BuildChallengesContent] AddLabel - Skip Text");

            return Challenge;
        }

        private GameObject BuildEnergyLinkContent(GameObject rootGameObject)
        {
            RectTransform root = rootGameObject.GetComponent<RectTransform>();

            return rootGameObject;
        }
    }
}
