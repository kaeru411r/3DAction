using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DemoManager : MonoBehaviour
{
    static public DemoManager Instance;

    [SerializeField] Text _text;
    [SerializeField] CharacterBase _player;
    [SerializeField] UnityEvent _gameOver;
    [SerializeField] UnityEvent _gameClear;

    List<EnemyFireController> _enemys = new List<EnemyFireController>();
    bool _isPlay = false;


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
        _isPlay = true;
        EnemyFireController[] es = GameObject.FindObjectsOfType<EnemyFireController>();
        foreach(EnemyFireController e in es)
        {
            Add(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Check();
        _text.text = $"残りの敵 : {_enemys.Count}機";
        if (_isPlay)
        {
            if (_enemys.Count <= 0)
            {
                GameClear();
            }
            else if (_player.Hp <= 0)
            {
                GameOver();
            }
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
        StartCoroutine(GameOverSceneChange());
    }

    IEnumerator GameOverSceneChange()
    {
        yield return new WaitForSeconds(1);
        Load();
    }

    public void GameClear()
    {
        CallGameEnd();
        CallGameClear();
        StartCoroutine(GameClearSceneChange());
    }

    IEnumerator GameClearSceneChange()
    {
        yield return new WaitForSeconds(1);
        Load();
    }

    void CallGameOver()
    {
        if(OnGameOver != null)
        {
            OnGameOver.Invoke();
        }
        if(_gameOver != null)
        {
            _gameOver.Invoke();
        }
    }
    void CallGameClear()
    {
        if (OnGameClear != null)
        {
            OnGameClear.Invoke();
        }
        if(_gameClear != null)
        {
            _gameClear.Invoke();
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
