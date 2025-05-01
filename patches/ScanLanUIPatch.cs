using HarmonyLib;
using RefreshButtonPlugin;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[HarmonyPatch]
public class ScanLanUIPatch
{
    [HarmonyPatch(typeof(ScanLanUI), "Start")]
    class StartPatch
    {
        static void Postfix(ScanLanUI __instance)
        {
            var parent = __instance.transform.Find("Dialog/Container/Viewport/Content/Scroll View/Viewport");
            var oldButtonObj = parent.transform.Find("ButtonSubnets");
            var buttonObj = UnityEngine.Object.Instantiate(oldButtonObj, parent);
            buttonObj.name = "ButtonRefresh";
            var image = buttonObj.Find("Image").GetComponent<Image>();
            image.sprite = DataUtils.LoadSprite("Refresh");
            var buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(-165.52f, -40.5f);
            var toolTip = buttonObj.gameObject.GetComponent<Tooltip>();
            toolTip.texto = "Refresh UI";
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

        private static async Task OnRefreshClick(ScanLanUI scanLan)
        {
            FieldInfo subnetsField = AccessTools.Field(typeof(ScanLanUI), "subnetsEnabled");
            bool shouldEnable = (bool)subnetsField.GetValue(scanLan);
            if (shouldEnable) subnetsField.SetValue(scanLan, false);
            MethodInfo enableMethod = AccessTools.Method(typeof(ScanLanUI), "EnableButtons");
            enableMethod.Invoke(scanLan, new object[] { false });
            scanLan.loadingPanel.SetActive(true);
            MethodInfo clearMethod = AccessTools.Method(typeof(ScanLanUI), "ClearObjects");
            clearMethod.Invoke(scanLan, null);
            scanLan.StartCoroutine("StartDraw");
            await Task.Delay(200);
            if (shouldEnable) scanLan.OnClickLayerButton();
        }
    }

    [HarmonyPatch(typeof(ScanLanUI), "EnableButtons")]
    class EnableButtonsPatch
    {
        static void Postfix(ScanLanUI __instance, ref bool enable)
        {
            var refreshButton = __instance.transform.GetComponentsInChildren<Button>(true).FirstOrDefault(x => x.name == "ButtonRefresh");
            if (refreshButton != null) refreshButton.gameObject.SetActive(enable);
        }
    }
}