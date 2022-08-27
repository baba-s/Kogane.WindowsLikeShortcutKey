using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Kogane.Internal
{
    /// <summary>
    /// Windows のようにショートカットキーを使用できるようにするエディタ拡張
    /// </summary>
    internal static class WindowsLikeShortcutKey
    {
        [Shortcut( "Kogane/Delete", KeyCode.Delete )]
        private static void Delete()
        {
            EditorApplication.ExecuteMenuItem( "Edit/Delete" );
        }

        [Shortcut( "Kogane/Redo", KeyCode.Y, ShortcutModifiers.Action )]
        private static void Redo()
        {
            EditorApplication.ExecuteMenuItem( "Edit/Redo" );
        }

        [Shortcut( "Kogane/Rename", KeyCode.F2 )]
        private static void Rename()
        {
            var focusedWindowType     = EditorWindow.focusedWindow.GetType();
            var focusedWindowFullName = focusedWindowType.FullName;

            if ( focusedWindowFullName == "UnityEditor.SceneHierarchyWindow" )
            {
                var sceneHierarchyWindowType = focusedWindowType;
                var assembly                 = focusedWindowType.Assembly;
                var sceneHierarchyType       = assembly.GetType( "UnityEditor.SceneHierarchy" );
                var sceneHierarchyField      = sceneHierarchyWindowType.GetField( "m_SceneHierarchy", BindingFlags.Instance | BindingFlags.NonPublic );
                var renameGO                 = sceneHierarchyType.GetMethod( "RenameGO", BindingFlags.Instance | BindingFlags.NonPublic );
                var sceneHierarchyWindow     = EditorWindow.GetWindow( sceneHierarchyWindowType );
                var sceneHierarchy           = sceneHierarchyField.GetValue( sceneHierarchyWindow );

                renameGO.Invoke( sceneHierarchy, null );
            }
            else if ( focusedWindowFullName == "UnityEditor.ProjectBrowser" )
            {
                var projectBrowserType         = focusedWindowType;
                var renameSelectedAssetsMethod = projectBrowserType.GetMethod( "RenameSelectedAssets", BindingFlags.Static | BindingFlags.NonPublic );

                renameSelectedAssetsMethod.Invoke( null, null );
            }
        }
    }
}