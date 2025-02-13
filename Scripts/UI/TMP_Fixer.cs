﻿using FxManager.UI.Extensions;
using FxManager.Utility.Helpers;
using Kingmaker.UI.MVVM;
using Kingmaker.UI.MVVM._PCView.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using FxManager.Utility.UnityExtensions;

namespace FxManager.UI
{
    public class TMP_Fixer : MonoBehaviour
    {
        [SerializeField]
        bool _copySourceTextColor = false;

        [SerializeField]
        List<string> _fixPathes = new List<string>()
        {
            "LogCanvas/HUDLayout/CombatLog_New/TooglePanel/ToogleAll/ToogleAll/",
            
        };

        [SerializeField]
        List<TextMeshProUGUI> _excludedTextMeshProUGUI = new List<TextMeshProUGUI>();

        public void Start()
        {
            Transform transform = null;
            
            var obj = RootUIConfig.FindObjectOfType<MainMenuPCView>();

            if (obj != null && (transform = obj.transform.Find("SceneUICanvas/SideBar/Buttons/Continue/")) == null)
            {
                foreach (var path in _fixPathes)
                {
                    if ((transform = WrathHelpers.GetStaticCanvas().transform.parent.Find(path)) != null)
                        break;
                }
            }

            if (transform != null)
            {

                if (!transform.TryGetComponentIncludeChildren<TextMeshProUGUI>(out var comp))
                    throw new NullReferenceException("Could not locate TextMeshProUGUI fix path!");


                foreach (var fix in gameObject.GetComponentsInChildren<TextMeshProUGUI>().Except(_excludedTextMeshProUGUI))
                {
                    fix.AssignFontFromSource(comp);
                }
            }
        }
    }
}
