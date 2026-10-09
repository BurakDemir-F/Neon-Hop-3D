using System;
using DG.Tweening;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackableItem : MonoBehaviour, IPoolObjectController
    {
        [SerializeField] private StackableItemDataSo _itemData;
        
        private AttributeCollection _attributeCollection = new();

        public AttributeCollection AttributeCollection => _attributeCollection;

        public Transform Transform => transform;
        public IItemStack CurrentStack { get; set; }

        public void Initialize(StackableItemDataSo itemData, IContextProvider contextProvider)
        {
            if (itemData != null)
            {
                _itemData = itemData;
            }

            Initialize(contextProvider);
        }

        public void Initialize(IContextProvider contextProvider)
        {
            _attributeCollection.Clear();

            if (_itemData != null && _itemData.Brain != null && _itemData.Brain.AttributeBaseList != null)
            {
                foreach (var attributeBase in _itemData.Brain.AttributeBaseList)
                    _attributeCollection.UpdateObject(attributeBase);
            }
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);

            ResetDissolve();

            if (_attributeCollection.TryGetAttribute<ColorIdAttribute>(out var colorIdAttribute))
            {
                if (_renderer == null)
                    _renderer = GetComponentInChildren<MeshRenderer>();
                if (_renderer != null)
                {
                    _renderer.material.color = ColorIntConverter.IntToColor(colorIdAttribute.GetId());
                }
            }
        }

        private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");
        private MeshRenderer _renderer;
        private Tween _dissolveTween;

        public void ResetDissolve()
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<MeshRenderer>();
            if (_renderer != null && _renderer.material != null && _renderer.material.HasProperty(DissolveAmountId))
            {
                _renderer.material.SetFloat(DissolveAmountId, 0f);
            }
        }

        public void SetDissolve(float amount)
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<MeshRenderer>();
            if (_renderer != null && _renderer.material != null && _renderer.material.HasProperty(DissolveAmountId))
            {
                _renderer.material.SetFloat(DissolveAmountId, amount);
            }
        }

        public void Crack(Action onComplete = null)
        {
            var hasCrackAttribute = _attributeCollection.TryGetObject<CrackAttribute>(out var crackAttribute);
            
            if (!hasCrackAttribute)
            {
                onComplete?.Invoke();
                ReturnToPool();
                return;
            }

            _dissolveTween?.Kill();
            DG.Tweening.ShortcutExtensions.DOKill(transform);

            // Detach from stack root so it dissolves cleanly in place
            transform.SetParent(null, true);

            float dissolveVal = 0f;
            SetDissolve(0f);

            _dissolveTween = DG.Tweening.DOTween.To(() => dissolveVal, x =>
            {
                dissolveVal = x;
                SetDissolve(dissolveVal);
            }, 1f, 0.35f)
            .SetEase(DG.Tweening.Ease.InQuad)
            .OnComplete(() =>
            {
                _dissolveTween = null;
                onComplete?.Invoke();
                ReturnToPool();
            });
        }
        
        public string Key { get; set; }
        public void GetFromPool()
        {
            _dissolveTween?.Kill();
            _dissolveTween = null;
            DG.Tweening.ShortcutExtensions.DOKill(transform);
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            ResetDissolve();
        }

        void IPoolObjectSetter.ReturnedToPool()
        {
            _dissolveTween?.Kill();
            _dissolveTween = null;
            DG.Tweening.ShortcutExtensions.DOKill(transform);
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            ResetDissolve();
        }

        public IPool Pool { get; set; }
        public GameObject Go => gameObject;
        public T As<T>() where T : class, IPoolObject
        {
            return this as T;
        }

        public void ReturnToPool()
        {
            Pool?.Return(this);
        }
    }
}