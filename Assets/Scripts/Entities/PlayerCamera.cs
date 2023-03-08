using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 _zoomFollowOffest;

    [field: SerializeField]
    private float _movementSpeed = 20f;

    [field: SerializeField]
    private float _rotationSpeed = 5f;

    [field: SerializeField]
    private float _edgeScrollSize = 20f;

    [field: SerializeField]
    private bool _keyScrollEnabled = true;

    [field: SerializeField]
    private bool _edgeScrollEnabled = true;

    [field: SerializeField]
    private bool _rotationEnabled = true;

    [field: SerializeField]
    private bool _zoomEnabled = true;

    [field: SerializeField]
    private float _zoomAmount = 1;

    [field: SerializeField]
    private float _zoomFollowOffestMin = -10f;

    [field: SerializeField]
    private float _zoomFollowOffestMax = -20f;

    [field: SerializeField]
    private float _zoomSpeed = 10f;

    protected CinemachineTransposer transposer { get; private set; }

    [field: SerializeField]
    protected CinemachineVirtualCamera virtualCamera { get; private set; }    

    // Start is called before the first frame update
    private void Start()
    {

#if DEBUG
        _edgeScrollEnabled = false;
#endif

        this.transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _zoomFollowOffest = this.transposer.m_FollowOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            this.transform.position = Vector3.zero;

        if (_keyScrollEnabled || _edgeScrollEnabled)
            HandleCameraScroll();

        if (_rotationEnabled)
            HandleCameraRotation();

        if (_zoomEnabled)
            HandleCameraZoom();
    }

    private void HandleCameraScroll()
    {
        Vector3 inputDirection = new Vector3();

        if ((Input.GetKey(KeyCode.S) && _keyScrollEnabled)
            || (Input.mousePosition.y < _edgeScrollSize) && _edgeScrollEnabled)
        {
            inputDirection.z = -1f;
        }

        if ((Input.GetKey(KeyCode.W) && _keyScrollEnabled)
            || (Input.mousePosition.y > Screen.height - _edgeScrollSize) && _edgeScrollEnabled)
        {
            inputDirection.z = +1f;
        }

        if ((Input.GetKey(KeyCode.A) && _keyScrollEnabled)
            || (Input.mousePosition.x < _edgeScrollSize) && _edgeScrollEnabled)
        {
            inputDirection.x = -1f;
        }

        if ((Input.GetKey(KeyCode.D) && _keyScrollEnabled)
            || (Input.mousePosition.x > Screen.width - _edgeScrollSize) && _edgeScrollEnabled)
        {
            inputDirection.x = +1f;
        }

        Vector3 movementDirection =
            transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += movementDirection * _movementSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        float rotationDirection = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotationDirection = +1f;
        if (Input.GetKey(KeyCode.E))
            rotationDirection = -1f;

        transform.eulerAngles += new Vector3(0, rotationDirection * _rotationSpeed * Time.deltaTime, 0);
    }

    private void HandleCameraZoom()
    {
        float mouseScrollDelta = Input.mouseScrollDelta.y;
        Vector3 zoomDirection = _zoomFollowOffest.normalized;

        if (mouseScrollDelta > 0)
            _zoomFollowOffest -= zoomDirection * _zoomAmount;
        
        if (mouseScrollDelta < 0)
            _zoomFollowOffest += zoomDirection * _zoomAmount;

        if(_zoomFollowOffest.magnitude < _zoomFollowOffestMin)
            _zoomFollowOffest = zoomDirection * _zoomFollowOffestMin;
        
        if(_zoomFollowOffest.magnitude > _zoomFollowOffestMax)
            _zoomFollowOffest = zoomDirection * _zoomFollowOffestMax;

        transposer.m_FollowOffset =
            Vector3.Lerp(transposer.m_FollowOffset, _zoomFollowOffest, Time.deltaTime * _zoomSpeed);
    }
}
