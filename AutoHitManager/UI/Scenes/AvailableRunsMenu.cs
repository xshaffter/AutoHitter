using AutoHitManager.Cat;
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
        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            Action<MenuSelectable> cancelAction = _ => {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            };
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
                        new Vector2(0f, -502f)
                    )
                ))
                .SetDefaultNavGraph(new GridNavGraph(1))
                .AddContent(
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c => {
                        foreach (var run in Global.GlobalSaveData.Runs)
                        {
                            c.AddMenuButton
                            (
                                run.Name,
                                new MenuButtonConfig
                                {
                                    Label = $"View {run.Name} run",
                                    SubmitAction = _ =>
                                    {
                                        Global.RunDetail = Global.GlobalSaveData.Runs.IndexOf(run);
                                        UIManager.instance.UIGoToDynamicMenu(RunDetailMenu.BuildMenu(menu));
                                    },
                                    CancelAction = cancelAction,
                                    Style = MenuButtonStyle.VanillaStyle
                                }
                            );
                        }
                        // should be guaranteed from `MenuBuilder.AddContent`
                        if (c.Layout is RegularGridLayout layout)
                        {
                            var l = layout.ItemAdvance;
                            l.x = new RelLength(750f);
                            layout.ChangeColumns(2, 0.5f, l, 0.5f);
                        }
                        GridNavGraph navGraph = c.NavGraph as GridNavGraph;
                        navGraph.ChangeColumns(2);
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
                                SubmitAction = cancelAction,
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
