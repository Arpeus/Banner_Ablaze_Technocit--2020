using System.Collections;
using UnityEngine;

public class HexMapCamera : MonoBehaviour
{

    public float stickMinZoom, stickMaxZoom;

    public float swivelMinZoom, swivelMaxZoom;

    public float moveSpeedMinZoom, moveSpeedMaxZoom;

    public float rotationSpeed;

    public float _followSpeed;

    Vector3 m_targetPosition;

    public Vector3 _targetPosition
    {
        get
        {
            return m_targetPosition;
        }
        set
        {
            m_targetPosition = value;
            if (m_isFollowing == false)
            {
                StartCoroutine(CameraFollowing());
            }
        }
    }

    bool m_isFollowing;

    Transform swivel, stick;

    public HexGrid grid;

    float zoom = 1f;

    float rotationAngle;



    static HexMapCamera instance;

    public static bool Locked
    {
        set
        {
            instance.enabled = !value;
        }
    }

    public static void ValidatePosition()
    {
        //instance.AdjustPosition(0f, 0f);
    }

    void Awake()
    {
        grid = FindObjectOfType<HexGrid>();
        if(FindObjectOfType<HexGameUI>()!= null) FindObjectOfType<HexGameUI>().m_mainCamera = this;
        instance = this;
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }

    /*
	void OnEnable () {
		instance = this;
	}*/

    void Update()
    {
        if(GameManager.Instance.EType_StateAnim == AnimState.EType_IsNotPlaying)
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0f)
            {
                AdjustZoom(zoomDelta);
            }

            float rotationDelta = Input.GetAxis("Rotation");
            if (rotationDelta != 0f)
            {
                AdjustRotation(rotationDelta);
            }

            float xDelta = Input.GetAxis("Horizontal");
            float zDelta = Input.GetAxis("Vertical");
            if (xDelta != 0f || zDelta != 0f)
            {
                AdjustPosition(xDelta, zDelta);
            }
        }
    }

    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    void AdjustRotation(float delta)
    {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (rotationAngle < 0f)
        {
            rotationAngle += 360f;
        }
        else if (rotationAngle >= 360f)
        {
            rotationAngle -= 360f;
        }
        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    void AdjustPosition(float xDelta, float zDelta)
    {
        Vector3 direction =
            transform.localRotation *
            new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance =
            Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
            damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);

        m_isFollowing = false;
    }

    Vector3 ClampPosition(Vector3 position)
    {
        float xMax = (grid.cellCountX - 0.5f) * (2f * HexMetrics.innerRadius);
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
        position.z = Mathf.Clamp(position.z, 0f, zMax);

        return position;
    }

    
    IEnumerator CameraFollowing()
    {
        float targetDistance = Mathf.Infinity;

        m_isFollowing = true;
        while (targetDistance >= 0.01f && m_isFollowing == true)
        {
            // Can replace 1 by a variable
            transform.position = Vector3.Lerp(transform.position, _targetPosition, 1*Time.deltaTime);
            targetDistance = Vector3.Distance(_targetPosition, transform.position);

            yield return null;
        }
        m_isFollowing = false;
    }
}