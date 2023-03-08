using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private Vector3 _dragSelectP1;
    private bool _isDragSelection;

    private List<BaseEntity> _selectedEntities = new List<BaseEntity>();

    public bool HasSelectedEntities => _selectedEntities.Count > 0;

    private void Awake()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _dragSelectP1 = Input.mousePosition;

        if(Input.GetMouseButton(0))
        {
            if ((_dragSelectP1 - Input.mousePosition).magnitude > 40)
                _isDragSelection = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isDragSelection)
            {
                SelectSingleEntity();
            }
            else
            {                
                _isDragSelection = false;
                SelectMultipleEntities();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(_selectedEntities.Count > 0)
            {                         
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Plane plane = new Plane(Vector3.up, 0);
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 worldPosition = ray.GetPoint(distance);
                    foreach (BaseEntity entity in _selectedEntities)
                        entity.SetDestination(worldPosition);
                }
            }
        }
    }

    private void OnGUI()
    {
        if(_isDragSelection)
        {
            var rect = GUIHelper.GetScreenRect(_dragSelectP1, Input.mousePosition);
            GUIHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            GUIHelper.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        BaseEntity entity = gameObject.GetComponent<BaseEntity>();
        if (entity != null
            && entity.isSelectable
            && !entity.isSelected)
        {
            entity.isSelected = true;
            _selectedEntities.Add(entity);
        }
    }

    private void SelectSingleEntity()
    {
        if ((!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            && this.HasSelectedEntities)
        {
            UnselectAllEntities();
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject gameObject = hit.transform.gameObject;
            BaseEntity entity = gameObject.GetComponent<BaseEntity>();
            if (entity != null
                && entity.isSelectable
                && !entity.isSelected)
            {
                entity.isSelected = true;
                _selectedEntities.Add(entity);
            }
        }
    }

    private void SelectMultipleEntities()
    {
        if ((!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            && this.HasSelectedEntities)
        {
            UnselectAllEntities();
        }

        Vector3[] verts = new Vector3[4];
        Vector3[] vecs = new Vector3[4];

        int i = 0;
        Vector3 dragSelectP2 = Input.mousePosition;
        Vector2[] corners = GetSelectionBoundingBox(_dragSelectP1, dragSelectP2);
        foreach (Vector2 corner in corners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);
            Plane plane = new Plane(Vector3.up, 0);
            if (plane.Raycast(ray, out float distance))                
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                verts[i] = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
                vecs[i] = ray.origin - worldPosition;
                //sDebug.DrawLine(Camera.main.ScreenToWorldPoint(corner), worldPosition, Color.red, 10.0f);
            }
            i++;
        }

        Mesh selectionMesh = GenerateSelectionMesh(verts, vecs);

        MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = selectionMesh;
        
        MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Diffuse"));
                
        MeshCollider selectionBox = this.gameObject.AddComponent<MeshCollider>();
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;
        selectionBox.enabled = true;

        float t = 0.02f;
        Destroy(selectionBox, t);
        Destroy(meshRenderer, t);
        Destroy(meshFilter, t);
    }

    private void UnselectAllEntities()
    {
        if (this.HasSelectedEntities)
            _selectedEntities.ForEach(x => x.isSelected = false);

        _selectedEntities.Clear();
    }
    
    private Vector2[] GetSelectionBoundingBox(Vector2 p1, Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(p1, p2);
        var topRight = Vector3.Max(p1, p2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }

    private Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }
}
