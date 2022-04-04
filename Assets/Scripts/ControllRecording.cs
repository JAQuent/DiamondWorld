using System.Collections;
using UnityEngine;
using UXF;

public class COntrollRecording : MonoBehaviour
{
    public PositionRotationTracker positionRotationTracker;

    public void TrialBegin()
    {
        StartCoroutine(ManualRecord());
    }

    public void TrialEnd()
    {
        StopAllCoroutines();
    }

    private IEnumerator ManualRecord()
    {
        while (true)
        {
            if (positionRotationTracker.Recording) positionRotationTracker.RecordRow();

            yield return new WaitForSeconds(0.2f);
        }
    }
}