using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [HideInInspector]
    public Room m_parentRoom = null;

    [Header("Assigned Objects")]

    public GameObject m_portalWindow = null;
    public Camera m_portalCamera = null;

    public Portal m_connectedPortal = null;

    private GameObject m_playerObj = null;
    private Camera m_playerCamera = null;

    private RenderTexture m_viewTexture = null;

    private List<Entity> m_collidingEntities = new List<Entity>();

    //Plane equations
    private Vector3 m_planeForwardVector = Vector3.zero;

    [HideInInspector]
    public bool m_portalBrokenFlag = false;

    /// <summary>
    /// Setup the portal
    /// </summary>
    public void Init(Room p_parentRoom)
    {
        m_parentRoom = p_parentRoom;

        //Remove cameras parent, always it to bemoved globally
        //m_portalCamera.transform.SetParent(null, true);

        m_playerObj = GameObject.FindGameObjectWithTag(CustomTags.PLAYER);

        if (m_playerObj == null)
        {
            Debug.Log("Player isnt found, need the tag of player");
            return;
        }

        m_playerCamera = m_playerObj.GetComponentInChildren<Camera>();

        //Setup render target
        //Grab varibles from other classes
        m_viewTexture = new RenderTexture(m_playerCamera.pixelWidth, m_playerCamera.pixelHeight, 24);

        //Setup connected camera
        m_portalCamera.forceIntoRenderTexture = true;
        m_portalCamera.targetTexture = m_viewTexture;

        //Setup material
        MeshRenderer windowMeshRenderer = m_portalWindow.GetComponent<MeshRenderer>();

        Material windowMaterial = Instantiate(windowMeshRenderer.material);
        windowMeshRenderer.material = windowMaterial;

        windowMaterial.SetTexture("_MainTex", m_viewTexture);

        m_portalCamera.enabled = false;

        //plane stuff
        m_planeForwardVector = transform.forward;
        m_planeForwardVector = m_planeForwardVector.normalized;
    }

    private void Update()
    {
        if(ShouldIUpdateCamera())
        {
            UpdateConnectedCamera();
        }

        //Loop through colliding entites
        for (int entityIndex = 0; entityIndex < m_collidingEntities.Count; entityIndex++)
        {
            Entity currentEntity = m_collidingEntities[entityIndex];

            if (MovedThroughWindow(currentEntity.transform.position))
            {

                //Position
                Matrix4x4 worldMatrix = m_connectedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * currentEntity.transform.localToWorldMatrix;

                //Move to other side of portal
                currentEntity.transform.position = worldMatrix.GetColumn(3);
                currentEntity.transform.rotation = worldMatrix.rotation;

                currentEntity.transform.RotateAround(m_connectedPortal.transform.position, m_connectedPortal.transform.up, 180.0f);

                currentEntity.transform.position += m_connectedPortal.transform.forward * 0.05f;

                m_collidingEntities.RemoveAt(entityIndex);
                entityIndex--;

                //Update current room
                currentEntity.SetCurrentRoom(m_connectedPortal, m_connectedPortal.m_parentRoom);
            }
        }
    }

    private void LateUpdate()
    {
        if (ShouldIUpdateCamera())
        {
            PrintCamera();
        }
    }

    private void UpdateConnectedCamera()
    {
        m_playerCamera.transform.RotateAround(transform.position, transform.up, 180.0f);
        Matrix4x4 originalCameraLocalToWorld = m_playerCamera.transform.localToWorldMatrix;
        m_playerCamera.transform.RotateAround(transform.position, transform.up, -180.0f);

        //Offset matrix
        Matrix4x4 cameraOffsetMatrix = m_connectedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * originalCameraLocalToWorld;

        //Apply transform
        m_portalCamera.transform.SetPositionAndRotation(cameraOffsetMatrix.GetColumn(3), cameraOffsetMatrix.rotation);
    }


    private void PrintCamera()
    {
        m_portalCamera.enabled = true;

        m_portalCamera.Render();

        m_portalCamera.enabled = false;
    }

    /// <summary>
    /// TODO actually check if I should run
    /// </summary>
    /// <returns></returns>
    public bool ShouldIUpdateCamera()
    {
        return m_parentRoom.IsActiveRoom && m_connectedPortal != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();

        if (entity != null && !m_collidingEntities.Contains(entity))
        {
            m_collidingEntities.Add(entity);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();

        if (entity != null && m_collidingEntities.Contains(entity))
        {
            m_collidingEntities.Remove(entity);
        }
    }

    /// <summary>
    /// Based off location of entity determing if entering or exiting
    /// </summary>
    /// <param name="p_position">Position of entity</param>
    /// <returns>Entering when moving close to trigger forward</returns>
    private bool MovedThroughWindow(Vector3 p_position)
    {
        Vector3 centerToPos = p_position - transform.position;

        centerToPos = centerToPos.normalized;

        return Vector3.Dot(m_planeForwardVector, centerToPos) < 0.0f;
    }
}
