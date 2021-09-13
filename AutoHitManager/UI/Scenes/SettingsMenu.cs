using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoHitManager.Cat;
using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine;
using UnityEngine.UI;
using Patch = Modding.Patches;

namespace AutoHitManager.UI.Scenes
{
    public static class SettingsMenu
    {
        public static MenuScreen BuildMenu(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            var dels = toggleDelegates.Value;
            Action<MenuSelectable> cancelAction = _ => {
                UIManager.instance.UIGoToDynamicMenu(AutoHitMod.LoadedInstance.screen);
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
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c =>
                    {
                        c.AddKeybind(
                            "PrevSplit",
                            Global.GlobalSaveData.binds.prevSplit,
                            new KeybindConfig
                            {
                                Label = "Previous Split",
                                CancelAction = cancelAction
                            },
                            out var prevSplitKeybind
                        ).AddKeybind(
                            "NextSplit",
                            Global.GlobalSaveData.binds.nextSplit,
                            new KeybindConfig
                            {
                                Label = "Next Split",
                                CancelAction = cancelAction
                            },
                            out var nextSplitKeybind
                        ).AddKeybind(
                            "Mark PB",
                            Global.GlobalSaveData.binds.SetPB,
                            new KeybindConfig
                            {
                                Label = "Set PB",
                                CancelAction = cancelAction
                            },
                            out var markPBKeybind
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
