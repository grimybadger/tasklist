using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Task : MonoBehaviour
{
   [field: Header("General Settings")]
   [field: SerializeField] public TMP_Text Description { get; set; }
   [field: SerializeField] public TMP_Text Time { get; private set; }
   [field: SerializeField] public bool IsTimerPaused { get; set; }

   [field: Header("Statistics")]
   [field: SerializeField] public string StartDate { get; private set; }
   [field: SerializeField] public string EndDate { get; private set; }
   [field: SerializeField] public string TimeElasped { get; private set; }
   [field: SerializeField] public bool TaskCompleted { get; private set; }
   [field: SerializeField] public bool IsInUse { get; private set; } = true;

   private void Start()
   {
      StartCoroutine(Timer());
   }
   public bool ToggleTimer() => IsTimerPaused = !IsTimerPaused;
   public void ResetSettings()
   {
      gameObject.SetActive(true);
      Description.text = "New Task";
      IsTimerPaused = false;
      StartDate = DateTime.Now.ToString();
      EndDate = string.Empty;
      TimeElasped = string.Empty;
      TaskCompleted = false;
      IsInUse = true;
      StartCoroutine(Timer());
   }

   public void TimeStamp()
   {
      EndDate = DateTime.Now.ToString();
      TimeSpan elaspedTime = DateTime.Now - DateTime.Parse(StartDate);
      TimeElasped = elaspedTime.ToString("c");
      IsInUse = false;
   }
   private IEnumerator Timer()
   {
      StartDate = DateTime.Now.ToString();
      double seconds = default;
      while (gameObject.activeSelf == true)
      {
         if (!IsTimerPaused)
         {
            seconds++;
            Time.text = TimeSpan.FromSeconds(seconds).ToString(@"h\:mm\:ss");
         }
         yield return new WaitForSeconds(1f);
      }
   }
}