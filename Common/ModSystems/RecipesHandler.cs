namespace Entrogic.Common.ModSystems
{
    internal class RecipesHandler : ModSystem
	{
		// A place to store the recipe group so we can easily use it later
		public static RecipeGroup GoldBarRecipeGroup;

		public override void Unload() {
			GoldBarRecipeGroup = null;
		}

		public override void AddRecipeGroups() {
			// Create a recipe group and store it
			// Language.GetTextValue("LegacyMisc.37") is the word "Any" in english, and the corresponding word in other languages
			GoldBarRecipeGroup = new RecipeGroup(() => $"{Lang.GetItemNameValue(ItemID.GoldBar)} / {Lang.GetItemNameValue(ItemID.PlatinumBar)}",
				ItemID.GoldBar, ItemID.PlatinumBar);

			RecipeGroup.RegisterGroup("Entrogic: Gold Bar", GoldBarRecipeGroup);
		}
	}
}
