using Parkitect.UI;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoMod.Src.Window
{
    internal class WindowHelper
    {
        public static Sprite DefaultPanelSprite = null;
        
        private static TMP_FontAsset DefaultFont = null;
        private static float DefaultFontSize = 0f;
        private static FontStyles DefaultFontStyle = FontStyles.Normal;
        
        private static Color TextMeshProUGUIColor = Colors.ConvertFromHex(Colors.Font);
        private static readonly Color ButtonColor = Colors.ConvertFromHex(Colors.LightGrey);
        private static readonly Color ButtonHighlightColor = Colors.ConvertFromHex(Colors.LightGrey);
        private static readonly Color ButtonPressedColor = Colors.ConvertFromHex(Colors.DarkGrey);

        public static void Prepare()
        {
            WindowHelper.TrySetLabelStyle();
            WindowHelper.TrySetPanelSprite();
        }

        public static bool TryCloneInactive<T>(Func<UIAssetManager, T> pick, out T wrapper) where T : Component
        {
            wrapper = default(T);
            bool result;
            try
            {
                UIAssetManager instance = ScriptableSingleton<UIAssetManager>.Instance;
                if (instance == null)
                {
                    result = false;
                }
                else
                {
                    T t = pick(instance);
                    if (t == null)
                    {
                        result = false;
                    }
                    else
                    {
                        GameObject gameObject = t.gameObject;
                        bool activeSelf = gameObject.activeSelf;
                        gameObject.SetActive(false);
                        try
                        {
                            wrapper = UnityEngine.Object.Instantiate<T>(t);
                        }
                        finally
                        {
                            gameObject.SetActive(activeSelf);
                        }
                        wrapper.gameObject.SetActive(false);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                wrapper = default(T);
                result = false;
            }
            return result;
        }

        public static bool TrySetLabelStyle()
        {
            if (WindowHelper.DefaultFont != null)
            {
                return true;
            }

            AttractionSettingDropdownUI attractionSettingDropdownUI;
            if (!WindowHelper.TryCloneInactive<AttractionSettingDropdownUI>((UIAssetManager m) => m.attractionSettingDropdownUIGO, out attractionSettingDropdownUI))
            {
                return false;
            }

            string label = "__archipelago_label__";
            try
            {
                attractionSettingDropdownUI.setLabelText(label);
                foreach (TMP_Text tmp_Text in attractionSettingDropdownUI.GetComponentsInChildren<TMP_Text>(true))
                {
                    if (tmp_Text != null && tmp_Text.text == label)
                    {
                        WindowHelper.DefaultFont = tmp_Text.font;
                        WindowHelper.DefaultFontSize = tmp_Text.fontSize;
                        WindowHelper.DefaultFontStyle = tmp_Text.fontStyle;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WindowHelper.DefaultFont = null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(attractionSettingDropdownUI.gameObject);
            }

            return true;
        }

        public static bool TrySetPanelSprite()
        {
            if (WindowHelper.DefaultPanelSprite != null)
            {
                return true;
            }

            AttractionSettingDropdownUI attractionSettingDropdownUI;
            if (!WindowHelper.TryCloneInactive<AttractionSettingDropdownUI>((UIAssetManager m) => m.attractionSettingDropdownUIGO, out attractionSettingDropdownUI))
            {
                return false;
            }

            try
            {
                foreach (Image image in attractionSettingDropdownUI.GetComponentsInChildren<Image>(true))
                {
                    if (image != null && image.sprite != null)
                    {
                        WindowHelper.DefaultPanelSprite = image.sprite;
                        break;
                    }
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(attractionSettingDropdownUI.gameObject);
            }


            return true;
        }

        public static GameObject CreateUIObject(string name, Transform parent, Type[] types)
        {
            GameObject gameObject = new GameObject(name, types);
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        public static GameObject CreateContentBox(Transform parent, string name, Sprite backgroundSprite, float width, float height, float flexibleWidth = 1f, float flexibleHeight = 1f)
        {
            GameObject gameObject = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(Image),
                typeof(LayoutElement)
            });

            LayoutElement LayoutElement = gameObject.GetComponent<LayoutElement>();
            LayoutElement.minWidth = width;
            LayoutElement.minHeight = height;
            LayoutElement.flexibleWidth = flexibleWidth;
            LayoutElement.flexibleHeight = flexibleHeight;

            // background
            Image Image = gameObject.GetComponent<Image>();
            Image.sprite = backgroundSprite;
            Image.raycastTarget = false;
            Image.maskable = true;
            Image.type = Image.Type.Sliced;
            Image.color = Colors.ConvertFromHex(Colors.LightGrey);
            Image.fillCenter = true;

            return gameObject;
        }


        public static Button CreateMenuButton(Transform parent, string name, Sprite backgroundSprite, Sprite sprite, Action onClick)
        {
            float width = sprite.textureRect.width / 4;
            float height = sprite.textureRect.height / 4;
            float padding = 12f;

            float totalSize = width + padding;

            // root of Button
            GameObject MenuBtn = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(LayoutElement),
            });

            LayoutElement layoutElement = MenuBtn.GetComponent<LayoutElement>();
            layoutElement.minWidth = totalSize;
            layoutElement.preferredWidth = totalSize;
            layoutElement.minHeight = totalSize;
            layoutElement.preferredHeight = totalSize;
            layoutElement.flexibleWidth = 0f;
            layoutElement.flexibleHeight = 0f;

            // background
            Image btnMenuBackground = MenuBtn.GetComponent<Image>();
            btnMenuBackground.sprite = backgroundSprite;
            btnMenuBackground.raycastTarget = false;
            btnMenuBackground.maskable = true;
            btnMenuBackground.type = Image.Type.Sliced;
            btnMenuBackground.color = Colors.ConvertFromHex(Colors.LightGrey);
            btnMenuBackground.fillCenter = true;

            // image/sprite
            GameObject gameObjectSprite = WindowHelper.CreateUIObject(name + "Sprite", MenuBtn.transform, new Type[]
            {
                typeof(RectTransform),
                typeof(Image),
            });

            // sprite size and position
            RectTransform spriteRectTransform = gameObjectSprite.GetComponent<RectTransform>();
            spriteRectTransform.sizeDelta = new Vector2(width, height);
            spriteRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            spriteRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            spriteRectTransform.pivot = new Vector2(0.5f, 0.5f);
            spriteRectTransform.anchoredPosition = Vector2.zero;

            Image spriteImage = gameObjectSprite.GetComponent<Image>();
            spriteImage.sprite = sprite;
            spriteImage.raycastTarget = false;
            spriteImage.maskable = true;
            spriteImage.type = Image.Type.Simple;
            spriteImage.color = Color.white;

            // button
            Button button = MenuBtn.GetComponent<Button>();
            button.targetGraphic = btnMenuBackground;
            button.onClick.AddListener(() => { onClick(); });

            return button;
        }

        //public static Button CreateThemedButton(Transform parent, string label, float width, float height, Action onClick, float fontSize = 14f)
        //{
        //    GameObject gameObject = WindowHelper.CreateUIObject("Btn", parent, new Type[]
        //    {
        //        typeof(RectTransform),
        //        typeof(Image),
        //        typeof(Button),
        //        typeof(LayoutElement)
        //    });
        //    RectTransform rectTransform = (RectTransform)gameObject.transform;
        //    rectTransform.sizeDelta = new Vector2(width, height);
        //    LayoutElement component = gameObject.GetComponent<LayoutElement>();
        //    component.preferredWidth = width;
        //    component.minWidth = width;
        //    component.preferredHeight = height;
        //    component.minHeight = height;
        //    component.flexibleWidth = 0f;
        //    Image component2 = gameObject.GetComponent<Image>();
        //    component2.color = Colors.ConvertFromHex(Colors.LightGrey);
        //    GameObject gameObject2 = WindowHelper.CreateUIObject("TextMeshProUGUI", gameObject.transform, new Type[]
        //    {
        //        typeof(RectTransform)
        //    });
        //    RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
        //    rectTransform2.anchorMin = Vector2.zero;
        //    rectTransform2.anchorMax = Vector2.one;
        //    rectTransform2.offsetMin = Vector2.zero;
        //    rectTransform2.offsetMax = Vector2.zero;

            //    TMP_FontAsset font = null;
            //    float fontSize2 = 14f;
            //    FontStyles fontStyle2 = new FontStyles();

            //    if (WindowHelper.TryGetLabelStyle(out font, out fontSize2, out fontStyle2))
            //    {
            //        TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
            //        textMeshProUGUI.font = font;
            //        textMeshProUGUI.fontSize = fontSize2;
            //        textMeshProUGUI.fontStyle = fontStyle2;
            //        textMeshProUGUI.color = Colors.ConvertFromHex(Colors.DarkGrey);
            //        textMeshProUGUI.alignment = TextAlignmentOptions.Midline;
            //        textMeshProUGUI.enableWordWrapping = false;
            //        textMeshProUGUI.overflowMode = 0;
            //        textMeshProUGUI.raycastTarget = false;
            //        textMeshProUGUI.text = label;
            //    }
            //    else
            //    {
            //        TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
            //        textMeshProUGUI.font = WindowHelper.DefaultFont;
            //        textMeshProUGUI.fontSize = Mathf.RoundToInt((fontSize > 0f) ? fontSize : 14f);
            //        textMeshProUGUI.color = WindowHelper.TextMeshProUGUIColor;
            //        textMeshProUGUI.alignment = TextAlignmentOptions.Midline;
            //        textMeshProUGUI.raycastTarget = false;
            //        textMeshProUGUI.text = label;
            //    }

            //    Button button = gameObject.GetComponent<Button>();
            //    button.targetGraphic = component2;
            //    ColorBlock colors = button.colors;
            //    colors.normalColor = WindowHelper.ButtonColor;
            //    colors.highlightedColor = WindowHelper.ButtonHighlightColor;
            //    colors.pressedColor = WindowHelper.ButtonPressedColor;
            //    colors.selectedColor = WindowHelper.ButtonColor;
            //    colors.fadeDuration = 0.1f;
            //    button.colors = colors;
            //    button.onClick.AddListener(() => { onClick(); });
            //    return button;
            //}

        public static GameObject AddLabel(Transform parent, string name, string text, float fontSize = 12f, TextAlignmentOptions alignment = TextAlignmentOptions.MidlineLeft)
        {
            GameObject gameObject = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            });

            TextMeshProUGUI TextMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI.font = WindowHelper.DefaultFont;
            TextMeshProUGUI.fontSize = (int)fontSize;
            TextMeshProUGUI.color = WindowHelper.TextMeshProUGUIColor;
            TextMeshProUGUI.alignment = alignment;
            TextMeshProUGUI.text = text;
            TextMeshProUGUI.enableWordWrapping = false;
            TextMeshProUGUI.overflowMode = TextOverflowModes.Overflow;

            return gameObject;
        }

        public static Sprite CreateSpriteFrom(string filename, Vector4 border = default)
        {
            if (!filename.Contains(".png"))
            {
                filename += ".png";
            }

            string imagePath = System.IO.Path.Combine(Constants.ModAssetSrc, filename);

            if (File.Exists(imagePath))
            {
                byte[] fileData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

                if (texture.LoadImage(fileData))
                {
                    if (border != default)
                    {
                        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
                    }

                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }

            return null;
        }

        public static GameObject CreateBlankPage(Transform parent, string name)
        {
            GameObject gameObject = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(VerticalLayoutGroup)
            });

            VerticalLayoutGroup VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
            VerticalLayoutGroup.spacing = 10f;
            VerticalLayoutGroup.childControlWidth = true;
            VerticalLayoutGroup.childForceExpandWidth = true;

            return gameObject;
        }

        public static Transform BuildColumn(Transform parent, string name, float width)
        {
            GameObject gameObject = WindowHelper.CreateUIObject(name, parent, new Type[]
            {
                typeof(RectTransform),
                typeof(VerticalLayoutGroup),
                typeof(LayoutElement)
            });

            VerticalLayoutGroup component = gameObject.GetComponent<VerticalLayoutGroup>();
            component.spacing = 4f;
            component.childAlignment = 0;
            component.childControlWidth = true;
            component.childControlHeight = true;
            component.childForceExpandWidth = true;
            component.childForceExpandHeight = false;
            
            LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
            //component2.minWidth = width;
            //component2.preferredWidth = width;
            //component2.flexibleWidth = 0f;
            component2.flexibleWidth = 1f;
            
            return gameObject.transform;
        }
    }
}
