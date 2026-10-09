using UnityEngine;
using General.GridSystem;
using DG.Tweening;

namespace Game.Sorcerum
{
    /// <summary>
    /// Dynamically frames the 3D grid for mobile screens (portrait and landscape).
    /// Ensures the grid is always centered and fully visible within the safe viewport,
    /// leaving appropriate space for the top HUD and bottom ball tray.
    /// </summary>
    public class MobileCameraFramer : MonoBehaviour
    {
        [Header("Camera Angles & Framing")]
        [SerializeField] private float _pitchAngle = 48f;
        [SerializeField] private float _horizontalMargin = 1.28f;
        [SerializeField] private float _verticalMargin = 1.25f;
        [SerializeField] private float _minDistance = 8.0f;
        [SerializeField] private float _targetCenterZ = 1.2f;
        [SerializeField] private float _safeViewportRatioY = 0.65f;

        private Camera _camera;
        private IGridData _currentGridData;
        private int _lastScreenWidth;
        private int _lastScreenHeight;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelStarted -= HandleLevelStarted;
                GameManager.Instance.OnLevelStarted += HandleLevelStarted;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelStarted -= HandleLevelStarted;
            }
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelStarted -= HandleLevelStarted;
                GameManager.Instance.OnLevelStarted += HandleLevelStarted;
            }
        }

        private void HandleLevelStarted(LevelDataSo levelData)
        {
            if (levelData != null && levelData.LevelDataVo != null)
            {
                FrameGrid(levelData.LevelDataVo.MapGrid);
            }
        }

        public void FrameGrid(IGridData gridData, bool animate = false)
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>() ?? Camera.main;
                if (_camera == null) return;
            }

            _currentGridData = gridData;
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;

            if (gridData == null || gridData.Dimensions.x <= 0 || gridData.Dimensions.y <= 0)
                return;

            float step = gridData.CellSize + gridData.Padding;
            float gridWidth = (gridData.Dimensions.x - 1) * step + gridData.CellSize;
            float gridDepth = (gridData.Dimensions.y - 1) * step + gridData.CellSize;

            float aspect = (float)Mathf.Max(Screen.width, 1) / Mathf.Max(Screen.height, 1);
            float fovV = _camera.fieldOfView;
            float tanHalfFov = Mathf.Tan(fovV * 0.5f * Mathf.Deg2Rad);

            // Required distance to fit grid width with horizontal margin
            float requiredDistanceX = (gridWidth * 0.5f * _horizontalMargin) / (aspect * tanHalfFov);

            // Required distance to fit grid depth with vertical margin in safe area
            float stackHeight = 1.5f;
            float depthProj = gridDepth * Mathf.Sin(_pitchAngle * Mathf.Deg2Rad) + stackHeight * Mathf.Cos(_pitchAngle * Mathf.Deg2Rad);
            float requiredDistanceY = (depthProj * 0.5f * _verticalMargin) / (tanHalfFov * _safeViewportRatioY);

            float targetDistance = Mathf.Max(requiredDistanceX, requiredDistanceY, _minDistance);

            // Target focal center on ground
            Vector3 focalPoint = new Vector3(0f, 0f, _targetCenterZ);

            // Calculate camera position given pitch angle and focal point
            float rad = _pitchAngle * Mathf.Deg2Rad;
            Vector3 targetPosition = focalPoint + new Vector3(
                0f,
                targetDistance * Mathf.Sin(rad),
                -targetDistance * Mathf.Cos(rad)
            );

            Quaternion targetRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);

            if (animate && Application.isPlaying)
            {
                _camera.transform.DOKill();
                _camera.transform.DOMove(targetPosition, 0.45f).SetEase(Ease.OutCubic);
                _camera.transform.DORotateQuaternion(targetRotation, 0.45f).SetEase(Ease.OutCubic);
            }
            else
            {
                _camera.transform.position = targetPosition;
                _camera.transform.rotation = targetRotation;
            }
        }

        private void LateUpdate()
        {
            // Re-frame if screen aspect ratio or resolution changed during runtime
            if (_currentGridData != null && (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight))
            {
                FrameGrid(_currentGridData, false);
            }
        }
    }
}
