using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectItemScript : MonoBehaviour
{
    [SerializeField] public string LevelID; // Scene name to load
    [SerializeField] public Sprite StageImage; // Stage image
    [SerializeField] public string StageName;

    public string CharacterName { get; internal set; }
    public Sprite PixelArtImage { get; internal set; }
    public string PixelStyleText { get; internal set; }
}
