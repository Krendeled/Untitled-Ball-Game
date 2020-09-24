using UnityEngine;
using UnityEngine.Serialization;

namespace UntitledBallGame
{
    public class Parallax : MonoBehaviour
    {
        [System.Serializable]
        private class ParallaxLayer
        {
            public SpriteRenderer sprite;
            public Vector2 effectValue;
            public float yThreshold;
            
            public Transform Transform { get; set; }
            public Vector2 StartPos { get; set; }
            public Vector2 Size { get; set; }
            public float XRepeat { get; set; }
        }

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

            backgroundLayer.StartPos = Vector2.zero;
            var spriteTransform = backgroundLayer.sprite.transform;
            spriteTransform.position = Vector2.zero;
            backgroundLayer.Transform = spriteTransform;
            backgroundLayer.XRepeat = 1;
            backgroundLayer.Size = backgroundLayer.sprite.bounds.size;

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].StartPos = Vector2.zero;
                layers[i].sprite.transform.position = Vector2.zero;
                layers[i].Transform = layers[i].sprite.transform;
                layers[i].XRepeat = Mathf.CeilToInt(_levelBounds.Box.size.x / layers[i].sprite.bounds.size.x);
                layers[i].Size = new Vector2(layers[i].sprite.bounds.size.x * layers[i].XRepeat,
                    layers[i].sprite.bounds.size.y);

                if (layers[i].effectValue.x != 1)
                {
                    if (layers[i].XRepeat > 1)
                    {
                        var parent = new GameObject(layers[i].sprite.gameObject.name);
                        parent.transform.position = Vector2.zero;
                        parent.transform.parent = transform;
                        layers[i].Transform = parent.transform;

                        float startX = layers[i].Transform.position.x - layers[i].Size.x / 2 +
                                       layers[i].sprite.bounds.extents.x;

                        for (int j = 0; j < layers[i].XRepeat; j++)
                        {
                            var newPos = new Vector2(startX + layers[i].sprite.bounds.size.x * j,
                                layers[i].Transform.position.y);
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
            Vector2 sizeDelta = camBounds.size / backgroundLayer.Size;

            if (Mathf.Max(sizeDelta.x, sizeDelta.y) != 1)
            {
                if (sizeDelta.x > sizeDelta.y)
                    backgroundLayer.Transform.localScale = Vector2.one * sizeDelta.x;
                else
                    backgroundLayer.Transform.localScale = Vector2.one * sizeDelta.y;
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

            layer.Transform.position = new Vector2(layer.StartPos.x + distX,
                Mathf.Clamp(distY, layer.yThreshold, float.PositiveInfinity));

            var quarter = layer.Size.x / 4;
            var half = layer.Size.x / 2;

            if (relativeX > layer.StartPos.x + quarter)
                layer.StartPos = new Vector2(layer.StartPos.x + half, layer.StartPos.y);
            else if (relativeX < layer.StartPos.x - quarter)
                layer.StartPos = new Vector2(layer.StartPos.x - half, layer.StartPos.y);
        }
    }
}