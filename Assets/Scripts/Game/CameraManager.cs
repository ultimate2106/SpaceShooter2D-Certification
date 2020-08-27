using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Camera Shake
    private bool _isShakeActive = false;

    private float _shakeDuration = 1f;
    private float _currenShakeDuration;

    // Amplitude of the shake. A larger value shakes the camera harder
    private float _shakeAmount = 0.2f;

    private float _decreaseFactor = 1.0f;

    private Vector3 _originalPos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;
        _currenShakeDuration = _shakeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isShakeActive && _currenShakeDuration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;

            _currenShakeDuration -= Time.deltaTime * _decreaseFactor;
        }
        else
        {
            _currenShakeDuration = _shakeDuration;
            _isShakeActive = false;
        }
    }

    public void ShakeCamera()
    {
        _isShakeActive = true;
    }
}
