using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    //public bool left_hand;
    public GameObject tool;

    public Dashboard dash;

    private bool initTurn;

    public FixedJoint fj;
    private GameObject grabbedObject;
    private bool isGrabbing, hasLetGo;

    private Vector3 contactPoint;
    private GameObject illusionHandGrabber;

    private OVRInput.Controller controller;
    private LineRenderer lr;
    private RaycastHit ray;

    private GameObject tempSelectedObject;
    private GameObject permaSelectedObject;

    private bool leftPrimaryButtonDown = false;
    private bool rightPrimaryButtonDown = false;

    private GameObject grabbingPoint;

    // Use this for initialization
    void Start () {
        controller = OVRInput.Controller.LTouch;

        lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update() {

        OVRInput.Update();
        transform.localPosition = OVRInput.GetLocalControllerPosition(controller);
        transform.localRotation = OVRInput.GetLocalControllerRotation(controller);

        ///----------------------------------------
        /// Grabbing selection mode
        ///----------------------------------------}

        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
            if (isGrabbing) {

                GetComponent<MeshRenderer>().enabled = false;

                if (!initTurn) {
                    illusionHandGrabber = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    illusionHandGrabber.transform.localScale *= 0.1f;

                    tool.GetComponent<ITool>().GetNewPos(transform.position);

                    initTurn = true;
                }
                illusionHandGrabber.transform.position = grabbingPoint.transform.position;

                InvokeRepeating("calculatePos", 0.01f, 1f);

                tool.GetComponent<ITool>().GetCurrPos(transform.position);
                tool.GetComponent<ITool>().Interact();
                tool.GetComponent<ITool>().BeingInteractedWith(true);
            }
        }
        else if(!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
            if(tool != null) tool.GetComponent<ITool>().BeingInteractedWith(false);

            Destroy(illusionHandGrabber);
            Destroy(grabbingPoint);

            GetComponent<MeshRenderer>().enabled = true;

            isGrabbing = false;
            initTurn = false;
        }

        ///----------------------------------------
        /// Laser selection mode
        ///----------------------------------------

        if(OVRInput.Get(OVRInput.Touch.PrimaryThumbRest))
        {
            if (lr.enabled == false) lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.forward * 50f);

            if(Physics.Raycast(transform.position, transform.forward * 50f, out ray))
            {
                if(ray.collider.tag == "Player" && ray.collider.gameObject != permaSelectedObject)
                {
                    tempSelectedObject = ray.collider.gameObject;
                    tempSelectedObject.GetComponent<Selectable>().SelectObject();

                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && !leftPrimaryButtonDown) 
                    {
                        if (permaSelectedObject != null) {
                            
                            permaSelectedObject.GetComponent<Selectable>().DeselectObject();
                            permaSelectedObject.GetComponent<Selectable>().PermaDeselectObject();
                            permaSelectedObject.GetComponent<Selectable>().isHighlighted = false;
                            permaSelectedObject = null;
                            dash.GetCurrCar(null);
                        }
                        permaSelectedObject = tempSelectedObject;
                        permaSelectedObject.GetComponent<Selectable>().PermaSelectObject();                        
                        permaSelectedObject.GetComponent<Selectable>().isHighlighted = true;
                        dash.GetCurrCar(permaSelectedObject.transform.parent.gameObject);
                        tempSelectedObject = null;
                        leftPrimaryButtonDown = true;
                    }
                }
                else 
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && !leftPrimaryButtonDown && permaSelectedObject != null)
                    {
                        leftPrimaryButtonDown = true;
                        permaSelectedObject.GetComponent<Selectable>().DeselectObject();
                        permaSelectedObject.GetComponent<Selectable>().PermaDeselectObject();
                        permaSelectedObject.GetComponent<Selectable>().isHighlighted = false;
                        permaSelectedObject = null;
                        dash.GetCurrCar(null);
                    }

                    if(tempSelectedObject != null) tempSelectedObject.GetComponent<Selectable>().DeselectObject();
                }
            }
        }
        else
            if(tempSelectedObject != null) tempSelectedObject.GetComponent<Selectable>().DeselectObject();

        if (OVRInput.GetUp(OVRInput.RawButton.Y) && permaSelectedObject != null)
        {
            permaSelectedObject.transform.parent.GetComponent<RCCar>().Respawn();
        }


        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) == 0.0f) leftPrimaryButtonDown = false;

        if (!OVRInput.Get(OVRInput.Touch.PrimaryThumbRest)) lr.enabled = false;

    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "grabbable" && !isGrabbing) {
            tool = other.gameObject;
            if(grabbingPoint == null) {
                grabbingPoint = new GameObject();
                grabbingPoint.transform.position = transform.position;
                grabbingPoint.transform.parent = other.gameObject.transform;
            }
            isGrabbing = true;
        }
    }

    void calculatePos() {
        tool.GetComponent<ITool>().GetNewPos(transform.position);
    }
}
