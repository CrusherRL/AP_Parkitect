using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoMod.Src.Window.Scripts
{
    public class TabController : MonoBehaviour
    {
        protected readonly List<Button> MenuButtons = new List<Button>();
        protected readonly List<Image> MenuButtonImages = new List<Image>();
        protected readonly List<GameObject> Pages = new List<GameObject>();

        public event Action<int> OnSwitch;

        public void SwitchTab(int tabIndex)
        {
            for (int i = 0; i < this.MenuButtons.Count; i++)
            {
                this.Pages[i].SetActive(false);
                this.MenuButtonImages[i].color = Colors.ConvertFromHex("#555555");
                this.OnSwitch?.Invoke(i);
            }

            this.Pages[tabIndex].SetActive(true);
            this.MenuButtonImages[tabIndex].color = Colors.ConvertFromHex("#666666");
        }

        public void AddPage(int index, GameObject page)
        {
            this.Pages.Insert(index, page);
        }

        public void AddMenuButton(int index, Button button)
        {
            this.MenuButtons.Insert(index, button);

            button.onClick.AddListener(() => { this.SwitchTab(index); });
        }

        public void AddMenuButtonImage(int index, Image image)
        {
            this.MenuButtonImages.Insert(index, image);
        }
    }
}
