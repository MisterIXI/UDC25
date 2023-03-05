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
            value = Selection.activeObject is LevelNodeContainer ? Selection.activeObject : null,
            allowSceneObjects = false
        };
        _currentContainer = (LevelNodeContainer)fileField.value;
        var saveButton = new Button(() => SaveOrLoad(true))
        {
            text = "Save"
        };
        toolbar.Add(saveButton);
        var loadButton = new Button(() => SaveOrLoad(false))
        {
            text = "Load"
        };
        toolbar.Add(loadButton);
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
        if (_currentContainer == null)
        {
            EditorUtility.DisplayDialog("No file selected", "Please select a file to save or load", "OK");
            return;
        }
        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_currentContainer);
        }
        else
        {
            var nodelist = _graphView.nodes.ToList();
            if (nodelist.Count > 0)
            {
                if (!EditorUtility.DisplayDialog("Load Graph", "Loading a graph will delete the current one, are you sure?", "Yes", "No"))
                    return;
                _graphView.DeleteElements(nodelist);
            }

            saveUtility.LoadGraph(_currentContainer);
        }
    }
}