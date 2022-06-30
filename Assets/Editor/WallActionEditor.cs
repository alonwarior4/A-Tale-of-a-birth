using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;
using NUnit.Framework.Constraints;

[CustomPropertyDrawer(typeof(W_Actions))]
public class WallActionEditor : PropertyDrawer
{
    Rect objectTypePos;
    Rect positionPos;
    Rect rotationPos;
    Rect scalePos;
    Rect buttonPos;
    Vector3 euler;
    Rect textPos;
    Rect hamzmaniPos;
    Rect waitTimePos;
    Rect savePos;
    Rect existPos;
    Rect triggerPos;
    string value;
    string nameOffset;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label , property);
        EditorGUI.indentLevel = 0;


        var enumPos = new Rect(position.x, position.y, position.width, 18);
        EditorGUI.PropertyField(enumPos, property.FindPropertyRelative("w_Action"), new GUIContent("Wall Action"));
       

        switch (property.FindPropertyRelative("w_Action").enumValueIndex)
        {
            case 0:
                break;


            case 1:
                objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_sprite"), new GUIContent("Sprite"));

                var audioPos = new Rect(position.x, position.y + 40, position.width, 18);
                audioPos.width *= 0.6f;
                EditorGUI.PropertyField(audioPos, property.FindPropertyRelative("w_FSound"), new GUIContent("Sound", "Audio clip played at start of fade animation"));
                audioPos.x += (audioPos.width / 1.5f);
                EditorGUI.indentLevel = Mathf.RoundToInt(audioPos.width * 0.03f);
                EditorGUI.PropertyField(audioPos, property.FindPropertyRelative("w_audioVolume"), new GUIContent("Valume", "The Valume of audio clip between 0 and 1 , 1 means loudest and 0 means mute"));

                EditorGUI.indentLevel = 0;

                var timePos = new Rect(position.x, position.y + 60, position.width, 18);
                timePos.width *= 0.6f;
                var floatProperty = property.FindPropertyRelative("w_FTime");
                EditorGUI.PropertyField(timePos, floatProperty, new GUIContent("F_Time"));
                timePos.x += (timePos.width / 1.5f);
                EditorGUI.indentLevel = Mathf.RoundToInt(timePos.width * 0.03f);
                EditorGUI.PropertyField(timePos , property.FindPropertyRelative("w_SortingLayer") , new GUIContent("Layer"));
                    
                EditorGUI.indentLevel = 0;
              
                positionPos = new Rect(position.x, position.y + 80, position.width, 18);
                EditorGUI.PropertyField(positionPos, property.FindPropertyRelative("w_Position"), new GUIContent("Position"));


                rotationPos = new Rect(position.x, position.y + 100, position.width, position.height);
                euler = property.FindPropertyRelative("w_Rotation").quaternionValue.eulerAngles;
                EditorGUI.Vector3Field(rotationPos, new GUIContent("Rotation"), euler);
                property.FindPropertyRelative("w_Rotation").quaternionValue = Quaternion.Euler(euler);


                scalePos = new Rect(position.x, position.y + 120, position.width, position.height);
                EditorGUI.PropertyField(scalePos, property.FindPropertyRelative("w_Scale"), new GUIContent("Scale"));


                savePos = new Rect(position.x, position.y + 140, position.width, 18);
                EditorGUI.PropertyField(savePos, property.FindPropertyRelative("w_UseLater"), new GUIContent("Save For Later"));

                hamzmaniPos = new Rect(position.x, position.y + 160, position.width, 18);
                EditorGUI.PropertyField(hamzmaniPos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Simultaneously"));

               
                if (property.FindPropertyRelative("w_IsSimultaneously").boolValue == true)
                {
                    waitTimePos = new Rect(position.x, position.y + 180, position.width, 18);
                    EditorGUI.PropertyField(waitTimePos, property.FindPropertyRelative("w_WaitTime"), new GUIContent("Wait Time"));
                }

                var setChildPos = new Rect(position.x, position.y + 200, position.width, 18);
                EditorGUI.PropertyField(setChildPos, property.FindPropertyRelative("w_SetChildDesk"), new GUIContent("Desk Child", "if true this object will be child of desk else child of wall"));

                var sortinglayerPos = new Rect(position.x, position.y + 220, position.width, 18);
                EditorGUI.PropertyField(sortinglayerPos, property.FindPropertyRelative("w_sortingLayerName"), new GUIContent("Layer Name" , "sorting layer name : (Wall) for wall objects and (Desk) for desk objects"));

                buttonPos = new Rect(position.x, position.y + 240, position.width, 20);
                nameOffset = (property.FindPropertyRelative("w_sprite").objectReferenceValue) ? property.FindPropertyRelative("w_sprite").objectReferenceValue.name : "";
                if (GUI.Button(buttonPos, "Save " + nameOffset))
                {
                    property.FindPropertyRelative("w_Position").vector3Value = Selection.activeGameObject.transform.position;
                    property.FindPropertyRelative("w_Rotation").quaternionValue = Selection.activeGameObject.transform.rotation;
                    property.FindPropertyRelative("w_Scale").vector3Value = Selection.activeGameObject.transform.localScale;
                    if (Selection.activeGameObject.GetComponent<SpriteRenderer>())
                    {
                        property.FindPropertyRelative("w_SortingLayer").intValue = Selection.activeGameObject.GetComponent<SpriteRenderer>().sortingOrder;
                        property.FindPropertyRelative("w_sortingLayerName").stringValue = Selection.activeGameObject.GetComponent<SpriteRenderer>().sortingLayerName;
                    }
                }
               

                textPos = new Rect(position.x, position.y + 270, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 2:
                objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                objectTypePos.width *= 0.6f;
                EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_AnimPrefab"), new GUIContent("Animation Prefab"));
                objectTypePos.x += (objectTypePos.width / 1.5f);
                var animTimePos = property.FindPropertyRelative("w_FTime");
                EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.016f);
                EditorGUI.PropertyField(objectTypePos, animTimePos, new GUIContent("F_Time"));

                EditorGUI.indentLevel = 0;

                savePos = new Rect(position.x, position.y + 40, position.width, 18);
                EditorGUI.PropertyField(savePos, property.FindPropertyRelative("w_UseLater"), new GUIContent("Save For Later"));

                var waitPos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(waitPos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Simultaneously"));

                if(property.FindPropertyRelative("w_IsSimultaneously").boolValue == true)
                {
                    waitTimePos = new Rect(position.x, position.y + 80, position.width, 18);
                    EditorGUI.PropertyField(waitTimePos, property.FindPropertyRelative("w_WaitTime"), new GUIContent("Wait Time" , "Time To Continue Story Chain after"));
                }
               
                positionPos = new Rect(position.x, position.y + 100, position.width, 18);
                EditorGUI.PropertyField(positionPos, property.FindPropertyRelative("w_Position"), new GUIContent("Position"));

                var triggerArrayPos = new Rect(position.x, position.y + 120, position.width, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("w_Trigger")));
                EditorGUI.PropertyField(triggerArrayPos, property.FindPropertyRelative("w_Trigger"), new GUIContent("Trigger Config") , true);

                var setchildPos = new Rect(position.x,position.y + 140 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("w_Trigger")), position.width, 18);
                EditorGUI.PropertyField(setchildPos, property.FindPropertyRelative("w_SetChildDesk"), new GUIContent("Desk Child", "if true will be child of desk"));

                buttonPos = new Rect(position.x, position.y + 160 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("w_Trigger")), position.width, 20);
                nameOffset = (property.FindPropertyRelative("w_AnimPrefab").objectReferenceValue) ? property.FindPropertyRelative("w_AnimPrefab").objectReferenceValue.name : "";
                if (GUI.Button(buttonPos, "Save " + nameOffset))
                {
                    property.FindPropertyRelative("w_Position").vector3Value = Selection.activeGameObject.transform.position;
                }

                
                textPos = new Rect(position.x, 30 + buttonPos.y, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 3:

                existPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(existPos, property.FindPropertyRelative("w_IsExistInScene"), new GUIContent("Is Exist" , "is this exist in scene or created in runtime"));

                if(property.FindPropertyRelative("w_IsExistInScene").boolValue == true)
                {
                    objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_ObjName"), new GUIContent("SceneObj Name" , "the GameObject Exist In Scene Before Runtime With This Name"));
                }
                else
                {
                    objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_sprite"), new GUIContent("Sprite" , "The Sprite that made a gameobject with before and saved for later"));                    
                }

                var replaceSpritePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(replaceSpritePos, property.FindPropertyRelative("w_ReplaceSprite"), new GUIContent("Replace Sprite", "Sprite For Repalce"));

                hamzmaniPos = new Rect(position.x, position.y + 80, position.width, 18);
                EditorGUI.PropertyField(hamzmaniPos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Is Simultaneously"));

               
                textPos = new Rect(position.x, position.y + 110, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");

                break;


            case 4:

                var IsCamPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(IsCamPos, property.FindPropertyRelative("w_IsCamera"), new GUIContent("Is Camera", "If Your Animator to change is camera animator check this"));

                existPos = new Rect(position.x, position.y + 40, position.width, 18);
                EditorGUI.PropertyField(existPos, property.FindPropertyRelative("w_IsExistInScene"), new GUIContent("Is Exist In Scene", "For objects that not spawning in runtime"));

                if (property.FindPropertyRelative("w_IsCamera").boolValue == true)
                {
                    triggerPos = new Rect(position.x, position.y + 60, position.width, 18);
                    EditorGUI.PropertyField(triggerPos, property.FindPropertyRelative("w_Trigger"), new GUIContent("Trigger Config"), true);
                }
                else
                {
                    if (property.FindPropertyRelative("w_IsExistInScene").boolValue == true)
                    {
                        objectTypePos = new Rect(position.x, position.y + 60, position.width, 18);
                        EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_ObjName"), new GUIContent("Scene Obj Name", "Name Of Existing object"));

                        triggerPos = new Rect(position.x, position.y + 80, position.width, 18);
                        EditorGUI.PropertyField(triggerPos, property.FindPropertyRelative("w_Trigger"), new GUIContent("Trigger Config"), true);
                    }
                    else
                    {
                        objectTypePos = new Rect(position.x, position.y + 60, position.width, 18);
                        EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_ObjName"), new GUIContent("Scene Obj Name", "Name Of Existing object"));

                        triggerPos = new Rect(position.x, position.y + 80, position.width, 18);
                        EditorGUI.PropertyField(triggerPos, property.FindPropertyRelative("w_Trigger"), new GUIContent("Trigger Config"), true);
                    }
                }

               
                var waitBoolPos = new Rect(position.x, position.y + 100 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("w_Trigger")), position.width, 18);
                EditorGUI.PropertyField(waitBoolPos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Simultaneously"));

                if (property.FindPropertyRelative("w_IsSimultaneously").boolValue == true)
                {
                    waitTimePos = new Rect(position.x, 20 + waitBoolPos.y, position.width, 18);
                    EditorGUI.PropertyField(waitTimePos, property.FindPropertyRelative("w_WaitTime"), new GUIContent("Wait Time", "Wait Some Then Continue chain"));
                }

                textPos = new Rect(position.x, 50 + waitBoolPos.y, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 5:
                textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 6:
                textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 7:

                existPos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(existPos, property.FindPropertyRelative("w_IsExistInScene"), new GUIContent("Exist In Scene", "Check if Object not creating in runtime"));

                if(property.FindPropertyRelative("w_IsExistInScene").boolValue == true)
                {
                    objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    objectTypePos.width *= 0.6f;
                    EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_ObjName"), new GUIContent("Obj Name"));
                    objectTypePos.x += (objectTypePos.width / 1.5f);
                    var fadeProperty = property.FindPropertyRelative("w_FTime");
                    EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.017f);
                    EditorGUI.PropertyField(objectTypePos, fadeProperty, new GUIContent("F_Time"));
                }
                else
                {
                    objectTypePos = new Rect(position.x, position.y + 40, position.width, 18);
                    objectTypePos.width *= 0.6f;
                    EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_sprite"), new GUIContent("Created Obj"));
                    objectTypePos.x += (objectTypePos.width / 1.5f);
                    var fadeProperty = property.FindPropertyRelative("w_FTime");
                    EditorGUI.indentLevel = Mathf.RoundToInt(position.width * 0.017f);
                    EditorGUI.PropertyField(objectTypePos, fadeProperty, new GUIContent("F_Time"));
                }

                EditorGUI.indentLevel = 0;

                var simulatePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(simulatePos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Simultaneously"));

                if (property.FindPropertyRelative("w_IsSimultaneously").boolValue == true)
                {
                    waitTimePos = new Rect(position.x, position.y+ 80, position.width, 18);
                    EditorGUI.PropertyField(waitTimePos, property.FindPropertyRelative("w_WaitTime"), new GUIContent("Wait Time", "Wait Some Then Continue chain"));
                }
               
                textPos = new Rect(position.x, position.y + 110, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 8:
                objectTypePos = new Rect(position.x, position.y + 20, position.width, 18);
                EditorGUI.PropertyField(objectTypePos, property.FindPropertyRelative("w_IsExistInScene"), new GUIContent("Is Exist" , "Check if was in scene before runtime"));

                if(property.FindPropertyRelative("w_IsExistInScene").boolValue == true)
                {
                    var namePos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(namePos, property.FindPropertyRelative("w_ObjName"), new GUIContent("Object Name", "Name that object created or existed with"));
                }
                else
                {
                    var spPos = new Rect(position.x, position.y + 40, position.width, 18);
                    EditorGUI.PropertyField(spPos, property.FindPropertyRelative("w_sprite"), new GUIContent("Ref Sprite", "Sprite That Game Object Made With That"));
                }

                simulatePos = new Rect(position.x, position.y + 60, position.width, 18);
                EditorGUI.PropertyField(simulatePos, property.FindPropertyRelative("w_IsSimultaneously"), new GUIContent("Is Simultaneously"));

                textPos = new Rect(position.x, position.y + 90, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 9:
                textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            case 10:
                textPos = new Rect(position.x, position.y + 30, position.width, 2);
                GUI.contentColor = Color.black;
                value = EditorGUI.TextField(textPos, GUIContent.none, "");
                break;


            default:
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty actions = property.FindPropertyRelative("w_Action");
        SerializedProperty Triggers = property.FindPropertyRelative("w_Trigger");
        SerializedProperty useLater = property.FindPropertyRelative("w_UseLater");
        SerializedProperty simu = property.FindPropertyRelative("w_IsSimultaneously");
        SerializedProperty isCamera = property.FindPropertyRelative("w_IsCamera");
        SerializedProperty sprite = property.FindPropertyRelative("w_sprite");
        SerializedProperty replaceSp = property.FindPropertyRelative("w_ReplaceSprite");
        SerializedProperty time = property.FindPropertyRelative("w_Time");
        SerializedProperty layerSort = property.FindPropertyRelative("w_SortingLayer");
        SerializedProperty prefabanim = property.FindPropertyRelative("w_AnimPrefab");
        SerializedProperty objName = property.FindPropertyRelative("w_ObjName");
        SerializedProperty wasInScene = property.FindPropertyRelative("w_IsExistInScene");
        SerializedProperty position = property.FindPropertyRelative("w_Position");
        SerializedProperty rotation = property.FindPropertyRelative("w_Rotation");
        SerializedProperty scale = property.FindPropertyRelative("w_Scale");
        SerializedProperty sound = property.FindPropertyRelative("w_FSound");
        SerializedProperty volume = property.FindPropertyRelative("w_audioVolume");
        SerializedProperty setChild = property.FindPropertyRelative("w_SetChildDesk");
        SerializedProperty sortlayer = property.FindPropertyRelative("w_sortingLayerName");

        float totalHeight = 0;
        if(replaceSp != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(replaceSp);
        }
        if (sortlayer != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(sortlayer);
        }
        if (actions != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(actions);
        }
        if(Triggers != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(Triggers , true);

        }
        if (useLater != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(useLater);

        }
        if(simu != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(simu);

        }
        if (isCamera != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(isCamera);

        }
        if (sprite != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(sprite);

        }
        if(time != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(time);
        }
        if(layerSort != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(layerSort);
        }
        if(prefabanim != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(prefabanim);
        }
        if(objName != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(objName);
        }
        if(wasInScene != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(wasInScene);
        }
        if(position != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(position);
        }
        if(rotation != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(rotation);
        }
        if(scale != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(scale);
        }
        if(sound != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(sound);
        }
        if(volume != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(volume);
        }
        if (setChild != null)
        {
            totalHeight += EditorGUI.GetPropertyHeight(setChild);
        }


        return totalHeight + 30 ;
    }

   
}



