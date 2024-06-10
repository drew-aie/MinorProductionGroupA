using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PointsTimer : MonoBehaviour
{
    // Text field for displaying time
    [SerializeField]
    private TextMeshProUGUI _Time;
    [SerializeField]
    private TMP_Text _TotalPoints;
    [SerializeField]
    private TMP_Text _Kids;
    [SerializeField]
    // Maximum points

    private float _maxPoints = 1000f;
    // total points
    
    private float _globalpoints;
    private float _localpoints;
    // Points decrease rate per second
    private int _value = 0;
    private int _decreaseRate = 25;
    [SerializeField]
    private CandyInterations _human;
    [SerializeField]
    private CandyInterations _monster;
    [SerializeField, Min(2)]
    private int _maxkids = 10;
    private bool _maxReached = false;
    private float _totalgivencandy;
    private ChildController _childInteractions;
    [SerializeField]
    private DoorInteractionScript _doorInteraction;
    [SerializeField]
    private float _secondstowait = 1f;
    private bool _stopDecreasing = false;
    [SerializeField]
    private Gradient _timerGradient;
    [SerializeField]
    private RawImage _timerBarFG;
    private float _timePercentage;
    [SerializeField]
    private FadeInOut _Fade;
    private Tweener _doTweenMove;
    [SerializeField]
    private TMP_Text _PointsDifference;
    [SerializeField]
    private ScoreManager _scoreManager;

    public bool MaxReached()
    {
        if (_totalgivencandy >= _maxkids)
        {
            _maxReached = true;
        }
        return _maxReached;
    }
    public float MaxKids
    {
        get { return _totalgivencandy; }
    }
    private void Start()
    {

        _childInteractions = FindObjectOfType<ChildController>();
        _localpoints = _maxPoints;
        _doTweenMove = DOTween.To(() => _value, (x) => _value = x, _decreaseRate, 8).SetEase(Ease.InOutExpo);
        _localpoints = Mathf.Clamp(_localpoints, 0, _maxPoints);
        _totalgivencandy = Mathf.Clamp(_totalgivencandy, 0, _maxkids);
        _value = Mathf.Clamp(_value, 0, 25);


    }
    private void Update()
    {
        
        
        _Kids.text = _totalgivencandy.ToString() + "/" + _maxkids ;
        if (_stopDecreasing)
        {
            _localpoints = 0;
            
            StopDecreasingPoints();
        }
        else if (!_stopDecreasing && _localpoints <= 0)
        {
            _localpoints = 0;
            
        }
        _timePercentage = Mathf.Clamp01(_localpoints / _maxPoints);
        Vector3 newScale = _timerBarFG.rectTransform.localScale;
        newScale.x = _timePercentage;
        _timerBarFG.rectTransform.localScale = newScale;
        _timerBarFG.color = _timerGradient.Evaluate(_timePercentage);
        
        
        if (_totalgivencandy == _maxkids)
        {
            _doorInteraction.GetComponent<Collider>().enabled = false;
            _Kids.text = "<s>" + "Children in " + "<br>" + "Neighbourhood: " + "<br>" + _totalgivencandy.ToString() + "/" + _maxkids + "<s>";
            StopDecreasingPoints();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
    public void StartDecreasing()
    {
        _stopDecreasing = false;

        StartCoroutine(DecreasePointsOverTime());
        
    }
    IEnumerator DecreasePointsOverTime()
    {

        while ( _totalgivencandy < _maxkids)
        {

                // Wait for 1 second
                yield return new WaitForSeconds(0.03f);
                // Decrease points
                _localpoints -= _value;

                

            
        }

    }

    public void StopDecreasingPoints()
    {
        _stopDecreasing = true;
        StopCoroutine(DecreasePointsOverTime());
        _localpoints = _maxPoints;

        if (_value <= _decreaseRate)
        {
            _value = 0;
            _doTweenMove.Restart();
        }
    }

    public void OnOptionSelected()
    {
        // Call this function when an option is selected
        StopDecreasingPoints();
        
        

        _scoreManager.SetPoints = Mathf.Clamp(_scoreManager.GetPoints, 0, Mathf.Infinity);

       
    }
    public void AddPoints()
    {
        
        if (_totalgivencandy < _maxkids )
        {
            _scoreManager.SetPoints = _scoreManager.GetPoints + _localpoints;
            
            _totalgivencandy++;
            _scoreManager.SetPoints = Mathf.Clamp(_scoreManager.GetPoints, 0, Mathf.Infinity);
            
            _Fade.Fading();
            _TotalPoints.text = _scoreManager.GetPoints.ToString();
            _PointsDifference.text = "+" + _localpoints.ToString();
        }

    }

    public void RemovePoints()
    {
        
        if (_totalgivencandy < _maxkids)
        {
            _scoreManager.SetPoints = _scoreManager.GetPoints - _localpoints;
            
            _totalgivencandy++;
            _scoreManager.SetPoints = Mathf.Clamp(_scoreManager.GetPoints, 0, Mathf.Infinity);
            
            _Fade.Fading();
            _TotalPoints.text = _scoreManager.GetPoints.ToString();
            _PointsDifference.text = "-" + _localpoints.ToString();

        }

    }
}
