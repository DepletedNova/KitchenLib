using KitchenLib.Customs;
using UnityEngine;

namespace KitchenLib.TestMod
{
	public class Mod : BaseMod
	{
		public Mod() : base("kitchenlib.testmod", ">=1.0.0 <=1.0.5") { }
        public static CustomAppliance d;
		public static CustomItem sushiRoll;

		public static CustomProcess rollProcess;

		public static AssetBundle bundle;
		public override void OnApplicationStart() {
			base.OnApplicationStart();
			rollProcess = AddProcess<RollProcess>();
			AddItemProcess<ChopSushi>();
			bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/sushimod");
			d = AddAppliance<TestingTerminalAppliance>();
			sushiRoll = AddItem<SushiRoll>();
		}
 
    }
}