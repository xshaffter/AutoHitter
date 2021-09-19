using AutoHitManager.Cat;
using AutoHitManager.UI.Managers;
using MenuApiPlusPlus.Cat;
using MenuApiPlusPlus.Components;
using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AutoHitManager.UI.Scenes
{
    public static class AvailableRunsMenu
    {
        internal static MenuScreen PreviousScreen;
        internal static int buttonBehaviour;
        internal static int VIEW = 0;
        internal static int EDIT = 1;

        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            PreviousScreen = previousScreen;
            void cancelAction(MenuSelectable _)
            {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            }
            MenuScreen menu = null;
            menu = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "RunList")
                .CreateTitle("Runs", MenuTitleStyle.vanillaStyle)
                .CreateContentPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 603)),
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
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c => {
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
                            ChildAnchor = new Vector2(0.5f, 1f),
                            ParentAnchor = new Vector2(0.5f, 1f),
                            Offset = new Vector2(-200, 0)
                        }, new RelVector2(new Vector2(-200f, -105)), 1),
                        scroll =>
                        {
                            if (Global.GlobalSaveData.Runs.Count > 0)
                            {
                                var inputs = new List<TextInputConfig>();
                                foreach (var run in Global.GlobalSaveData.Runs)
                                {
                                    var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                    rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
                                    var config = new TextInputConfig
                                    {
                                        Text = $"View {run.Name} run",
                                        submitAction = _ =>
                                        {
                                            Global.RunDetail = Global.GlobalSaveData.Runs.IndexOf(run);
                                            UIManager.instance.UIGoToDynamicMenu(RunDetailMenu.BuildMenu(menu));
                                        },
                                        PostEnterAction = config =>
                                        {
                                            run.Name = config.Text;
                                            config.Text = $"View {run.Name} run";
                                        }
                                    };
                                    inputs.Add(config);
                                    scroll.AddTextInput(
                                        run.Name,
                                        config
                                    );
                                }
                                scroll.AddCustomPaneContent(
                                    new RelLength(0f),
                                    new RegularGridLayout(new AnchoredPosition
                                    {
                                        ChildAnchor = new Vector2(0f, 1f),
                                        ParentAnchor = new Vector2(0f, 1f),
                                        Offset = new Vector2(300f, 0)
                                    }, new RelVector2(new Vector2(175f, -105)), 2),
                                    pane =>
                                    {
                                        foreach (var run in Global.GlobalSaveData.Runs)
                                        {
                                            var config = inputs.Find(cfg => cfg.Text == $"View {run.Name} run");
                                            int index = Global.GlobalSaveData.Runs.IndexOf(run);
                                            var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                            rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 70f);
                                            pane.AddMenuButton(
                                                "Edit",
                                                new MenuButtonConfig
                                                {
                                                    Label = "Edit",
                                                    CancelAction = cancelAction,
                                                    SubmitAction = _ =>
                                                    {
                                                        if (!config.IsFocused)
                                                        {
                                                            scroll.ResetFocus();
                                                            config.Focus();
                                                        }
                                                    },
                                                    Style = MenuButtonStyle.VanillaStyle,
                                                }
                                            ).AddMenuButton(
                                                $"{run.Name}_delete",
                                                new MenuButtonConfig
                                                {
                                                    Label = "Delete",
                                                    CancelAction = cancelAction,
                                                    Style = MenuButtonStyle.VanillaStyle,
                                                    SubmitAction = btn =>
                                                    {
                                                        Global.GlobalSaveData.Runs.Remove(run);
                                                        UIManager.instance.UIGoToDynamicMenu(AvailableRunsMenu.BuildMenu(previousScreen));
                                                    },
                                                    Proceed = true
                                                }
                                            );
                                        }
                                    }
                                );
                            }
                            else
                            {
                                var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 80f);
                                scroll.AddTextPanel
                                (
                                    "No Runs",
                                    new RelVector2(new Vector2(500f, MenuButtonStyle.VanillaStyle.TextSize)),
                                    new TextPanelConfig
                                    {
                                        Text = $"There is no runs available",
                                        Font = TextPanelConfig.TextFont.TrajanRegular,
                                        Size = MenuButtonStyle.VanillaStyle.TextSize,
                                        Anchor = TextAnchor.MiddleCenter
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
                            "AddButton",
                            new MenuButtonConfig
                            {
                                Label = "add Run",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    Global.GlobalSaveData.Runs.Add(new RunConfig
                                    {
                                        Name = "Dummy",
                                        Splits = new List<SplitConfig> { new SplitConfig("Dummy split") }
                                    });
                                    UIManager.instance.UIGoToDynamicMenu(AvailableRunsMenu.BuildMenu(PreviousScreen));
                                },
                                Style = MenuButtonStyle.VanillaStyle
                            }
                        ).AddMenuButton(
                            "BackButton",
                            new MenuButtonConfig
                            {
                                Label = "Back",
                                CancelAction = cancelAction,
                                SubmitAction = cancelAction,
                                Style = MenuButtonStyle.VanillaStyle
                            }
                        );
                    }
                )
                .Build();
            return menu;
        }
    }
}
