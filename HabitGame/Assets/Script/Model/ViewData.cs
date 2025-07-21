using AYellowpaper.SerializedCollections;
using UnityEngine;

//note
//기존에는 기획자가 구성한 XML 기획시트로 View에 표기되는 데이터를 관리했으나
//지금은 ScriptableObject로 관리한다.
//기획 데이터의 구분도 모두 생략하고, 여기에 넣고 관리한다. (현재는 소규모 프로젝트)

[CreateAssetMenu(fileName = "ViewData", menuName = "ScriptableObjects/ViewData")]
public class ViewData : ScriptableObject, IModel
{
    public SerializedDictionary<AlarmPresenter.EButtons, float> AlarmTimeDictionary = new();
}