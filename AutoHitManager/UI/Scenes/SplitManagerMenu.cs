using AutoHitManager.Cat;
using AutoHitManager.UI.Managers;
using Modding.Menu;
using Modding.Menu.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AutoHitManager.UI.Scenes
{
    public static class SplitManagerMenu
    {
        public const int RENAME = 0;
        public const int BEFORE = 1;
        public const int AFTER = 2;
        public const int DELETE = 3;
        internal static int buttonBehaviour = RENAME;
        internal static MenuScreen PreviousScreen;


        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            PreviousScreen = previousScreen;
            var run = Global.GlobalSaveData.Runs[Global.RunDetail];
            void cancelAction(MenuSelectable _)
            {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            }
            var _menu = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "AutoHitSettings")
                .CreateTitle("Config", MenuTitleStyle.vanillaStyle)
                .CreateContentPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 603f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, 60f)
                    )
                ))
                .CreateControlPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 559f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -602f)
                    )
                ))
                .SetDefaultNavGraph(new GridNavGraph(1))
                .AddContent(
                    RegularGridLayout.CreateVerticalLayout(90f),
                    c =>
                    {
                        c.AddScrollPaneContent(new ScrollbarConfig
                         {
                             CancelAction = _ => { },
                             Navigation = new Navigation { mode = Navigation.Mode.Explicit },
                             Position = new AnchoredPosition
                             {
                                 ChildAnchor = new Vector2(0f, 1f),
                                 ParentAnchor = new Vector2(1f, 1f),
                                 Offset = new Vector2(-310f, 0f)
                             },
                             SelectionPadding = _ => (-60, 0)
                         },
                        new RelLength(0f),
                        RegularGridLayout.CreateVerticalLayout(105f),
                        scroll =>
                        {
                            foreach (var split in run.Splits)
                            {
                                var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
                                scroll.AddMenuButton(
                                    split.Name,
                                    new MenuButtonConfig
                                    {
                                        Label = split.Name,
                                        CancelAction = cancelAction,
                                        Style = MenuButtonStyle.VanillaStyle,
                                        SubmitAction = btn => KeyboardManager.UsageButton(run, split),
                                        Proceed = true
                                    }
                                );
                            }
                        });
                    }
                )
                .AddControls(
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c =>
                    {
                        c.AddMenuButton(
                            "SplitBefore",
                            new MenuButtonConfig
                            {
                                Label = "Add Split Before",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    buttonBehaviour = BEFORE;
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                            }
                        ).AddMenuButton(
                            "SplitAfter",
                            new MenuButtonConfig
                            {
                                Label = "Add Split After",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    buttonBehaviour = AFTER;
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                            }
                        ).AddMenuButton(
                            "SplitDelete",
                            new MenuButtonConfig
                            {
                                Label = "Delete Split",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    buttonBehaviour = DELETE;
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                            }
                        ).AddMenuButton(
                            "BackButton",
                            new MenuButtonConfig
                            {
                                Label = "Back",
                                CancelAction = cancelAction,
                                SubmitAction = cancelAction,
                                Style = MenuButtonStyle.VanillaStyle,
                                Proceed = true
                            }
                        );
                    }
                )
                .Build();
            return _menu;
        }
    }
}
