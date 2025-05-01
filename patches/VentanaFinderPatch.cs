using HarmonyLib;
using RefreshButtonPlugin;
using UnityEngine;
using UnityEngine.UI;

[HarmonyPatch]
public class VentanaFinderPatch
{
    [HarmonyPatch(typeof(VentanaFinder), "Start")]
    class StartPatch
    {
        static void Postfix(VentanaFinder __instance)
        {
            var barRect = __instance.transform.parent.Find("BarraDir").GetComponent<RectTransform>();
            barRect.offsetMin = new Vector2(211.6046f, -38.0499f);
            var parent = __instance.transform.parent.Find("PanelButtons");
            var origButtonObj = parent.Find("ButtonHome");
            var buttonObj = UnityEngine.Object.Instantiate(origButtonObj, parent);
            buttonObj.name = "Refresh";
            var imageObj = buttonObj.Find("Image");
            var oldImage = imageObj.GetComponent<Image>();
            oldImage.sprite = DataUtils.LoadSprite("Refresh");
            var oldButton = buttonObj.GetComponent<Button>();
            var colors = oldButton.colors;
            UnityEngine.Object.DestroyImmediate(oldButton);
            var button = buttonObj.gameObject.AddComponent<Button>();
            button.colors = colors;
            button.targetGraphic = buttonObj.GetComponent<Image>();
            button.onClick.AddListener(() =>
            {
                OnRefreshClick(__instance);
            });
        }

        private static void OnRefreshClick(VentanaFinder finder)
        {
            finder.EntrarCarpeta(finder.GetCurrentFolder());
        }
    }
}