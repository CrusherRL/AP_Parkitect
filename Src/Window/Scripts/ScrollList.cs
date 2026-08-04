using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoMod.Src.Window.Scripts
{
    class ScrollList : MonoBehaviour
    {
        [SerializeField] private ScrollRect ScrollRect;
        [SerializeField] private GameObject ItemPrefab;

        void OnStart()
        {
            this.ScrollRect = transform.GetComponent<ScrollRect>();
            this.ItemPrefab = ScrollRect.content.Find("Item").gameObject;
            this.ItemPrefab.SetActive(false);
        }

        public void AddItem(string message)
        {
            this.OnStart();
            GameObject item = Object.Instantiate(this.ItemPrefab, this.ScrollRect.content);

            item.GetComponentInChildren<TextMeshProUGUI>().text = message;
            item.SetActive(true);
        }
    }
}
