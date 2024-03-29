#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;
using Zenject;

namespace ServiceCreator
{
    [CreateAssetMenu( fileName = "ServiceCreator", menuName = "Tools/ServiceCreator" )]
    public class ServiceCreator : ScriptableObject
    {
        public Context context;
        public string  parentFolder;
        public string  serviceName;

        private const string GamePath    = "Assets/_Game/";
        private const string ScriptsPath = GamePath + "Scripts";
        private const string ConfigPath  = GamePath + "Config";
        private const string PrefabsPath = GamePath + "Prefabs";

        private string ModifiedParentFolder => parentFolder.Length > 0 ? "/" + parentFolder : "";

        private string ServiceScriptsPath => $"{ScriptsPath}{ModifiedParentFolder}/{serviceName}/";
        private string ServiceConfigPath  => $"{ConfigPath}{ModifiedParentFolder}/{serviceName}/";
        private string ServicePrefabPath  => $"{PrefabsPath}{ModifiedParentFolder}/{serviceName}/";

        public void Create( )
        {
            CreateFolders( );
            CreateScripts( );

            AssetDatabase.SaveAssets( );
        }

        public void CreateFiles( )
        {
            CreateViewPrefab( );
            CreateConfig( );

            AssetDatabase.SaveAssets( );
        }

        private void CreateFolders( )
        {
            AssetDatabase.CreateFolder( ScriptsPath, serviceName );
            AssetDatabase.CreateFolder( PrefabsPath, serviceName );
            AssetDatabase.CreateFolder( ConfigPath, serviceName );
        }

        private void CreateScripts( )
        {
            var namespaceName = parentFolder.Length > 0
                ? $"namespace Scripts.{parentFolder}.{serviceName}"
                : $"namespace Scripts.{serviceName}";

            var viewName = $"{serviceName}View";
            var viewContent = "using UnityEngine;\n\n"                       +
                              $"{namespaceName}\n"                           +
                              "{\n\tpublic class "                           + viewName + " : MonoBehaviour \n" +
                              "\t{\n"                                        +
                              "\t\tpublic Transform meshTransform;\n "     +
                              "\t\tpublic GameObject gameObjectCached;\n\n " +
                              "\t\tprivate void Reset( )\n\t\t{\n"           +
                              "\t\t\ttransformCached = transform;\n"         +
                              "\t\t\tgameObjectCached = gameObject;\n"       +
                              "\t\t}\n\t}\n}";

            CreateScript( viewName, viewContent );

            var configName = $"{serviceName}Config";
            var configContent = "using UnityEngine;\n\n"               +
                                $"{namespaceName}\n"                   +
                                "{\n\t[CreateAssetMenu( fileName = \"" + configName + "\", menuName = \"Configs/" +
                                configName                             + "\" )]\n"  +
                                "\tpublic class "                      + configName + " : ScriptableObject \n" +
                                "\t{\n"                                +
                                "\t}\n}";

            CreateScript( configName, configContent );

            var interfaceName = $"I{serviceName}Service";
            var interfaceContent = "using UnityEngine;\n\n" +
                                   $"{namespaceName}\n"     +
                                   "{\n\tpublic interface " + interfaceName + " \n" +
                                   "\t{\n"                  +
                                   "\t}\n}";

            CreateScript( interfaceName, interfaceContent );

            var serviceScriptName = $"{serviceName}Service";
            var serviceContent = "using UnityEngine;\nusing Zenject;\n\n" +
                                 $"{namespaceName}\n" +
                                 "{\n\tpublic class " + serviceScriptName + $" : {interfaceName}\n" +
                                 "\t{\n" +
                                 $"\t\tprivate {configName} _config;\n\n" +
                                 "\t\t[Inject]\n" +
                                 $"\t\tprivate void Construct( {configName} config )\n" +
                                 "\t\t{\n\t\t\t_config = config;\n" +
                                 "\t\t}\n" +
                                 "\t}\n}";

            CreateScript( serviceScriptName, serviceContent );

            var installerName = $"{serviceName}Installer";
            var installerContent = "using UnityEngine;\nusing Zenject;\n\n" +
                                   $"{namespaceName}\n" +
                                   "{\n\t[CreateAssetMenu( fileName = \"" + installerName +
                                   "\", menuName = \"Installers/" + installerName + "\" )]\n" +
                                   "\n\tpublic class " + installerName + " : ScriptableObjectInstaller \n" +
                                   "\t{\n" +
                                   $"\t\tpublic {configName} config;\n\n" +
                                   "\t\tpublic override void InstallBindings( )\n\t\t{\n" +
                                   "\t\t\tContainer.BindInstance( config );\n" +
                                   $"\t\t\tContainer.Bind<{interfaceName}>( ).To<{serviceScriptName}>( ).AsSingle( ).NonLazy( );\n" +
                                   "\t\t}\n" +
                                   "\t}\n}";

            CreateScript( installerName, installerContent );

            CompilationPipeline.RequestScriptCompilation( );
        }

        private void CreateViewPrefab( )
        {
            var prefabName = serviceName + "View";
            var tempPrefab = new GameObject( prefabName );
            tempPrefab.AddComponent( Type.GetType( $"Scripts.{serviceName}.{prefabName}" ) );

            var prefabPath = ServicePrefabPath + prefabName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset( tempPrefab, prefabPath );

            DestroyImmediate( tempPrefab );
        }

        private void CreateConfig( )
        {
            var configName = $"{serviceName}Config";
            var config     = ScriptableObject.CreateInstance( Type.GetType( $"Scripts.{serviceName}.{configName}" ) );
            var configPath = $"{ServiceConfigPath}{configName}.asset";
            AssetDatabase.CreateAsset( config, configPath );

            AssetDatabase.SaveAssets( );

            var installerName = $"{serviceName}Installer";
            var installerType = Type.GetType( $"Scripts.{serviceName}.{installerName}" );
            var installer     = ScriptableObject.CreateInstance( installerType );
            var installerPath = $"{ServiceConfigPath}{installerName}.asset";
            AssetDatabase.CreateAsset( installer, installerPath );

            var assetType = installer.GetType( );
            var fieldInfo = assetType.GetField( "config" );
            Debug.Log( fieldInfo );
            if ( fieldInfo != null )
                fieldInfo.SetValue( installer, config );

            AssetDatabase.SaveAssets( );

            if ( context )
            {
                var list = context.ScriptableObjectInstallers.ToList( );
                list.Add( installer as ScriptableObjectInstaller );
                context.ScriptableObjectInstallers = list.AsEnumerable( );
            }

            EditorUtility.SetDirty( context );
            AssetDatabase.SaveAssets( );
        }

        private void CreateScript( string fileName, string scriptContent )
        {
            var newPath = ServiceScriptsPath + fileName + ".cs";
            System.IO.File.WriteAllText( newPath, scriptContent );

            AssetDatabase.Refresh( );
            AssetDatabase.ImportAsset( newPath );
        }
    }
}
#endif