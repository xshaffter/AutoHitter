using AutoHitManager.Cat;
using AutoHitManager.UI.Managers;
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
                    new RelVector2(new Vector2(1920f, 903f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -60f)
                    )
                ))
                .CreateControlPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 259f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -652f)
                    )
                ))
                .SetDefaultNavGraph(new GridNavGraph(1))
                .AddContent(
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c => {
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
                            if (Global.GlobalSaveData.Runs.Count > 0)
                            {
                                foreach (var run in Global.GlobalSaveData.Runs)
                                {
                                    var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                    rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
                                    scroll.AddMenuButton
                                    (
                                        run.Name,
                                        new MenuButtonConfig
                                        {
                                            Label = $"View {run.Name} run",
                                            SubmitAction = _ =>
                                            {
                                                if (buttonBehaviour == VIEW)
                                                {
                                                    Global.RunDetail = Global.GlobalSaveData.Runs.IndexOf(run);
                                                    UIManager.instance.UIGoToDynamicMenu(RunDetailMenu.BuildMenu(menu));
                                                }
                                                else
                                                {
                                                    KeyboardRunEdit.UsageButton(run);
                                                }
                                            },
                                            CancelAction = cancelAction,
                                            Style = MenuButtonStyle.VanillaStyle
                                        }
                                    );
                                }
                            }
                            else
                            {
                                var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
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
                            "Edit",
                            new MenuButtonConfig
                            {
                                Label = "Edit",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    buttonBehaviour = EDIT;
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                            }
                        ).AddMenuButton(
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
                            },
                            out var addButton
                        ).AddMenuButton(
                            "BackButton",
                            new MenuButtonConfig
                            {
                                Label = "Back",
                                CancelAction = cancelAction,
                                SubmitAction = cancelAction,
                                Style = MenuButtonStyle.VanillaStyle
                            },
                            out var backButton
                        );
                    }
                )
                .Build();
            return menu;
        }
    }
}
