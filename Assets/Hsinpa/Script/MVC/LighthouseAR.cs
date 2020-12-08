using Hsinpa.CloudAnchor;
using Hsinpa.Model;
using Hsinpa.View;
using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LighthouseAR : Singleton<LighthouseAR>
{
    [SerializeField]
    private LightHouseAnchorManager _LightHouseAnchorManager;

    protected LighthouseAR() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Subject subject;

    private Observer[] observers = new Observer[0];

    private ModelsManager _modelManager;
    public ModelsManager modelManager => _modelManager;

    private int readyPipeline = 0;
    private int targetReadyPipeline = 2;

    private void Awake()
    {
        subject = new Subject();

        RegisterAllController(subject);

        Init();
    }

    private  void Start()
    {
        RegisterModels();

        _modelManager.firestoreModel.OnInit += AppStart;
        _LightHouseAnchorManager.OnCloudAnchorIsSetUp += AppStart;

        _LightHouseAnchorManager.SetUp();
    }

    private void AppStart(bool success) {
        readyPipeline++;
        if (targetReadyPipeline <= readyPipeline)
            Notify(EventFlag.Event.GameStart);
    }

    public void Notify(string entity, params object[] objects)
    {
        subject.notify(entity, objects);
    }

    public void Init()
    {
        Modals.instance.CloseAll();
    }

    private void RegisterAllController(Subject p_subject)
    {
        Transform ctrlHolder = transform.Find("Controller");

        if (ctrlHolder == null) return;

        observers = transform.GetComponentsInChildren<Observer>();

        foreach (Observer observer in observers)
        {
            subject.addObserver(observer);
        }
    }

    private void RegisterModels() {
        _modelManager = transform.Find("Model").GetComponent<ModelsManager>();
        _modelManager.SetUp();
    }


    public T GetObserver<T>() where T : Observer
    {
        foreach (Observer observer in observers)
        {
            if (observer.GetType() == typeof(T)) return (T)observer;
        }

        return default(T);
    }

    private void OnApplicationQuit()
    {

    }

}
