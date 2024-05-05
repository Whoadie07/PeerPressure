using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CarouselUI : MonoBehaviour
{
    [Header("Carousel Members")]
    [SerializeField, Tooltip("Array containing gameobjects used for options.")] private GameObject[] _optionsObjects;

    [SerializeField, Tooltip("Button that increments index by 1.")] private GameObject _nextButton;

    [SerializeField, Tooltip("Button that decrements index by 1.")] private GameObject _prevButton;

    [Header("Settings")]

    [SerializeField, Tooltip("Time to deactivate inbetween refires.")] private float _resetDuration = 0.1f;

    [SerializeField, Tooltip("If true, when the index reaches either limit the next/previous buttons are hidden.")] private bool _doesNotCycleBack = false;

    private int _currentIndex = 0;
    public int CurrentIndex
    {
        get { return _currentIndex; }
        set { _currentIndex = value; }
    }
    public delegate void InputDetected();
    public event InputDetected InputEvent = delegate { };

    private bool _isProcessing = false; //HERE TO DELAY REFIRES
    private WaitForSeconds _resetDelay; //WORKS WITH DELAY COROUTINE
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_optionsObjects.Length == 0 || _optionsObjects == null) //ERROR IF THE OPTIONS ARRAY IS EMPTY
        {
            Debug.LogError($"Carousel UI at {this.gameObject.name} has incomplete options array. Please fix.");

            return;
        }

        _resetDelay = new WaitForSeconds(_resetDuration);

        UpdateUI();
    }
    private void UpdateUI()
    {
        foreach (GameObject text in _optionsObjects)
        {
            text.SetActive(false); //IF ONE OF THE OPTIONS IS NULL IT WILL CREATE AN ERROR HERE
        }

        _optionsObjects[_currentIndex].SetActive(true);

        if (_doesNotCycleBack && _currentIndex == _optionsObjects.Length - 1)
        {
            _nextButton.SetActive(false);
        }
        else
        {
            _nextButton.SetActive(true);
        }

        if (_doesNotCycleBack && _currentIndex == 0)
        {
            _prevButton.SetActive(false);
        }
        else
        {
            _prevButton.SetActive(true);
        }

    }
    private IEnumerator LockoutDelay()
    {
        _isProcessing = true; //PREVENTS BUTTON MASHING

        yield return _resetDelay;

        _isProcessing = false;

        yield break;
    }

    //METHOD ACCESSED BY NEXT BUTTON
    public void PressNext()
    {
        if (_isProcessing)
        {
            return;
        }

        if (_doesNotCycleBack && _currentIndex == _optionsObjects.Length - 1)
        {
            return;
        }

        StartCoroutine(LockoutDelay());

        if (_currentIndex < (_optionsObjects.Length - 1))
        {
            _currentIndex += 1;

            UpdateUI();
        }
        else
        {
            _currentIndex = 0;

            UpdateUI();
        }

        InputEvent?.Invoke();
    }

    //METHOD ACCESSED BY PREVIOUS BUTTON
    public void PressPrevious()
    {
        if (_isProcessing)
        {
            return;
        }

        if (_doesNotCycleBack && _currentIndex == 0)
        {
            return;
        }

        StartCoroutine(LockoutDelay());

        if (_currentIndex > 0)
        {
            _currentIndex -= 1;

            UpdateUI();
        }
        else
        {
            _currentIndex = (_optionsObjects.Length - 1);

            UpdateUI();
        }

        InputEvent?.Invoke();
    }
}
