using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterVeiw;

    [SerializeField]
    Vector3 _delta = new Vector3(3.0f, 6.0f, -5.0f);

    [SerializeField]
    public GameObject _player = null;

    // Start is called before the first frame update
    void Start()
    {

        //_player = GameObject.FindGameObjectWithTag("Player");
        transform.rotation = new Quaternion(40, 40, 0,0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_player != null && _mode == Define.CameraMode.QuarterVeiw)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = new Vector3(_player.transform.position.x, 1.0f, 0) + _delta;
                //transform.LookAt(_player.gameObject.transform);
                transform.LookAt(new Vector3(_player.transform.position.x, 1.0f, 0) + Vector3.right * 4);

            }
        }

    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterVeiw;
        _delta = delta;
    }


    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }
    IEnumerator ZoomOutCoroutine()
    {
        
        while (Camera.main.orthographicSize < 5.99f)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 6, 0.01f);
            yield return null;
        }
        
        
    }
}
