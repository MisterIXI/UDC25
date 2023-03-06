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
    private ObjectField _fileField;
    [MenuItem("Project pirates/LevelGraph")]
    public static void ShowWindow()
    {
        ShowWindow(null);
    }

    public static void ShowWindow(LevelNodeContainer container)
    {
        var window = GetWindow<LevelGraph>();
        window.titleContent = new GUIContent("LevelGraph");
        window.Show();
        if (container != null)
            window.InitWithContainer(container);
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
    private void InitWithContainer(LevelNodeContainer container)
    {
        _currentContainer = container;
        _fileName = container.name;
        ClearGraph();
        GraphSaveUtility.GetInstance(_graphView).LoadGraph(container);
        _fileField.SetValueWithoutNotify(container);
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
            value = Selection.activeObject is LevelNodeContainer ? Selection.activeObject : _currentContainer,
            allowSceneObjects = false
        };
        fileField.RegisterValueChangedCallback(evt =>
        { _currentContainer = (LevelNodeContainer)evt.newValue; });
        toolbar.Add(fileField);
        _fileField = fileField;
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
        var debugButton = new Button(() => _graphView.nodes.ToList().ForEach(n => { n.RefreshExpandedState(); n.RefreshPorts(); }))
        {
            text = "Debug"
        };
        toolbar.Add(debugButton);
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
                ClearGraph();
            }

            saveUtility.LoadGraph(_currentContainer);
        }
    }

    public void ClearGraph()
    {
        _graphView.DeleteElements(_graphView.nodes.ToList());
        _graphView.DeleteElements(_graphView.edges.ToList());
    }
}