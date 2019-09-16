#if !(UNITY_EDITOR || DEBUG)
#define AL_OPTIMIZE
#endif

using Active.Core;
using UnityEngine;
using UnityEditor;
using Logger = Active.Core.Logger;

namespace Active.Editors{
public class LogTree : ActiveWindow, Logger.TreeView{

    public static LogTree instance;
    public static GameObject current;

  #if AL_OPTIMIZE
    void OnGUI() => Label("Log-Tree disabled in optimized mode");
  #else
    void OnGUI(){
        // On play, Logger is reloaded, but LogTree is not. As a result
        // repaint may skip; defensively reassign here.
        Logger.treeView = instance;
        instance = this;
        UpdateCurrent();
        if(current){
            scroll = EditorGUILayout.BeginScrollView(scroll);
            Title(current.name);
            if(!Application.isPlaying){
                Label("Log-Tree will appear while playing");
            }else if(!current.activeInHierarchy){
                Label("(currently inactive)");
            }else{
                int count = ActiveStepperCount(current);
                if(count == 0){
                    Label("Enable logging to start");
                }else{
                    foreach(var k in current.GetComponents<Stepper>()){
                        if(k.isLogging && !string.IsNullOrEmpty(k.log)){
                            if(count > 1) Title(k.GetType().Name);
                            Label(k.log);
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }else{
            Label("Select an active agent/stepper to view");
        }
    }
    #endif

    void OnSelectionChange() => RequestPaint();

  #if !AL_OPTIMIZE
    void UpdateCurrent(){
        var go = Selection.activeGameObject;
        if(go == current) return;
        if(current == null || IsLogging(go)) current = go;
    }

    bool IsLogging(GameObject go){
        Stepper[] steppers = go?.GetComponents<Stepper>();
        if(steppers == null || steppers.Length == 0) return false;
        foreach(var k in steppers) if(k.isLogging) return true;
        return false;
    }

    int ActiveStepperCount(GameObject go){
        Stepper[] steppers = go?.GetComponents<Stepper>();
        if(steppers == null || steppers.Length == 0) return 0;
        int c = 0;
        foreach(var k in steppers) if(k.isLogging) c++;
        return c;
    }
  #endif

    [MenuItem("Window/Log-Tree")]
    static void Init(){
        instance = (LogTree)EditorWindow.GetWindow(typeof(LogTree));
        instance.Show();
        Logger.treeView = instance;
    }

}}
