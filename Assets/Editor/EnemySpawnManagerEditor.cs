using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnemySpawnManager))]
public class EnemySpawnManagerEditor : Editor
{
    int canPlaceSpawns = -1;
    bool enemyLimit = false;

    EnemySpawnManager manager;

    public override void OnInspectorGUI()
    {
        manager = (EnemySpawnManager)target;

        if (manager.waves == null)
        {
            manager.waves = new List<Wave>();
        }
        GUI.enabled = false;
        if (enemyLimit = EditorGUILayout.Toggle("Limit Enemy Spawns", enemyLimit))
        {
            manager.enemySpawnLimit = EditorGUILayout.IntSlider("Enemy Spawn Limit", manager.enemySpawnLimit, 0, 100);
        }
        else
        {
            manager.enemySpawnLimit = -1;
        }
        GUI.enabled = true;
        manager.blockOnScreenSpawns = EditorGUILayout.Toggle("Block On-Screen Spawning", manager.blockOnScreenSpawns);

        if (Application.isPlaying)
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("Exit Play mode to create or edit waves.");
        }

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

            EditorGUILayout.BeginHorizontal();
            // General enemy spawning settings
            if (GUILayout.Button(canPlaceSpawns != i ? "New Spawnpoint" : "Placing spawnpoint..."))
            {
                canPlaceSpawns = i;
            }
            manager.waves[i].spawnpointColour = BrightenColour(EditorGUILayout.ColorField(manager.waves[i].spawnpointColour));         
            EditorGUILayout.EndHorizontal();

            if (manager.waves[i].spawnpoints == null)
            {
                manager.waves[i].spawnpoints = new List<EnemySpawn>();
            }

            // Spawnpoint foldout menu
            manager.waves[i].showSpawnpoints = EditorGUILayout.Foldout(manager.waves[i].showSpawnpoints, "Spawnpoints");
            if (manager.waves[i].showSpawnpoints)
            {
                EditorGUILayout.BeginVertical("Box");
                if (manager.waves[i].spawnpoints.Count == 0)
                {
                    EditorStyles.label.wordWrap = true;
                    EditorGUILayout.LabelField("No spawnpoints in wave. Add a spawnpoint by pressing the New Spawnpoint button followed by clicking a location on the scene.");
                }
                for (int j = 0; j < manager.waves[i].spawnpoints.Count; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    manager.waves[i].spawnpoints[j].position = EditorGUILayout.Vector3Field("Spawn (" + (j + 1) + ")", manager.waves[i].spawnpoints[j].position);
                    if (GUILayout.Button("Remove", GUILayout.MaxWidth(100)))
                    {
                        manager.waves[i].spawnpoints.Remove(manager.waves[i].spawnpoints[j]);
                        EditorGUILayout.EndHorizontal();
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

            manager.waves[i].startTrigger = (SpawnTrigger)EditorGUILayout.ObjectField(new GUIContent("Start Trigger"), manager.waves[i].startTrigger, typeof(SpawnTrigger), true);

            
            manager.waves[i].enemy = (Enemy)EditorGUILayout.ObjectField(new GUIContent("Enemy"), manager.waves[i].enemy, typeof(Enemy), true);

            manager.waves[i].enemyAmount = EditorGUILayout.IntField("Enemy Amount", manager.waves[i].enemyAmount);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Estimated Wave Duration");
            GUILayout.Label(string.Format("{0:0}:{1:00}", Mathf.FloorToInt(manager.waves[i].averageSpawningDuration / 60), manager.waves[i].averageSpawningDuration % 60), GUILayout.MaxWidth(40));
            manager.waves[i].averageSpawningDuration = GUILayout.HorizontalSlider(manager.waves[i].averageSpawningDuration, 0, 300);
            EditorGUILayout.EndHorizontal();

            //manager.waves[i].difficulty = EditorGUILayout.Slider("Difficulty Scaling", manager.waves[i].difficulty, 0.5f, 2f);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Wave"))
        {
            Wave wave = new Wave();
            manager.waves.Add(wave);
            wave.spawnpointColour = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        }

        GUI.enabled = true;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }

    Color BrightenColour(Color colour)
    {
        colour.a = 1;
        return colour;
    }

    private void OnSceneGUI()
    {
        if (manager != null)
        {
            for (int i = 0; i < manager.waves.Count; i++)
            {
                if (canPlaceSpawns == i)
                {
                    int controlId = GUIUtility.GetControlID(FocusType.Passive);

                    switch (Event.current.type)
                    {
                        case EventType.MouseDown:
                            GUIUtility.hotControl = controlId;
                            Event.current.Use();
                            canPlaceSpawns = -1;

                            RaycastHit hit;
                            Vector2 mousePos = Event.current.mousePosition;
                            Ray ray = Camera.current.ViewportPointToRay(Camera.current.ScreenToViewportPoint(new Vector2(mousePos.x, Screen.height - mousePos.y - 36)));
                            if (Physics.Raycast(ray, out hit))
                            {
                                manager.waves[i].spawnpoints.Add(new EnemySpawn(hit.point));
                            }
                            break;
                    }
                }
            }
        }
    }
}
