using GlobalEnums;
using MenuApiPlusPlus.Behaviours;
using MenuApiPlusPlus.Cat;
using Modding.Menu;
using Modding.Menu.Config;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MenuButton = Modding.Patches.MenuButton;

namespace MenuApiPlusPlus.Components
{
    public static class TextInputContent
    {
        public static List<TextInputConfig> InputsLoaded = new List<TextInputConfig>();
        public static ContentArea AddTextInput(this ContentArea content, string name, TextInputConfig config) => content.AddTextInput(name, config, out _);

        public static ContentArea AddTextInput(this ContentArea content, string name, TextInputConfig config, out TextInput input)
        {
            var resetter = content.ContentObject.GetComponent<FocusResetter>();
            if (resetter == null)
            {
                resetter = content.ContentObject.AddComponent<FocusResetter>();
            }
            resetter.content = content;

            var style = MenuButtonStyle.VanillaStyle;
            var option = new GameObject();
            GameObject.DontDestroyOnLoad(option);
            option.transform.SetParent(content.ContentObject.transform, false);
            option.AddComponent<CanvasRenderer>();
            var optionRt = option.AddComponent<RectTransform>();
            new RelVector2(new RelLength(0f, 1f), style.Height).GetBaseTransformData().Apply(optionRt);
            content.Layout.ModifyNext(optionRt);

            input = option.AddComponent<TextInput>();

            Action<MenuButton> defaultSubmit = _ =>
            {
                if (!config.IsFocused)
                {
                    content.ResetFocus();
                    config.RequestFocus();
                }
            };

            InputsLoaded.Add(config);


            var submitAction = config.submitAction;
            if (submitAction == null)
            {
                submitAction = defaultSubmit;
            }

            input.buttonType = (UnityEngine.UI.MenuButton.MenuButtonType)MenuButton.MenuButtonType.CustomSubmit;
            input.submitAction = submitAction;
            input.cancelAction = CancelAction.DoNothing;
            input.proceed = false;
            var label = new GameObject("Label");
            GameObject.DontDestroyOnLoad(label);
            label.transform.SetParent(option.transform, false);
            // CanvasRenderer
            label.AddComponent<CanvasRenderer>();

            var labelRt = label.AddComponent<RectTransform>();
            labelRt.sizeDelta = new Vector2(0f, 0f);
            labelRt.pivot = new Vector2(0.5f, 0.5f);
            labelRt.anchorMin = new Vector2(0f, 0f);
            labelRt.anchorMax = new Vector2(1f, 1f);
            labelRt.anchoredPosition = new Vector2(0f, 0f);

            var labelText = label.AddComponent<Text>();
            labelText.font = MenuResources.TrajanBold;
            labelText.fontSize = style.TextSize;
            labelText.resizeTextMaxSize = style.TextSize;
            labelText.alignment = TextAnchor.LowerLeft;
            labelText.text = config._text;
            labelText.supportRichText = true;
            labelText.verticalOverflow = VerticalWrapMode.Overflow;
            labelText.horizontalOverflow = HorizontalWrapMode.Overflow;
            config.TextLabel = labelText;
            config.OldText = config._text;


            // LeftCursor object
            var textCursor = new GameObject("TextCursor");
            GameObject.DontDestroyOnLoad(textCursor);
            textCursor.transform.SetParent(label.transform, false);
            // CanvasRenderer
            textCursor.AddComponent<CanvasRenderer>();
            // RectTransform
            var cursorTransform = textCursor.AddComponent<RectTransform>();
            cursorTransform.sizeDelta = new Vector2(164f, 119f);
            cursorTransform.pivot = new Vector2(0.5f, 0.5f);
            cursorTransform.anchorMin = new Vector2(0f, 0.5f);
            cursorTransform.anchorMax = new Vector2(0f, 0.5f);
            cursorTransform.anchoredPosition = new Vector2(-65f, 0f);
            cursorTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Animator
            var cursorAnimation = textCursor.AddComponent<Animator>();
            cursorAnimation.runtimeAnimatorController = MenuResources.MenuCursorAnimator;
            cursorAnimation.updateMode = AnimatorUpdateMode.UnscaledTime;
            cursorAnimation.applyRootMotion = false;
            // Image
            textCursor.AddComponent<Image>();
            // Post Component Config
            input.selectHighlight = cursorAnimation;

            label.AddComponent<FixVerticalAlign>();

            var labelCsf = label.AddComponent<ContentSizeFitter>();
            labelCsf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            content.NavGraph.AddNavigationNode(input);

            return content;
        }

        public static void ResetFocus(this ContentArea content)
        {
            foreach (var input in InputsLoaded)
            {
                if (input.IsFocused)
                {
                    input.UnFocus();
                }
            }
        }

        public static void RequestFocus(this TextInputConfig config)
        {
            if (!config.IsFocused)
            {
                config.Focus();
            }
        }
    }
}
