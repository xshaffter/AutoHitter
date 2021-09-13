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
    public static class RunDetailMenu
    {
        public static MenuScreen BuildMenu()
        {
            var run = Global.GlobalSaveData.Runs[Global.RunDetail] ?? Global.GlobalSaveData.Runs.First();
            Global.Log(run);
            Action<MenuSelectable> cancelAction = _ => {
                UIManager.instance.UIGoToDynamicMenu(AutoHitMod.LoadedInstance.screen);
            };
            Action<MenuPreventDeselect> cancelAction2 = _ => {
                UIManager.instance.UIGoToDynamicMenu(AutoHitMod.LoadedInstance.screen);
            };
            return new MenuBuilder(UIManager.instance.UICanvas.gameObject, "RunDetailMenu")
                .CreateTitle(run.Name, MenuTitleStyle.vanillaStyle)
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
                    c =>
                    {
                        var content = c.AddScrollPaneContent(
                            new ScrollbarConfig
                            {
                                CancelAction = cancelAction2,
                                Navigation = Navigation.defaultNavigation,
                                Position = new AnchoredPosition
                                (
                                    new Vector2(0.5f, 0.5f),
                                    new Vector2(0.5f, 0.5f)
                                )
                            },
                            new RelLength(500),
                            RegularGridLayout.CreateVerticalLayout(105f),
                            scroll =>
                            {
                                if (run.History.Count > 0)
                                {
                                    foreach (var run in run.History)
                                    {
                                        scroll.AddMenuButton(
                                            $"Run #{run.number}",
                                            new MenuButtonConfig
                                            {
                                                Label = $"Run #{run.number}",
                                                CancelAction = cancelAction,
                                                SubmitAction = cancelAction,
                                                Style = MenuButtonStyle.VanillaStyle,
                                                Proceed = true
                                            }
                                        );
                                    }
                                }
                            }
                        );
                        
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
                    new SingleContentLayout(new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -64f)
                    )),
                    c => c.AddMenuButton(
                        "BackButton",
                        new MenuButtonConfig
                        {
                            Label = "Back",
                            CancelAction = cancelAction,
                            SubmitAction = cancelAction,
                            Style = MenuButtonStyle.VanillaStyle,
                            Proceed = true
                        },
                        out var backButton
                    )
                )
                .Build();
        }
    }
}
