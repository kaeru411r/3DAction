using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoManager : SingletonMonoBehaviour<DemoManager>
{
    [SerializeField] Text _text;

    List<EnemyFireController> _enemys = new List<EnemyFireController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check();
        _text.text = $"残りの敵 : {_enemys.Count}機";
        if(_enemys.Count <= 0)
        {
            Load();
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
        for(int i = 0; i < _enemys.Count; i++)
        {
            if (!_enemys[i])
            {
                _enemys.RemoveAt(i);
                i--;
            }
        }
    }
}
