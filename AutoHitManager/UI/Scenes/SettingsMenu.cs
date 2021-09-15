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
        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            var boolOptions = new string[] { "Yes", "No" };
            var MaxSplitOptions = new string[] { "5", "10", "15", "20", "25", "30" };
            void cancelAction(MenuSelectable _)
            {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            }
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
                        c.AddHorizontalOption(
                            "MaxVisibleSplits",
                            new HorizontalOptionConfig
                            {
                                Label = "Max visible splits",
                                CancelAction = cancelAction,
                                Options = MaxSplitOptions,
                                Description = new DescriptionInfo
                                {
                                    Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle,
                                    Text = "Max visible splits in your OBS widget"
                                },
                                ApplySetting = (_, i) =>
                                {
                                    Global.GlobalSaveData.MaxVisibleSplits = int.Parse(MaxSplitOptions[i]);
                                },
                                RefreshSetting = (self, _) =>
                                {
                                    int index = MaxSplitOptions.ToList().IndexOf(Global.GlobalSaveData.MaxVisibleSplits.ToString());
                                    self.optionList.SetOptionTo(index);
                                }
                            },
                            out var maxSplitOption
                        ).AddHorizontalOption(
                            "PracticeMode",
                            new HorizontalOptionConfig
                            {
                                Label = "Practice mode",
                                CancelAction = cancelAction,
                                Options = boolOptions,
                                Description = new DescriptionInfo
                                {
                                    Style = DescriptionStyle.HorizOptionSingleLineVanillaStyle,
                                    Text = "If activated, your health won't get under 1 point"
                                },
                                ApplySetting = (self, _) =>
                                {
                                    Global.PracticeMode = self.optionList.GetSelectedOptionText();
                                },
                                RefreshSetting = (self, _) =>
                                {
                                    int index = boolOptions.ToList().IndexOf(Global.PracticeMode);
                                    self.optionList.SetOptionTo(index);
                                }
                            },
                            out var practiceMode
                        )
                        .AddKeybind(
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
                        maxSplitOption.GetComponent<MenuSetting>().RefreshValueFromGameSettings();
                        practiceMode.GetComponent<MenuSetting>().RefreshValueFromGameSettings();
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
