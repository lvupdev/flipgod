using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SuperPowerController : MonoBehaviour
{
    protected int superPowerLV; //초능력 강화 레벨
    protected int skillLV; //필살기 강화 레벨
    protected BottleController bottleController;
    protected GameObject bottle;
    protected PlayerImageController playerImageController;
    protected SuperPowerPanelController SPPController;
    protected RadialBlurImageEffect blurEffect;
    protected float blurTime; //블러가 적용되는 시간
    protected float height; //게임화면 높이
    protected float width; //게임화면 넓이
    protected Vector2 initPos;//화면을 눌렀을 때의 위치
    protected Vector2 endPos;//화면에서 손을 땠을 떄의 위치
    protected bool isTouch;
}
