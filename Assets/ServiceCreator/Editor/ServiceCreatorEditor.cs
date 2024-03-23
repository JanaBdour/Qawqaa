using UnityEditor;
using UnityEngine;

namespace ServiceCreator.Editor
{
    [CustomEditor(typeof(ServiceCreator))]
    public class ServiceCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI( )
        {
            base.OnInspectorGUI( );
            var creator = target as ServiceCreator;
            if ( !creator || creator.serviceName.Length <= 0 ) return;
            
            if( GUILayout.Button( "Create Scripts" ))
                creator.Create( );
            
            if( GUILayout.Button( "Create Files" ))
                creator.CreateFiles( );
        }
    }
}