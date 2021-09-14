using AutoHitManager.Cat;
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
    public static class SplitManagerMenu
    {

        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            var run = Global.GlobalSaveData.Runs[Global.RunDetail];
            Action<MenuSelectable> cancelAction = _ => {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            };
            Action<MenuPreventDeselect> cancelAction2 = _ =>
            {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            };
            return new MenuBuilder(UIManager.instance.UICanvas.gameObject, "AutoHitSettings")
                .CreateTitle("Config", MenuTitleStyle.vanillaStyle)
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
                    RegularGridLayout.CreateVerticalLayout(90f),
                    c =>
                    {
                        c.AddScrollPaneContent(new ScrollbarConfig
                         {
                             CancelAction = cancelAction2,
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
                                c.AddMenuButton(
                                    split.Name,
                                    new MenuButtonConfig
                                    {
                                        Label = split.Name,
                                        CancelAction = cancelAction,
                                        Style = MenuButtonStyle.VanillaStyle,
                                        SubmitAction = _ => { },
                                        Proceed = true
                                    }
                                );
                            }
                        });
                        // should be guaranteed from `MenuBuilder.AddContent`
                        if (c.Layout is RegularGridLayout layout)
                        {
                            var l = layout.ItemAdvance;
                            l.x = new RelLength(300f);
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
