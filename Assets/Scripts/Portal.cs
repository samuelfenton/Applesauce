using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Assigned Objects")]
    public Portal m_connectedPortal = null;
    public GameObject m_portalWindow = null;
    public Camera m_portalCamera = null;

    private GameObject m_playerObj = null;
    private Camera m_playerCamera = null;

    private RenderTexture m_viewTexture = null;

    /// <summary>
    /// Setup the portal
    /// </summary>
    private void Awake()
    {
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
    }

    private void Update()
    {
        if(ShouldIUpdateCamera())
        {
            UpdateConnectedCamera();
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
        return true;
    }
}
