using System;
using UEGP3.Core;
using UnityEditor;
using UnityEngine;

namespace UEGP3.Code.Editor.Core
{
    [CustomEditor(typeof(ScriptableAudioEvent))]
    public class ScriptableAudioEventEditor : UnityEditor.Editor
    {
        private AudioSource _previewAudioSource;

        private SerializedProperty _clipsProperty;

        private SerializedProperty _randomizeVolumeProperty;
        private SerializedProperty _volumeProperty;
        private SerializedProperty _minVolumeProperty;
        private SerializedProperty _maxVolumeProperty;
        
        private SerializedProperty _randomizePitchProperty;
        private SerializedProperty _pitchProperty;
        private SerializedProperty _minPitchProperty;
        private SerializedProperty _maxPitchProperty;
        
        private void OnEnable()
        {
            // Create an invisible and not saved Audio Source which will be used only for previewing.
            _previewAudioSource = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview Source", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();

            _clipsProperty = serializedObject.FindProperty("_clips");

            _randomizeVolumeProperty = serializedObject.FindProperty("_randomizeVolume");
            _volumeProperty = serializedObject.FindProperty("_volume");
            _minVolumeProperty = serializedObject.FindProperty("_minVolume");
            _maxVolumeProperty = serializedObject.FindProperty("_maxVolume");

            _randomizePitchProperty = serializedObject.FindProperty("_randomizePitch");
            _pitchProperty = serializedObject.FindProperty("_pitch");
            _minPitchProperty = serializedObject.FindProperty("_minPitch");
            _maxPitchProperty = serializedObject.FindProperty("_maxPitch");
        }

        private void OnDisable()
        {
            // We need to destroy preview audio source, when we stop inspecting the AudioEvent
            DestroyImmediate(_previewAudioSource.gameObject);
        }

        public override void OnInspectorGUI()
        {
            ScriptableAudioEvent scriptableAudioEvent = (ScriptableAudioEvent) target;
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(_clipsProperty, true);

            EditorGUILayout.PropertyField(_randomizeVolumeProperty);
            if (scriptableAudioEvent.RandomizeVolume)
            {
                EditorGUILayout.PropertyField(_minVolumeProperty);
                EditorGUILayout.PropertyField(_maxVolumeProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(_volumeProperty);
            }

            EditorGUILayout.PropertyField(_randomizePitchProperty);
            if (scriptableAudioEvent.RandomizePitch)
            {
                EditorGUILayout.PropertyField(_minPitchProperty);
                EditorGUILayout.PropertyField(_maxPitchProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(_pitchProperty);
            }
            
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Preview SFX"))
            {
                scriptableAudioEvent.Play(_previewAudioSource);
            }
        }
    }
}
