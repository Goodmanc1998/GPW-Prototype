using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnemySpawnManager))]
public class EnemySpawnManagerEditor : Editor
{
    bool canPlaceSpawns = false;
    bool enemyLimit = false;

    EnemySpawnManager manager;

    public override void OnInspectorGUI()
    {
        manager = (EnemySpawnManager)target;

        if (manager.waves == null)
        {
            manager.waves = new List<Wave>();
        }
        if (manager.spawnpoints == null)
        {
            manager.spawnpoints = new List<EnemySpawn>();
        }

        // Spawnpoint foldout menu
        manager.showSpawnpoints = EditorGUILayout.Foldout(manager.showSpawnpoints, "Spawnpoints");
        if (manager.showSpawnpoints)
        {
            for (int i = 0; i < manager.spawnpoints.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                manager.spawnpoints[i].position = EditorGUILayout.Vector3Field("Spawn (" + i + ")", manager.spawnpoints[i].position);
                if (GUILayout.Button("Remove"))
                {
                    manager.spawnpoints.Remove(manager.spawnpoints[i]);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        // General enemy spawning settings
        if (GUILayout.Button(canPlaceSpawns == false ? "New Spawnpoint" : "Placing spawnpoint..."))
        {
            canPlaceSpawns = true;
        }
        manager.enemy = (EnemyScript)EditorGUILayout.ObjectField(new GUIContent("Enemy Object"), manager.enemy, typeof(EnemyScript), true);
        if (enemyLimit = EditorGUILayout.Toggle("Limit Enemy Spawns", enemyLimit))
        {
            manager.enemySpawnLimit = EditorGUILayout.IntSlider("Enemy Spawn Limit", manager.enemySpawnLimit, 0, 100);
        }
        else
        {
            manager.enemySpawnLimit = -1;
        }
        manager.blockOnScreenSpawns = EditorGUILayout.Toggle("Block On-Screen Spawning", manager.blockOnScreenSpawns);

        // Wave options
        for (int i = 0; i < manager.waves.Count; i++)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Wave " + (i + 1), EditorStyles.boldLabel);
            if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(25)))
            {
                manager.waves.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            manager.waves[i].enemyAmount = EditorGUILayout.IntField("Enemy Amount", manager.waves[i].enemyAmount);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Time Wave Begins");
            GUILayout.Label(string.Format("{0:0}:{1:00}", Mathf.FloorToInt(manager.waves[i].waveStartTime / 60), manager.waves[i].waveStartTime % 60), GUILayout.MaxWidth(40));
            manager.waves[i].waveStartTime = GUILayout.HorizontalSlider(manager.waves[i].waveStartTime, 0, 300);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Estimated Wave Duration");
            GUILayout.Label(string.Format("{0:0}:{1:00}", Mathf.FloorToInt(manager.waves[i].averageSpawningDuration / 60), manager.waves[i].averageSpawningDuration % 60), GUILayout.MaxWidth(40));
            manager.waves[i].averageSpawningDuration = GUILayout.HorizontalSlider(manager.waves[i].averageSpawningDuration, 0, 300);
            EditorGUILayout.EndHorizontal();

            manager.waves[i].difficulty = EditorGUILayout.Slider("Difficulty Scaling", manager.waves[i].difficulty, 0.5f, 2f);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        if (GUILayout.Button("Add Wave"))
        {
            manager.waves.Add(new Wave());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (canPlaceSpawns)
        {
            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlId;
                    Event.current.Use();
                    canPlaceSpawns = false;

                    RaycastHit hit;
                    Vector2 mousePos = Event.current.mousePosition;
                    Ray ray = Camera.current.ViewportPointToRay(Camera.current.ScreenToViewportPoint(new Vector2(mousePos.x, Screen.height - mousePos.y - 36)));
                    if (Physics.Raycast(ray, out hit))
                    {
                        manager.spawnpoints.Add(new EnemySpawn(hit.point));
                    }
                    break;
            }
        }
    }
}
