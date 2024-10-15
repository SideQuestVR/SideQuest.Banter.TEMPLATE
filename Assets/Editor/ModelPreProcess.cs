using UnityEditor;

public class ModelPreProcess : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter)assetImporter;

        // Set the material import mode to 'Import via Material Description'
        modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;

        // Set material naming to 'From Model's Material'
        modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;

        // Set the material location to 'Embedded in Prefab'
        modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;

        // Set the material search to 'Project-Wide'
        modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;

       // Debug.Log("Import settings set for model: " + modelImporter.assetPath);
    }
}
