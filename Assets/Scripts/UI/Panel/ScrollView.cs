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

    ButtonFunctions.Add(ButtonType.task, new List<Action> { NewTask, DefaultRightClickFunction });
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
  private GameObject AvailableTask()
  {
    GameObject go = _taskList.FirstOrDefault(x => !x.GetComponent<Task>().IsInUse);
    return go;
  }
  private GameObject CreateTask()
  {
    GameObject go = Instantiate(_task, _transform);
    _taskList.Add(go);
    return go;
  }
  private void NewTask()
  {
    if (HasReachedLimit() && AvailableTask() == null) return;

    GameObject go = AvailableTask() == null ? CreateTask() : AvailableTask();
    go.gameObject.GetComponent<Task>().ResetSettings();
    Task t = go.GetComponent<Task>();
    t.Time.gameObject.SetActive(_taskList[0].GetComponent<Task>().Time.gameObject.activeSelf);
    
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
    return _taskList.Count == SizeLimit ? true: false;
  }
  private void ExitProgram()
  {
    Application.Quit();
  }
  private void DefaultRightClickFunction()
  {
    Debug.Log("Default Right Click Function");
  }
  private IEnumerator InputField(TMP_Text text, Button button)
  {
    string newText = string.Empty;
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