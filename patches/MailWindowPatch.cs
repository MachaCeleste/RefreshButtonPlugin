using HarmonyLib;
using RefreshButtonPlugin;
using UnityEngine;
using UnityEngine.UI;

[HarmonyPatch]
public class MailWindowPatch
{
    [HarmonyPatch(typeof(MailWindow), "Start")]
    class StartPatch
    {
        static void Postfix(MailWindow __instance)
        {
            var parent = __instance.transform.Find("Dialog/Container/Viewport/Content/BotonesPrincipal");
            var origButtonObj = parent.Find("Login");
            var buttonObj = UnityEngine.Object.Instantiate(origButtonObj, parent);
            buttonObj.name = "Refresh";
            var rect = buttonObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(616.3636f, -15.2001f);
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

        private static void OnRefreshClick(MailWindow mail)
        {
            mail.OnLogin();
        }
    }
}