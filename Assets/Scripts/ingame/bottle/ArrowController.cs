using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private PlayerImageController playerImageController;
    private PadStrength padStrength;
    private PadDirection padDirection;
    private SkillButton skillButton;
    private BottleSelectController bottleSelectController;

    // Start is called before the first frame update
    void Start()
    {
        playerImageController = GameObject.Find("Player").GetComponent<PlayerImageController>();
        padStrength = GameObject.Find("Pad_Strength").GetComponent<PadStrength>();
        padDirection = GameObject.Find("Joystick").GetComponent<PadDirection>();
        skillButton = GameObject.Find("Button_Skill").GetComponent<SkillButton>();
        bottleSelectController = GameObject.Find("BottleManager").GetComponent<BottleSelectController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerImageController.getBottlePosition();

        if (padStrength.isTouch || skillButton.getUsingSkill() || bottleSelectController.bottleController.isSuperPowerAvailabe)
            transform.GetChild(0).gameObject.SetActive(false);
        else if (padDirection.getIsTouch())
        {
            transform.GetChild(0).gameObject.SetActive(true);
            double angle = Mathf.Atan2(padDirection.getDirection().y, padDirection.getDirection().x) * (180.0 / Mathf.PI) +185;
            if (padDirection.getDirection().magnitude == 0) this.transform.rotation = Quaternion.Euler(0, 0, 0); //드래그를 안 했을 시 회전하지 않는다.
            else this.transform.rotation = Quaternion.Euler(0, 0, (float)angle); //조이스틱의 방향에 따라 방향을 변경
        }
    }
}
