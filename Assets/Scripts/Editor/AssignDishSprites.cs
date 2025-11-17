using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssignDishSprites : EditorWindow
{
    [MenuItem("Tools/Assign Dish Sprites to Recipes")]
    static void AssignSprites()
    {
        // Mapping of recipe names to dish image file names
        var recipeToImageMapping = new Dictionary<string, string>
        {
            { "Göz Küresi Çorbası", "Göz küresi çorbası" },
            { "Kan Gölü Sosu", "kan gölü sosu" },
            { "Kertenkele Kuyrukları", "kertenkele kuyrukları(tabak)" },
            { "Kurtçuk Kanepesi", "Kurtçuk Kanepesi" },
            { "Canavar Kalbi Izgara", "Canavar Kalbi Izgara" },
            { "Zombi Beyin Keki", "Zombi Beyin Keki" },
            { "Yaratık Kaburgaları", "Yaratık Kaburgaları" },
            { "İğrenç Pizza", "iğrenç pizza" },
            { "Dokunaç Güveci", "Dokunaç Güveci" },
            { "Kesik Damar Spagetti", "Kesik Damar Spagetti" },
            { "Zehirli Mantar Sepeti", "Zehirli Mantar Sepeti" },
            { "Beyin Salatası", "beyin salatası" },
            { "Örümcek Yumurtaları", "Örümcek Yumurtaları" },
            { "Kanlı Şırıngalar", "Kanlı Şırıngalar" },
            { "Çürümüş Diş Pastası", "Çürümüş Diş Pastası" },
            { "Parmak Sucukları (Tatlı)", "Parmak Sucukları (Tatlı)" },
            { "Kusmuk Şekerlemesi", "Kusmuk Şekerlemesi" }
        };

        string recipesPath = "Assets/Resources/Recipes";
        string dishImagesPath = "Assets/Images/Orders";

        int successCount = 0;
        int failCount = 0;
        List<string> failedRecipes = new List<string>();

        // Load all recipe assets
        string[] recipeGuids = AssetDatabase.FindAssets("t:RecipeData", new[] { recipesPath });

        foreach (string guid in recipeGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RecipeData recipe = AssetDatabase.LoadAssetAtPath<RecipeData>(path);

            if (recipe != null && recipeToImageMapping.ContainsKey(recipe.recipeName))
            {
                string imageName = recipeToImageMapping[recipe.recipeName];

                // Try to find the sprite
                string[] extensions = { ".png", ".jpg", ".jpeg" };
                Sprite foundSprite = null;

                foreach (string ext in extensions)
                {
                    string spritePath = $"{dishImagesPath}/{imageName}{ext}";
                    foundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

                    if (foundSprite != null)
                    {
                        break;
                    }
                }

                if (foundSprite != null)
                {
                    recipe.dishSprite = foundSprite;
                    EditorUtility.SetDirty(recipe);
                    successCount++;
                    Debug.Log($"✓ Assigned sprite to: {recipe.recipeName}");
                }
                else
                {
                    failCount++;
                    failedRecipes.Add(recipe.recipeName);
                    Debug.LogWarning($"✗ Could not find sprite for: {recipe.recipeName} (looking for: {imageName})");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string message = $"Dish Sprite Assignment Complete!\n\n" +
                        $"Success: {successCount}\n" +
                        $"Failed: {failCount}";

        if (failedRecipes.Count > 0)
        {
            message += "\n\nFailed recipes:\n";
            foreach (string name in failedRecipes)
            {
                message += $"- {name}\n";
            }
        }

        EditorUtility.DisplayDialog("Dish Sprites Assigned", message, "OK");
        Debug.Log(message);
    }
}
