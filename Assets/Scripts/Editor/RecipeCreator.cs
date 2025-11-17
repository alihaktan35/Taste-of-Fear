using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RecipeCreator : EditorWindow
{
    [MenuItem("Tools/Create All Recipes")]
    static void CreateAllRecipes()
    {
        // Recipe definitions based on the image provided
        var recipes = new Dictionary<string, List<(string ingredient, int quantity)>>
        {
            { "Göz Küresi Çorbası", new List<(string, int)> {
                ("göz", 2),
                ("kara yosun", 1),
                ("pıhtılanmış kan şişe", 3)
            }},
            { "Kan Gölü Sosu", new List<(string, int)> {
                ("parlak kan şişe", 2),
                ("sivri kemik", 3),
                ("baharat", 1)
            }},
            { "Kertenkele Kuyrukları", new List<(string, int)> {
                ("kertenkele kuyruğu", 4),
                ("irin tozu", 1),
                ("diş cipsleri", 2)
            }},
            { "Kurtçuk Kanepesi", new List<(string, int)> {
                ("kurtçuklar", 4),
                ("çürük ekmek", 2),
                ("bilinmeyen yağ", 1)
            }},
            { "Canavar Kalbi Izgara", new List<(string, int)> {
                ("et (pişmiş)", 1),
                ("koyu zehir", 4)
            }},
            { "Zombi Beyin Keki", new List<(string, int)> {
                ("beyin", 1),
                ("pıhtılanmış kan şişe", 2),
                ("kara yosun", 2),
                ("kafatası", 1)
            }},
            { "Yaratık Kaburgaları", new List<(string, int)> {
                ("sivri kemik", 4),
                ("et", 1),
                ("baharat", 2) // Added missing ingredient - adjust if needed
            }},
            { "İğrenç Pizza", new List<(string, int)> {
                ("çürük ekmek", 2),
                ("irin tozu", 1),
                ("böcek yumurtası", 2),
                ("siyah zeytin", 4)
            }},
            { "Dokunaç Güveci", new List<(string, int)> {
                ("dokunaç", 4),
                ("koyu zehir", 1),
                ("kanayan patates", 2)
            }},
            { "Kesik Damar Spagetti", new List<(string, int)> {
                ("kara spagetti", 2),
                ("parlak kan şişe", 1)
            }},
            { "Zehirli Mantar Sepeti", new List<(string, int)> {
                ("mantar", 2),
                ("asit", 1),
                ("kemik çubuğu", 3)
            }},
            { "Beyin Salatası", new List<(string, int)> {
                ("beyin", 1),
                ("balçık", 1),
                ("kertenkele kuyruğu", 2)
            }},
            { "Örümcek Yumurtaları", new List<(string, int)> {
                ("örümcek", 1),
                ("böcek yumurtası", 2),
                ("pıhtılanmış kan şişe", 1)
            }},
            { "Kanlı Şırıngalar", new List<(string, int)> {
                ("şırınga", 4),
                ("parlak kan şişe", 1)
            }},
            { "Çürümüş Diş Pastası", new List<(string, int)> {
                ("çürümüş dişler", 4),
                ("kurabiye", 2)
            }},
            { "Parmak Sucukları (Tatlı)", new List<(string, int)> {
                ("parmak", 4),
                ("pıhtılanmış kan şişe", 1)
            }},
            { "Kusmuk Şekerlemesi", new List<(string, int)> {
                ("asit", 2),
                ("bilinmeyen yağ", 1),
                ("kırmızı şeker", 4)
            }}
        };

        // Create Resources folder if it doesn't exist
        string resourcesPath = "Assets/Resources";
        string recipesPath = resourcesPath + "/Recipes";

        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        if (!AssetDatabase.IsValidFolder(recipesPath))
        {
            AssetDatabase.CreateFolder(resourcesPath, "Recipes");
        }

        // Create each recipe
        foreach (var recipeEntry in recipes)
        {
            string recipeName = recipeEntry.Key;
            List<(string ingredient, int quantity)> ingredients = recipeEntry.Value;

            // Create ScriptableObject
            RecipeData recipe = ScriptableObject.CreateInstance<RecipeData>();
            recipe.recipeName = recipeName;

            // Add ingredients
            foreach (var ing in ingredients)
            {
                recipe.ingredients.Add(new RecipeIngredient
                {
                    ingredientName = ing.ingredient,
                    quantity = ing.quantity
                });
            }

            // Save asset
            string assetPath = $"{recipesPath}/{recipeName}.asset";
            AssetDatabase.CreateAsset(recipe, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created {recipes.Count} recipes in {recipesPath}");
        EditorUtility.DisplayDialog("Success", $"Created {recipes.Count} recipes!", "OK");
    }
}
