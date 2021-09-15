using AutoHitManager.Cat;
using AutoHitManager.Managers;
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
    class HistoryMenu
    {
        public static MenuScreen BuildMenu(MenuScreen previousScreen)
        {
            var run = Global.GlobalSaveData.Runs[Global.RunDetail];
            var history = run.History.Find(history => history.number == Global.HistoryId);
            void cancelAction(MenuSelectable _)
            {
                UIManager.instance.UIGoToDynamicMenu(previousScreen);
            }
            MenuScreen menu = null;
            menu = new MenuBuilder(UIManager.instance.UICanvas.gameObject, "RunDetailMenu")
                .CreateTitle(run.Name, MenuTitleStyle.vanillaStyle)
                .CreateContentPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 703f)),
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
                        new Vector2(0f, -702f)
                    )
                ))
                .SetDefaultNavGraph(new ChainedNavGraph())
                .AddContent(
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c =>
                    {
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
                            foreach (var split in history.Splits)
                            {
                                var rt = scroll.ContentObject.GetComponent<RectTransform>();
                                rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y + 105f);
                                scroll.AddMenuButton(
                                    split.Name(),
                                    new MenuButtonConfig
                                    {
                                        Label = $"{split.Name()} with {split.Hits} hits",
                                        CancelAction = cancelAction,
                                        SubmitAction = _ => {},
                                        Style = MenuButtonStyle.VanillaStyle
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
                           "SetPB",
                           new MenuButtonConfig
                           {
                               Label = "Set as PB",
                               CancelAction = cancelAction,
                               SubmitAction = _ =>
                               {
                                   BindableFunctions.SetPB(history);
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
