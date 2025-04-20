using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, Resettable
{
    [SerializeField] GameObject door_top;
    [SerializeField] GameObject door_bottom;
    [SerializeField] float doorOpenTime = 0.3f;
    [SerializeField] float doorCloseTime = 0.1f;
    float doorLen;
    Vector3 doorTopInitialPos;
    Vector3 doorBottomInitialPos;

    void Start()
    {
        doorLen = door_top.transform.localScale.y;
        doorTopInitialPos = door_top.transform.localPosition;
        doorBottomInitialPos = door_bottom.transform.localPosition;
    }

    public void Open()
    {
        door_top.transform.DOLocalMoveY(doorTopInitialPos.y + doorLen, doorOpenTime);
        door_bottom.transform.DOLocalMoveY(doorBottomInitialPos.y - doorLen, doorOpenTime);
    }

    public void Close()
    {
        door_top.transform.DOLocalMoveY(doorTopInitialPos.y, doorCloseTime);
        door_bottom.transform.DOLocalMoveY(doorBottomInitialPos.y, doorCloseTime);
    }

    public void DoReset()
    {
        door_top.transform.localPosition = doorTopInitialPos;
        door_bottom.transform.localPosition = doorBottomInitialPos;
    }
}