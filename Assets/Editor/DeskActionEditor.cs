using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(D_Actions))]
public class DeskActionEditor : PropertyDrawer
{
    Rect D_objectTypePos;
    Rect D_positionPos;
    Rect D_rotationPos;
    Rect D_scalePos;
    Rect D_buttonPos;
    Vector3 D_euler;
    Rect D_textPos;
    Rect waitToEndPos;
    Rect D_savePos;
    Rect D_hamzamaniPos;
    Rect D_waitTimePos;
    Rect D_ExistPos;
    Rect D_triggerPos;
    string D_value;
    string offsetName;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.indentLevel = 0;

        var d_ActionEnumPos = new Rect(position.x, position.y, position.width, 18);
        EditorGUI.PropertyField(d_ActionEnumPos, property.FindPropertyRelative("d_DeskJob"), new GUIContent("Desk Action"));


        switch (property.FindPropertyRelative("d_DeskJob").enumValueIndex)
        {
            case 0:
                break;


            case 1:
                D_textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 2:
                D_textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 3:
                D_objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_PuzzlePrefab"), new GUIContent("Puzzle Prefab"));

                //var boolPos = new Rect(position.x, position.y + 40, position.width, 18);
                //boolPos.width *= 0.6f;
                //EditorGUI.PropertyField(boolPos, property.FindPropertyRelative("d_CantResolve"), new GUIContent("No Resolve"));

                //if (property.FindPropertyRelative("d_CantResolve").boolValue == true)
                //{                
                //    boolPos.x += (boolPos.width / 1.5f);
                //    EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.014f);
                //    EditorGUI.PropertyField(boolPos, property.FindPropertyRelative("d_PuzzleTryTime"), new GUIContent("try Time"));
                //}

                EditorGUI.indentLevel = 0;


                D_positionPos = new Rect(position.x, position.y + 40, position.width, 18);
                EditorGUI.PropertyField(D_positionPos, property.FindPropertyRelative("d_Position"), new GUIContent("Position"));

                D_rotationPos = new Rect(position.x, position.y + 60, position.width, position.height);
                D_euler = property.FindPropertyRelative("d_Rotation").quaternionValue.eulerAngles;
                EditorGUI.Vector3Field(D_rotationPos, new GUIContent("Rotation"), D_euler);
                property.FindPropertyRelative("d_Rotation").quaternionValue = Quaternion.Euler(D_euler);


                D_buttonPos = new Rect(position.x, position.y + 80, position.width, 20);
                offsetName = property.FindPropertyRelative("d_PuzzlePrefab").objectReferenceValue ? property.FindPropertyRelative("d_PuzzlePrefab").objectReferenceValue.name : "";
                if (GUI.Button(D_buttonPos, "Save " + offsetName))
                {
                    property.FindPropertyRelative("d_Position").vector3Value = Selection.activeGameObject.transform.position;
                    property.FindPropertyRelative("d_Rotation").quaternionValue = Selection.activeGameObject.transform.rotation;
                }

                D_textPos = new Rect(position.x, position.y + 110, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 4:
                D_objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_Sprite"), new GUIContent("Sprite"));

                var audioPos = new Rect(position.x, position.y + 40, position.width, 18);
                audioPos.width *= 0.6f;
                EditorGUI.PropertyField(audioPos, property.FindPropertyRelative("d_Sound"), new GUIContent("Sound", "Audio clip played at start of fade animation"));
                audioPos.x += (audioPos.width / 1.5f);
                EditorGUI.indentLevel = Mathf.RoundToInt(audioPos.width * 0.025f);
                EditorGUI.PropertyField(audioPos, property.FindPropertyRelative("d_Volume"), new GUIContent("Valume", "The Valume of audio clip between 0 and 1 , 1 means loudest and 0 means mute"));

                EditorGUI.indentLevel = 0;

                var timePos = new Rect(position.x, position.y + 60, position.width, 18);
                timePos.width *= 0.6f;
                var floatProperty = property.FindPropertyRelative("d_Ftime");
                EditorGUI.PropertyField(timePos, floatProperty, new GUIContent("F_Time" , "Time Passed to fade sprite compelete"));
                timePos.x += (timePos.width / 1.5f);
                EditorGUI.indentLevel = Mathf.RoundToInt(timePos.width * 0.025f);
                EditorGUI.PropertyField(timePos , property.FindPropertyRelative("d_SortingOrder") , new GUIContent("Layer"));
                    
                EditorGUI.indentLevel = 0;
              
                D_positionPos = new Rect(position.x, position.y + 80, position.width, 18);
                EditorGUI.PropertyField(D_positionPos, property.FindPropertyRelative("d_Position"), new GUIContent("Position"));


                D_rotationPos = new Rect(position.x, position.y + 100, position.width, position.height);
                D_euler = property.FindPropertyRelative("d_Rotation").quaternionValue.eulerAngles;
                EditorGUI.Vector3Field(D_rotationPos, new GUIContent("Rotation"), D_euler);
                property.FindPropertyRelative("d_Rotation").quaternionValue = Quaternion.Euler(D_euler);


                D_scalePos = new Rect(position.x, position.y + 120, position.width, position.height);
                EditorGUI.PropertyField(D_scalePos, property.FindPropertyRelative("d_Scale"), new GUIContent("Scale"));


                D_savePos = new Rect(position.x, position.y + 140, position.width, 18);
                EditorGUI.PropertyField(D_savePos, property.FindPropertyRelative("d_UseLater"), new GUIContent("Save For Later"));

                D_hamzamaniPos = new Rect(position.x, position.y + 160, position.width, 18);
                EditorGUI.PropertyField(D_hamzamaniPos, property.FindPropertyRelative("d_IsSimultaneously"), new GUIContent("Simultaneously"));

               
                if (property.FindPropertyRelative("d_IsSimultaneously").boolValue == true)
                {
                    D_waitTimePos = new Rect(position.x, position.y + 180, position.width, 18);
                    EditorGUI.PropertyField(D_waitTimePos, property.FindPropertyRelative("d_WaitTime"), new GUIContent("Wait Time"));
                }

                var sortingLayerPos = new Rect(position.x, position.y + 200, position.width, 18);
                EditorGUI.PropertyField(sortingLayerPos, property.FindPropertyRelative("d_SortingLayerName"), new GUIContent("Layer Name" , "Sorting layer name : (Wall) for wall objects and (Desk) for desk objects"));

                var setChildPos = new Rect(position.x, position.y + 220, position.width, 18);
                EditorGUI.PropertyField(setChildPos, property.FindPropertyRelative("d_SetWallChild"), new GUIContent("Wall Child", "if true will be child of wall else will be desk child"));

                D_buttonPos = new Rect(position.x, position.y + 240, position.width, 20);
                offsetName = (property.FindPropertyRelative("d_Sprite").objectReferenceValue) ? property.FindPropertyRelative("d_Sprite").objectReferenceValue.name : "";
                if (GUI.Button(D_buttonPos, "Save " + offsetName))
                {
                    property.FindPropertyRelative("d_Position").vector3Value = Selection.activeGameObject.transform.position;
                    property.FindPropertyRelative("d_Rotation").quaternionValue = Selection.activeGameObject.transform.rotation;
                    property.FindPropertyRelative("d_Scale").vector3Value = Selection.activeGameObject.transform.localScale;
                    if (Selection.activeGameObject.GetComponent<SpriteRenderer>())
                    {
                        property.FindPropertyRelative("d_SortingOrder").intValue = Selection.activeGameObject.GetComponent<SpriteRenderer>().sortingOrder;
                        property.FindPropertyRelative("d_SortingLayerName").stringValue = Selection.activeGameObject.GetComponent<SpriteRenderer>().sortingLayerName;
                    }
                }

                D_textPos = new Rect(position.x, position.y + 270, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 5:
                D_ExistPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(D_ExistPos, property.FindPropertyRelative("d_isExistInScene"), new GUIContent("Is Exist", "is this exist in scene or created in runtime"));

                if (property.FindPropertyRelative("d_isExistInScene").boolValue == true)
                {
                    D_objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_ObjName"), new GUIContent("SceneObj Name", "the GameObject Exist In Scene Before Runtime With This Name"));
                }
                else
                {
                    D_objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_Sprite"), new GUIContent("Sprite", "The Sprite that made a gameobject with before and saved for later"));
                }

                var replaceSpritePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(replaceSpritePos, property.FindPropertyRelative("d_ReplaceSprite"), new GUIContent("Replace Sprite", "Sprite For Repalce"));


                D_hamzamaniPos = new Rect(position.x, position.y + 80, position.width, 18);
                EditorGUI.PropertyField(D_hamzamaniPos, property.FindPropertyRelative("d_IsSimultaneously"), new GUIContent("Is Simultaneously"));

                D_textPos = new Rect(position.x, position.y + 110, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 6:
                var IsCamPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(IsCamPos, property.FindPropertyRelative("d_IsCamera"), new GUIContent("Is Camera", "If Your Animator to change is camera animator check this"));

                D_ExistPos = new Rect(position.x, position.y + 40, position.width, 18);
                EditorGUI.PropertyField(D_ExistPos, property.FindPropertyRelative("d_isExistInScene"), new GUIContent("Is Exist In Scene", "For objects that not spawning in runtime"));

                if (property.FindPropertyRelative("d_IsCamera").boolValue == true)
                {
                    D_triggerPos = new Rect(position.x, position.y + 60, position.width, 18);
                    EditorGUI.PropertyField(D_triggerPos, property.FindPropertyRelative("d_triggerName"), new GUIContent("Trigger Name"));

                    var d_speedAnimPos = new Rect(position.x, position.y + 80, position.width, 18);
                    EditorGUI.PropertyField(d_speedAnimPos, property.FindPropertyRelative("d_animSpeed"), new GUIContent("animation Speed", "more than 1 speed up animation and less than 1 slow animation"));
                }
                else
                {
                    if (property.FindPropertyRelative("d_isExistInScene").boolValue == true)
                    {
                        D_objectTypePos = new Rect(position.x, position.y + 60, position.width, 18);
                        EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_ObjName"), new GUIContent("Scene Obj Name", "Name Of Existing object"));

                        D_triggerPos = new Rect(position.x, position.y + 80, position.width, 18);
                        EditorGUI.PropertyField(D_triggerPos, property.FindPropertyRelative("d_triggerName"), new GUIContent("Trigger Name"));

                        var d_speedAnimPos = new Rect(position.x, position.y + 100, position.width, 18);
                        EditorGUI.PropertyField(d_speedAnimPos, property.FindPropertyRelative("d_animSpeed"), new GUIContent("animation Speed", "more than 1 speed up animation and less than 1 slow animation"));
                    }
                    else
                    {
                        D_objectTypePos = new Rect(position.x, position.y + 60, position.width, 18);
                        EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_ObjName"), new GUIContent("Scene Obj Name", "Name Of Existing object"));

                        D_triggerPos = new Rect(position.x, position.y + 80, position.width, 18);
                        EditorGUI.PropertyField(D_triggerPos, property.FindPropertyRelative("d_triggerName"), new GUIContent("Trigger Name"), true);

                        var d_speedAnimPos = new Rect(position.x, position.y + 100, position.width, 18);
                        EditorGUI.PropertyField(d_speedAnimPos, property.FindPropertyRelative("d_animSpeed"), new GUIContent("animation Speed", "more than 1 speed up animation and less than 1 slow animation"));
                    }
                }


                D_hamzamaniPos = new Rect(position.x, position.y + 120 , position.width, 18);
                EditorGUI.PropertyField(D_hamzamaniPos, property.FindPropertyRelative("d_IsSimultaneously"), new GUIContent("Simultaneously"));

                if (property.FindPropertyRelative("d_IsSimultaneously").boolValue == true)
                {
                    D_waitTimePos = new Rect(position.x, position.y + 140 , position.width, 18);
                    EditorGUI.PropertyField(D_waitTimePos, property.FindPropertyRelative("d_WaitTime"), new GUIContent("Wait Time", "Wait Some Then Continue chain"));
                }

                D_textPos = new Rect(position.x, position.y + 170, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 7:
                D_ExistPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(D_ExistPos, property.FindPropertyRelative("d_isExistInScene"), new GUIContent("Exist In Scene", "Check if Object not creating in runtime"));

                if (property.FindPropertyRelative("d_isExistInScene").boolValue == true)
                {
                    D_objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    D_objectTypePos.width *= 0.6f;
                    EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_ObjName"), new GUIContent("Obj Name"));
                    D_objectTypePos.x += (D_objectTypePos.width / 1.5f);
                    var fadeProperty = property.FindPropertyRelative("d_Ftime");
                    EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.017f);
                    EditorGUI.PropertyField(D_objectTypePos, fadeProperty, new GUIContent("F_Time"));
                }
                else
                {
                    D_objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    D_objectTypePos.width *= 0.6f;
                    EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_Sprite"), new GUIContent("Created Obj"));
                    D_objectTypePos.x += (D_objectTypePos.width / 1.5f);
                    var fadeProperty = property.FindPropertyRelative("d_Ftime");
                    EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.017f);
                    EditorGUI.PropertyField(D_objectTypePos, fadeProperty, new GUIContent("F_Time"));
                }

                EditorGUI.indentLevel = 0;

                var simulatePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(simulatePos, property.FindPropertyRelative("d_IsSimultaneously"), new GUIContent("Simultaneously"));

                if (property.FindPropertyRelative("d_IsSimultaneously").boolValue == true)
                {
                    D_waitTimePos = new Rect(position.x, position.y + 80, position.width, 18);
                    EditorGUI.PropertyField(D_waitTimePos, property.FindPropertyRelative("d_WaitTime"), new GUIContent("Wait Time", "Wait Some Then Continue chain"));
                }

                D_textPos = new Rect(position.x, position.y + 110, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            case 8:
                D_objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(D_objectTypePos, property.FindPropertyRelative("d_isExistInScene"), new GUIContent("Is Exist", "Check if was in scene before runtime"));

                if (property.FindPropertyRelative("d_isExistInScene").boolValue == true)
                {
                    var namePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(namePos, property.FindPropertyRelative("d_ObjName"), new GUIContent("Object Name", "Name that object created or existed with"));
                }
                else
                {
                    var spPos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(spPos, property.FindPropertyRelative("d_Sprite"), new GUIContent("Ref Sprite", "Sprite That Game Object Made With That"));
                }

                simulatePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(simulatePos, property.FindPropertyRelative("d_IsSimultaneously"), new GUIContent("Is Simultaneously"));

                D_textPos = new Rect(position.x, position.y + 90, position.width, 2);
                GUI.contentColor = Color.black;
                D_value = EditorGUI.TextField(D_textPos, GUIContent.none, "");
                break;


            default:
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty djob = property.FindPropertyRelative("d_DeskJob");
        SerializedProperty pPref = property.FindPropertyRelative("d_PuzzlePrefab");
        SerializedProperty cResolve = property.FindPropertyRelative("d_CantResolve");
        SerializedProperty pTry = property.FindPropertyRelative("d_PuzzleTryTime");
        SerializedProperty isSimu = property.FindPropertyRelative("d_IsSimultaneously");
        SerializedProperty wTime = property.FindPropertyRelative("d_WaitTime");
        SerializedProperty dsp = property.FindPropertyRelative("d_Sprite");
        SerializedProperty dfTime = property.FindPropertyRelative("d_Ftime");
        SerializedProperty sLayer = property.FindPropertyRelative("d_SortingOrder");
        SerializedProperty dTName = property.FindPropertyRelative("d_triggerName");
        SerializedProperty dPos = property.FindPropertyRelative("d_Position");
        SerializedProperty dRot = property.FindPropertyRelative("d_Rotation");
        SerializedProperty dsc = property.FindPropertyRelative("d_Scale");
        SerializedProperty uLater = property.FindPropertyRelative("d_UseLater");
        SerializedProperty isExist = property.FindPropertyRelative("d_isExistInScene");
        SerializedProperty d_sound = property.FindPropertyRelative("d_Sound");
        SerializedProperty d_vol = property.FindPropertyRelative("d_Volume");
        SerializedProperty dName = property.FindPropertyRelative("d_ObjName");
        SerializedProperty dreplace = property.FindPropertyRelative("d_ReplaceSprite");
        SerializedProperty danimSp = property.FindPropertyRelative("d_animSpeed");
        SerializedProperty dcam = property.FindPropertyRelative("d_IsCamera");
        SerializedProperty dSortLayer = property.FindPropertyRelative("d_SortingLayerName");
        SerializedProperty dSetChild = property.FindPropertyRelative("d_SetWallChild");

        float dTotalHeight = 0;

        if(djob != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(djob);
        }
        if (dSetChild != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dSetChild);
        }
        if (dSortLayer != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dSortLayer);
        }
        if (pPref != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(pPref);
        }
        if(cResolve != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(cResolve);
        }
        if(pTry != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(pTry);
        }
        if (isSimu != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(isSimu);
        }
        if (wTime != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(wTime);
        }
        if (dsp != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dsp);
        }
        if (dfTime != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dfTime);
        }
        if (sLayer != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(sLayer);
        }
        if (dTName != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dTName);
        }
        if (dPos != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dPos);
        }
        if (dRot != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dRot);
        }
        if (dsc != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dsc);
        }
        if(uLater != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(uLater);
        }
        if(isExist != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(isExist);
        }
        if(d_sound != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(d_sound);
        }
        if(d_vol != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(d_vol);
        }
        if(dName != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dName);
        }
        if(dreplace != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dreplace);
        }
        if(danimSp != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(danimSp);
        }
        if(dcam != null)
        {
            dTotalHeight += EditorGUI.GetPropertyHeight(dcam);
        }

        return dTotalHeight + 30;
    }
}
