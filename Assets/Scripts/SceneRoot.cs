using Drone;
using LevelProgression;
using UnityEngine;
using Util;

public class SceneRoot : MonoSingleton<SceneRoot>
{
    [SerializeField]
    private DroneRoot m_Drone;

    public DroneRoot Drone => m_Drone;
}