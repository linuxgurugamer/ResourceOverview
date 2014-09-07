﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ResourceOverview
{

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    class ResourceOverview : PluginBase
    {
        private IButton roButton;
        private bool roWindowVisible;
		private bool roWindowHover;
		

		private ApplicationLauncherButton appLauncherButton = null;


		private ResourceWindow roWindow;

        public void Start()
        {
			LogDebug("start");
			roWindow = gameObject.AddComponent<ResourceWindow>();
			
            if (ToolbarManager.ToolbarAvailable) 
            {
                roButton = ToolbarManager.Instance.add("RO", "ROButton");
                roButton.TexturePath = "ResourceOverview/icons/ro_toolbar_button";
                roButton.ToolTip = "Resource Overview Window";
                roButton.OnClick += (e) =>
                {
					roWindow.windowVisible = !roWindow.windowVisible;
                };
            }
            else
            {
				GameEvents.onGUIApplicationLauncherReady.Add(onGUIAppLauncherReady);
				GameEvents.onGUIApplicationLauncherDestroyed.Add(onGUIAppLauncherDestroyed);
            }
			
        }



		private void onGUIAppLauncherDestroyed()
		{
			LogDebug("onGUIAppLauncherDestroyed");
			if (appLauncherButton != null)
			{
				LogDebug("removing app launcher button from onGUIAppLauncherDestroyed");
				ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
			}
		}


		private void onGUIAppLauncherReady()
		{
			LogDebug("onGUIAppLauncherReady");
			if (appLauncherButton == null)
			{
				LogDebug("onGUIAppLauncherReady adding button");
				Texture2D btnTexture = new Texture2D(38, 38);
				btnTexture.LoadImage(System.IO.File.ReadAllBytes("GameData/ResourceOverview/icons/ro_app_button.png"));

				appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
					onAppLaunchToggleOn, onAppLaunchToggleOff,
					onAppLaunchHoverOn, onAppLaunchHoverOff,
					null, null,
					ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB,
					(Texture)btnTexture);
			}
		}


		public void Update()
		{
			// TODO: add hover to window?
			roWindow.windowVisible = roWindowVisible || roWindowHover;

		}

		void OnDestroy()
		{
			LogDebug("destroy");
		
			if (ToolbarManager.ToolbarAvailable)
			{
				roButton.Destroy();
			}
			else
			{
				if (appLauncherButton != null)
				{
					LogDebug("removing app launcher button from OnDestroy");
					ApplicationLauncher.Instance.RemoveModApplication(appLauncherButton);
				}
				GameEvents.onGUIApplicationLauncherDestroyed.Remove(onGUIAppLauncherDestroyed);
				GameEvents.onGUIApplicationLauncherReady.Remove(onGUIAppLauncherReady);
			}
		}

		private void onAppLaunchHoverOn()
		{
			roWindowHover = true;
		}

		private void onAppLaunchHoverOff()
		{
			roWindowHover = false;
		}

		private void onAppLaunchToggleOff()
		{
			roWindowVisible = false;
		}

		private void onAppLaunchToggleOn()
		{
			roWindowVisible = true;
		}


	}
}