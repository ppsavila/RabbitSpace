using System;
using DG.Tweening;
using UnityEngine;

public class ModalVfx : MonoBehaviour
{
   [field: SerializeField] private RectTransform RectTransform { get; set; }
   [field: SerializeField] private CanvasGroup CanvasGroup { get; set; }
   
   public void Show()
   {
      RectTransform.localScale = new Vector3(.9f,.9f,.9f);
      CanvasGroup.alpha = 0;
      Sequence seq = DOTween.Sequence();
      seq.Insert(0, RectTransform.DOScale(1f, .2f)).SetEase(Ease.InBack);
      seq.Insert(.1f, CanvasGroup.DOFade(1f, .2f));
      seq.Play();
   }

   public void Hide(Action action = null)
   {
      CanvasGroup.blocksRaycasts = false;
      Sequence seq = DOTween.Sequence();
      seq.Insert(0f, CanvasGroup.DOFade(0f, .3f));
      seq.Insert(.1f, RectTransform.DOScale(.9f, .2f)).SetEase(Ease.OutBack);
      seq.OnComplete(() =>
      {
         action?.Invoke();
         gameObject.SetActive(false);
         CanvasGroup.blocksRaycasts = true;
      });
      seq.Play();
   }


   public void OnEnable()
   {
      Show();
   }
   
}
