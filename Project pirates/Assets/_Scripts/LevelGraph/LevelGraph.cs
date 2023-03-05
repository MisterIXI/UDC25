using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class LevelGraph : EditorWindow
{
    private LevelGraphView _graphView;
    private string _fileName = "New Graph";
    private LevelNodeContainer _currentContainer;
    [MenuItem("Project pirates/LevelGraph")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelGraph>();
        window.titleContent = new GUIContent("LevelGraph");
        window.Show();
    }

    private void OnGUI()
    {

    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        GenerateMiniMap();
        GenerateBlackBoard();
    }
    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
    private void ConstructGraphView()
    {
        _graphView = new LevelGraphView
        {
            name = "LevelGraph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() => _graphView.CreateNode("LevelNode"))
        {
            text = "Create Node"
        };
        toolbar.Add(nodeCreateButton);
        var referenceField = new ObjectField
        {
            objectType = typeof(AnchorList),
            value = null,
            allowSceneObjects = false
        };
        var fileField = new ObjectField("Source File")
        {
            objectType = typeof(LevelNodeContainer),
            value = Selection.activeObject,
            allowSceneObjects = false
        };
        fileField.RegisterValueChangedCallback(evt =>
        { _currentContainer = (LevelNodeContainer)evt.newValue; });
        toolbar.Add(fileField);
        rootVisualElement.Add(toolbar);
    }
    private void GenerateMiniMap()
    {

    }

    private void GenerateBlackBoard()
    {

    }

    private void SaveOrLoad(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name", "OK");
            return;
        }
        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_currentContainer);
        }
        else
        {
            saveUtility.LoadGraph(_currentContainer);
        }
    }
}