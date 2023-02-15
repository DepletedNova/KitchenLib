using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using System.Linq;
using System;

namespace KitchenLib
{
    public class KLMenu<T> : Menu<T>
    {
        public KLMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id) { }
        protected void BoolOption(BoolPreference pref)
        {
			this.Add<bool>(new Option<bool>(new List<bool>
			{
				false,
				true
			}, (bool)pref.Value, new List<string>
			{
				this.Localisation["SETTING_DISABLED"],
				this.Localisation["SETTING_ENABLED"]
			}, null)).OnChanged += delegate(object _, bool f)
			{
				pref.Value = f;
			};
        }
		private string mod_id = "";
		private int CreateNewProfileIndex;
		protected void AddProfileSelector(string mod_id, Action<string> action)
		{
			this.mod_id = mod_id;
			List<string> profiles = GlobalPreferences.GetProfiles(mod_id).ToList();
			string current_profile = GlobalPreferences.GetProfile(mod_id);
			
			if (profiles.Count > 0)
			{
				if (!profiles.Contains(current_profile))
					current_profile = profiles[0];
			}
			else
			{
				current_profile = "";
			}

			profiles.Add("Create");
			CreateNewProfileIndex = profiles.Count - 1;


			Option<string> options = new Option<string>(
				profiles,
				current_profile,
				profiles);

			SelectElement element = AddSelect<string>(options);
			options.OnChanged += (s, args) =>
			{
				current_profile = args;
			};
			element.OnOptionChosen += CreateNew;
			element.OnOptionHighlighted += (i) =>
			{
				if (current_profile != "Create")
					action(current_profile);
			};
		}

		private void CreateNew(int i)
		{
			if (i == CreateNewProfileIndex)
			{
				base.RequestSubMenu(typeof(TextEntryMainMenu), true);
				TextInputView.RequestTextInput(base.Localisation["NEW_PROFILE_PROMPT"], "", 20, new Action<TextInputView.TextInputState, string>(this.CreateNewProfile));
			}
		}

		private void CreateNewProfile(TextInputView.TextInputState result, string name)
		{
			if (result == TextInputView.TextInputState.TextEntryComplete)
				GlobalPreferences.AddProfile(mod_id, name);
			base.RequestSubMenu(base.GetType(), true);
		}
    }
}