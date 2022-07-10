using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoManager : MonoBehaviour
{
    static public DemoManager Instance;

    [SerializeField] Text _text;

    List<EnemyFireController> _enemys = new List<EnemyFireController>();


    public Action OnGameClear;
    public Action OnGameOver;
    public Action OnGameEnd;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Check();
        _text.text = $"残りの敵 : {_enemys.Count}機";
        if (_enemys.Count <= 0)
        {
            //Load();
        }
    }

    public void Load()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Add(EnemyFireController e)
    {
        _enemys.Add(e);
    }

    void Check()
    {
        for (int i = 0; i < _enemys.Count; i++)
        {
            if (!_enemys[i])
            {
                _enemys.RemoveAt(i);
                i--;
            }
        }
    }

    public void GameOver()
    {
        CallGameEnd();
        CallGameOver();
    }

    public void GameClear()
    {
        CallGameEnd();
        CallGameClear();
    }

    void CallGameOver()
    {
        if(OnGameOver != null)
        {
            OnGameOver.Invoke();
        }
    }
    void CallGameClear()
    {
        if (OnGameClear != null)
        {
            OnGameClear.Invoke();
        }
    }
    void CallGameEnd()
    {
        if (OnGameEnd != null)
        {
            OnGameEnd.Invoke();
        }
    }
}
