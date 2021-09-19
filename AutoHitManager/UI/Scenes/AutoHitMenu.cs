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
    public static class AutoHitMenu
    {
        public static MenuScreen BuildMenu(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            var dels = toggleDelegates.Value;
            void cancelAction(MenuSelectable _)
            {
                dels.ApplyChange();
                UIManager.instance.UIGoToDynamicMenu(modListMenu);
            }
            MenuScreen menu = null;
            menu = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "AutoHit")
                .CreateTitle("AutoHit", MenuTitleStyle.vanillaStyle)
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
                        c.AddHorizontalOption(
                            "ToggleModOption",
                            new HorizontalOptionConfig
                            {
                                Label = "Mod Enabled",
                                Options = new string[] { "Off", "On" },
                                ApplySetting = (_, i) => dels.SetModEnabled(i == 1),
                                RefreshSetting = (s, _) => s.optionList.SetOptionTo(dels.GetModEnabled() ? 1 : 0),
                                CancelAction = cancelAction
                            },
                            out var toggleModOption
                        ).AddMenuButton(
                            "Runs",
                            new MenuButtonConfig
                            {
                                Label = "Runs",
                                CancelAction = cancelAction,
                                SubmitAction = _ =>
                                {
                                    var self = AvailableRunsMenu.BuildMenu(menu);
                                    UIManager.instance.UIGoToDynamicMenu(self);
                                },
                                Style = MenuButtonStyle.VanillaStyle,
                                Proceed = true
                            }
                        ).AddMenuButton(
                            "Config",
                            new MenuButtonConfig
                            {
                                Label = "Config",
                                SubmitAction = _ =>
                                {
                                    var self = SettingsMenu.BuildMenu(menu);
                                    UIManager.instance.UIGoToDynamicMenu(self);
                                },
                                CancelAction = cancelAction,
                                Style = MenuButtonStyle.VanillaStyle,
                                Proceed = true
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
                        toggleModOption.GetComponent<MenuSetting>().RefreshValueFromGameSettings();
                    }
                )
                .AddControls(
                    RegularGridLayout.CreateVerticalLayout(105f),
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
            return menu;
        }
    }
}
