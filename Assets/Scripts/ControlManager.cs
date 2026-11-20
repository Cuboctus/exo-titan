
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance { get; private set; }

    [System.Serializable]
    public class LimbControls
    {
        public Key xPositive;
        public Key xNegative;
        public Key zPositive;
        public Key zNegative;
    }

    [Header("Default Controls")]
    public LimbControls leftArm = new LimbControls
    {
        xPositive = Key.F,
        xNegative = Key.G,
        zPositive = Key.V,
        zNegative = Key.R
    };

    public LimbControls rightArm = new LimbControls
    {
        xPositive = Key.J,
        xNegative = Key.H,
        zPositive = Key.M,
        zNegative = Key.U
    };

    public LimbControls leftLeg = new LimbControls
    {
        xPositive = Key.D,
        xNegative = Key.S,
        zPositive = Key.C,
        zNegative = Key.E
    };

    public LimbControls rightLeg = new LimbControls
    {
        xPositive = Key.K,
        xNegative = Key.L,
        zPositive = Key.Comma,
        zNegative = Key.I
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadControls();
    }

    public LimbControls GetControls(string limb)
    {
        switch (limb)
        {
            case "LeftArm": return leftArm;
            case "RightArm": return rightArm;
            case "LeftLeg": return leftLeg;
            case "RightLeg": return rightLeg;
            default: return null;
        }
    }

    public void SetKey(string limb, string direction, Key key)
    {
        LimbControls controls = GetControls(limb);

        if (controls == null)
            return;

        switch (direction)
        {
            case "XPositive": controls.xPositive = key; break;
            case "XNegative": controls.xNegative = key; break;
            case "ZPositive": controls.zPositive = key; break;
            case "ZNegative": controls.zNegative = key; break;
        }

        SaveControls();
    }

    private void SaveControls()
    {
        foreach (string limbName in new[]
        {
            "LeftArm", "RightArm", "LeftLeg", "RightLeg"
        })
        {
            LimbControls c = GetControls(limbName);

            PlayerPrefs.SetInt(limbName + "_XP", (int)c.xPositive);
            PlayerPrefs.SetInt(limbName + "_XN", (int)c.xNegative);
            PlayerPrefs.SetInt(limbName + "_ZP", (int)c.zPositive);
            PlayerPrefs.SetInt(limbName + "_ZN", (int)c.zNegative);
        }

        PlayerPrefs.Save();
    }

    private void LoadControls()
    {
        foreach (string limbName in new[]
        {
            "LeftArm", "RightArm", "LeftLeg", "RightLeg"
        })
        {
            LimbControls c = GetControls(limbName);

            c.xPositive = (Key)PlayerPrefs.GetInt(
                limbName + "_XP", (int)c.xPositive);

            c.xNegative = (Key)PlayerPrefs.GetInt(
                limbName + "_XN", (int)c.xNegative);

            c.zPositive = (Key)PlayerPrefs.GetInt(
                limbName + "_ZP", (int)c.zPositive);

            c.zNegative = (Key)PlayerPrefs.GetInt(
                limbName + "_ZN", (int)c.zNegative);
        }
    }
}
