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
using MenuApiPlusPlus.Components;
using MenuApiPlusPlus.Cat;

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
                    RegularGridLayout.CreateVerticalLayout(70f),
                    c =>
                    {
                        c.AddScrollPaneContent(new ScrollbarConfig
                         {
                             CancelAction = _ => { },
                             Navigation = new Navigation { mode = Navigation.Mode.Vertical },
                             Position = new AnchoredPosition
                             {
                                 ChildAnchor = new Vector2(0f, 1f),
                                 ParentAnchor = new Vector2(1f, 1f),
                                 Offset = new Vector2(-310f, 0f)
                             },
                             SelectionPadding = _ => (-60, 0)
                         },
                        new RelLength(0f),
                        new RegularGridLayout(new AnchoredPosition
                        {
                            ChildAnchor = new Vector2(0f, 1f),
                            ParentAnchor = new Vector2(0f, 1f),
                            Offset = new Vector2(-150, 0)
                        }, new RelVector2(new Vector2(-150f, -105)), 1),
                        scroll =>
                        {
                            foreach (var split in run.Splits)
                            {
                                int index = run.Splits.IndexOf(split);
                                var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                rt.sizeDelta = new Vector2(-150f, rt.sizeDelta.y + 70f);
                                TextInputConfig config = null;
                                config = new TextInputConfig
                                {
                                    Text = split.Name,
                                    submitAction = _ =>
                                    {
                                        if (!config.IsFocused)
                                        {
                                            scroll.ResetFocus();
                                            config.RequestFocus();
                                        }
                                    },
                                    PostEnterAction = config =>
                                    {
                                        split.Name = config.Text;
                                    }
                                };
                                scroll.AddTextInput(
                                    split.Name,
                                    config
                                );
                            }
                            scroll.AddCustomPaneContent(
                                new RelLength(0f),
                                new RegularGridLayout(new AnchoredPosition
                                {
                                    ChildAnchor = new Vector2(0f, 1f),
                                    ParentAnchor = new Vector2(0f, 1f),
                                    Offset = new Vector2(350f, 0)
                                }, new RelVector2(new Vector2(157f, -105)), 2),
                                pane =>
                                {
                                    foreach (var split in run.Splits)
                                    {
                                        int index = run.Splits.IndexOf(split);
                                        var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                        rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 70f);
                                        pane.AddMenuButton(
                                            $"{split.Name}_delete",
                                            new MenuButtonConfig
                                            {
                                                Label = "Delete",
                                                CancelAction = cancelAction,
                                                Style = MenuButtonStyle.VanillaStyle,
                                                SubmitAction = btn =>
                                                {
                                                    run.Splits.Remove(split);
                                                    UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(previousScreen));
                                                },
                                                Proceed = true
                                            }
                                        ).AddMenuButton(
                                        $"{split.Name}_add",
                                        new MenuButtonConfig
                                        {
                                            Label = "Add",
                                            CancelAction = cancelAction,
                                            Style = MenuButtonStyle.VanillaStyle,
                                            SubmitAction = btn =>
                                            {
                                                var newSplit = new SplitConfig("Dummy");
                                                run.Splits.Insert(index + 1, newSplit);
                                                UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(previousScreen));
                                            },
                                            Proceed = true
                                        });
                                    }
                                }
                            );
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
                                Label = "Add Split First",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    run.Splits.Insert(0, new SplitConfig("Dummy"));
                                    UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(previousScreen));
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
