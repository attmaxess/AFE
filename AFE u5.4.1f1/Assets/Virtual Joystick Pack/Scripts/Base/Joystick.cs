using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)] public float handleLimit = 1f;
    public JoystickMode joystickMode = JoystickMode.AllAxis;

    protected Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }


    static Joystick singleton;
    public static Joystick Singleton
    {
        get
        {
            if (singleton == null)
            {
                Debug.LogError("Dont Have Joystick. Crate A Joystick");
                return null;
            }

            return singleton;
        }
    }


    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Debug.LogWarning("Your scene have many Joystick. Destroy others");
            DestroyImmediate(gameObject);
        }
    }

    void OnDestroy()
    {
        singleton = null;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }

    protected void ClampJoystick()
    {
        if (joystickMode == JoystickMode.Horizontal)
            inputVector = new Vector2(inputVector.x, 0f);
        if (joystickMode == JoystickMode.Vertical)
            inputVector = new Vector2(0f, inputVector.y);
    }
}

public enum JoystickMode { AllAxis, Horizontal, Vertical }
