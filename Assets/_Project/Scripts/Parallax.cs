using UnityEngine;
using UnityEngine.Serialization;

namespace UntitledBallGame.UI
{
    public class Parallax : MonoBehaviour
    {
#pragma warning disable CA2235 // Mark all non-serializable fields
        [System.Serializable]
        private class ParallaxLayer
        {
            [FormerlySerializedAs("Sprite")] public SpriteRenderer sprite;
            [FormerlySerializedAs("EffectValue")] public Vector2 effectValue;
            [FormerlySerializedAs("YThreshold")] public float yThreshold;

            [FormerlySerializedAs("Transform")] [HideInInspector]
            public Transform transform;

            [FormerlySerializedAs("StartPos")] [HideInInspector]
            public Vector2 startPos;

            [FormerlySerializedAs("Size")] [HideInInspector]
            public Vector2 size;

            [FormerlySerializedAs("XRepeat")] [HideInInspector]
            public float xRepeat;
        }
#pragma warning restore CA2235 // Mark all non-serializable fields

        [SerializeField] private ParallaxLayer backgroundLayer;
        [SerializeField] private ParallaxLayer[] layers;

        private CameraController _cameraController;
        private LevelBounds _levelBounds;

        private void Awake()
        {
            _levelBounds = FindObjectOfType<LevelBounds>();
            if (_levelBounds == null)
            {
                Debug.Log($"[{nameof(Parallax)}] LevelBounds was not found.");
                gameObject.SetActive(false);
                return;
            }

            _cameraController = FindObjectOfType<CameraController>();
            if (_cameraController == null)
            {
                Debug.Log($"[{nameof(Parallax)}] CameraController was not found.");
                gameObject.SetActive(false);
                return;
            }

            backgroundLayer.startPos = Vector2.zero;
            backgroundLayer.sprite.transform.position = Vector2.zero;
            backgroundLayer.transform = backgroundLayer.sprite.transform;
            backgroundLayer.xRepeat = 1;
            backgroundLayer.size = backgroundLayer.sprite.bounds.size;

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].startPos = Vector2.zero;
                layers[i].sprite.transform.position = Vector2.zero;
                layers[i].transform = layers[i].sprite.transform;
                layers[i].xRepeat = Mathf.CeilToInt(_levelBounds.Box.size.x / layers[i].sprite.bounds.size.x);
                layers[i].size = new Vector2(layers[i].sprite.bounds.size.x * layers[i].xRepeat,
                    layers[i].sprite.bounds.size.y);

                if (layers[i].effectValue.x != 1)
                {
                    if (layers[i].xRepeat > 1)
                    {
                        var parent = new GameObject(layers[i].sprite.gameObject.name);
                        parent.transform.position = Vector2.zero;
                        parent.transform.parent = transform;
                        layers[i].transform = parent.transform;

                        float startX = layers[i].transform.position.x - layers[i].size.x / 2 +
                                       layers[i].sprite.bounds.extents.x;

                        for (int j = 0; j < layers[i].xRepeat; j++)
                        {
                            var newPos = new Vector2(startX + layers[i].sprite.bounds.size.x * j,
                                layers[i].transform.position.y);
                            if (j == 0)
                            {
                                layers[i].sprite.transform.position = newPos;
                                layers[i].sprite.transform.parent = parent.transform;
                            }
                            else
                            {
                                Instantiate(layers[i].sprite.gameObject, newPos, Quaternion.identity, parent.transform);
                            }
                        }
                    }
                }
            }
        }

        private void LateUpdate()
        {
            var camBounds = _cameraController.CalculateCameraBounds();
            Vector2 sizeDelta = camBounds.size / backgroundLayer.size;

            if (Mathf.Max(sizeDelta.x, sizeDelta.y) != 1)
            {
                if (sizeDelta.x > sizeDelta.y)
                    backgroundLayer.transform.localScale = Vector2.one * sizeDelta.x;
                else
                    backgroundLayer.transform.localScale = Vector2.one * sizeDelta.y;
            }

            UpdateLayer(backgroundLayer);

            for (int i = 0; i < layers.Length; i++)
            {
                UpdateLayer(layers[i]);
            }
        }

        private void UpdateLayer(ParallaxLayer layer)
        {
            float relativeX = _cameraController.CameraPosition.x * (1 - layer.effectValue.x);
            float distX = _cameraController.CameraPosition.x * layer.effectValue.x;
            float distY = _cameraController.CameraPosition.y * layer.effectValue.y;

            layer.transform.position = new Vector2(layer.startPos.x + distX,
                Mathf.Clamp(distY, layer.yThreshold, float.PositiveInfinity));

            var quarter = layer.size.x / 4;
            var half = layer.size.x / 2;

            if (relativeX > layer.startPos.x + quarter)
                layer.startPos = new Vector2(layer.startPos.x + half, layer.startPos.y);
            else if (relativeX < layer.startPos.x - quarter)
                layer.startPos = new Vector2(layer.startPos.x - half, layer.startPos.y);
        }
    }
}