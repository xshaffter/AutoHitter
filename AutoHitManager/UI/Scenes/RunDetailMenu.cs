using AutoHitManager.Cat;
using AutoHitManager.Managers;
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
    public static class RunDetailMenu
    {
        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            var run = Global.GlobalSaveData.Runs[Global.RunDetail];
            void cancelAction(MenuSelectable _)
            { 
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            }
            MenuScreen menu = null;
            menu = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "RunDetailMenu")
                .CreateTitle(run.Name, MenuTitleStyle.vanillaStyle)
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
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c =>
                    {
                        c.AddMenuButton(
                            "Splits",
                            new MenuButtonConfig
                            {
                                Label = "Manage Splits",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(menu));
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                                Proceed = true
                            }
                        ).AddTextPanel(
                            "History",
                            new RelVector2(new Vector2(315f, MenuButtonStyle.VanillaStyle.TextSize)),
                            new TextPanelConfig
                            {
                                Text = $"=History=",
                                Font = TextPanelConfig.TextFont.TrajanRegular,
                                Size = MenuButtonStyle.VanillaStyle.TextSize,
                                Anchor = TextAnchor.MiddleCenter
                            }
                        ).AddScrollPaneContent(new ScrollbarConfig
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
                            if (run.History != null && run.History.Count > 0)
                            {
                                foreach (var run in run.History)
                                {
                                    var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                    rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
                                    c.AddMenuButton(
                                        $"Run #{run.number}",
                                        new MenuButtonConfig
                                        {
                                            Label = $"Run #{run.number} with PB { run.Hits() }",
                                            CancelAction = cancelAction,
                                            SubmitAction = _ => 
                                            {
                                                Global.HistoryId = run.number;
                                                UIManager.instance.UIGoToDynamicMenu(HistoryMenu.BuildMenu(menu));
                                            },
                                            Style = MenuButtonStyle.VanillaStyle,
                                            Proceed = true
                                        }
                                    );
                                }
                            }
                            else
                            {
                                c.AddTextPanel(
                                    "No history",
                                    new RelVector2(new Vector2(315f, MenuButtonStyle.VanillaStyle.TextSize)),
                                    new TextPanelConfig
                                    {
                                        Text = $"There is no history available",
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
                            "ResetPB",
                            new MenuButtonConfig
                            {
                                Label = "Reset PB",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    run.PB = null;
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
                                Style = MenuButtonStyle.VanillaStyle,
                                Proceed = true
                            }
                        );
                    }
                )
                .Build();
            return menu;
        }
    }
}
