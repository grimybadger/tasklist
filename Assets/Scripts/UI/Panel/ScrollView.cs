using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ScrollView : MonoBehaviour
{
  [Header("Prefab Settings")]
  [SerializeField] private Transform _transform;
  [SerializeField] private GameObject _task;

  [Header("General Settings")]
  [SerializeField] private List<GameObject> _images;
  [SerializeField] private GameObject _textInput;
  [SerializeField] private GameObject _taskCompletePanel;
  public Button Button { get; private set; }
  private Dictionary<ButtonType, List<Action>> ButtonFunctions = new();
  private List<GameObject> _taskList = new();
  private const int SizeLimit = 35;
  private const int LeftMouseClick = 0;
  private const int RightMouseClick = 1;

  private void Start()
  {
    Button.ButtonPress += OnButtonPress;

    ButtonFunctions.Add(ButtonType.task, new List<Action> { CreateTask, DefaultRightClickFunction });
    ButtonFunctions.Add(ButtonType.description, new List<Action> { NameTask, TaskComplete });
    ButtonFunctions.Add(ButtonType.timeElasped, new List<Action> { ShowTimeElasped, DefaultRightClickFunction });
    ButtonFunctions.Add(ButtonType.time, new List<Action> { PauseTimer, DefaultRightClickFunction });
    ButtonFunctions.Add(ButtonType.exit, new List<Action> { ExitProgram, DefaultRightClickFunction });
  }

  private void OnButtonPress(object sender, ButtonPressEventArgs e)
  {
    Button = (Button)sender;

    if (e.PointerEventData.button == PointerEventData.InputButton.Left)
    {
      ButtonFunctions[Button.ButtonType][LeftMouseClick]();
    }
    else if (e.PointerEventData.button == PointerEventData.InputButton.Right)
    {
      ButtonFunctions[Button.ButtonType][RightMouseClick]();
    }

  }
  private GameObject IsTaskAvaliable()
  {
    GameObject go = _taskList.FirstOrDefault(x => !x.GetComponent<Task>().IsInUse);
    return go;
  }

  private void CreateTask()
  {
    GameObject go;

    if (IsTaskAvaliable() != null)
    {
      go = IsTaskAvaliable();
      go.gameObject.GetComponent<Task>().ResetSettings();
    }
    else
    {
      if (HasReachedLimit()) return;
      go = Instantiate(_task, _transform);
      _taskList.Add(go);
    }
    Task t = go.GetComponent<Task>();

    foreach (var item in _taskList)
    {
      t.Time.gameObject.SetActive(item.GetComponent<Task>().Time.gameObject.activeSelf);
      break;
    }
    Debug.Log($"Tasklist size: {_taskList.Count}");
  }
  private void NameTask()
  {
    if (_textInput.gameObject.activeSelf == true) return;
    _textInput.gameObject.SetActive(true);
    StartCoroutine(InputField(Button.GetComponentInParent<Task>().Description, Button));
  }
  private void TaskComplete()
  {
    Task t = Button.GetComponentInParent<Task>();
    t.TimeStamp();
    t.gameObject.SetActive(false);
  }
  private void ShowTimeElasped()
  {
    foreach (var task in _taskList)
    {
      Task t = task.GetComponent<Task>();
      t.Time.gameObject.SetActive(!t.Time.gameObject.activeSelf);
    }
  }
  private void PauseTimer()
  {
    Button.GetComponentInParent<Task>().ToggleTimer();
  }
  private bool HasReachedLimit()
  {
    if (_taskList.Count == SizeLimit)
    {
      Debug.Log("Limit sized reached.");
      return true;
    }
    return false;
  }
  private void ExitProgram()
  {
    Debug.Log("GoodBye");
    Application.Quit();
  }
  private void DefaultRightClickFunction()
  {
    Debug.Log("Default Right Click Function");
  }

  private IEnumerator InputField(TMP_Text text, Button button)
  {
    string newText = default;
    text.text = string.Empty;
    _textInput.GetComponent<TMP_InputField>().text = string.Empty;

    while (_textInput.gameObject.activeSelf == true)
    {
      _textInput.gameObject.transform.position = button.gameObject.transform.position;
      newText = _textInput.GetComponent<TMP_InputField>().text;
      if (Input.GetKeyDown(KeyCode.Return))
        _textInput.gameObject.SetActive(false);
      yield return null;
    }

    text.text = newText == "" ? "New Task" : newText;
    _textInput.GetComponent<TMP_InputField>().text = _textInput.GetComponent<TMP_InputField>().placeholder.GetComponent<TMP_Text>().text;
    Debug.Log("Input is off");
  }

}