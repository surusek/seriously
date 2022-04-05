using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RaceMechanics : MonoBehaviour
{
    public Text endStr, lapStr, velocityStr;

    private int _lap = 0;
    public int goal;

    public  Text  bestTimeStr, totalTimeStr,  currentTimeStr;
    private float bestTime,    totalTime = 0, currentTime = 0;

    private bool _check1, _check2, _end, _ended;

    private bool IsReachingGoal(Collider co) {
        if (co.tag == "Finish") {
            if (_lap > 0)
                return _check1 && _check2;
            else
                return true;
        }
        
        return false;
    }

    private void FormatLaps() {
        lapStr.text = String.Format("Okrąż.: {0}/{1}", _lap, goal);
    }

    private void FormatTime() {
        totalTimeStr.text = String.Format("Czas: {0:F2}", totalTime);
        currentTimeStr.text = String.Format("Czas okrąż.: {0:F2}", currentTime);
    }

    private void FormatBestTime() {
        string str = bestTime == float.MaxValue ? "--,--" : bestTime.ToString("F2");
        bestTimeStr.text = String.Format("Najlepszy czas okrąż.: {0}", str);
    }

    void FixedUpdate() {
        if (_lap < 1 || _lap > goal)
            return;

        totalTime += 0.02f;
        currentTime += 0.02f;
        FormatTime();
    }

    void OnTriggerEnter(Collider co) {
        if (IsReachingGoal(co)) {
            if (_lap > 0) {
                if (currentTime < bestTime) {
                    bestTime = currentTime;
                    FormatBestTime();
                }
            }
            else {
                endStr.text = "";
            }

            ++_lap;
            currentTime = 0;
            _check1 = false;
            _check2 = false;

            if (_lap > goal)
                _end = true;
            else
                FormatLaps();
        }

        if (co.tag == "checkpoint")
            _check1 = true;

        if (co.tag == "checkpoint2" && _check1)
            _check2 = true;
    }

    void Start() {
        endStr.text = "Przekrocz linię startu, aby rozpocząć!";
        FormatLaps();
        FormatTime();
        bestTime = float.MaxValue;
        FormatBestTime();
    }

    void Update() {
        if (_end) {
            _ended = true;
            Destroy(GetComponent<CarBehaviour>());
            endStr.text = "Gratulacje!";
            _end = false;
        }
        else if (_ended) {
            if (Input.GetButtonDown("Jump"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        velocityStr.text = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z.ToString("F0");
    }

}
