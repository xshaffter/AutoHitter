using Modding.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MenuApiPlusPlus.Components
{
    public static class CustomPaneContent
    {

        /// <summary>
        /// Creates a scrollable window.<br/>
        /// The scrolling content will be the same width as the parent.
        /// </summary>
        /// <param name="content">The <c>ContentArea</c> to put the scrollable window in.</param>
        /// <param name="config">The configuration options for the scrollbar.</param>
        /// <param name="contentHeight">The height of the scroll window.</param>
        /// <param name="layout">The layout to apply to the added content.</param>
        /// <param name="action">The action that will get called to add the content.</param>
        /// <returns></returns>
        public static ContentArea AddCustomPaneContent(
            this ContentArea content,
            RelLength contentHeight,
            IContentLayout layout,
            Action<ContentArea> action
        ) => content.AddCustomPaneContent(contentHeight, layout, action, out _);

        /// <summary>
        /// Creates a scrollable window.<br/>
        /// The scrolling content will be the same width as the parent.
        /// </summary>
        /// <param name="content">The <c>ContentArea</c> to put the scrollable window in.</param>
        /// <param name="config">The configuration options for the scrollbar.</param>
        /// <param name="contentHeight">The height of the scroll window.</param>
        /// <param name="layout">The layout to apply to the added content.</param>
        /// <param name="action">The action that will get called to add the content.</param>
        /// <param name="scrollContent">The created scrollable window game object.</param>
        /// <param name="scroll">The <c>Scrollbar</c> component on the created scrollbar.</param>
        /// <returns></returns>
        public static ContentArea AddCustomPaneContent(
            this ContentArea content,
            RelLength contentHeight,
            IContentLayout layout,
            Action<ContentArea> action,
            out GameObject scrollContent
        )
        {
            // ScrollMask
            var scrollMask = new GameObject("PaneMask");
            GameObject.DontDestroyOnLoad(scrollMask);
            scrollMask.transform.SetParent(content.ContentObject.transform, false);
            // RectTransform
            var scrollMaskRt = scrollMask.AddComponent<RectTransform>();
            scrollMaskRt.sizeDelta = new Vector2(0f, 0f);
            scrollMaskRt.pivot = new Vector2(0.5f, 0.5f);
            scrollMaskRt.anchorMin = new Vector2(0f, 0f);
            scrollMaskRt.anchorMax = new Vector2(1f, 1f);
            scrollMaskRt.anchoredPosition = new Vector2(0f, 0f);
            // CanvasRenderer
            scrollMask.AddComponent<CanvasRenderer>();
            // Scrolling Pane
            var scrollPane = new GameObject("Pane");
            GameObject.DontDestroyOnLoad(scrollPane);
            scrollPane.transform.SetParent(scrollMask.transform, false);
            // RectTransform
            var scrollPaneRt = scrollPane.AddComponent<RectTransform>();
            RectTransformData.FromSizeAndPos(
                new RelVector2(new RelLength(0f, 1f), contentHeight),
                new AnchoredPosition(new Vector2(0.5f, 1f), new Vector2(0.5f, 1f))
            ).Apply(scrollPaneRt);
            // CanvasRenderer
            scrollPane.AddComponent<CanvasRenderer>();

            action(new ContentArea(
                scrollPane,
                layout,
                new GridNavGraph(3)
            ));


            scrollContent = scrollMask;
            return content;
        }
    }
}
